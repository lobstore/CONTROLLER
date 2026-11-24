using LOGIYGames.Character;

namespace LOGIYGames.Character.Movement
{
    public class SprintMovementState : MovementStateBase
    {
        GroundLocomotionController controller;
        public SprintMovementState(Character.Actor ctx, MovementStateData stateData) : base(ctx, stateData)
        {
            controller = new GroundLocomotionController(ctx, 50, 10, 3.5f);
        }

        public override void PhysicsUpdate()
        {
            base.PhysicsUpdate();
            if (!Data.IsAnimationDrivenMovement)
            {
                if (Actor.Input.MovementInput.magnitude > 0)
                    controller.Move();

            }
            if (!Data.IsAnimationDrivenRotation)
            {
                Actor.Rotate();

            }
        }

        public override bool CanExit()
        {
            return !Actor.Input.SprintPressing || Actor.Input.MovementInput.magnitude == 0;
        }
        public override bool CanEnter()
        {
            return base.CanEnter() && Actor.Sensors.IsGrounded &&
                        Actor.Input.SprintPressing && Actor.GetInputRelativeDirection() == Shared.Enums.Direction.Forward;
        }
    }

}
