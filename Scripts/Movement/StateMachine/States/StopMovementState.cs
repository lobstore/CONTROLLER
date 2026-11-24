using LOGIYGames.Shared.Enums;

namespace LOGIYGames.Character.Movement
{
    public class StopMovementState : TimedMovementState
    {
        public StopMovementState(Actor ctx, TimedMovementStateData stateData) : base(ctx, stateData) { }
        public override void Enter()
        {
            Direction dir = Actor.GetRelativeMovementDirection();
            Actor.EventBus.Publish(new MovementStoppedEvent
            {
                movementSpeed = Actor.RuntimeMovement.CurrentSpeed,
                direction = dir,
            });
            base.Enter();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Actor.Rotate();
        }
        public override bool CanEnter()
        {
            return base.CanEnter()
                && Actor.Input.MovementInput.magnitude == 0 && Actor.RotationStrategy is not InputPlanarRotation;
        }
    }
}