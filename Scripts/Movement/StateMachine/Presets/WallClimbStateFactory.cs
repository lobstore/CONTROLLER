using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "WallClimbStateFactory", menuName = "MovementStateMachine/Factories/WallClimbStateFactory")]
    public class WallClimbStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new WallClimbMovementState(character, stateData);
        }
    }
}
