using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "BackTurnMovementStateFactory", menuName = "MovementStateMachine/Factories/BackTurnMovementStateFactory")]
    public class BackTurnMovementStateFactory : MovementStateFactory
    {
        public TurnMovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new BackTurnMovementState(character, stateData);
        }
    }
}
