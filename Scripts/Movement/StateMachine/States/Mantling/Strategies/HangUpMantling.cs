using LOGIYGames.Character;
using LOGIYGames.Shared.Enums;
using UnityEngine;

namespace LOGIYGames
{
    public class HangUpMantling : MantlingStrategy
    {
        private float obstacleHeight;
        public HangUpMantling(Actor chr, MantlingData data) : base(chr, data) { }
        public override bool CanEnter()
        {
            if (!HasClearPathToRayOrigin())
                return false;

            TargetTopPoint = PerformDetectionTopDown(checkDistance);

            if (TargetTopPoint.collider != null)
                obstacleHeight = CalculateObstacleHeight();
            else
                return false;

            if (!HasEnoughSpaceAhead())
                return false;

            return obstacleHeight > Motor.Height * 0.5 &&
                   obstacleHeight <= Motor.Height + 0.3f;
        }

        public override bool CanExit()
        {
            return Duration.IsFinished;
        }

        public override void Enter()
        {
            MantlingType = ChooseMantlingType();
            Duration.Start();
            MantleStartPosition = Motor.transform.position;

            MantleTargetTransform = TargetTopPoint.collider.transform;

            if (MantleTargetTransform != null)
            {
                MantleTargetLocalPoint = MantleTargetTransform.InverseTransformPoint(TargetTopPoint.point);

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
            return TargetTopPoint.point.y - Motor.transform.position.y;
        }

        private Vector3 GetTopDownRayOrigin(float forwardDistance)
        {
            return Motor.transform.position +
                   Motor.transform.forward * (Motor.Radius + forwardDistance) +
                   Motor.transform.up * (Motor.Height + 0.3f);
        }
        private bool HasClearPathToRayOrigin()
        {
            //Vector3 rayOrigin = GetTopDownRayOrigin(checkDistance);

            //Vector3 start =
            //    Motor.transform.position +
            //    Motor.transform.up * (Motor.Height + 0.3f);

            //Vector3 direction = rayOrigin - start;
            //float distance = direction.magnitude;

            //Debug.DrawLine(start, rayOrigin, Color.yellow);

            //return !Physics.Raycast(
            //    start,
            //    direction.normalized,
            //    out _,
            //    distance,
            //    mantlingLayers,
            //    QueryTriggerInteraction.Ignore);
            return !Actor.Sensors.IsObstacleHeadFront && !Actor.Sensors.IsObstacleAbove;
        }

        private bool HasEnoughSpaceAhead()
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
                -Motor.transform.up * (Motor.Height + 0.3f),
                Color.red);

            if (Physics.Raycast(
                origin,
                -Motor.transform.up,
                out RaycastHit hit,
                Motor.Height + 0.3f,
                mantlingLayers))
            {
                return hit;
            }

            return default;
        }

        private MantlingType ChooseMantlingType()
        {
            if (obstacleHeight <= Motor.Height * 0.7f)
                return MantlingType.BracedLow;

            return MantlingType.BracedHigh;
        }
    }
}
