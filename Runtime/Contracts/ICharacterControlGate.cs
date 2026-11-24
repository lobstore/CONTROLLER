namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Optional integration point for stun, pause, cutscene, possession and gameplay
    /// restriction systems. Return false to suppress player input for this character.
    /// </summary>
    public interface ICharacterControlGate
    {
        bool CanControl(CharacterAgent character);
    }
}