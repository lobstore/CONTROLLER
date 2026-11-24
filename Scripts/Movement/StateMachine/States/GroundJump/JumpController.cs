using LOGIYGames.Shared.Enums;
using UnityEngine;
namespace LOGIYGames.Character.Movement
{
    public class DashJumpController : JumpController
    {
        public DashJumpController(Actor actor) : base(actor)
        {
        }

        public override void Jump(float verticalForce, float planarForce)
        {
            Direction direction;
            direction = Actor.GetInputRelativeDirection();
            Motor.AddImpulse(Actor.RuntimeMovement.TargetDirection * planarForce + Actor.transform.up * verticalForce);
            Actor.EventBus.Publish(new JumpPerformedEvent
            {
                jumpType = JumpType.Dash,
                planarForce = planarForce,
                verticalForce = verticalForce,
                direction = direction
            });
        }
    }
    public class GroundJumpController : JumpController
    {
        public GroundJumpController(Actor actor) : base(actor)
        {
        }
        public override void Jump(float verticalForce, float planarForce)
        {
            Motor.ResetGravity();
            Direction direction;
            direction = Actor.GetInputRelativeDirection();
            if (Actor.Input.MovementInput.magnitude > 0)
            {
                Motor.AddImpulse(Actor.RuntimeMovement.TargetDirection * planarForce + Actor.transform.up * verticalForce);

            }
            else
            {
                Motor.AddImpulse(Actor.transform.up * verticalForce);
            }
            Actor.EventBus.Publish(new JumpPerformedEvent
            {
                jumpType = JumpType.GroundJump,
                planarForce = planarForce,
                verticalForce = verticalForce,
                direction = direction
            });
        }

    }
    public class RollJumpController : JumpController
    {
        public RollJumpController(Actor actor) : base(actor)
        {
        }
        public override void Enter()
        {
            base.Enter();
            //if (Character.Input.MovementInput.magnitude > 0)
            //{
            //    Character.RotateToDirection(Character.RuntimeMovement.TargetDirection);

            //}
        }
        public override void Exit()
        {
            base.Exit();
        }
        public override void Jump(float verticalForce, float planarForce)
        {
            Direction direction;
            direction = Actor.GetInputRelativeDirection();
            if (Actor.Input.MovementInput.magnitude > 0)
            {
                Motor.AddImpulse(Actor.RuntimeMovement.TargetDirection * planarForce + Actor.transform.up * verticalForce);
            }
            else
            {
                Motor.AddImpulse(Actor.transform.forward * planarForce + Actor.transform.up * verticalForce);
            }
            Actor.EventBus.Publish(new JumpPerformedEvent
            {
                jumpType = JumpType.Roll,
                planarForce = planarForce,
                verticalForce = verticalForce,
                direction = direction
            });
        }
    }
    public class SlipJumpController : JumpController
    {
        public SlipJumpController(Actor actor) : base(actor)
        {
        }

        public override void Jump(float verticalForce, float planarForce)
        {
            Direction direction;
            direction = Actor.GetInputRelativeDirection();
            Motor.AddImpulse(Actor.transform.forward * planarForce);
            Actor.EventBus.Publish(new JumpPerformedEvent
            {
                jumpType = JumpType.Slip,
                planarForce = planarForce,
                verticalForce = verticalForce,
                direction = direction
            });
        }
    }
    public class BracedJumpController : JumpController
    {
        public BracedJumpController(Actor actor) : base(actor)
        {
        }

        public override void Jump(float verticalForce, float planarForce)
        {
            Direction direction;
            direction = Actor.GetInputRelativeDirection();
            if (Actor.Input.MovementInput.magnitude > 0)
            {
                switch (direction)
                {
                    case Direction.Left:
                        Motor.AddImpulse(-Actor.transform.right * planarForce);
                        break;
                    case Direction.Right:
                        Motor.AddImpulse(Actor.transform.right * planarForce);
                        break;
                    case Direction.L45:
                        Motor.AddImpulse(-Actor.transform.right * planarForce + Actor.transform.up * verticalForce / 2);
                        break;
                    case Direction.L135:
                        Motor.AddImpulse(-Actor.transform.right * planarForce + Actor.transform.up * -verticalForce / 2);
                        break;
                    case Direction.R45:
                        Motor.AddImpulse(Actor.transform.right * planarForce + Actor.transform.up * verticalForce / 2);
                        break;
                    case Direction.R135:
                        Motor.AddImpulse(Actor.transform.right * planarForce + Actor.transform.up * -verticalForce / 2);
                        break;
                    case Direction.Forward:
                        Motor.AddImpulse(Actor.transform.up * verticalForce);
                        break;
                    case Direction.Backward:
                        Motor.AddImpulse(-Actor.transform.up * verticalForce);
                        break;
                    case Direction.NoMovement:
                        Motor.AddImpulse(Actor.Sensors.LegsFrontHit.normal * planarForce + verticalForce * Actor.transform.up);
                        break;
                    default:
                        break;
                }
            }
            else
            {
                Motor.AddImpulse(Actor.Sensors.LegsFrontHit.normal * planarForce + verticalForce * Actor.transform.up);
            }
            Actor.EventBus.Publish(new JumpPerformedEvent
            {
                jumpType = JumpType.BracedJump,
                planarForce = planarForce,
                verticalForce = verticalForce,
                direction = direction
            });
        }
    }
    public class WallrunJumpController : JumpController
    {
        public WallrunJumpController(Actor actor) : base(actor)
        {
        }

        public override void Jump(float verticalForce, float planarForce)
        {
            Direction direction;
            direction = Actor.Sensors.IsObstacleLegsLeft ? Direction.Right : Direction.Left;
            Actor.EventBus.Publish(new JumpPerformedEvent
            {
                jumpType = JumpType.WallRunJump,
                planarForce = planarForce,
                verticalForce = verticalForce,
                direction = direction
            });
            Vector3 normal = Actor.Sensors.IsObstacleLegsLeft ? Actor.Sensors.LegsLeftHit.normal : Actor.Sensors.LegsRightHit.normal;
            Motor.AddImpulse(normal.normalized * planarForce + Actor.transform.up * verticalForce);
        }
    }

    public abstract class JumpController
    {
        protected MovementWrapperBase Motor;
        protected Actor Actor;
        float Cost;

        public JumpController(Actor actor)
        {
            Actor = actor;
            Motor = actor.Motor;
        }

        public virtual void Enter()
        {
        }
        public virtual void Exit()
        {
        }
        public virtual bool CanExecute()
        {
            return true;
        }
        public abstract void Jump(float verticalForce, float planarForce);

    }
}
