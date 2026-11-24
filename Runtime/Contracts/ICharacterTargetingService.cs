using UnityEngine;

namespace LOGIYGames.CharacterController
{
    public interface ICharacterTargetingService
    {
        Transform CurrentAimPoint { get; }
        bool HasTarget { get; }

        bool TryAcquire(Transform observer, Vector3 viewForward);
        bool TryCycle(Transform observer, Vector3 cameraRight, int direction);
        bool SetTarget(Transform target);
        void Clear();
    }
}