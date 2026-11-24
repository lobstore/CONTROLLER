using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    public abstract class MantlingFactory : ScriptableObject
    {
       [SerializeField] protected MantlingData mantlingData;
        public abstract MantlingStrategy Create(Character.Actor chr);
    }
}
