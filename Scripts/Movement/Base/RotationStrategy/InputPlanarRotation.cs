using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    public class InputPlanarRotation : IRotationStrategy
    {
        private readonly Character.Actor character;
        public InputPlanarRotation(Character.Actor character)
        {
            this.character = character;
        }

        public Quaternion GetRotation()
        {
            Vector2 lookInput = CameraManager.Instance.CameraInput.LookInput;

            return Quaternion.Euler(0f, character.CameraTarget.TrackingTarget.transform.eulerAngles.y + lookInput.x, 0f);
        }
    }
}
