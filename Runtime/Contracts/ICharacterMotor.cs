using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Physics/movement boundary for a character. Implementations own collision
    /// handling and velocity integration; locomotion only supplies intent.
    /// </summary>
    public interface ICharacterMotor
    {
        Vector3 Position { get; }
        Quaternion Rotation { get; }
        Vector3 Velocity { get; }
        bool IsGrounded { get; }

        void Tick(
            Vector3 movementDirection,
            bool sprint,
            bool jumpPressed,
            CharacterMovementProfile profile,
            float deltaTime);

        void SetRotation(Quaternion rotation);
        void Teleport(Vector3 position, Quaternion rotation);
        void SetControlEnabled(bool enabled);
    }
}