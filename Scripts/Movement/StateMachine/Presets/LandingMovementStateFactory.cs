using UnityEngine;

namespace LOGIYGames.Character.Movement
{
    [CreateAssetMenu(fileName = "LandingMovementStateFactory", menuName = "MovementStateMachine/Factories/LandingMovementStateFactory")]
    public class LandingMovementStateFactory : MovementStateFactory
    {
        public TimedMovementStateData stateData;
        protected override MovementStateBase CreateState(Character.Actor character)
        {
            return new LandingMovementState(character, stateData);
        }
    }
}
