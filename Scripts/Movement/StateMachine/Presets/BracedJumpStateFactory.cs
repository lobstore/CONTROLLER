using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "BracedJumpStateFactory", menuName = "MovementStateMachine/Factories/BracedJumpStateFactory")]
    public class BracedJumpStateFactory : MovementStateFactory
    {
        public JumpStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new BracedJumpMovementState(character, stateData);
        }
    }
}
