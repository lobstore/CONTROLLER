using LOGIYGames.Character;
using UnityEngine;
namespace LOGIYGames.Character.Movement
{
    public class FlyMovementState : MovementStateBase
    {
        public FlyMovementState(Character.Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
        }
        Vector3 dir;
        public override void Enter()
        {
            base.Enter();
            Actor.GetComponent<MovementWrapperBase>().UseGravity = false;
            _animator.SetBool("IsFlying", true);
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!Data.IsAnimationDrivenMovement)
            {
                Motor.ChangeHorizontalVelocity(Actor.RuntimeMovement.TargetDirection * Actor.RuntimeMovement.CurrentSpeed * 3.5f);
                Motor.ChangeVerticalVelocity(Actor.RuntimeMovement.TargetDirection.y * Actor.RuntimeMovement.CurrentSpeed * 3.5f);

            }
            if (!Data.IsAnimationDrivenRotation)
            {
                Actor.Rotate();

            }
        }
        public override bool CanExit()
        {
            return base.CanExit() && !Actor.Input.SubAttackPressed;
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Input.SubAttackPressed;
        }
        public override void Exit()
        {
            base.Exit();
            _animator.SetBool("IsFlying", false);
            Actor.GetComponent<MovementWrapperBase>().UseGravity = true;
        }
    }
}
