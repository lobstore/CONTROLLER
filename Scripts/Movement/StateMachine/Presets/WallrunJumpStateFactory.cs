using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "WallrunJumpStateFactory", menuName = "MovementStateMachine/Factories/WallrunJumpStateFactory")]
    public class WallrunJumpStateFactory : MovementStateFactory
    {
        public JumpStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new WallrunJumpMovementState(character, stateData);
        }
    }
}
