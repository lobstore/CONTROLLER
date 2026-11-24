using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [CreateAssetMenu(
        fileName = "CameraView",
        menuName = "LOGIY Games/Character Controller/Camera View")]
    public sealed class CameraViewDefinition : ScriptableObject
    {
        [SerializeField] private string viewId = "ThirdPerson";
        [SerializeField] private CameraProjectionMode projection = CameraProjectionMode.ThirdPerson;
        [SerializeField] private bool allowLook = true;
        [SerializeField] private bool orbitWhenThirdPerson = true;
        [SerializeField, Min(0.1f)] private float distance = 4f;
        [SerializeField] private Vector3 pivotOffset = new Vector3(0f, 1.45f, 0f);
        [SerializeField] private Vector3 shoulderOffset = new Vector3(0.45f, 0.1f, 0f);
        [SerializeField, Range(1f, 179f)] private float fieldOfView = 60f;
        [SerializeField, Min(0f)] private float lookSensitivity = 2.5f;
        [SerializeField, Range(-89f, 0f)] private float minimumPitch = -35f;
        [SerializeField, Range(0f, 89f)] private float maximumPitch = 70f;
        [SerializeField, Range(-89f, 89f)] private float initialPitch = 12f;
        [SerializeField, Min(0f)] private float followSharpness = 18f;
        [SerializeField, Min(0f)] private float collisionRadius = 0.2f;
        [SerializeField] private LayerMask collisionMask = Physics.DefaultRaycastLayers;

        public string ViewId { get { return viewId; } }
        public CameraProjectionMode Projection { get { return projection; } }
        public bool AllowLook { get { return allowLook; } }
        public bool OrbitWhenThirdPerson { get { return orbitWhenThirdPerson; } }
        public float Distance { get { return distance; } }
        public Vector3 PivotOffset { get { return pivotOffset; } }
        public Vector3 ShoulderOffset { get { return shoulderOffset; } }
        public float FieldOfView { get { return fieldOfView; } }
        public float LookSensitivity { get { return lookSensitivity; } }
        public float MinimumPitch { get { return minimumPitch; } }
        public float MaximumPitch { get { return maximumPitch; } }
        public float InitialPitch { get { return initialPitch; } }
        public float FollowSharpness { get { return followSharpness; } }
        public float CollisionRadius { get { return collisionRadius; } }
        public LayerMask CollisionMask { get { return collisionMask; } }

        private void OnValidate()
        {
            if (string.IsNullOrWhiteSpace(viewId))
                viewId = name.Replace(" ", string.Empty);

            distance = Mathf.Max(0.1f, distance);
            minimumPitch = Mathf.Clamp(minimumPitch, -89f, 0f);
            maximumPitch = Mathf.Clamp(maximumPitch, 0f, 89f);
            initialPitch = Mathf.Clamp(initialPitch, minimumPitch, maximumPitch);
            fieldOfView = Mathf.Clamp(fieldOfView, 1f, 179f);
        }
    }
}