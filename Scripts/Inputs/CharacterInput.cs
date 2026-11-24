using System;
using UnityEngine;

namespace LOGIYGames.Character
{
    [Serializable]
    public struct CharacterInput
    {
        public Vector2 MovementInput;
        public Vector3 LookForward;
        public Vector3 LookRight;

        public bool SubAttackPressed;
        public bool JumpPressed;
        public bool EvadePressed;
        public bool SprintPressing;
        public bool CrouchPressed;
        public bool CrouchPressing;
        public bool InteractStarted;
        public bool InteractCanceled;
        public bool InteractPressing;
        public bool InteractPerformed;
        public bool InteractCompleted;
        public bool AttackPressed;
        public bool AbilityPressed;
        public bool LockPressed;
    }
}
