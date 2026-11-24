namespace LOGIYGames.CharacterController
{
    /// <summary>
    /// Supplies one immutable input snapshot per rendered frame.
    /// Implement this interface to integrate a custom input stack.
    /// </summary>
    public interface ICharacterInputSource
    {
        CharacterInputFrame Read();
    }
}