using UnityEngine;

namespace LOGIYGames.CharacterController
{
    public struct CharacterFacingContext
    {
        public CharacterFacingMode Mode;
        public Vector3 MovementDirection;
        public Vector3 CameraForward;
        public Vector3 LockOnDirection;
        public Vector3 CurrentForward;
    }
}