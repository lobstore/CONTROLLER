using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "SlipJumpMovementStateFactory", menuName = "MovementStateMachine/Factories/SlipJumpMovementStateFactory")]
    public class SlipJumpMovementStateFactory : MovementStateFactory
    {
        public JumpStateData stateData;
        protected override MovementStateBase CreateState(Actor character)
        {
            return new SlipJumpMovementState(character, stateData);
        }
    }
}
