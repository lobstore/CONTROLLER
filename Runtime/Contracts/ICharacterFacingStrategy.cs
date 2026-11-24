using UnityEngine;

namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Optional per-character policy for deciding which direction the character faces.
    /// Return Vector3.zero to keep current rotation for this tick.
    /// </summary>
    public interface ICharacterFacingStrategy
    {
        Vector3 GetFacingDirection(CharacterFacingContext context);
    }
}