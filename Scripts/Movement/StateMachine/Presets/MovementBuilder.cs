using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{

    public abstract class MovementBuilder : ScriptableObject
    {
        public abstract void Build(Character.Actor movementStateDriver);
    }
}
