using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "GroundJumpStateFactory", menuName = "MovementStateMachine/Factories/GroundJumpStateFactory")]
    public class GroundJumpStateFactory : MovementStateFactory
    {
        public JumpStateData stateData;

        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new GroundJumpMovementState(character, stateData);
        }
    }

}
