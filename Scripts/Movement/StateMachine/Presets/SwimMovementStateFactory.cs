using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "SwimMovementStateFactory", menuName = "MovementStateMachine/Factories/SwimMovementStateFactory")]
    public class SwimMovementStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new SwimMovementState(character, stateData);
        }
    }
}
