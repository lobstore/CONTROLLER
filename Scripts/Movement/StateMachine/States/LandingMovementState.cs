using LOGIYGames.Character;
using LOGIYGames.Shared.Enums;

namespace LOGIYGames.Character.Movement
{
    public class LandingMovementState : TimedMovementState
    {
        GroundLocomotionController Lcontroller;
        MovementWrapperBase controller;
        public LandingMovementState(Character.Actor ctx, TimedMovementStateData stateData) : base(ctx, stateData)
        {
            controller = ctx.GetComponent<MovementWrapperBase>();
            Lcontroller = new GroundLocomotionController(ctx, 20, 10, 3.5f);
        }

        public override void Enter()
        {
            base.Enter();
            //_durationTimer.Reset(MathF.Abs(Motor.LastGroundedReport.GroundedVelocity.y) / 10);
            Direction dir = Actor.GetRelativeMovementDirection();
            Actor.EventBus.Publish(new LandedEvent
            {
                horizontalDirection = dir,
                fallingSpeed = controller.LastGroundedReport.GroundedVelocity.y
            });
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();

            if (!Data.IsAnimationDrivenRotation)
            {
                Actor.Rotate();

            }
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Sensors.IsGrounded && Actor.RotationStrategy is not InputPlanarRotation;
        }
    }

}
