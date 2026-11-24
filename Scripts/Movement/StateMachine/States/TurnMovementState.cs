using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    public class TurnMovementState : TimedMovementState
    {
        GroundLocomotionController controller;
        TurnMovementStateData TurnData;
        Quaternion targetRotation;
        public TurnMovementState(Actor ctx, TurnMovementStateData stateData) : base(ctx, stateData)
        {
            controller = new GroundLocomotionController(ctx, 50, 30, 3.5f);
            TurnData = stateData;
        }
        public override void Enter()
        {
            Actor.EventBus.Publish(new TurnPerformedEvent
            {
                movementSpeed = Actor.RuntimeMovement.CurrentSpeed,
                angle = Actor.RuntimeMovement.DeltaYaw
            });
            targetRotation = Actor.RuntimeMovement.TargetRotation;
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
                && Actor.RotationStrategy is LookRelativeRotation
                && Mathf.Abs(Actor.RuntimeMovement.DeltaYaw) > TurnData.MinAngle
                && Mathf.Abs(Actor.RuntimeMovement.DeltaYaw) < TurnData.MaxAngle;
        }
    }
}