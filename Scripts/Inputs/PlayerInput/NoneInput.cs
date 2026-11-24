using UnityEngine;



namespace LOGIYGames.Character
{
    public class NoneInput : ICharacterInputReader
    {
        public CharacterInput GetInput()
        {
            return new CharacterInput();
        }
    }
}
