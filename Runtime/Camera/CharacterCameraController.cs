using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Drives physical Unity Camera outputs from data-driven view definitions.
    /// A project can map first/third person projections or individual view IDs
    /// to separate Cameras, or reuse one output. No virtual-camera vendor required.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class CharacterCameraController : MonoBehaviour, ICharacterCameraService
    {
        [SerializeField] private UnityEngine.Camera defaultCamera;
        [SerializeField] private CameraOutputBinding[] cameraOutputs = new CameraOutputBinding[0];

        private CharacterCameraProfile _profile;
        private CameraViewDefinition _activeView;
        private UnityEngine.Camera _activeCamera;
        private float _yaw;
        private float _pitch;
        private bool _orbitInitialized;
        private bool _snapNextFrame = true;

        public UnityEngine.Camera Camera { get { return _activeCamera; } }
        public Transform CameraTransform { get { return _activeCamera != null ? _activeCamera.transform : null; } }
        public CameraViewDefinition ActiveView { get { return _activeView; } }
        public string ActiveViewId { get { return _activeView == null ? string.Empty : _activeView.ViewId; } }

        public event System.Action<CameraViewDefinition> ViewChanged;

        private void Awake()
        {
            if (defaultCamera == null)
                defaultCamera = GetComponent<UnityEngine.Camera>();
            if (defaultCamera == null)
                defaultCamera = UnityEngine.Camera.main;

            _activeCamera = defaultCamera;
            if (cameraOutputs == null)
                return;

            for (int i = 0; i < cameraOutputs.Length; i++)
            {
                CameraOutputBinding binding = cameraOutputs[i];
                if (binding != null && binding.Camera != null && binding.Camera != defaultCamera)
                    SetCameraEnabled(binding.Camera, false);
            }
        }

        public void SetProfile(CharacterCameraProfile profile)
        {
            _profile = profile;
            _orbitInitialized = false;
            _snapNextFrame = true;

            if (_profile == null)
            {
                _activeView = null;
                return;
            }

            SetView(_profile.GetInitialView());
        }

        public bool SetView(string viewId)
        {
            if (_profile == null)
                return false;

            return SetView(_profile.GetView(viewId));
        }

        public bool SetView(CameraViewDefinition view)
        {
            if (view == null)
                return false;

            _activeView = view;
            SelectOutput(view);
            if (_activeCamera != null)
                _activeCamera.fieldOfView = view.FieldOfView;

            _snapNextFrame = true;
            ViewChanged?.Invoke(view);
            return true;
        }

        public bool CycleView(int direction)
        {
            if (_profile == null || _profile.ViewCount == 0)
                return false;

            int currentIndex = -1;
            for (int i = 0; i < _profile.ViewCount; i++)
            {
                CameraViewDefinition candidate = _profile.GetViewAt(i);
                if (candidate == _activeView)
                {
                    currentIndex = i;
                    break;
                }
            }

            int step = direction < 0 ? -1 : 1;
            for (int attempt = 0; attempt < _profile.ViewCount; attempt++)
            {
                currentIndex = (currentIndex + step + _profile.ViewCount) % _profile.ViewCount;
                CameraViewDefinition candidate = _profile.GetViewAt(currentIndex);
                if (candidate != null)
                    return SetView(candidate);
            }

            return false;
        }

        public void Tick(
            CharacterAgent character,
            CharacterInputFrame input,
            Transform lockOnAimPoint,
            float deltaTime)
        {
            if (_activeCamera == null || character == null ||
                character.Motor == null || _activeView == null)
                return;

            CameraViewDefinition view = _activeView;
            if (!_orbitInitialized)
            {
                _yaw = character.Motor.Rotation.eulerAngles.y;
                _pitch = view.InitialPitch;
                _orbitInitialized = true;
                _snapNextFrame = true;
            }

            if (view.AllowLook &&
                (view.Projection == CameraProjectionMode.FirstPerson || view.OrbitWhenThirdPerson))
            {
                _yaw += input.Look.x * view.LookSensitivity;
                _pitch -= input.Look.y * view.LookSensitivity;
                _pitch = Mathf.Clamp(_pitch, view.MinimumPitch, view.MaximumPitch);
            }
            else if (view.Projection == CameraProjectionMode.ThirdPerson && !view.OrbitWhenThirdPerson)
            {
                _yaw = character.Motor.Rotation.eulerAngles.y;
            }

            Transform cameraTransform = _activeCamera.transform;
            if (view.Projection == CameraProjectionMode.FirstPerson)
            {
                TickFirstPerson(character, view, cameraTransform, deltaTime);
                return;
            }

            TickThirdPerson(character, view, cameraTransform, lockOnAimPoint, deltaTime);
        }

        private void SelectOutput(CameraViewDefinition view)
        {
            UnityEngine.Camera selected = null;
            if (cameraOutputs != null)
            {
                // Exact view binding wins over generic projection binding.
                for (int i = 0; i < cameraOutputs.Length; i++)
                {
                    CameraOutputBinding binding = cameraOutputs[i];
                    if (binding != null && binding.Camera != null &&
                        !string.IsNullOrWhiteSpace(binding.ViewId) &&
                        string.Equals(binding.ViewId, view.ViewId, System.StringComparison.Ordinal))
                    {
                        selected = binding.Camera;
                        break;
                    }
                }

                if (selected == null)
                {
                    for (int i = 0; i < cameraOutputs.Length; i++)
                    {
                        CameraOutputBinding binding = cameraOutputs[i];
                        if (binding != null && binding.Camera != null &&
                            string.IsNullOrWhiteSpace(binding.ViewId) &&
                            binding.Projection == view.Projection)
                        {
                            selected = binding.Camera;
                            break;
                        }
                    }
                }
            }

            if (selected == null)
                selected = defaultCamera;

            if (selected == null)
            {
                Debug.LogError("No Camera output is configured for view '" + view.ViewId + "'.", this);
                return;
            }

            if (_activeCamera != null && _activeCamera != selected)
                SetCameraEnabled(_activeCamera, false);

            _activeCamera = selected;
            SetCameraEnabled(_activeCamera, true);
        }

        private static void SetCameraEnabled(UnityEngine.Camera camera, bool enabled)
        {
            if (camera == null)
                return;

            camera.enabled = enabled;
            AudioListener listener = camera.GetComponent<AudioListener>();
            if (listener != null)
                listener.enabled = enabled;
        }

        private void TickFirstPerson(
            CharacterAgent character,
            CameraViewDefinition view,
            Transform cameraTransform,
            float deltaTime)
        {
            Transform anchor = character.FirstPersonAnchor;
            Vector3 desiredPosition = anchor.position;
            Quaternion desiredRotation = Quaternion.Euler(_pitch, _yaw, 0f);

            ApplyPose(cameraTransform, desiredPosition, desiredRotation, view.FollowSharpness, deltaTime);
        }

        private void TickThirdPerson(
            CharacterAgent character,
            CameraViewDefinition view,
            Transform cameraTransform,
            Transform lockOnAimPoint,
            float deltaTime)
        {
            Transform pivotTransform = character.CameraPivot;
            Vector3 pivot = pivotTransform.position + pivotTransform.rotation * view.PivotOffset;
            Vector3 focusPoint = pivot;

            if (lockOnAimPoint != null)
            {
                Vector3 lockDirection = lockOnAimPoint.position - pivot;
                if (lockDirection.sqrMagnitude > 0.0001f)
                {
                    Quaternion lockRotation = Quaternion.LookRotation(lockDirection.normalized, Vector3.up);
                    Vector3 angles = lockRotation.eulerAngles;
                    _yaw = angles.y;
                    _pitch = NormalizeAngle(angles.x);
                    _pitch = Mathf.Clamp(_pitch, view.MinimumPitch, view.MaximumPitch);
                }

                focusPoint = lockOnAimPoint.position;
            }

            Quaternion orbitRotation = Quaternion.Euler(_pitch, _yaw, 0f);
            Vector3 direction = orbitRotation * Vector3.forward;
            Vector3 shoulder = orbitRotation * view.ShoulderOffset;
            Vector3 desiredPosition = pivot + shoulder - direction * view.Distance;

            Vector3 castDirection = desiredPosition - pivot;
            float castDistance = castDirection.magnitude;
            if (view.CollisionRadius > 0f && castDistance > 0.001f &&
                Physics.SphereCast(
                    pivot,
                    view.CollisionRadius,
                    castDirection / castDistance,
                    out RaycastHit hit,
                    castDistance,
                    view.CollisionMask,
                    QueryTriggerInteraction.Ignore))
            {
                desiredPosition = pivot + castDirection.normalized * Mathf.Max(0.15f, hit.distance - 0.06f);
            }

            Quaternion desiredRotation = Quaternion.LookRotation(
                focusPoint - desiredPosition,
                Vector3.up);

            ApplyPose(cameraTransform, desiredPosition, desiredRotation, view.FollowSharpness, deltaTime);
        }

        private void ApplyPose(
            Transform target,
            Vector3 position,
            Quaternion rotation,
            float sharpness,
            float deltaTime)
        {
            if (_snapNextFrame || sharpness <= 0f)
            {
                target.SetPositionAndRotation(position, rotation);
                _snapNextFrame = false;
                return;
            }

            float blend = 1f - Mathf.Exp(-sharpness * Mathf.Max(0f, deltaTime));
            target.position = Vector3.Lerp(target.position, position, blend);
            target.rotation = Quaternion.Slerp(target.rotation, rotation, blend);
        }

        private static float NormalizeAngle(float angle)
        {
            if (angle > 180f)
                angle -= 360f;
            return angle;
        }
    }
}