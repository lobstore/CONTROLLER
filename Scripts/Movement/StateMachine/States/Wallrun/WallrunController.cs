using UnityEngine;
namespace LOGIYGames.Character
{
    public class WallrunController
    {
        Vector3 normal;
        MovementWrapperBase Motor;
        Actor Actor;
        float baseSpeed = 5f;
        public bool AffectedByGravity;
        public float gravityMultoplier;
        Vector3 dir;
        public WallrunController(Actor actor)
        {
            Actor = actor;
            Motor = Actor.Motor;
        }
        public void Enter()
        {
            if (!AffectedByGravity)
            {
                Motor.UseGravity = false;

            }
            Actor.EventBus.Publish(new WallrunEnterEvent
            {
                IsRightSide = Actor.Sensors.IsObstacleLegsRight ? true : false
            });
            Actor.GetComponent<Animator>().SetBool("IsWallRunning", true);
            Motor.GravityMultiplier = gravityMultoplier;
            Motor.ChangeVerticalVelocity(0);
        }
        public void Exit()
        {
            Motor.UseGravity = true;
            Motor.GravityMultiplier = 1;
            Actor.GetComponent<Animator>().SetBool("IsWallRunning", false);
        }
        public void LogicUpdate()
        {
            normal = Actor.Sensors.IsObstacleLegsRight ? Actor.Sensors.LegsRightHit.normal : Actor.Sensors.LegsLeftHit.normal;
            Vector3 targetdir;
            if (Actor.Sensors.IsObstacleLegsRight)
            {
                targetdir = Vector3.Cross(-normal, Actor.transform.up).normalized;

            }
            else
            {
                targetdir = Vector3.Cross(normal, Actor.transform.up).normalized;
            }
            dir = Vector3.Lerp(dir, targetdir, Time.deltaTime * 5);
        }
        public void PhysicsUpdate()
        {
            Magnit();
            Rotate();
            Run();
        }

        private void Run()
        {
            Vector3 hor = new Vector3(Motor.Velocity.x, 0, Motor.Velocity.z);
            float maxSpeed = Actor.RuntimeMovement.CurrentSpeed * baseSpeed;

            if (hor.magnitude < maxSpeed)
            {
                Motor.AddAcceleration(dir * 30);
            }
            else
            {
                Motor.AddAcceleration(-hor * 5);
            }

        }

        private void Magnit()
        {
            bool right = Actor.Sensors.IsObstacleLegsRight;
            var dir = right ? -Actor.Sensors.LegsRightHit.normal : -Actor.Sensors.LegsLeftHit.normal;
            Motor.AddAcceleration(dir * 10);
        }
        private void Rotate()
        {
            if (Actor.RotationStrategy is InputPlanarRotation)
            {
                Actor.Rotate();
                return;
            }
            if (Actor.Sensors.IsObstacleLegsRight)
            {

                Actor.RotateToDirection(Vector3.Cross(-normal, Actor.transform.up), Actor.RuntimeMovement.TurnSmoothTime);
            }
            else
            {
                Actor.RotateToDirection(Vector3.Cross(normal, Actor.transform.up), Actor.RuntimeMovement.TurnSmoothTime);
            }
        }
        public bool CanExit()
        {
            return Actor.Sensors.IsObstacleLegsFront || Actor.Input.MovementInput.y == 0 || Actor.Sensors.IsGrounded || !HasLegsSideWallHit();
        }
        public bool CanEnter()
        {
            return HasLegsSideWallHit()
                && !Actor.Sensors.IsGrounded
                && Actor.Input.MovementInput.y > 0
                && !Actor.Sensors.IsObstacleLegsFront
                && Actor.transform.InverseTransformDirection(Motor.Velocity).z > 2
                && Motor.Velocity.y > -4;
        }

        private bool HasLegsSideWallHit()
        {
            return Actor.Sensors.IsObstacleLegsLeft || Actor.Sensors.IsObstacleLegsRight;
        }
    }
}
