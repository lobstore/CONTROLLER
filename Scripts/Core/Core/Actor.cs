using Alchemy.Inspector;
using LOGIYGames.Character.Movement;
using LOGIYGames.Core;
using LOGIYGames.Core.Events;
using LOGIYGames.EffectSystem.Examples;
using LOGIYGames.Extensions;
using LOGIYGames.Shared.Enums;
using System;
using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;
namespace LOGIYGames.Character
{
    public partial class Actor : MonoModuleBase, IControllable
    {
        [SerializeReference][ReadOnly] ICharacterInputReader InputReader;
        [field: SerializeField][field: ReadOnly] public CharacterInput Input { get; private set; }
        [field: SerializeReference][field: ReadOnly] public IMovementStrategy MovementStrategy { get; set; }
        [field: SerializeReference][field: ReadOnly] public IRotationStrategy RotationStrategy { get; set; }
        public IEventDispatcher EventBus { get; private set; } = new EventDispatcher();
        [field: SerializeField] public CameraTarget CameraTarget { get; private set; }
        [ReadOnly] public MovementData RuntimeMovement;
        #region Modules
        public MovementWrapperBase Motor { get; private set; }
        public SensorsModule Sensors { get; private set; }
        #endregion

        [Header("State Machine Configuration")]
        public StateMachine MovementStateMachine { get; private set; }
        public MovementBuilder movementPreset;
        private Dictionary<Type, MovementStateBase> m_movementStates = new();
        private void Awake()
        {
            Motor = GetComponent<MovementWrapperBase>();
            Sensors = GetComponent<SensorsModule>();
            InitializeStateMachine();
        }
        public override void OnFixedUpdate(float fixedDeltaTime)
        {
            base.OnFixedUpdate(fixedDeltaTime);
            MovementStateMachine.FixedUpdate();
        }
        public override void OnLateUpdate(float deltaTime)
        {
            base.OnLateUpdate(deltaTime);

            MovementStateMachine.LateUpdate();
        }
        public override void OnUpdate(float deltaTime)
        {
            base.OnUpdate(deltaTime);
            if (InputReader != null)
            {
                Input = InputReader.GetInput();
            }
            if (MovementStrategy != null)
            {
                RuntimeMovement.TargetDirection = MovementStrategy.GetMovementDirection();
            }
            if (RotationStrategy != null)
            {
                RuntimeMovement.TargetRotation = RotationStrategy.GetRotation();

            }
            UpdateSpeed();
            CalculateDeltaYaw();
            MovementStateMachine.Update();
        }

        #region Rotation Methods
        public void RotateToDirection(Vector3 desiredDirection, float turnSmoothTime = 0)
        {
            Quaternion targetRotation = Quaternion.LookRotation(desiredDirection, Motor.transform.up);
            Rotate(targetRotation, turnSmoothTime);
        }
        public void RotateToPosition(Vector3 position, float turnSmoothTime = 0)
        {
            Vector3 desiredDirection = position - Motor.transform.position;
            RotateToDirection(desiredDirection.normalized, turnSmoothTime);
        }
        public void Rotate(Quaternion targetRotation, float turnSpeed = 0)
        {

            if (turnSpeed > 0f)
            {
                Quaternion smoothedRotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turnSpeed * Time.deltaTime);
                Motor.SetRotation(smoothedRotation);
            }
            else
            {
                Motor.SetRotation(targetRotation);
            }
        }
        public void Rotate()
        {
            if (RotationStrategy is InputPlanarRotation)
            {
                Rotate(RuntimeMovement.TargetRotation);
            }
            else
            {
                Rotate(RuntimeMovement.TargetRotation, RuntimeMovement.TurnSmoothTime);
            }

        }
        [SerializeField] private float maxHeadAngle = 80f;

        public void RotateHead(Quaternion quaternion)
        {
            Transform head = CameraTarget.TrackingTarget;
            Transform parent = head.parent;

            // Переводим желаемый поворот в локальный (относительно родителя)
            Quaternion local = Quaternion.Inverse(parent.rotation) * quaternion;

            local.ToAngleAxis(out float angle, out Vector3 axis);

            // Кратчайший путь (0..180)
            if (angle > 180f)
            {
                angle = 360f - angle;
                axis = -axis;
            }

            // Ограничиваем конусом 80°
            angle = Mathf.Min(angle, maxHeadAngle);

            // Применяем обратно
            Quaternion clampedLocal = Quaternion.AngleAxis(angle, axis);
            head.rotation = parent.rotation * clampedLocal;
        }
        public void ResetHeadRotation()
        {
            CameraTarget.TrackingTarget.localRotation = Quaternion.identity;
        }
        private void CalculateDeltaYaw()
        {
            RuntimeMovement.DeltaYaw = Mathf.DeltaAngle(transform.eulerAngles.y, RuntimeMovement.TargetRotation.eulerAngles.y);
            if (Mathf.Abs(RuntimeMovement.DeltaYaw) < 0.01f)
            {
                RuntimeMovement.DeltaYaw = 0;
            }
        }

