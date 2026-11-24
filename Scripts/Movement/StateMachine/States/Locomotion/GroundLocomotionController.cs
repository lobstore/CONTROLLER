using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    public class GroundLocomotionController
    {
        float baseSpeed;
        float acceleration;
        float deceleration;
        MovementWrapperBase Motor;
        Character.Actor Actor;

        private bool isPhysical = true;
        public GroundLocomotionController(Character.Actor actor, float accel, float  decel, float bSpeed)
        {
            Actor = actor;
            Motor = Actor.Motor;
            baseSpeed = bSpeed;
            acceleration = accel;
            deceleration = decel;
        }
        public void Move()
        {
            Vector3 dir = Actor.RuntimeMovement.TargetDirection;
            var groundNormal = Actor.Sensors.GroundHit.normal;
            Vector3 hor = new Vector3(Motor.Velocity.x, 0, Motor.Velocity.z);
            if (isPhysical)
            {
                if (Actor.Sensors.IsValidSlope())
                {
                    dir = Vector3.ProjectOnPlane(dir, groundNormal);

                    float maxSpeed = Actor.RuntimeMovement.CurrentSpeed * baseSpeed;
                    if (hor.magnitude < maxSpeed)
                    {
                        Motor.AddAcceleration(dir * acceleration);

                    }
                    else
                    {
                        Motor.AddAcceleration(-hor * deceleration);
                    }

                }
            }
            else
            {
                Motor.ChangeHorizontalVelocity(dir * Actor.RuntimeMovement.CurrentSpeed * baseSpeed);
            }


        }

    }
}
