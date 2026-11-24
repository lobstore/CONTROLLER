using UnityEngine;

namespace LOGIYGames.CharacterController
{
    [CreateAssetMenu(
        fileName = "CharacterMovementProfile",
        menuName = "LOGIY Games/Character Controller/Movement Profile")]
    public sealed class CharacterMovementProfile : ScriptableObject
    {
        [SerializeField, Min(0f)] private float walkSpeed = 4f;
        [SerializeField, Min(0f)] private float sprintSpeed = 7f;
        [SerializeField, Min(0f)] private float acceleration = 24f;
        [SerializeField, Min(0f)] private float gravity = 24f;
        [SerializeField, Min(0f)] private float jumpHeight = 1.25f;
        [SerializeField, Min(0f)] private float rotationSharpness = 18f;

        public float WalkSpeed { get { return walkSpeed; } }
        public float SprintSpeed { get { return sprintSpeed; } }
        public float Acceleration { get { return acceleration; } }
        public float Gravity { get { return gravity; } }
        public float JumpHeight { get { return jumpHeight; } }
        public float RotationSharpness { get { return rotationSharpness; } }
    }
}