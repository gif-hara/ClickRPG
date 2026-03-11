using System.Collections.Generic;
using ClickRPG.CharacterControllers.Brains;
using R3;
using SoulLike;
using UnityEngine;

namespace ClickRPG.CharacterControllers
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private ICharacterBrain brain = null!;

        [SerializeField]
        private CharacterStatus baseStatus = null!;

        private readonly MessageBroker broker = new();

        public IMessageBroker Broker => broker;

        private readonly CharacterBrainController brainController = new();

        private readonly ReactiveProperty<int> hitPoint = new();

        public ReadOnlyReactiveProperty<int> HitPoint => hitPoint;

        private readonly ReactiveProperty<int> strength = new();

        public ReadOnlyReactiveProperty<int> Strength => strength;

        public Dictionary<CharacterSkillType, int> SkillPowers { get; } = new();

        void Awake()
        {
            brainController.Setup(this, brain);
            hitPoint.Value = baseStatus.HitPoint;
            strength.Value = baseStatus.Strength;
        }

        public void TakeDamage(int damage)
        {
            hitPoint.Value = Mathf.Max(hitPoint.Value - damage, 0);
            if (hitPoint.Value == 0)
            {
                Destroy(gameObject);
            }
        }

        public void ApplySkills(List<Skill> skills)
        {
            SkillPowers.Clear();
            foreach (var skill in skills)
            {
                SkillPowers[skill.Type] = skill.Power;
                switch (skill.Type)
                {
                    case CharacterSkillType.NormalBullet:
                        break;
                    case CharacterSkillType.Shotgun:
                        break;
                    case CharacterSkillType.MachineGun:
                        break;
                    case CharacterSkillType.Sniper:
                        break;
                    case CharacterSkillType.Laser:
                        break;
                    case CharacterSkillType.Horming:
                        break;
                    case CharacterSkillType.HitPointUp:
                        hitPoint.Value += skill.Power;
                        break;
                    case CharacterSkillType.StrengthUp:
                        strength.Value += skill.Power;
                        break;
                    case CharacterSkillType.CooldownLevelUp:
                        break;
                    case CharacterSkillType.CriticalRateUp:
                        break;
                    case CharacterSkillType.CriticalDamageUp:
                        break;
                    case CharacterSkillType.CooldownWhenCritical:
                        break;
                    case CharacterSkillType.StrengthUpWhenDefeatEnemy:
                        break;
                    case CharacterSkillType.CriticalRateUpWhenDefeatEnemy:
                        break;
                    case CharacterSkillType.PenetrateUp:
                        break;
                    case CharacterSkillType.BulletSpeedUp:
                        break;
                    case CharacterSkillType.ExperienceUp:
                        break;
                    case CharacterSkillType.RecoveryWhenDefeatEnemy:
                        break;
                }
            }
        }
    }
}
