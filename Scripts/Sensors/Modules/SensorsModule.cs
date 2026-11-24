using Alchemy.Inspector;
using LOGIYGames.Extensions;
using UnityEngine;
using UnityEngine.Events;

namespace LOGIYGames
{

    public class SensorsModule : MonoModuleBase
    {

        [Header("Detection Settings")]
        [SerializeField] private float m_castUpSphereRadius = 0.2f;
        [SerializeField] private float m_castDownSphereRadius = 0.2f;
        [SerializeField] private float m_legsDistanceCheck = 0.8f;
        [SerializeField] private float m_detectionOriginYOffset = 0f;
        [SerializeField] float legsDetectionYOffset = 0;
        [SerializeField] float headDetectionYOffset = 0;
        public UnityEvent<bool> GroundedEvent = new();

        [Header("Layer Masks")]
        [SerializeField] private LayerMask m_includeLayers;
        [SerializeField] private LayerMask m_groundLayers;

        [Header("Debug Info")]
        [SerializeField] private bool m_showDebugInfo = true;
        private string m_aboveObstacleName;
        private string m_legsRightObstacleName;
        private string m_legsLeftObstacleName;
        private string m_legsForwardObstacleName;
        private string m_legsBehindObstacleName;
        [ShowIf(nameof(m_showDebugInfo))][SerializeField] Color rayColor;
        [ShowIf(nameof(m_showDebugInfo))][SerializeField] Color groundPlaneColor;
        [ShowIf(nameof(m_showDebugInfo))][SerializeField] Color groundedSphereColor;
        [ShowIf(nameof(m_showDebugInfo))][SerializeField] Color ungroundedSphereColor;
        [ShowIf(nameof(m_showDebugInfo))][SerializeField] Color aboveObstacleCollidedSphereColor;
        [ShowIf(nameof(m_showDebugInfo))][SerializeField] Color aboveObstacleNotCollidedSphereColor;

        [SerializeField] Collider collider;
        public float OriginYOffset {  get => m_detectionOriginYOffset; set => m_detectionOriginYOffset = value; }
        // Detection origin calculated from capsule bounds
        public Vector3 DetectionOrigin
        {
            get
            {
                return new Vector3(
                    collider.bounds.center.x,
                    collider.bounds.center.y + m_detectionOriginYOffset,
                    collider.bounds.center.z
                );
            }
        }

        // Detection Results
        private RaycastHit m_groundHit;
        private RaycastHit m_legsLeftHit;
        private RaycastHit m_legsRightHit;
        private RaycastHit m_aboveHit;
        private RaycastHit m_legsFrontHit;
        private RaycastHit m_headFrontHit;
        private RaycastHit m_legsBehindHit;

        // Public Properties
        public RaycastHit GroundHit => m_groundHit;
        public RaycastHit AboveHit => m_aboveHit;
        public RaycastHit LegsFrontHit => m_legsFrontHit;
        public RaycastHit LegsBehindHit => m_legsBehindHit;
        public RaycastHit HeadFrontHit => m_headFrontHit;
        public RaycastHit LegsRightHit => m_legsRightHit;
        public RaycastHit LegsLeftHit => m_legsLeftHit;

        public bool IsObstacleBelow { get; private set; }
        public bool IsObstacleAbove { get; private set; }
        public bool IsOnSlope { get; private set; }
        private bool isGrounded;
        public bool IsGrounded
        {
            get
            {
                return isGrounded;
            }
            private set
            {
                if (value != isGrounded)
                {
                    if (value)
                    {
                        GroundedEvent.Invoke(true);
                    }
                    else
                    {
                        GroundedEvent.Invoke(false);
                    }
                }
                isGrounded = value;
            }
        }
        public bool IsObstacleLegsFront { get; private set; }
        public bool IsObstacleHeadFront { get; private set; }
        public bool IsObstacleLegsBehind { get; private set; }
        public bool IsObstacleLegsRight { get; private set; }
        public bool IsObstacleLegsLeft { get; private set; }
        /// <summary>
        /// value > 0: Up,
        /// value < 0: Down, 
        /// value = 0: On plane, or Not grounded
        /// </summary>
        public float GroundAngle { get; private set; }

        [Range(0, 90)]
        public float MaxStableSlopeAngle;
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            PerformDetection();
        }

