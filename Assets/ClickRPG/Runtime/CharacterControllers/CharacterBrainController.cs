using System;
using ClickRPG.CharacterControllers.Brains;
using R3;

namespace ClickRPG.CharacterControllers
{
    [Serializable]
    public class CharacterBrainController
    {
        private CompositeDisposable scope;

        public void Setup(Character character, ICharacterBrain brain)
        {
            scope = new CompositeDisposable();
            brain.Attach(character, scope);
            scope.RegisterTo(character.destroyCancellationToken);
        }
    }
}
