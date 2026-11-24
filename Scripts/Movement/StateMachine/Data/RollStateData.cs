using System;
using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [Serializable]
    public class RollStateData : TimedMovementStateData
    {
        [Header("Roll Forces")]
        public float PlanarForce = 5f;
        public float VerticalForce = 2f;
    }
}
