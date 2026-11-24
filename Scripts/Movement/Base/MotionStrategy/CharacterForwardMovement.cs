using LOGIYGames.Character;
using UnityEngine;
using UnityEngine.EventSystems;

namespace LOGIYGames
{
    public class CharacterForwardMovement : IMovementStrategy
    {

        Character.Actor Character;

        public CharacterForwardMovement(Character.Actor character)
        {
            Character = character;
        }

        public Vector3 GetMovementDirection()
        {
            if (Character.Input.MovementInput.magnitude > 0)
            {
                return Character.transform.forward;
            }else
            {
                return Vector3.zero;
            }
        }
    }
}

