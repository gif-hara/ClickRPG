using System;
using R3;
using UnityEngine;

namespace ClickRPG.CharacterControllers.Abilities
{
    [Serializable]
    public sealed class CharacterHandleStatus : ICharacterAbility
    {
        [SerializeField]
        private CharacterStatus baseStatus = null!;

        private readonly ReactiveProperty<int> hitPoint = new();

        public ReadOnlyReactiveProperty<int> HitPoint => hitPoint;

        private readonly ReactiveProperty<int> strength = new();

        public ReadOnlyReactiveProperty<int> Strength => strength;

        private readonly ReactiveProperty<int> cooldownLevel = new();

        public ReadOnlyReactiveProperty<int> CooldownLevel => cooldownLevel;

        private Character character = null!;

        public void Initialize(Character character)
        {
            this.character = character;
            hitPoint.Value = baseStatus.HitPoint;
            strength.Value = baseStatus.Strength;
            cooldownLevel.Value = baseStatus.CooldownLevel;
        }

        public void TakeDamage(int damage)
        {
            hitPoint.Value = Mathf.Max(hitPoint.Value - damage, 0);
            if (hitPoint.Value == 0)
            {
                UnityEngine.Object.Destroy(character.gameObject);
            }
        }
    }
}