        public override void OnLateUpdate(float deltaTime)
        {
            base.OnLateUpdate(deltaTime);
            if (m_showDebugInfo)
            {
                UpdateDebugInfo();
            }
        }
        public float GetPlaneAbsAngle(Vector3 normal)
        {
            return Vector3.Angle(normal, Vector3.up);
        }
        public bool IsValidSlope()
        {
            float angle = Vector3.Angle(m_groundHit.normal, transform.up);
            bool validAngle = Mathf.Abs(angle) <= MaxStableSlopeAngle;
            return validAngle;
        }
        public float GroundCheckDistanceExtend { get; set; } 
        private void PerformDetection()
        {
            AboveObstaclesDetection();
            LegsLeftObstaclesDetection();
            LegsRightObstaclesDetection();
            LegsFrontObstaclesDetection();
            HeadFrontObstaclesDetection();
            LegsBehindObstaclesDetection();
            IsGrounded = Physics.SphereCast(
                DetectionOrigin,
                m_castDownSphereRadius,
                -transform.up,
                out m_groundHit,
                 collider.LowerHalfHeight() + GroundCheckDistanceExtend,
                m_groundLayers
            );

            GroundAngle = Vector3.SignedAngle(

                GroundHit.normal,
                transform.up,
                transform.right
            );
            IsOnSlope = GroundAngle == 0 ? false : true;
        }

        private void LegsFrontObstaclesDetection()
        {
            IsObstacleLegsFront = Physics.Raycast(
                DetectionOrigin + transform.up * legsDetectionYOffset,
                transform.forward,
                out m_legsFrontHit,
                m_legsDistanceCheck,
                m_includeLayers
            );
        }
        private void HeadFrontObstaclesDetection()
        {
            IsObstacleHeadFront = Physics.Raycast(
                DetectionOrigin + transform.up * collider.TopHalfHeight(),
                transform.forward,
                out m_headFrontHit,
                m_legsDistanceCheck,
                m_includeLayers
            ); ;
        }
        private void LegsBehindObstaclesDetection()
        {
            IsObstacleLegsBehind = Physics.Raycast(
                DetectionOrigin + transform.up * legsDetectionYOffset,
                -transform.forward,
                out m_legsBehindHit,
                m_legsDistanceCheck,
                m_includeLayers
            );
        }
        private void LegsRightObstaclesDetection()
        {
            IsObstacleLegsRight = Physics.Raycast(
                DetectionOrigin + transform.up * legsDetectionYOffset,
                transform.right,
                out m_legsRightHit,
                m_legsDistanceCheck,
                m_includeLayers
            );
        }
        private void LegsLeftObstaclesDetection()
        {
            IsObstacleLegsLeft = Physics.Raycast(
                DetectionOrigin + transform.up * legsDetectionYOffset,
                -transform.right,
                out m_legsLeftHit,
                m_legsDistanceCheck,
                m_includeLayers
            );
        }
        private void AboveObstaclesDetection()
        {
            IsObstacleAbove = Physics.SphereCast(
                DetectionOrigin,
                m_castUpSphereRadius,
                transform.up,
                out m_aboveHit,
                collider.TopHalfHeight(),
                m_includeLayers
            );
        }

        private void UpdateDebugInfo()
        {
            m_aboveObstacleName = m_aboveHit.transform?.name;
            m_legsForwardObstacleName = m_legsFrontHit.transform?.name;
        }

        private void OnDrawGizmosSelected()
        {

            if (!m_showDebugInfo || collider == null) return;

            // Draw sphere casts
            DrawSphereCasts(DetectionOrigin);
            DrawBelowPlane();

            Debug.DrawRay(DetectionOrigin + transform.up * legsDetectionYOffset, transform.forward * m_legsDistanceCheck, rayColor);
            Debug.DrawRay(DetectionOrigin + transform.up * legsDetectionYOffset, transform.right * m_legsDistanceCheck, rayColor);
            Debug.DrawRay(DetectionOrigin + transform.up * legsDetectionYOffset, -transform.right * m_legsDistanceCheck, rayColor);

            Debug.DrawRay(DetectionOrigin + transform.up * collider.TopHalfHeight(), transform.forward * m_legsDistanceCheck, rayColor);
        }

        private void DrawBelowPlane()
        {
            DebugDraw.DrawPlane(m_groundHit.point, m_groundHit.normal, 1, groundPlaneColor);
        }

        private void DrawSphereCasts(Vector3 origin)
        {
            Gizmos.color = IsObstacleAbove ? aboveObstacleCollidedSphereColor : aboveObstacleNotCollidedSphereColor;
            Gizmos.DrawWireSphere(origin + transform.up * collider.TopHalfHeight(), m_castUpSphereRadius);

            Gizmos.color = IsGrounded ? groundedSphereColor : ungroundedSphereColor;
            Gizmos.DrawWireSphere(origin + -transform.up * (collider.LowerHalfHeight() + GroundCheckDistanceExtend), m_castDownSphereRadius);

        }
    }
}
