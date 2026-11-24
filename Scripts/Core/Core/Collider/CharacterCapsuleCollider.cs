using LOGIYGames;
using UniGLTF.Extensions.VRMC_springBone;
using UnityEngine;

public class CharacterCapsuleCollider : MonoBehaviour
{
    [field: SerializeField] public CapsuleCollider Collider { get; private set; }
    [field: SerializeField] public CharacterColliderData CharacterColliderData { get; private set; }
    [field: SerializeField] public StepData StepData { get; private set; }

    private void Awake()
    {
        Resize();
    }

    private void OnValidate()
    {
        Resize();
    }

    public void Resize()
    {
        CalculateCapsuleColliderDimensions();
    }

    private void CalculateCapsuleColliderDimensions()
    {
        Collider.radius = CharacterColliderData.Radius;

        Collider.height = CharacterColliderData.Height * (1f - StepData.StepHeightPercentage);

        RecalculateCapsuleColliderCenter();

        RecalculateColliderRadius();
    }
    public void Floating()
    {
        Ray downRay = new Ray(Collider.bounds.center, -transform.up);
        if (Physics.Raycast(downRay, out RaycastHit hit, StepData.FloatingRayDistance))
        {
            var distanceToFloatingPoint = Collider.center.y * transform.localScale.y - hit.distance;
            if (distanceToFloatingPoint == 0)
            {
                return;
            }
            float amoutToLift = distanceToFloatingPoint * StepData.StepReachForce - GetComponent<MovementWrapperBase>().Velocity.y;
            Vector3 liftForce = new Vector3(0, amoutToLift, 0f);

            GetComponent<MovementWrapperBase>().ChangeVerticalVelocity(liftForce.y);
        }
    }
    private void RecalculateCapsuleColliderCenter()
    {
        float colliderHeightDifference = CharacterColliderData.Height - Collider.height;

        Vector3 newColliderCenter = new Vector3(0f, CharacterColliderData.CenterY + (colliderHeightDifference / 2f), 0f);

        Collider.center = newColliderCenter;
    }

    private void RecalculateColliderRadius()
    {
        float halfColliderHeight = Collider.height / 2f;

        if (halfColliderHeight >= Collider.radius)
        {
            return;
        }

        Collider.radius = halfColliderHeight;
    }
}