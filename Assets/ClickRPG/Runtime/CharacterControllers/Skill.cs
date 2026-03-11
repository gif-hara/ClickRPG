using System;
using UnityEngine;

namespace ClickRPG.CharacterControllers
{
    [Serializable]
    public sealed class Skill
    {
        [field: SerializeField]
        public CharacterSkillType Type { get; private set; } = default!;

        [field: SerializeField, Min(0f)]
        public int Power { get; private set; } = 1;
    }
}
