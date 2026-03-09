using System;
using R3;
using UnityEngine;

namespace ClickRPG.CharacterControllers.Brains
{
    [Serializable]
    public sealed class Player : ICharacterBrain
    {
        public void Attach(Character character, CompositeDisposable scope)
        {
            Debug.Log("Player brain attached");
        }
    }
}
