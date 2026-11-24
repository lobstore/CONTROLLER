using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "MantlingStateFactory", menuName = "MovementStateMachine/Factories/MantlingStateFactory")]
    public class MantlingStateFactory : MovementStateFactory
    {
        public MantlingMovmentStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new MantlingMovementState(character, stateData);
        }
    }
}
