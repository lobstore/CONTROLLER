using System;
using System.Collections.Generic;
using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [DisallowMultipleComponent]
    public sealed class CharacterDirector : MonoBehaviour
    {
        [Header("Roster")]
        [SerializeField] private CharacterDefinition initialCharacter;
        [SerializeField] private CharacterDefinition[] roster = new CharacterDefinition[0];
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private bool spawnNewCharactersAtCurrentPosition = true;

        [Header("Services")]
        [SerializeField] private MonoBehaviour inputSourceComponent;
        [SerializeField] private MonoBehaviour cameraServiceComponent;
        [SerializeField] private MonoBehaviour targetingServiceComponent;
        [SerializeField] private MonoBehaviour controlGateComponent;

        private readonly Dictionary<CharacterDefinition, CharacterAgent> _instances =
            new Dictionary<CharacterDefinition, CharacterAgent>();

        private ICharacterInputSource _inputSource;
        private ICharacterCameraService _cameraService;
        private ICharacterTargetingService _targetingService;
        private ICharacterControlGate _controlGate;
        private CharacterLocomotionController _locomotion;
        private CharacterAgent _currentCharacter;
        private CharacterInputFrame _lastInput;
        private CharacterFacingMode _facingModeBeforeLockOn;
        private bool _hasFacingModeBeforeLockOn;
        private string _viewIdBeforeLockOn;
        private bool _hasCameraViewBeforeLockOn;

        public CharacterAgent CurrentCharacter { get { return _currentCharacter; } }
        public CharacterFacingMode FacingMode { get { return _locomotion == null ? CharacterFacingMode.MovementDirection : _locomotion.FacingMode; } }
        public Transform CurrentLockOnTarget { get { return _targetingService != null ? _targetingService.CurrentAimPoint : null; } }

        public event Action<CharacterAgent> CharacterChanged;
        public event Action<Transform> LockOnTargetChanged;

        private void Awake()
        {
            if (cameraServiceComponent == null)
                cameraServiceComponent = GetComponent<CharacterCameraController>();
            if (targetingServiceComponent == null)
                targetingServiceComponent = GetComponent<LockOnTargeting>();

            _inputSource = inputSourceComponent as ICharacterInputSource;
            _cameraService = cameraServiceComponent as ICharacterCameraService;
            _targetingService = targetingServiceComponent as ICharacterTargetingService;
            _controlGate = controlGateComponent as ICharacterControlGate;

            ValidateService(inputSourceComponent, _inputSource, nameof(ICharacterInputSource));
            ValidateService(cameraServiceComponent, _cameraService, nameof(ICharacterCameraService));
            ValidateService(targetingServiceComponent, _targetingService, nameof(ICharacterTargetingService));
            ValidateService(controlGateComponent, _controlGate, nameof(ICharacterControlGate));
        }

        private void Start()
        {
            CharacterDefinition startDefinition = initialCharacter;
            if (startDefinition == null && roster != null && roster.Length > 0)
                startDefinition = roster[0];

            if (startDefinition != null)
                SwitchCharacter(startDefinition, false);
        }

        private void Update()
        {
            if (_currentCharacter == null)
                return;

            // Restore previous camera/facing policies if a target is destroyed or unloaded.
            if ((_hasFacingModeBeforeLockOn || _hasCameraViewBeforeLockOn) &&
                _targetingService != null && !_targetingService.HasTarget)
                ClearLockOn();

            bool canControl = _controlGate == null || _controlGate.CanControl(_currentCharacter);
            _lastInput = canControl && _inputSource != null
                ? _inputSource.Read()
                : CharacterInputFrame.Empty;

            if (_lastInput.LockOnPressed)
                ToggleLockOn();

            if (_lastInput.NextTargetPressed)
                CycleLockOnTarget(1);
            else if (_lastInput.PreviousTargetPressed)
                CycleLockOnTarget(-1);

            if (_lastInput.NextCharacterPressed)
                CycleCharacter(1);
            else if (_lastInput.PreviousCharacterPressed)
                CycleCharacter(-1);

            if (_lastInput.NextCameraPressed && _cameraService != null)
                _cameraService.CycleView(1);

            if (canControl)
            {
                Transform cameraTransform = _cameraService != null
                    ? _cameraService.CameraTransform
                    : null;

                _locomotion.Tick(
                    _currentCharacter,
                    _lastInput,
                    cameraTransform,
                    CurrentLockOnTarget,
                    Time.deltaTime);
            }
            else
            {
                // Keep gravity and motor simulation alive, but suppress input,
                // movement intent and automatic facing while restricted.
                CharacterMovementProfile profile = _currentCharacter.Definition.MovementProfile;
                if (profile != null)
                    _currentCharacter.Motor.Tick(Vector3.zero, false, false, profile, Time.deltaTime);
            }
        }

        private void LateUpdate()
        {
            if (_cameraService == null || _currentCharacter == null)
                return;

            _cameraService.Tick(
                _currentCharacter,
                _lastInput,
                CurrentLockOnTarget,
                Time.deltaTime);
        }

        public bool SwitchCharacter(CharacterDefinition definition, bool transferCurrentPosition = true)
        {
            if (definition == null)
                return false;

            if (_currentCharacter != null && _currentCharacter.Definition == definition)
                return true;

            bool wasCreated;
            CharacterAgent nextCharacter = GetOrCreateCharacter(definition, out wasCreated);
            if (nextCharacter == null)
                return false;

            CharacterAgent previousCharacter = _currentCharacter;
            if (wasCreated && previousCharacter != null &&
                transferCurrentPosition && spawnNewCharactersAtCurrentPosition)
            {
                nextCharacter.Motor.Teleport(
                    previousCharacter.Motor.Position,
                    previousCharacter.Motor.Rotation);
            }

            ClearLockOn();

            if (previousCharacter != null)
            {
                previousCharacter.Motor.SetControlEnabled(false);
                previousCharacter.gameObject.SetActive(false);
            }

            nextCharacter.gameObject.SetActive(true);
            nextCharacter.Motor.SetControlEnabled(true);
            _currentCharacter = nextCharacter;
            _locomotion = new CharacterLocomotionController(
                definition.DefaultFacingMode,
                nextCharacter.FacingStrategy);

            if (_cameraService != null)
                _cameraService.SetProfile(definition.CameraProfile);

            CharacterChanged?.Invoke(nextCharacter);
            return true;
        }

        public bool TrySwitchCharacter(CharacterId id)
        {
            if (!id.IsValid || roster == null)
                return false;

            for (int i = 0; i < roster.Length; i++)
            {
                if (roster[i] != null && roster[i].Id == id)
                    return SwitchCharacter(roster[i]);
            }

            return false;
        }

        public void SetFacingMode(CharacterFacingMode mode)
        {
            if (_locomotion == null)
                return;

            _locomotion.SetFacingMode(mode);
            _hasFacingModeBeforeLockOn = false;
        }

        public bool SetCameraView(string viewId)
        {
            if (_cameraService == null || !_cameraService.SetView(viewId))
                return false;

            // An explicit view change during lock-on becomes the new restoration target.
            if (_targetingService != null && _targetingService.HasTarget)
                _hasCameraViewBeforeLockOn = false;

            return true;
        }

        public bool SetLockOnTarget(Transform target)
        {
            if (target == null || _targetingService == null ||
                !_targetingService.SetTarget(target) || !_targetingService.HasTarget)
                return false;

            BeginLockOnPolicies();
            LockOnTargetChanged?.Invoke(CurrentLockOnTarget);
            return true;
        }

        public void ClearLockOn()
        {
            bool hadTarget = _targetingService != null && _targetingService.HasTarget;
            if (_targetingService != null)
                _targetingService.Clear();

            if (_hasFacingModeBeforeLockOn && _locomotion != null)
                _locomotion.SetFacingMode(_facingModeBeforeLockOn);

            if (_hasCameraViewBeforeLockOn && _cameraService != null &&
                !string.IsNullOrWhiteSpace(_viewIdBeforeLockOn))
                _cameraService.SetView(_viewIdBeforeLockOn);

            _hasFacingModeBeforeLockOn = false;
            _hasCameraViewBeforeLockOn = false;
            _viewIdBeforeLockOn = null;

            if (hadTarget)
                LockOnTargetChanged?.Invoke(null);
        }

        public bool CycleCharacter(int direction)
        {
            if (roster == null || roster.Length == 0)
                return false;

            int currentIndex = -1;
            if (_currentCharacter != null)
            {
                for (int i = 0; i < roster.Length; i++)
                {
                    if (roster[i] == _currentCharacter.Definition)
                    {
                        currentIndex = i;
                        break;
                    }
                }
            }

            int step = direction < 0 ? -1 : 1;
            for (int attempt = 0; attempt < roster.Length; attempt++)
            {
                currentIndex = (currentIndex + step + roster.Length) % roster.Length;
                CharacterDefinition candidate = roster[currentIndex];
                if (candidate != null && SwitchCharacter(candidate))
                    return true;
            }

            return false;
        }

        private void ToggleLockOn()
        {
            if (_targetingService == null || _currentCharacter == null)
                return;

            if (_targetingService.HasTarget)
            {
                ClearLockOn();
                return;
            }

            Transform cameraTransform = _cameraService != null
                ? _cameraService.CameraTransform
                : null;
            Vector3 forward = cameraTransform != null
                ? cameraTransform.forward
                : _currentCharacter.Motor.Rotation * Vector3.forward;

            if (_targetingService.TryAcquire(_currentCharacter.CameraPivot, forward))
            {
                BeginLockOnPolicies();
                LockOnTargetChanged?.Invoke(CurrentLockOnTarget);
            }
        }

        private void BeginLockOnPolicies()
        {
            if (!_hasFacingModeBeforeLockOn && _locomotion != null)
            {
                _facingModeBeforeLockOn = _locomotion.FacingMode;
                _hasFacingModeBeforeLockOn = true;
            }

            if (_locomotion != null)
                _locomotion.SetFacingMode(CharacterFacingMode.LockOnTarget);

            if (_cameraService != null)
            {
                if (!_hasCameraViewBeforeLockOn &&
                    !string.IsNullOrWhiteSpace(_cameraService.ActiveViewId))
                {
                    _viewIdBeforeLockOn = _cameraService.ActiveViewId;
                    _hasCameraViewBeforeLockOn = true;
                }

                CharacterCameraProfile profile = _currentCharacter.Definition.CameraProfile;
                if (profile != null && !string.IsNullOrWhiteSpace(profile.LockOnViewId))
                    _cameraService.SetView(profile.LockOnViewId);
            }
        }

        private void CycleLockOnTarget(int direction)
        {
            if (_targetingService == null || !_targetingService.HasTarget ||
                _currentCharacter == null)
                return;

            Transform cameraTransform = _cameraService != null
                ? _cameraService.CameraTransform
                : null;
            Vector3 right = cameraTransform != null
                ? cameraTransform.right
                : _currentCharacter.Motor.Rotation * Vector3.right;

            if (_targetingService.TryCycle(_currentCharacter.CameraPivot, right, direction))
                LockOnTargetChanged?.Invoke(CurrentLockOnTarget);
        }

        private CharacterAgent GetOrCreateCharacter(CharacterDefinition definition, out bool wasCreated)
        {
            wasCreated = false;
            CharacterAgent cached;
            if (_instances.TryGetValue(definition, out cached))
            {
                if (cached != null)
                    return cached;

                _instances.Remove(definition);
            }

            if (definition.Prefab == null)
            {
                Debug.LogError($"Character definition '{definition.name}' has no prefab.", definition);
                return null;
            }

            Vector3 position = spawnPoint != null ? spawnPoint.position : transform.position;
            Quaternion rotation = spawnPoint != null ? spawnPoint.rotation : transform.rotation;
            GameObject instance = Instantiate(definition.Prefab, position, rotation);
            CharacterAgent agent = instance.GetComponent<CharacterAgent>();

            if (agent == null)
            {
                Debug.LogError(
                    $"Character prefab '{definition.Prefab.name}' must contain {nameof(CharacterAgent)}.",
                    definition.Prefab);
                Destroy(instance);
                return null;
            }

            string error;
            if (!agent.Initialize(definition, out error))
            {
                Debug.LogError(error, instance);
                Destroy(instance);
                return null;
            }

            agent.gameObject.SetActive(false);
            _instances.Add(definition, agent);
            wasCreated = true;
            return agent;
        }

        private static void ValidateService<T>(MonoBehaviour component, T service, string interfaceName)
            where T : class
        {
            if (component != null && service == null)
                Debug.LogError($"Assigned service component must implement {interfaceName}.", component);
        }

        private void OnValidate()
        {
            if (cameraServiceComponent == null)
                cameraServiceComponent = GetComponent<CharacterCameraController>();
            if (targetingServiceComponent == null)
                targetingServiceComponent = GetComponent<LockOnTargeting>();

            if (roster == null)
                return;

            HashSet<string> ids = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < roster.Length; i++)
            {
                CharacterDefinition definition = roster[i];
                if (definition == null)
                    continue;

                if (!definition.Id.IsValid)
                {
                    Debug.LogWarning($"Character asset '{definition.name}' has an empty ID.", definition);
                    continue;
                }

                if (!ids.Add(definition.Id.Value))
                    Debug.LogWarning($"Duplicate character ID '{definition.Id.Value}' in roster.", this);
            }
        }
    }
}