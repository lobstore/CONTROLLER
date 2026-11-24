using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "DashMovementStateFactory", menuName = "MovementStateMachine/Factories/DashMovementStateFactory")]
    public class DashStateFactory : MovementStateFactory
    {
        public JumpStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new DashMovementState(character, stateData);
        }
    }
}
