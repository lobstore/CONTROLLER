using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "WalkMovementStateFactory", menuName = "MovementStateMachine/Factories/WalkMovementStateFactory")]
    public class WalkMovementStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new WalkMovementState(character, stateData);
        }
    }
}
