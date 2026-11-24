namespace LOGIYGames.Character.Movement
{
    public class IdleMovementState : MovementStateBase
    {
        GroundLocomotionController controller;
        public IdleMovementState(Character.Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
            controller = new GroundLocomotionController(ctx, 50, 10, 3.5f);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!Data.IsAnimationDrivenMovement)
            {
                controller.Move();

            }
            if (!Data.IsAnimationDrivenRotation)
            {
                Actor.Rotate();

            }
        }

        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Input.MovementInput.magnitude == 0 && Actor.Sensors.IsGrounded;
        }
    }
}
