using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Converts player intent into world-space motor commands and applies the
    /// selected character-facing policy. It has no knowledge of input devices.
    /// </summary>
    public sealed class CharacterLocomotionController
    {
        public CharacterFacingMode FacingMode { get; private set; }
        private readonly ICharacterFacingStrategy _facingStrategy;

        public CharacterLocomotionController(
            CharacterFacingMode initialFacingMode,
            ICharacterFacingStrategy facingStrategy = null)
        {
            FacingMode = initialFacingMode;
            _facingStrategy = facingStrategy;
        }

        public void SetFacingMode(CharacterFacingMode mode)
        {
            FacingMode = mode;
        }

        public void Tick(
            CharacterAgent character,
            CharacterInputFrame input,
            Transform cameraTransform,
            Transform lockOnTarget,
            float deltaTime)
        {
            if (character == null || character.Motor == null || character.Definition == null)
                return;

            CharacterMovementProfile profile = character.Definition.MovementProfile;
            if (profile == null)
                return;

            Vector3 referenceForward = cameraTransform != null
                ? cameraTransform.forward
                : character.Motor.Rotation * Vector3.forward;
            Vector3 referenceRight = cameraTransform != null
                ? cameraTransform.right
                : character.Motor.Rotation * Vector3.right;

            Vector3 movementDirection = BuildMoveDirection(input.Move, referenceForward, referenceRight);
            character.Motor.Tick(movementDirection, input.SprintHeld, input.JumpPressed, profile, deltaTime);

            Vector3 lockDirection = lockOnTarget != null
                ? lockOnTarget.position - character.Motor.Position
                : Vector3.zero;

            CharacterFacingContext context = new CharacterFacingContext
            {
                Mode = FacingMode,
                MovementDirection = movementDirection,
                CameraForward = referenceForward,
                LockOnDirection = lockDirection,
                CurrentForward = character.Motor.Rotation * Vector3.forward
            };

            Vector3 facingDirection = _facingStrategy != null
                ? _facingStrategy.GetFacingDirection(context)
                : ResolveFacingDirection(
                    context.Mode,
                    context.MovementDirection,
                    context.CameraForward,
                    context.LockOnDirection,
                    context.CurrentForward);

            facingDirection = Vector3.ProjectOnPlane(facingDirection, Vector3.up);
            if (facingDirection.sqrMagnitude > 0.0001f)
            {
                Quaternion targetRotation = Quaternion.LookRotation(facingDirection.normalized, Vector3.up);
                float blend = profile.RotationSharpness <= 0f
                    ? 1f
                    : 1f - Mathf.Exp(-profile.RotationSharpness * deltaTime);

                character.Motor.SetRotation(
                    Quaternion.Slerp(character.Motor.Rotation, targetRotation, blend));
            }

            if (character.Animation != null)
            {
                float maxSpeed = Mathf.Max(profile.SprintSpeed, 0.01f);
                float normalizedSpeed = Vector3.ProjectOnPlane(character.Motor.Velocity, Vector3.up).magnitude / maxSpeed;
                character.Animation.Present(
                    normalizedSpeed,
                    character.Motor.IsGrounded,
                    character.Motor.Velocity.y,
                    input.SprintHeld,
                    deltaTime);
            }
        }

        public static Vector3 BuildMoveDirection(Vector2 input, Vector3 cameraForward, Vector3 cameraRight)
        {
            Vector3 forward = Vector3.ProjectOnPlane(cameraForward, Vector3.up);
            Vector3 right = Vector3.ProjectOnPlane(cameraRight, Vector3.up);

            if (forward.sqrMagnitude < 0.0001f)
                forward = Vector3.forward;
            if (right.sqrMagnitude < 0.0001f)
                right = Vector3.right;

            forward.Normalize();
            right.Normalize();

            Vector3 direction = forward * input.y + right * input.x;
            return direction.sqrMagnitude > 1f ? direction.normalized : direction;
        }

        public static Vector3 ResolveFacingDirection(
            CharacterFacingMode mode,
            Vector3 movementDirection,
            Vector3 cameraForward,
            Vector3 lockOnDirection,
            Vector3 currentForward)
        {
            Vector3 selected;
            switch (mode)
            {
                case CharacterFacingMode.MovementDirection:
                    selected = movementDirection;
                    break;
                case CharacterFacingMode.CameraForward:
                    selected = cameraForward;
                    break;
                case CharacterFacingMode.LockOnTarget:
                    selected = lockOnDirection;
                    break;
                case CharacterFacingMode.KeepCurrent:
                default:
                    selected = Vector3.zero;
                    break;
            }

            selected = Vector3.ProjectOnPlane(selected, Vector3.up);
            if (selected.sqrMagnitude > 0.0001f)
                return selected.normalized;

            if (mode == CharacterFacingMode.LockOnTarget ||
                mode == CharacterFacingMode.CameraForward)
            {
                Vector3 fallback = Vector3.ProjectOnPlane(currentForward, Vector3.up);
                return fallback.sqrMagnitude > 0.0001f ? fallback.normalized : Vector3.zero;
            }

            return Vector3.zero;
        }
    }
}