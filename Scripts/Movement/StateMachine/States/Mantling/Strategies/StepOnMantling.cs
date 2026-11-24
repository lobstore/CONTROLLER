using LOGIYGames.Character;
using LOGIYGames.Shared.Enums;
using UnityEngine;

namespace LOGIYGames
{
    public class StepOnMantling : MantlingStrategy
    {

        private float obstacleHeight;

        public StepOnMantling(Character.Actor chr, MantlingData data) : base(chr, data)
        {
        }

        public override bool CanEnter()
        {
            if (!HasClearPathToRayOrigin())
                return false;

            TargetTopPoint = PerformDetectionTopDown(checkDistance);

            if (TargetTopPoint.collider != null)
                obstacleHeight = CalculateObstacleHeight();
            else
                obstacleHeight = 0;

            if (!HasEnoughSpace())
                return false;

            return obstacleHeight > Motor.StepHeight &&
                   obstacleHeight <= Motor.Height*0.5f;
        }

        public override bool CanExit()
        {
            return Duration.IsFinished;
        }
        public override void Enter()
        {
            MantlingType = ChooseMantlingType();
            Duration.Start();
            MantleStartPosition = Actor.transform.position;

            MantleTargetTransform = TargetTopPoint.collider != null
                ? TargetTopPoint.collider.transform
                : null;

            if (MantleTargetTransform != null)
            {
                MantleTargetLocalPoint =
                    MantleTargetTransform.InverseTransformPoint(TargetTopPoint.point);

                MantleTargetPosition = TargetTopPoint.point;
            }
            else
            {
                MantleTargetPosition = TargetTopPoint.point;
            }
        }
        public override void Exit()
        {
            Motor.SetPosition(MantleTargetPosition);
        }
        public override void Tick()
        {
            if (MantleTargetTransform != null)
            {
                MantleTargetPosition =
                    MantleTargetTransform.TransformPoint(MantleTargetLocalPoint);
            }
            var target = Vector3.Lerp(
                 MantleStartPosition,
                 MantleTargetPosition,
                 Duration.Progress);
            Motor.SetPosition(target);

        }

        private float CalculateObstacleHeight()
        {
            return TargetTopPoint.point.y - Actor.transform.position.y;
        }

        private Vector3 GetTopDownRayOrigin(float forwardDistance)
        {
            return Actor.transform.position +
                   Actor.transform.forward * (Motor.Radius + forwardDistance) +
                   Actor.transform.up * (Motor.Height*0.5f);
        }
        private bool HasClearPathToRayOrigin()
        {
            Vector3 rayOrigin = GetTopDownRayOrigin(checkDistance);

            float radius = Motor.Radius;
            float height = Motor.Height;

            float yOffset =
                rayOrigin.y - Motor.transform.position.y;

            Vector3 bottom =
                Motor.transform.position +
                Motor.transform.up * (radius + yOffset);

            Vector3 top =
                bottom +
               Motor.transform.up * (height - radius * 2f);

            Debug.DrawLine(bottom, top, Color.yellow);
            Debug.DrawRay(
                (bottom + top) * 0.5f,
               Motor.transform.forward * checkDistance,
                Color.cyan);

            return !Physics.CapsuleCast(
                bottom,
                top,
                radius,
                Motor.transform.forward,
                out _,
                checkDistance,
                mantlingLayers,
                QueryTriggerInteraction.Ignore);
        }
        private bool HasEnoughSpace()
        {
            float radius = Motor.Radius;
            float height = Motor.Height;

            Vector3 bottom =
                TargetTopPoint.point +
                Motor.transform.forward * radius +
                Motor.transform.up * (radius + 0.1f);

            Vector3 top =
                bottom + Motor.transform.up * (height - radius * 2 - 0.1f);

            return !Physics.CheckCapsule(
                bottom,
                top,
                radius,
                mantlingLayers,
                QueryTriggerInteraction.Ignore);
        }

        private RaycastHit PerformDetectionTopDown(float forwardDistance)
        {
            Vector3 origin = GetTopDownRayOrigin(forwardDistance);

            Debug.DrawRay(
                origin,
                -Motor.transform.up * (Motor.Height * 0.49f),
                Color.red);

            if (Physics.Raycast(
                origin,
                -Motor.transform.up,
                out RaycastHit hit,
                Motor.Height *0.49f,
                mantlingLayers))
            {
                return hit;
            }

            return default;
        }

        private MantlingType ChooseMantlingType()
        {
            if (obstacleHeight <= Motor.Height * 0.4f)
                return MantlingType.StepOnLow;

            return MantlingType.StepOnHigh;
        }
    }
}
