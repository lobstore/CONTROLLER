using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// CharacterController-backed motor. Collision/velocity integration lives here
    /// and can be replaced by Rigidbody, navigation or network-authoritative motors.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterMotor : MonoBehaviour, ICharacterMotor
    {
        [SerializeField] private CharacterController characterController;

        private Vector3 _planarVelocity;
        private float _verticalVelocity;
        private bool _controlEnabled = true;

        public Vector3 Position { get { return transform.position; } }
        public Quaternion Rotation { get { return transform.rotation; } }
        public Vector3 Velocity { get { return _planarVelocity + Vector3.up * _verticalVelocity; } }
        public bool IsGrounded { get { return characterController != null && characterController.isGrounded; } }

        private void Awake()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
        }

        public void Tick(
            Vector3 movementDirection,
            bool sprint,
            bool jumpPressed,
            CharacterMovementProfile profile,
            float deltaTime)
        {
            if (!_controlEnabled || profile == null || characterController == null ||
                !characterController.enabled || deltaTime <= 0f)
                return;

            movementDirection = Vector3.ProjectOnPlane(movementDirection, Vector3.up);
            if (movementDirection.sqrMagnitude > 1f)
                movementDirection.Normalize();

            float targetSpeed = sprint ? profile.SprintSpeed : profile.WalkSpeed;
            Vector3 targetVelocity = movementDirection * targetSpeed;
            _planarVelocity = Vector3.MoveTowards(
                _planarVelocity,
                targetVelocity,
                profile.Acceleration * deltaTime);

            if (characterController.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = -2f;

            if (jumpPressed && characterController.isGrounded)
                _verticalVelocity = Mathf.Sqrt(2f * profile.Gravity * profile.JumpHeight);

            _verticalVelocity -= profile.Gravity * deltaTime;

            Vector3 displacement = (_planarVelocity + Vector3.up * _verticalVelocity) * deltaTime;
            characterController.Move(displacement);
        }

        public void SetRotation(Quaternion rotation)
        {
            transform.rotation = rotation;
        }

        public void Teleport(Vector3 position, Quaternion rotation)
        {
            bool wasEnabled = characterController != null && characterController.enabled;
            if (wasEnabled)
                characterController.enabled = false;

            transform.SetPositionAndRotation(position, rotation);
            _planarVelocity = Vector3.zero;
            _verticalVelocity = 0f;

            if (wasEnabled)
                characterController.enabled = true;
        }

        public void SetControlEnabled(bool enabled)
        {
            _controlEnabled = enabled;
            if (!enabled)
            {
                _planarVelocity = Vector3.zero;
                _verticalVelocity = 0f;
            }
        }

        private void OnValidate()
        {
            if (characterController == null)
                characterController = GetComponent<CharacterController>();
        }
    }
}