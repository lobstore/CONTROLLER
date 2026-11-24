using LOGIYGames.Character;

namespace LOGIYGames.Character.Movement
{
    public class CrouchMovementState : MovementStateBase
    {

        GroundLocomotionController controller;

        public CrouchMovementState(Character.Actor ctx, MovementStateData stateData) : base(ctx, stateData)
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
        public override void Enter()
        {
            base.Enter();
            _animator.SetBool("IsCrouching", true);

        }

        public override void Exit()
        {
            base.Exit();
            _animator.SetBool("IsCrouching", false);
        }
        public override bool CanEnter()
        {
            return base.CanEnter()
                && Actor.Input.CrouchPressing;

        }
        public override bool CanExit()
        {
            return base.CanExit()
                && !Actor.Input.CrouchPressing;
        }
    }

}
