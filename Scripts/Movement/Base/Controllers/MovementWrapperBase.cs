using UnityEngine;

namespace LOGIYGames
{
    public abstract class MovementWrapperBase : MonoBehaviour
    {
        public virtual GroundedReport LastGroundedReport { get; }
        public abstract bool UseGravity { get; set; }
        public float GravityMultiplier { get; set; } = 1;
        public abstract float StepHeight { get; }
        public abstract float Height { get; set; }
        public abstract float Radius { get; set; }
        public abstract bool IsNoClip { get; set; }
        public abstract Collider Collider { get; }
        public abstract Vector3 Velocity { get; }
        public virtual Vector3 HorizontalVelocity => new Vector3(Velocity.x, 0, Velocity.z);

        public virtual void AddAcceleration(Vector3 accelerationForce) { }
        public abstract void ChangeHorizontalVelocity(Vector3 velocity);
        public abstract void ChangeVerticalVelocity(float velocityY);
        public abstract void AddImpulse(Vector3 impulseForce);
        public virtual void ForceMove(Vector3 velocity) { }
        public virtual void SlopeHandle() { }

        public abstract void SetRotation(Quaternion rotation);
        public abstract void SetPosition(Vector3 position);
        public abstract void ResetGravity();
        public abstract void ResetVelocity();
        public abstract void DisableMovement();
        public abstract void EnableMovement();
    }
}