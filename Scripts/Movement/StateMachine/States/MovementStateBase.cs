using LOGIYGames.Core;
using LOGIYGames.Timers;
using System;
using UnityEngine;
namespace LOGIYGames.Character.Movement
{
    [Serializable]
    public abstract class MovementStateBase : IState
    {
        protected Character.Actor Actor;
        protected MovementWrapperBase Motor;
        public MovementStateData Data { get; protected set; }
        protected Animator _animator;
        protected CountdownTimer ActionFrameTimer;
        public bool IsActiveState { get; private set; }
        public bool IsActionFrameElapsed => ActionFrameTimer.IsFinished;
        public bool IsActionFrameInProgress => ActionFrameTimer.IsRunning;

        protected MovementStateBase(Character.Actor ctx, MovementStateData stateData)
        {
            _animator = ctx.GetComponent<Animator>();
            Actor = ctx;
            Data = stateData;
            Motor = ctx.GetComponent<MovementWrapperBase>();
            ActionFrameTimer = new CountdownTimer(Data.ActionFrameDuration);
        }
        public virtual bool CanEnter() { return true; }
        public virtual bool CanExit() { return true; }
        public virtual void Enter()
        {
            IsActiveState = true;
            Debug.Log("Entered State: " + GetType());
            if (Data.ResetVelocityOnEnter)
            {
                Motor.ResetVelocity();
            }
            if (Data.ResetSpeedOnEnter)
            {
                Actor.RuntimeMovement.CurrentSpeed = 0;
            }
            _animator.applyRootMotion = Data.IsAnimationDrivenMovement;
            Actor.RuntimeMovement.Acceleration = Data.Acceleration;
            Actor.RuntimeMovement.Deceleration = Data.Deceleration;
            Actor.RuntimeMovement.TargetSpeed = Data.TargetSpeed;
            Actor.RuntimeMovement.TurnSmoothTime = Data.TurnSmoothTime;
            ActionFrameTimer.Start();
        }

        public virtual void Exit()
        {
            IsActiveState = false;
            if (Data.ResetVelocityOnExit)
            {
                Motor.ResetVelocity();
            }
            if (Data.ResetSpeedOnExit)
            {
                Actor.RuntimeMovement.CurrentSpeed = 0;
            }
            ActionFrameTimer.Stop();
        }

        public virtual void LogicUpdate()
        {
        }

        public virtual void LateUpdate()
        {
        }
        public virtual void PhysicsUpdate()
        {

        }

    }
}
