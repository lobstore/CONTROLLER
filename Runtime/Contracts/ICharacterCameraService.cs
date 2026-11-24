using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Camera boundary consumed by the character director. A Cinemachine, split-screen
    /// or network camera adapter can implement this without changing character logic.
    /// </summary>
    public interface ICharacterCameraService
    {
        Transform CameraTransform { get; }
        string ActiveViewId { get; }
        void SetProfile(CharacterCameraProfile profile);
        bool SetView(string viewId);
        bool CycleView(int direction);
        void Tick(CharacterAgent character, CharacterInputFrame input, Transform lockOnAimPoint, float deltaTime);
    }
}