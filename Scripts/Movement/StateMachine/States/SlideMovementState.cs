namespace LOGIYGames.Character.Movement
{
    public class SlideMovementState : MovementStateBase
    {
        public SlideMovementState(Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void PhysicsUpdate()
        {
            //Character.Slide();
        }
        public override void Exit()
        {
            base.Exit();
        }
    }
}
