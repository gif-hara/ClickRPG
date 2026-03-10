using System;
using ClickRPG.CharacterControllers.Brains;
using R3;

namespace ClickRPG.CharacterControllers
{
    [Serializable]
    public class CharacterBrainController
    {
        private readonly CompositeDisposable scope = new();

        public void Setup(Character character, ICharacterBrain brain)
        {
            scope.Clear();
            if (brain == null)
            {
                return;
            }
            brain.Attach(character, scope);
            scope.RegisterTo(character.destroyCancellationToken);
        }
    }
}
