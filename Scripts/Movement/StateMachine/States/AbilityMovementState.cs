namespace LOGIYGames.Character.Movement
{
    public class AbilityMovementState : MovementStateBase
    {
        public AbilityMovementState(Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
        }

        public override void Enter()
        {
            base.Enter();
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
        }
        public override bool CanEnter()
        {
            return base.CanEnter();
        }
        public override bool CanExit()
        {
            return base.CanExit();
        }
    }
}
