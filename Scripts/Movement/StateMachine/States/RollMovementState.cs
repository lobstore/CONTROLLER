using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames.Character.Movement
{

    public class RollMovementState : TimedMovementState
    {
        private JumpStateData _jumpStateData;
        JumpController controller;
        public RollMovementState(Character.Actor ctx, JumpStateData stateData) : base(ctx, stateData)
        {
            _jumpStateData = stateData;
            controller = new RollJumpController(ctx);
        }
        Vector3 targetdir;
        void Roll()
        {
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
        public override void Enter()
        {
            base.Enter();
            controller.Enter();
            targetdir = Actor.RuntimeMovement.TargetDirection;
            Roll();
        }
        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!IsActionFrameElapsed)
            {
                if (Actor.Input.MovementInput.magnitude>0)
                {
                    Actor.RotateToDirection(targetdir, 800);

                }
                return;
            }

            if (Actor.Sensors.IsGrounded)
            {
                if (Actor.Input.MovementInput.magnitude > 0)
                {

                    Actor.RotateToDirection(Actor.RuntimeMovement.TargetDirection, Actor.RuntimeMovement.TurnSmoothTime);
                }
                if (Motor.HorizontalVelocity.magnitude < 7f)
                {
                    Motor.AddAcceleration(Actor.transform.forward * 30);
                }
                else
                {
                    Motor.AddAcceleration(-Motor.HorizontalVelocity * 20);
                }
            }


        }
        public override void Exit()
        {
            base.Exit();
            controller.Exit();
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Input.EvadePressed;
        }
    }

}
