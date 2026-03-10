using System;
using UnityEngine;

namespace ClickRPG.CharacterControllers
{
    [Serializable]
    public sealed class CharacterStatus
    {
        [field: SerializeField, Min(0)]
        public int HitPoint { get; private set; } = 100;

        [field: SerializeField, Min(0)]
        public int Strength { get; private set; } = 10;

        [field: SerializeField, Min(0)]
        public int CooldownLevel { get; private set; } = 0;
    }
}
