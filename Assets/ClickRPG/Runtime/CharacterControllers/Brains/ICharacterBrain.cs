using System;
using R3;

namespace ClickRPG.CharacterControllers.Brains
{
    public interface ICharacterBrain
    {
        void Attach(Character character, CompositeDisposable scope);
    }
}
