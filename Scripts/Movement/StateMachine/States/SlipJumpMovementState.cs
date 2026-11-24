namespace LOGIYGames.Character.Movement
{
    public class SlipJumpMovementState : TimedMovementState
    {
        private JumpStateData _jumpStateData;
        JumpController controller;
        public SlipJumpMovementState(Character.Actor ctx, JumpStateData stateData) : base(ctx, stateData)
        {
            _jumpStateData = stateData;
            controller = new SlipJumpController(ctx);
        }
        public override void Enter()
        {
            base.Enter();
            controller.Enter();
            float verticalForce;
            float planarForce;
            if (!Data.IsAnimationDrivenMovement)
            {
                verticalForce = _jumpStateData.VerticalJumpForce;
                planarForce = _jumpStateData.PlanarJumpForce;
            }
            else
            {
                verticalForce = 0;
                planarForce = 0;
            }
            controller.Jump(verticalForce, planarForce);
        }
        public override void Exit()
        {
            base.Exit();
            controller.Exit();
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Input.CrouchPressed && Actor.Input.MovementInput.magnitude > 0;
        }

    }

}
