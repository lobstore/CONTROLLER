using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "SprintMovementStateFactory", menuName = "MovementStateMachine/Factories/SprintMovementStateFactory")]
    public class SprintMovementStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new SprintMovementState(character, stateData);
        }
    }
}
