using LOGIYGames.Character;
using UnityEngine;

namespace LOGIYGames
{
    [CreateAssetMenu(fileName ="StepOnMantlingFactory", menuName = "Mantling/Factories/StepOn")]
    public class StepOnMantlingFactory : MantlingFactory
    {
        public override MantlingStrategy Create(Character.Actor chr)
        {
            return new StepOnMantling(chr, mantlingData);
        }
    }
}
