using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "RollMovementStateFactory", menuName = "MovementStateMachine/Factories/RollMovementStateFactory")]
    public class RollMovementStateFactory : MovementStateFactory
    {
        public JumpStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new RollMovementState(character, stateData);
        }
    }
}
