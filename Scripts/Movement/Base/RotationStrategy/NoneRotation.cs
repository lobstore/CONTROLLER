using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    public class NoneRotation : IRotationStrategy
    {
        Character.Actor Character;

        public NoneRotation(Character.Actor character)
        {
            Character = character;
        }

        public Quaternion GetRotation()
        {
            return Quaternion.LookRotation(Character.transform.forward);
        }
    }
}
