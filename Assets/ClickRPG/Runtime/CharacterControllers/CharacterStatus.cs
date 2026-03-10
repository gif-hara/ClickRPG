using System;
using UnityEngine;

namespace ClickRPG.CharacterControllers
{
    [Serializable]
    public sealed class CharacterStatus
    {
        [field: SerializeField]
        public int HitPoint { get; private set; } = 100;

        [field: SerializeField]
        public int Strength { get; private set; } = 10;

        [field: SerializeField]
        public int CooldownLevel { get; private set; } = 1;
    }
}
