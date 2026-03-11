using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using ClickRPG.CharacterControllers;
using UnityEngine;

namespace ClickRPG
{
    [Serializable]
    public class UserData
    {
        [field: SerializeField]
        public List<Skill> Skills { get; private set; } = new();

        public void SetSkills(IEnumerable<Skill> skills)
        {
            Skills.Clear();
            Skills.AddRange(skills);
        }
    }
}
