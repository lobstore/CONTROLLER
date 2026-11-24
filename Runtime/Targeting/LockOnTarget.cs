using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [DisallowMultipleComponent]
    public sealed class LockOnTarget : MonoBehaviour
    {
        [SerializeField] private Transform aimPoint;

        public Transform AimPoint { get { return aimPoint != null ? aimPoint : transform; } }

        private void OnValidate()
        {
            if (aimPoint == null)
                aimPoint = transform;
        }
    }
}