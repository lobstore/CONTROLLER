using LOGIYGames.Character;
using LOGIYGames.Core;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace LOGIYGames
{
    public enum ControlType
    {
        FirstPerson,
        ThirdPersonFreeLook,
        ThirdPersonLookForward,
        Top_Down
    }
    public class PlayerManager : PersistentSingleton<PlayerManager>
    {
        [SerializeField] Actor InitCharacter;
        [SerializeField] InputActionAsset InputActions;

        public UnityEvent<Actor> OnCharacterChanged = new();
        public Actor CurrentCharacter { get; private set; }
        public PlayerInputReader InputReader { get; private set; }
        public UnityEvent OnControlTypeChanged = new();
        [SerializeField] private ControlType currentControlType;

        public ControlType CurrentControlType
        {
            get { return currentControlType; }
            set
            {
                currentControlType = value;
                OnControlTypeChanged.Invoke();
            }
        }
        protected override void Awake()
        {
            base.Awake();

            InputReader = new(InputActions, Camera.main.transform);

        }

        private void OnEnable()
        {
            OnControlTypeChanged.AddListener(UpdateStrategies);
        }

        private void OnDisable()
        {
            OnControlTypeChanged.RemoveListener(UpdateStrategies);
        }
        private void Start()
        {
            SetPlayerControlOnCharacter(InitCharacter);
            InputReader?.Enable();
        }
        private void LateUpdate()
        {
            UpdateStrategies();
        }
        private void UpdateStrategies()
        {
            switch (currentControlType)
            {
                case ControlType.FirstPerson:
                    CurrentCharacter.MovementStrategy = new StrafeMovement(CurrentCharacter);
                    CurrentCharacter.RotationStrategy = new InputPlanarRotation(CurrentCharacter);
                    InputReader = new(InputActions, CurrentCharacter.transform);
                    break;
                case ControlType.ThirdPersonFreeLook:
                    CurrentCharacter.MovementStrategy = new StrafeMovement(CurrentCharacter);
                    CurrentCharacter.RotationStrategy = new LookRelativeRotation(CurrentCharacter);
                    InputReader = new(InputActions, Camera.main.transform);
                    break;
                case ControlType.ThirdPersonLookForward:
                    CurrentCharacter.MovementStrategy = new StrafeMovement(CurrentCharacter);
                    CurrentCharacter.RotationStrategy = new CharacterForwardRotation(CurrentCharacter);
                    InputReader = new(InputActions, Camera.main.transform);
                    break;
                case ControlType.Top_Down:
                    CurrentCharacter.MovementStrategy = new CharacterForwardMovement(CurrentCharacter);
                    CurrentCharacter.RotationStrategy = new LookRelativeRotation(CurrentCharacter);
                    InputReader = new(InputActions, Camera.main.transform);
                    break;
                default:
                    break;
            }
        }
        public void SetPlayerControlOnCharacter(Actor character)
        {
            CurrentCharacter = character;
            CurrentCharacter.SetInput(InputReader);
            OnCharacterChanged?.Invoke(CurrentCharacter);
        }
    }
}