        #endregion
        private void UpdateSpeed()
        {
            var mult = 1 - Mathf.Abs(Sensors.GroundAngle) / 180;
            float targetSpeed = RuntimeMovement.TargetSpeed * Mathf.Clamp(mult, 0.9f, 1f);
            if (Input.MovementInput.magnitude > 0)
            {
                if (RuntimeMovement.CurrentSpeed <= targetSpeed)
                {
                    RuntimeMovement.CurrentSpeed = Mathf.MoveTowards(RuntimeMovement.CurrentSpeed, targetSpeed, RuntimeMovement.Acceleration * Time.deltaTime);

                }
                else
                {
                    RuntimeMovement.CurrentSpeed = Mathf.MoveTowards(RuntimeMovement.CurrentSpeed, targetSpeed, RuntimeMovement.Deceleration * Time.deltaTime);
                }
            }
            else
            {
                if (RuntimeMovement.CurrentSpeed < 0.001f)
                {
                    RuntimeMovement.CurrentSpeed = 0;
                }
                else
                {

                    RuntimeMovement.CurrentSpeed = Mathf.MoveTowards(RuntimeMovement.CurrentSpeed, 0, RuntimeMovement.Deceleration * Time.deltaTime);
                }
            }
            //RuntimeMovement.CurrentSpeed = RuntimeMovement.TargetSpeed;
        }
        #region Movement State Machine
        private void InitializeStateMachine()
        {
            MovementStateMachine = new StateMachine();
            if (movementPreset != null)
            {
                movementPreset.Build(this);

            }
            else
            {
                Debug.LogError("No MovementPreset provided");
            }
        }

        public void AddMovementState(MovementStateBase state)
        {
            m_movementStates[state.GetType()] = state;

            MovementStateMachine.AddState(state);
        }
        public void RemoveMovementState<T>() where T : MovementStateBase
        {
            m_movementStates.Remove(typeof(T));

            MovementStateMachine.RemoveState<T>();
        }
        public T GetMovementState<T>() where T : MovementStateBase
        {
            if (m_movementStates.TryGetValue(typeof(T), out var state))
                return state as T;

            return null;
        }

        public bool HasMovementState<T>() where T : MovementStateBase
        {
            return m_movementStates.ContainsKey(typeof(T));
        }
        #endregion
        #region IControllable
        public void SetInput(ICharacterInputReader inputReader)
        {
            InputReader = inputReader;
        }
        public void ResetInput()
        {
            Input = new CharacterInput();
        }

        #endregion
        public Direction GetRelativeMovementDirection()
        {
            Vector3 localDir;
            localDir = Motor.transform.InverseTransformDirection(Motor.Velocity);
            Direction direction;
            if (localDir.magnitude == 0)
            {
                direction = Direction.NoMovement;
            }
            else
            {


                float forwardDot = Vector3.Dot(localDir, Vector3.forward);
                float rightDot = Vector3.Dot(localDir, Vector3.right);
                float upDot = Vector3.Dot(localDir, Vector3.up);
                if (Mathf.Abs(forwardDot) > Mathf.Abs(rightDot))
                {
                    if (forwardDot > 0.01f)
                        direction = Direction.Forward;
                    else
                        direction = Direction.Backward;
                }
                else
                {
                    if (rightDot > 0.01f)
                        direction = Direction.Right;
                    else
                        direction = Direction.Left;
                }
            }
            return direction;
        }

        public Direction GetInputRelativeDirection()
        {
            Vector2 input = Input.MovementInput;

            if (input.sqrMagnitude < 0.0001f)
                return Direction.NoMovement;

            Vector3 sightForward = Input.LookForward;
            Vector3 sightRight = Input.LookRight;

            sightForward.y = 0f;
            sightRight.y = 0f;

            sightForward.Normalize();
            sightRight.Normalize();

            // Направление движения в мировых координатах
            Vector3 moveDirection =
                sightForward * input.y +
                sightRight * input.x;

            moveDirection.Normalize();

            // Проекция направления движения на оси персонажа
            float forward = Vector3.Dot(transform.forward, moveDirection);
            float right = Vector3.Dot(transform.right, moveDirection);

            // Угол относительно направления персонажа
            float angle = Vector3.SignedAngle(
                transform.forward,
                moveDirection,
                Vector3.up
            );

            float absAngle = Mathf.Abs(angle);

            // Диагонали: угол примерно 45° или 135°
            if (absAngle >= 22.5f && absAngle < 67.5f)
            {
                return angle > 0f
                    ? Direction.R45
                    : Direction.L45;
            }

            if (absAngle >= 112.5f && absAngle < 157.5f)
            {
                return angle > 0f
                    ? Direction.R135
                    : Direction.L135;
            }

            // Основные направления
            if (Mathf.Abs(forward) > Mathf.Abs(right))
            {
                return forward > 0f
                    ? Direction.Forward
                    : Direction.Backward;
            }

            return right > 0f
                ? Direction.Right
                : Direction.Left;
        }
    }
}
