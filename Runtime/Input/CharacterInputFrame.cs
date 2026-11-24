using System;
using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// A value snapshot. Pressed fields are edge-triggered; SprintHeld is continuous.
    /// Look values are expected to be already normalized by the input binding setup.
    /// </summary>
    [Serializable]
    public struct CharacterInputFrame
    {
        public Vector2 Move;
        public Vector2 Look;
        public bool JumpPressed;
        public bool SprintHeld;
        public bool LockOnPressed;
        public bool NextTargetPressed;
        public bool PreviousTargetPressed;
        public bool NextCharacterPressed;
        public bool PreviousCharacterPressed;
        public bool NextCameraPressed;

        public static CharacterInputFrame Empty
        {
            get { return default(CharacterInputFrame); }
        }
    }
}