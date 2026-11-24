using LOGIYGames.Extensions;
namespace LOGIYGames.Character.Movement
{
    public class MantlingMovementState : MovementStateBase
    {
        #region Fields
        private MantlingController MantlingController;

        #endregion

        public MantlingMovementState(Character.Actor ctx, MantlingMovmentStateData stateData) : base(ctx, stateData)
        {
            MantlingController = ctx.gameObject.GetOrAddComponent<MantlingController>();
        }


        public override void Enter()
        {
            base.Enter();
            Motor.UseGravity = false;
            Motor.IsNoClip = true;
            MantlingController.BeginMantling();
        }
        public override void LateUpdate()
        {
            base.LateUpdate();
            MantlingController.Tick();
        }
        public override void Exit()
        {
            base.Exit();
            MantlingController.Cancel();

            Motor.UseGravity = true;
            Motor.IsNoClip = false;
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && MantlingController.CanEnter();
        }
        public override bool CanExit()
        {
            return MantlingController.CanExit();
        }
    }
}