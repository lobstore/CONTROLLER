using LOGIYGames.CharacterController;
using UnityEngine;
using UnityEngine.InputSystem;

namespace LOGIYGames.CharacterController.InputSystem
{
    /// <summary>
    /// Optional adapter for Unity's Input System. Actions are not enabled or
    /// disabled here: action-map lifetime remains with PlayerInput or the game.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class InputActionCharacterInputSource : MonoBehaviour, ICharacterInputSource
    {
        [Header("Axes")]
        [SerializeField] private InputActionReference move;
        [SerializeField] private InputActionReference look;
        [SerializeField] private bool lookUsesDelta = true;
        [SerializeField, Min(0f)] private float mouseDeltaScale = 0.1f;
        [SerializeField, Min(0f)] private float stickDegreesPerSecond = 180f;

        [Header("Character")]
        [SerializeField] private InputActionReference jump;
        [SerializeField] private InputActionReference sprint;
        [SerializeField] private InputActionReference nextCharacter;
        [SerializeField] private InputActionReference previousCharacter;

        [Header("Camera and Targeting")]
        [SerializeField] private InputActionReference lockOn;
        [SerializeField] private InputActionReference nextTarget;
        [SerializeField] private InputActionReference previousTarget;
        [SerializeField] private InputActionReference nextCamera;

        public CharacterInputFrame Read()
        {
            CharacterInputFrame frame = CharacterInputFrame.Empty;
            frame.Move = ReadVector2(move);
            frame.Look = ReadLook();
            frame.JumpPressed = WasPressed(jump);
            frame.SprintHeld = IsPressed(sprint);
            frame.LockOnPressed = WasPressed(lockOn);
            frame.NextTargetPressed = WasPressed(nextTarget);
            frame.PreviousTargetPressed = WasPressed(previousTarget);
            frame.NextCharacterPressed = WasPressed(nextCharacter);
            frame.PreviousCharacterPressed = WasPressed(previousCharacter);
            frame.NextCameraPressed = WasPressed(nextCamera);
            return frame;
        }

        private Vector2 ReadLook()
        {
            Vector2 value = ReadVector2(look);
            return lookUsesDelta
                ? value * mouseDeltaScale
                : value * stickDegreesPerSecond * Time.unscaledDeltaTime;
        }

        private static Vector2 ReadVector2(InputActionReference reference)
        {
            return reference != null && reference.action != null
                ? reference.action.ReadValue<Vector2>()
                : Vector2.zero;
        }

        private static bool WasPressed(InputActionReference reference)
        {
            return reference != null && reference.action != null &&
                   reference.action.WasPressedThisFrame();
        }

        private static bool IsPressed(InputActionReference reference)
        {
            return reference != null && reference.action != null &&
                   reference.action.IsPressed();
        }
    }
}