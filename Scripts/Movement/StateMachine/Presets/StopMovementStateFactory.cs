using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "StopMovementStateFactory", menuName = "MovementStateMachine/Factories/StopMovementStateFactory")]
    public class StopMovementStateFactory : MovementStateFactory
    {
        public TimedMovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new StopMovementState(character, stateData);
        }
    }
}
