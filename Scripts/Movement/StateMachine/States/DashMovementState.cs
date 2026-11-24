namespace LOGIYGames.Character.Movement
{
    public class DashMovementState : TimedMovementState
    {
        private JumpStateData _jumpStateData;
        JumpController controller;
        public DashMovementState(Character.Actor ctx, JumpStateData stateData) : base(ctx, stateData)
        {
            _jumpStateData = stateData;
            controller = new DashJumpController(ctx);
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
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            Actor.Rotate();
        }
        public override void Exit()
        {
            base.Exit();
            controller.Exit();
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Input.SprintPressing;
        }

    }
}
