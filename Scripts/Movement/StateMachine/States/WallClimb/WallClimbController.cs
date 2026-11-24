using LOGIYGames.Character;
using LOGIYGames.Shared.Enums;
using UnityEngine;

namespace LOGIYGames
{
    public class WallClimbController
    {
        float baseSpeed = 2f;
        MovementWrapperBase Motor;
        Character.Actor Actor;

        public WallClimbController(Character.Actor actor)
        {
            Actor = actor;
            Motor = Actor.Motor;
        }

        public void Enter()
        {
            Motor.UseGravity = false;
            Actor.GetComponent<Animator>().SetBool("IsWallClimbing", true);
        }
        public void Exit()
        {
            Motor.UseGravity = true;
            Actor.GetComponent<Animator>().SetBool("IsWallClimbing", false);
            Actor.ResetHeadRotation();
        }
        public void Magnit()
        {
            KeepDistanceFromWall();
            Actor.RotateToDirection(-Actor.Sensors.LegsFrontHit.normal, Actor.RuntimeMovement.TurnSmoothTime);
            Actor.RotateHead(Actor.RuntimeMovement.TargetRotation);
        }

        private void KeepDistanceFromWall()
        {
            Motor.AddAcceleration(-Actor.Sensors.LegsFrontHit.normal * 10);
        }
        public void Climb()
        {
            Vector2 input = Actor.Input.MovementInput;
            var dir = input.x * Actor.transform.right + input.y * Actor.transform.up;
            dir = Vector3.ProjectOnPlane(dir, Actor.Sensors.LegsFrontHit.normal).normalized;
            float maxSpeed = Actor.RuntimeMovement.CurrentSpeed * baseSpeed;

            if (Motor.Velocity.magnitude < maxSpeed)
            {

                Motor.AddAcceleration(dir * 30f);
            }
            else
            {
                Motor.AddAcceleration(-Motor.Velocity * 30f);
            }
        }

        public bool CanEnter()
        {
            return Actor.Sensors.IsObstacleHeadFront && Actor.Sensors.GetPlaneAbsAngle(Actor.Sensors.LegsFrontHit.normal) > Actor.Sensors.MaxStableSlopeAngle;
        }
        public bool CanExit()
        {
            return Actor.Input.InteractStarted ||
                     !Actor.Sensors.IsObstacleHeadFront
                     || Actor.Sensors.GetPlaneAbsAngle(Actor.Sensors.LegsFrontHit.normal) <= Actor.Sensors.MaxStableSlopeAngle
                     || (Actor.Sensors.IsGrounded && Actor.GetInputRelativeDirection() == Direction.Backward);
        }
    }
}
