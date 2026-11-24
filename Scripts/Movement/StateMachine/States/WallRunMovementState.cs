using LOGIYGames.Character;
namespace LOGIYGames.Character.Movement
{
    public class WallRunMovementState : MovementStateBase
    {
        WallrunController controller;
        public WallRunMovementState(Character.Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
            controller = new WallrunController(ctx);
        }
        public override void Enter()
        {
            base.Enter();
            controller.Enter();
        }
        public override void LogicUpdate()
        {
            base.LogicUpdate();

            controller.LogicUpdate();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            controller.PhysicsUpdate();
        }
        public override void Exit()
        {
            base.Exit();
            controller.Exit();
        }
        public override bool CanExit()
        {
            return base.CanExit() && controller.CanExit();
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && controller.CanEnter();
        }
    }
}
