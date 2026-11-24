using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "AbilityMovementStateFactory", menuName = "MovementStateMachine/Factories/AbilityMovementStateFactory")]
    public class AbilityMovementStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Actor character)
        {
            return new AbilityMovementState(character, stateData);
        }
    }
}
