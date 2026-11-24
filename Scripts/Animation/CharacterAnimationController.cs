using LOGIYGames.Character.Movement;
using LOGIYGames.Shared.Enums;
using System;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace LOGIYGames.Character.Animation
{
    public class CharacterAnimationController : MonoModuleBase
    {
        Actor Actor;
        Animator Animator;

        [SerializeField][Range(0, 0.5f)] private float rotationAnimationsBlendTime;
        [SerializeField][Range(0, 0.5f)] private float animationsCrossFadeSpeed;
        [SerializeField][Range(0, 0.5f)] private float locomotionCrossFadeSpeed;

        [SerializeField] CharacterAnimationsData _data;
        public bool UseRootMotion { get => Animator.applyRootMotion; set => Animator.applyRootMotion = value; }
        [SerializeField] AnimatorOverrideController defaultOverride;
        private void Awake()
        {
            Actor = GetComponent<Actor>();
            Animator = GetComponent<Animator>();
        }
        private void Start()
        {
            Actor.EventBus.Subscribe<JumpPerformedEvent>((evt) =>
            {
                switch (evt.jumpType)
                {
                    case JumpType.GroundJump:
                        switch (evt.direction)
                        {
                            case Direction.Forward:
                                PlayAnimation(_data.Jump_Grounded_Forward);
                                break;
                            case Direction.R45:
                                PlayAnimation(_data.Jump_Grounded_Forward);
                                break;
                            case Direction.L45:
                                PlayAnimation(_data.Jump_Grounded_Forward);
                                break;
                            case Direction.Backward:
                                PlayAnimation(_data.Jump_Grounded_Backward);
                                break;
                            case Direction.R135:
                                PlayAnimation(_data.Jump_Grounded_Backward);
                                break;
                            case Direction.L135:
                                PlayAnimation(_data.Jump_Grounded_Backward);
                                break;
                            case Direction.NoMovement:
                                PlayAnimation(_data.Jump_Grounded_Up);
                                break;
                            default:
                                PlayAnimation(_data.Jump_Grounded_Up);
                                break;
                        }
                        break;
                    case JumpType.BracedJump:
                        if (Actor.Input.MovementInput.magnitude > 0)
                        {
                            switch (evt.direction)
                            {
                                case Direction.Left:
                                    PlayAnimation(_data.Jump_Braced_Left);
                                    break;
                                case Direction.Right:
                                    PlayAnimation(_data.Jump_Braced_Right);
                                    break;
                                case Direction.Forward:
                                    PlayAnimation(_data.Jump_Braced_Up);
                                    break;
                                case Direction.Backward:
                                    PlayAnimation(_data.Jump_Braced_Down);
                                    break;
                                case Direction.R45:
                                    PlayAnimation(_data.Jump_Braced_Right);
                                    break;
                                case Direction.L45:
                                    PlayAnimation(_data.Jump_Braced_Left);
                                    break;
                                case Direction.L135:
                                    PlayAnimation(_data.Jump_Braced_Left);
                                    break;
                                case Direction.R135:
                                    PlayAnimation(_data.Jump_Braced_Right);
                                    break;
                                case Direction.NoMovement:

                                    break;
                                default:
                                    break;
                            }
                        }
                        else
                        {
                            PlayAnimation(_data.Jump_Braced_Backward);
                        }


                        break;
                    case JumpType.WallRunJump:
                        switch (evt.direction)
                        {
                            case Direction.Left:
                                PlayAnimation(_data.WallRunJump_Left);
                                break;
                            case Direction.Right:
                                PlayAnimation(_data.WallRunJump_Right);
                                break;
                            default:
                                break;
                        }

                        break;
                    case JumpType.Dash:
                        switch (evt.direction)
                        {
                            case Direction.Left:
                                PlayAnimation(_data.Dash_Left);
                                break;
                            case Direction.Right:
                                PlayAnimation(_data.Dash_Right);
                                break;
                            case Direction.Forward:
                                PlayAnimation(_data.Dash_Forward);
                                break;
                            case Direction.Backward:
                                PlayAnimation(_data.Dash_Backward);
                                break;
                            case Direction.L45:
                                PlayAnimation(_data.Dash_Left);
                                break;
                            case Direction.R45:
                                PlayAnimation(_data.Dash_Right);
                                break;
                            case Direction.R135:
                                PlayAnimation(_data.Dash_Right);
                                break;
                            case Direction.L135:
                                PlayAnimation(_data.Dash_Left);
                                break;
                            default:
                                break;
                        }
                        break;
                    case JumpType.Slip:
                        PlayAnimation(_data.Jump_Slip);
                        break;
                    case JumpType.Roll:
                        switch (evt.direction)
                        {
                            //case Direction.Left:
                            //    PlayAnimation(_data.Roll_Left);
                            //    break;
                            //case Direction.Right:
                            //    PlayAnimation(_data.Roll_Right);
                            //    break;
                            //case Direction.Forward:
                            //    PlayAnimation(_data.Roll_Forward);
                            //    break;
                            //case Direction.Backward:
                            //    PlayAnimation(_data.Roll_Backward);
                            //    break;
                            //case Direction.L45:
                            //    PlayAnimation(_data.Roll_Forward);
                            //    break;
                            //case Direction.R45:
                            //    PlayAnimation(_data.Roll_Forward);
                            //    break;
                            //case Direction.R135:
                            //    PlayAnimation(_data.Roll_Backward);
                            //    break;
                            //case Direction.L135:
                            //    PlayAnimation(_data.Roll_Backward);
                            //    break;
                            default:
                                PlayAnimation(_data.Roll_Forward);
                                break;
                        }
                        break;
                    default:
                        break;
                }
            });
            Actor.EventBus.Subscribe<LandedEvent>((evt) =>
            {
                switch (evt.horizontalDirection)
                {
                    //case Direction.Left:
                    //    if (evt.fallingSpeed > -7)
                    //    {
                    //        PlayAnimation(_data.Landing_Light_Left);

                    //    }
                    //    else if (evt.fallingSpeed < -7 && evt.fallingSpeed > -10)
                    //    {
                    //        PlayAnimation(_data.Landing_Hard_Forward);
                    //    }
                    //    else if (evt.fallingSpeed < -10)
                    //    {
                    //        PlayAnimation(_data.Landing_Break);
                    //    }
                    //    break;
                    //case Direction.Right:
                    //    if (evt.fallingSpeed > -7)
                    //    {
                    //        PlayAnimation(_data.Landing_Light_Right);

                    //    }
                    //    else if (evt.fallingSpeed < -7 && evt.fallingSpeed > -10)
                    //    {
                    //        PlayAnimation(_data.Landing_Hard_Forward);
                    //    }
                    //    else if (evt.fallingSpeed < -10)
                    //    {
                    //        PlayAnimation(_data.Landing_Break);
                    //    }
                    //    break;
                    case Direction.Forward:
                        if (evt.fallingSpeed > -7)
                        {
                            PlayAnimation(_data.Landing_Light_Forward);
                        }
                        else if (evt.fallingSpeed < -7 && evt.fallingSpeed > -10)
                        {
                            PlayAnimation(_data.Landing_Hard_Forward);
                        }
                        else if (evt.fallingSpeed < -10)
                        {
                            PlayAnimation(_data.Landing_Break);
                        }
                        break;
                    //case Direction.Backward:
                    //    if (evt.fallingSpeed > -7)
                    //    {
                    //        PlayAnimation(_data.Landing_Light_Backward);

                    //    }
                    //    else if (evt.fallingSpeed < -7 && evt.fallingSpeed > -10)
                    //    {
                    //        PlayAnimation(_data.Landing_Hard_Forward);
                    //    }
                    //    else if (evt.fallingSpeed < -10)
                    //    {
                    //        PlayAnimation(_data.Landing_Break);
                    //    }
                    //    break;
                    case Direction.NoMovement:
                        if (evt.fallingSpeed < -7)
                        {
                            PlayAnimation(_data.Landing_Break);

                        }
                        else
                        {
                            PlayAnimation(_data.Landing_Light_Idle);
                        }
                        break;
                    default:
                        if (evt.fallingSpeed < -7)
                        {
                            PlayAnimation(_data.Landing_Break);

                        }
                        else
                        {
                            PlayAnimation(_data.Landing_Light_Idle);
                        }
                        break;
                }

            });
            Actor.EventBus.Subscribe<TurnPerformedEvent>((evt) =>
            {
                if (evt.movementSpeed > 0.5f)
                {
                    if (evt.angle > 0)
                    {
                        PlayAnimation(_data.Run_Turn_90R);
                    }
                    else
                    {
                        PlayAnimation(_data.Run_Turn_90L);
                    }
                }
                else
                {
                    if (evt.angle > 0)
                    {
                        PlayAnimation(_data.Idle_Turn_90R);
                    }
                    else
                    {
                        PlayAnimation(_data.Idle_Turn_90L);
                    }
                }
            });
            Actor.EventBus.Subscribe<BackTurnPerformedEvent>((evt) =>
            {
                if (evt.movementSpeed > GetStateSpeed<WalkMovementState>())
                {
                    if (evt.angle > 0)
                    {
                        PlayAnimation(_data.Run_BackTurn_Right);
                    }
                    else
                    {
                        PlayAnimation(_data.Run_BackTurn_Left);
                    }
                }
                else if (evt.movementSpeed < GetStateSpeed<RunMovementState>())
                {
                    if (evt.angle > 0)
                    {
                        PlayAnimation(_data.Walk_BackTurn_Right);
                    }
                    else
                    {
                        PlayAnimation(_data.Walk_BackTurn_Left);
                    }
                }
            });
            Actor.EventBus.Subscribe<MovementStoppedEvent>((evt) =>
            {
                switch (evt.direction)
                {
                    case Direction.Forward:
                        if (evt.movementSpeed <= GetStateSpeed<WalkMovementState>())
                        {
                            PlayAnimation(_data.Walk_Stop_Forward);
                        }
                        else if (evt.movementSpeed <= GetStateSpeed<RunMovementState>())
                        {
                            PlayAnimation(_data.Run_Stop_Forward);
                        }
                        else if (evt.movementSpeed > GetStateSpeed<RunMovementState>())
                        {
                            PlayAnimation(_data.Sprint_Stop_Forward);
                        }
                        break;
                    case Direction.Backward:
                        if (evt.movementSpeed <= GetStateSpeed<WalkMovementState>())
                        {
                            PlayAnimation(_data.Walk_Stop_Backward);
                        }
                        else if (evt.movementSpeed <= GetStateSpeed<RunMovementState>())
                        {
                            PlayAnimation(_data.Run_Stop_Backward);
                        }
                        break;

                    default:
                        break;
                }

            });
            Actor.EventBus.Subscribe<MantlingEvent>((evt) =>
            {
                switch (evt.Type)
                {
                    case MantlingType.StepOnLow:
                        PlayAnimation(_data.Mantling_Stand_StepOn_Little);
                        break;
                    case MantlingType.StepOnHigh:
                        PlayAnimation(_data.Mantling_Stand_StepOn_High);
                        break;
                    case MantlingType.BracedLow:
                        PlayAnimation(_data.Mantling_Stand_HangOn_Little);
                        break;
                    case MantlingType.BracedHigh:
                        PlayAnimation(_data.Mantling_Stand_HangOn_High);
                        break;
                    default:
                        break;
                }
            });
            Actor.EventBus.Subscribe<WallrunEnterEvent>((evt) =>
            {
                if (evt.IsRightSide)
                {
                    PlayAnimation(_data.WallRun_Right);
                }
                else
                {
                    PlayAnimation(_data.WallRun_Left);
                }
            });
            Actor.EventBus.Subscribe<ComboAttackEvent>((evt) =>
            {
                PlayAnimation(evt.AnimationData.AnimationName);
                Animator.applyRootMotion = evt.AnimationData.UseRootMotion;
                Animator.SetFloat("MotionSpeed", evt.AnimationData.MotionSpeed);
            });
        }
        public void PlayAnimation(string animname, int layer = 0)
        {
            Animator.CrossFade(animname, animationsCrossFadeSpeed, layer);
        }
        public void PlayAnimation(int animhash, int layer = 0)
        {
            Animator.CrossFade(animhash, animationsCrossFadeSpeed, layer);
        }
        Vector2 XYinput;
        public override void OnFixedUpdate(float deltaTime)
        {
            base.OnFixedUpdate(deltaTime);
            Animator.SetBool("IsGrounded", Actor.Sensors.IsGrounded);
            Animator.SetFloat("Speed", Actor.RuntimeMovement.CurrentSpeed, locomotionCrossFadeSpeed, deltaTime);
            if (Actor.Input.MovementInput.magnitude > 0)
            {
                var local = Actor.transform.InverseTransformDirection(Actor.RuntimeMovement.TargetDirection).normalized;
                XYinput = new Vector2(local.x, local.z);
            }
            else
            {
                var local = Actor.transform.InverseTransformDirection(Actor.Motor.Velocity).normalized;
                XYinput = new Vector2(local.x, local.z);
            }
            var snapped = SnapDirection(XYinput);
            var horizontal = snapped.x;
            var vertical = snapped.y;

            //if (Character.Input.MovementInput.magnitude > 0)
            //{
            //    XYinput = RelativeInput();
            //}
            var snappedXYinput = SnapDirection(XYinput);
            if (Actor.RotationStrategy is LookRelativeRotation or InputRelativeRotation)
            {
                Animator.SetFloat(_data.HorizontalSpeed, 0);
                Animator.SetFloat(_data.VerticalSpeed, 1, locomotionCrossFadeSpeed, deltaTime);
            }
            else
            {
                Animator.SetFloat(_data.VerticalSpeed, vertical, locomotionCrossFadeSpeed, deltaTime);
                Animator.SetFloat(_data.HorizontalSpeed, horizontal, locomotionCrossFadeSpeed, deltaTime);
            }

            Animator.SetBool(_data.IsMoving, Actor.Input.MovementInput.magnitude > 0);
            Animator.SetBool(_data.IsFalling, Actor.GetMovementState<FallingMovementState>().IsActiveState);
            Animator.SetBool(_data.IsFocusing, Actor.Input.SubAttackPressed);

            Animator.SetFloat(_data.InputX, Actor.Input.MovementInput.x, locomotionCrossFadeSpeed, deltaTime);
            Animator.SetFloat(_data.InputY, Actor.Input.MovementInput.y, locomotionCrossFadeSpeed, deltaTime);
            Animator.SetFloat(_data.TurnAngle, Actor.RuntimeMovement.DeltaYaw, rotationAnimationsBlendTime, deltaTime);
        }

        private Vector2 RelativeInput()
        {
            Direction direction = Actor.GetInputRelativeDirection();
            Vector2 targetDir = Vector2.zero;
            switch (direction)
            {
                case Direction.Left:
                    targetDir = new Vector2(-1, 0);
                    break;
                case Direction.Right:
                    targetDir = new Vector2(1, 0);
                    break;
                case Direction.Forward:
                    targetDir = new Vector2(0, 1);
                    break;
                case Direction.Backward:
                    targetDir = new Vector2(0, -1);
                    break;
                case Direction.R45:
                    targetDir = new Vector2(1, 1).normalized;
                    break;
                case Direction.L45:
                    targetDir = new Vector2(-1, 1).normalized;
                    break;
                case Direction.R135:
                    targetDir = new Vector2(1, -1).normalized;
                    break;
                case Direction.L135:
                    targetDir = new Vector2(-1, -1).normalized;
                    break;
                case Direction.NoMovement:
                    targetDir = Vector3.zero;
                    break;
                default:
                    targetDir = Vector3.zero;
                    break;
            }

            return targetDir;
        }

        private static readonly Vector2[] EightDirections =
        {
            new Vector2( 1,  0), // Right
            new Vector2( 1,  1).normalized, // Top-Right
            new Vector2( 0,  1), // Up
            new Vector2(-1,  1).normalized, // Top-Left
            new Vector2(-1,  0), // Left
            new Vector2(-1, -1).normalized, // Bottom-Left
            new Vector2( 0, -1), // Down
            new Vector2( 1, -1).normalized  // Bottom-Right
        };

        private Vector2 SnapDirection(Vector2 input)
        {
            if (input.sqrMagnitude < 0.0001f) return Vector2.zero;

            Vector2 bestDirection = Vector2.zero;
            float maxDot = -1f;

            // Находим направление с максимальным скалярным произведением
            foreach (var dir in EightDirections)
            {
                float dot = Vector2.Dot(input, dir);
                if (dot > maxDot)
                {
                    maxDot = dot;
                    bestDirection = dir;
                }
            }

            return bestDirection;
        }
        private void OnAnimatorMove()
        {
            if (!Animator.applyRootMotion)
                return;

            Vector3 delta = Animator.velocity;
            if (Actor.Motor.UseGravity && Actor.Motor.Velocity.y <= 0)
            {
                Actor.Motor.ChangeHorizontalVelocity(delta);
            }
            else
            {

                Actor.Motor.ForceMove(delta);
            }
            Actor.Motor.SetRotation(transform.rotation * Animator.deltaRotation);
        }

        private float GetStateSpeed<T>() where T : MovementStateBase
        {
            return Actor.MovementStateMachine.GetState<T>().Data.TargetSpeed;
        }
    }
}
