using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    public class FallingController
    {
        private MovementWrapperBase Motor;
        private Character.Actor Actor;

        private float acceleration = 2f;
        private float maxHorizontalSpeed = 8f;

        public FallingController(Character.Actor actor)
        {
            Actor = actor;
            Motor = Actor.Motor;
        }

        public void Glide()
        {
            Vector2 input = Actor.Input.MovementInput;

            if (input.sqrMagnitude < 0.01f)
                return;

            Vector3 direction = Actor.RuntimeMovement.TargetDirection.normalized;

            Motor.AddAcceleration(direction * acceleration);

            Vector3 velocity = Motor.Velocity; // Подставь свой getter скорости
            Vector3 horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);

            if (horizontalVelocity.magnitude > maxHorizontalSpeed)
            {
                horizontalVelocity = horizontalVelocity.normalized * maxHorizontalSpeed;

                velocity.x = horizontalVelocity.x;
                velocity.z = horizontalVelocity.z;

                Motor.ChangeHorizontalVelocity(velocity); // Подставь свой метод установки скорости
            }
        }
    }
}
