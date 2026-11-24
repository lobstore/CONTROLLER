using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    [RequireComponent(typeof(Rigidbody))]
    [RequireComponent(typeof(CapsuleCollider))]
    public class RigidbodyControllerWrapper : MovementWrapperBase
    {
        #region Fields
        private Rigidbody rb;
        CapsuleCollider collider;
        SensorsModule sensors;
        private Transform m_currentGround;
        private Vector3 m_lastGroundPosition;
        private Quaternion m_lastGroundRotation;
        [SerializeField] float stepHeight;
        [SerializeField] private LayerMask groundLayers = ~0;
        #endregion


        #region Public Properties
        public override Collider Collider => collider;
        public override float StepHeight => stepHeight;
        public override float Height
        {
            get => collider.height;
            set => collider.height = value;
        }
        public override float Radius
        {
            get => collider.radius;
            set => collider.radius = value;
        }
        public override bool IsNoClip { get => !Collider.enabled; set => Collider.enabled = !value; }

        public override bool UseGravity
        {
            get => rb.useGravity;
            set => rb.useGravity = value;
        }

        public override Vector3 Velocity => rb.linearVelocity;
        #endregion

        #region Unity Lifecycle

        private void Awake()
        {
            Setup();
        }
        void Setup()
        {
            rb = GetComponent<Rigidbody>();
            collider = GetComponent<CapsuleCollider>();
            sensors = GetComponent<SensorsModule>();
            rb.freezeRotation = true;
            rb.useGravity = true;
        }

        [SerializeField] float SlopeSlideAcceleration = 10;
        private void FixedUpdate()
        {
           UpdateGroundMotion();
            SlopeHandle();
        }
        public override void SlopeHandle()
        {
            if (!sensors.IsValidSlope() && Velocity.y<0)
            {
                AddAcceleration(Vector3.ProjectOnPlane(-transform.up, sensors.GroundHit.normal) * SlopeSlideAcceleration);
            }
        }
        #endregion
        #region Moving Platform
        private void UpdateGroundMotion()
        {
            if (!sensors.IsGrounded)
            {
                m_currentGround = null;
                return;
            }

            Collider groundCollider = sensors.GroundHit.collider;

            if (groundCollider == null)
            {
                m_currentGround = null;
                return;
            }

            Transform platform = groundCollider.transform;

            // Ignore objects outside the selected layers.
            if ((groundLayers.value & (1 << platform.gameObject.layer)) == 0)
            {
                m_currentGround = null;
                return;
            }

            if (platform == transform || platform.IsChildOf(transform))
            {
                m_currentGround = null;
                return;
            }

            if (m_currentGround != platform)
            {
                m_currentGround = platform;
                m_lastGroundPosition = platform.position;
                m_lastGroundRotation = platform.rotation;
                return;
            }

            Vector3 previousPosition = m_lastGroundPosition;
            Quaternion previousRotation = m_lastGroundRotation;

            Vector3 currentPosition = platform.position;
            Quaternion currentRotation = platform.rotation;

            // Player position relative to the previous platform pose.
            Vector3 relativePosition =
                Quaternion.Inverse(previousRotation) *
                (rb.position - previousPosition);

            // Apply the platform's translation and rotation.
            Vector3 targetPosition =
                currentPosition + currentRotation * relativePosition;

            Vector3 groundDelta = targetPosition - rb.position;

            rb.MovePosition(rb.position + groundDelta);

            m_lastGroundPosition = currentPosition;
            m_lastGroundRotation = currentRotation;
        }


        #endregion
        public override void ChangeHorizontalVelocity(Vector3 a_move)
        {
            rb.linearVelocity = new Vector3(a_move.x, rb.linearVelocity.y, a_move.z);
        }
        public override void ForceMove(Vector3 a_move)
        {
            rb.linearVelocity = a_move;
        }
        public override void ResetVelocity()
        {
            rb.linearVelocity = Vector3.zero;
        }
        public override void SetRotation(Quaternion a_targetRotation)
        {
            rb.MoveRotation(a_targetRotation);
            rb.PublishTransform();
        }
        public override void SetPosition(Vector3 a_position)
        {
            rb.position = a_position;
        }
        public override void AddImpulse(Vector3 force)
        {
            rb.AddForce(force * rb.mass, ForceMode.Impulse);
        }
        public override void ResetGravity()
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                0,
                rb.linearVelocity.z);
        }
        public override void AddAcceleration(Vector3 accelerationForce)
        {
            rb.AddForce(accelerationForce, ForceMode.Acceleration);

        }
        public override void DisableMovement()
        {
        }
        public override void EnableMovement()
        {
        }
        public override void ChangeVerticalVelocity(float velocityY)
        {
            rb.linearVelocity = new Vector3(
                rb.linearVelocity.x,
                velocityY,
                rb.linearVelocity.z);
        }

    }
}