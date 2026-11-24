using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Allocation-free nearby target query with explicit acquisition and cycling.
    /// Put lockable targets on layers included by targetLayers.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class LockOnTargeting : MonoBehaviour, ICharacterTargetingService
    {
        [SerializeField, Min(0.5f)] private float detectionRadius = 18f;
        [SerializeField, Range(1f, 180f)] private float maximumAcquisitionAngle = 110f;
        [SerializeField] private LayerMask targetLayers = Physics.DefaultRaycastLayers;

        private readonly Collider[] _overlapBuffer = new Collider[64];

        public LockOnTarget CurrentTarget { get; private set; }
        public Transform CurrentAimPoint
        {
            get { return CurrentTarget != null ? CurrentTarget.AimPoint : null; }
        }
        public bool HasTarget { get { return CurrentTarget != null; } }

        public bool TryAcquire(Transform observer, Vector3 viewForward)
        {
            if (observer == null)
                return false;

            int count = Physics.OverlapSphereNonAlloc(
                observer.position,
                detectionRadius,
                _overlapBuffer,
                targetLayers,
                QueryTriggerInteraction.Ignore);

            LockOnTarget bestTarget = null;
            float bestScore = float.MaxValue;
            Vector3 forward = Vector3.ProjectOnPlane(viewForward, Vector3.up);
            if (forward.sqrMagnitude < 0.0001f)
                forward = observer.forward;
            forward.Normalize();

            for (int i = 0; i < count; i++)
            {
                Collider candidateCollider = _overlapBuffer[i];
                if (candidateCollider == null)
                    continue;

                LockOnTarget candidate = candidateCollider.GetComponentInParent<LockOnTarget>();
                if (!IsValidCandidate(candidate, observer))
                    continue;

                Vector3 toTarget = candidate.AimPoint.position - observer.position;
                Vector3 planarDirection = Vector3.ProjectOnPlane(toTarget, Vector3.up);
                if (planarDirection.sqrMagnitude < 0.0001f)
                    continue;

                float angle = Vector3.Angle(forward, planarDirection);
                if (angle > maximumAcquisitionAngle)
                    continue;

                float score = angle * 2f + toTarget.magnitude;
                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = candidate;
                }
            }

            Clear();
            CurrentTarget = bestTarget;
            return CurrentTarget != null;
        }

        public bool TryCycle(Transform observer, Vector3 cameraRight, int direction)
        {
            if (observer == null || CurrentTarget == null)
                return false;

            int count = Physics.OverlapSphereNonAlloc(
                observer.position,
                detectionRadius,
                _overlapBuffer,
                targetLayers,
                QueryTriggerInteraction.Ignore);

            Vector3 side = Vector3.ProjectOnPlane(cameraRight, Vector3.up);
            if (side.sqrMagnitude < 0.0001f)
                side = observer.right;
            side.Normalize();

            LockOnTarget bestTarget = null;
            float bestScore = float.MaxValue;

            for (int i = 0; i < count; i++)
            {
                Collider candidateCollider = _overlapBuffer[i];
                if (candidateCollider == null)
                    continue;

                LockOnTarget candidate = candidateCollider.GetComponentInParent<LockOnTarget>();
                if (!IsValidCandidate(candidate, observer) || candidate == CurrentTarget)
                    continue;

                Vector3 toTarget = Vector3.ProjectOnPlane(
                    candidate.AimPoint.position - observer.position,
                    Vector3.up);
                float sideAmount = Vector3.Dot(toTarget.normalized, side);
                if (direction >= 0 && sideAmount <= 0.05f)
                    continue;
                if (direction < 0 && sideAmount >= -0.05f)
                    continue;

                float score = toTarget.magnitude / Mathf.Max(0.1f, Mathf.Abs(sideAmount));
                if (score < bestScore)
                {
                    bestScore = score;
                    bestTarget = candidate;
                }
            }

            if (bestTarget == null)
                return false;

            CurrentTarget = bestTarget;
            return true;
        }

        public bool SetTarget(Transform target)
        {
            if (target == null)
            {
                Clear();
                return true;
            }

            LockOnTarget lockable = target.GetComponentInParent<LockOnTarget>();
            if (lockable == null || !lockable.isActiveAndEnabled)
                return false;

            CurrentTarget = lockable;
            return true;
        }

        public void Clear()
        {
            CurrentTarget = null;
        }

        private static bool IsValidCandidate(LockOnTarget candidate, Transform observer)
        {
            return candidate != null &&
                   candidate.isActiveAndEnabled &&
                   candidate.AimPoint != null &&
                   candidate.transform != observer &&
                   !candidate.transform.IsChildOf(observer);
        }
    }
}