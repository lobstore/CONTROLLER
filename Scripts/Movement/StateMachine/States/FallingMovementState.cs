using LOGIYGames.Character;

namespace LOGIYGames.Character.Movement
{
    public class FallingMovementState : MovementStateBase
    {
        FallingController controller;
        public FallingMovementState(Character.Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
            controller = new FallingController(ctx);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            controller.Glide();
            Actor.Rotate();
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && !Actor.Sensors.IsGrounded;
        }
    }

}
