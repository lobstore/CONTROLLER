using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    public class ToMoveDirectionRotation : IRotationStrategy
    {
        Character.Actor Character;

        public ToMoveDirectionRotation(Character.Actor character)
        {
            Character = character;
        }

        public Quaternion GetRotation()
        {
            return Quaternion.LookRotation(Character.RuntimeMovement.TargetDirection);
        }
    }
}

