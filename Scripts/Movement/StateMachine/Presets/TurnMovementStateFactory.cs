using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "TurnMovementStateFactory", menuName = "MovementStateMachine/Factories/TurnMovementStateFactory")]
    public class TurnMovementStateFactory : MovementStateFactory
    {
        public TurnMovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new TurnMovementState(character, stateData);
        }
    }
}
