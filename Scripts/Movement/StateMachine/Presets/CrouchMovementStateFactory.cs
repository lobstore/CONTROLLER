using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "CrouchMovementStateFactory", menuName = "MovementStateMachine/Factories/CrouchMovementStateFactory")]
    public class CrouchMovementStateFactory : MovementStateFactory
    {
        public MovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new CrouchMovementState(character, stateData);
        }
    }
}
