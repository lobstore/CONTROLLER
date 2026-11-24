using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    public class BackTurnMovementState : TimedMovementState
    {
        GroundLocomotionController controller;
        TurnMovementStateData TurnData;
        public BackTurnMovementState(Actor ctx, TurnMovementStateData stateData) : base(ctx, stateData)
        {
            TurnData = stateData;
            controller = new GroundLocomotionController(ctx, 20, 10, 3.5f);
        }
        public override void Enter()
        {
            Actor.MovementStrategy = new CharacterForwardMovement(Actor);
            Actor.EventBus.Publish(new BackTurnPerformedEvent
            {
                movementSpeed = Actor.RuntimeMovement.CurrentSpeed,
                angle = Actor.RuntimeMovement.DeltaYaw
            });
            base.Enter();
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
        public override bool CanEnter()
        {
            return base.CanEnter()
                && Mathf.Abs(Actor.RuntimeMovement.DeltaYaw) > TurnData.MinAngle && Actor.RotationStrategy is LookRelativeRotation;
        }
    }

}
