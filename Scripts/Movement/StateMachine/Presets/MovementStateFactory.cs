using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    public abstract class MovementStateFactory : ScriptableObject
    {
        public void Create(Actor character)
        {
            var state = CreateState(character);

            character.AddMovementState(state);
        }

        protected abstract MovementStateBase CreateState(Character.Actor character);
    }
}
