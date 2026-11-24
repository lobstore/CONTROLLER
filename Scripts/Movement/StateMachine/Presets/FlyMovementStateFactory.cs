using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "FlyMovementStateFactory", menuName = "MovementStateMachine/Factories/FlyMovementStateFactory")]
    public class FlyMovementStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new FlyMovementState(character, stateData);
        }
    }
}
