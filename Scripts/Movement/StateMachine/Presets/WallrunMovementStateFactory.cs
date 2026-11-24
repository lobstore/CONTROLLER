using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "WallrunMovementStateFactory", menuName = "MovementStateMachine/Factories/WallrunMovementStateFactory")]
    public class WallrunMovementStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Actor character)
        {
            return new WallRunMovementState(character, stateData);
        }
    }
}
