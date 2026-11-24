using LOGIYGames.CharacterController;
using LOGIYGames.Core.Gameplay.Restrictions;
using UnityEngine;

namespace LOGIYGames.CharacterController.Samples
{
    /// <summary>
    /// Example adapter: put RestrictionCoordinator on each character prefab,
    /// assign a shared Movement channel, and assign this adapter to the Director.
    /// Missing coordinators are treated as unrestricted so the sample is opt-in.
    /// </summary>
    [DisallowMultipleComponent]
    public sealed class RestrictionsControlGate : MonoBehaviour, ICharacterControlGate
    {
        [SerializeField] private RestrictionChannel movementChannel;

        public bool CanControl(CharacterAgent character)
        {
            if (character == null || movementChannel == null)
                return true;

            RestrictionCoordinator coordinator =
                character.GetComponent<RestrictionCoordinator>();

            return coordinator == null || !coordinator.IsBlocked(movementChannel);
        }
    }
}