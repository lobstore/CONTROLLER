namespace LOGIYGames.Character.Movement
{
    public class BracedJumpMovementState : TimedMovementState
    {
        private JumpStateData _jumpStateData;
        JumpController controller;
        public BracedJumpMovementState(Actor ctx, JumpStateData stateData) : base(ctx, stateData)
        {
            _jumpStateData = stateData;
            controller = new BracedJumpController(ctx);
        }

        public override void Enter()
        {
            base.Enter();
            if (Actor.Input.MovementInput.magnitude > 0)
            {
                Motor.UseGravity = false;
            }

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
            Motor.UseGravity = true;
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();
            //Motor.StepOrientation();
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Input.JumpPressed;
        }
    }
}
