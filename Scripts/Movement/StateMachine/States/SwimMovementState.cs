using LOGIYGames.Character;
using UnityEngine.TextCore.Text;
namespace LOGIYGames.Character.Movement
{
    public class SwimMovementState : MovementStateBase
    {
        public SwimMovementState(Character.Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
        }
        public override bool CanEnter()
        {
            return false;
        }
        public override void Enter()
        {
            base.Enter();
            Actor.GetComponent<MovementWrapperBase>().UseGravity = false;
        }
        public override void Exit()
        {
            base.Exit();
            Actor.GetComponent<MovementWrapperBase>().UseGravity = true;
        }
        public override bool CanExit()
        {
            return true;
        }
    }

}
