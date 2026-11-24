using System;
using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [Serializable]
    public class JumpStateData : TimedMovementStateData
    {
        [Header("Jump Forces")]
        public float PlanarJumpForce = 5f;
        public float VerticalJumpForce = 10f;
    }
}
