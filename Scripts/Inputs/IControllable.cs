using Unity.Cinemachine;
using UnityEngine;

namespace LOGIYGames.Character
{
    public interface IControllable
    {
        CameraTarget CameraTarget { get; }
        void SetInput(ICharacterInputReader inputReader);
    }
}
