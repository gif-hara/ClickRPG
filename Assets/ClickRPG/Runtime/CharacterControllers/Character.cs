using System;
using System.Collections.Generic;
using ClickRPG.CharacterControllers.Brains;
using R3;
using SoulLike;
using UnityEngine;
using UnityEngine.Pool;

namespace ClickRPG.CharacterControllers
{
    public sealed class Character : MonoBehaviour, IDisposable
    {
        [field: SerializeField]
        public Transform SceneView { get; private set; } = null!;

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

        private readonly ReactiveProperty<int> cooldownLevel = new();

        public ReadOnlyReactiveProperty<int> CooldownLevel => cooldownLevel;

        public Dictionary<CharacterSkillType, int> SkillPowers { get; } = new();

        private ObjectPool<Character> pool = null!;

        public CompositeDisposable Scope { get; } = new();

        public void Dispose()
        {
            if (pool != null)
            {
                pool.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Awake()
        {
            brainController.Setup(this, brain);
            hitPoint.Value = baseStatus.HitPoint;
            strength.Value = baseStatus.Strength;
            cooldownLevel.Value = baseStatus.CooldownLevel;
        }

        public void Spawn(Vector3 position)
        {
            pool ??= new ObjectPool<Character>(
                createFunc: () => Instantiate(this),
                actionOnRelease: character =>
                {
                    character.gameObject.SetActive(false);
                    character.Scope.Clear();
                },
                actionOnGet: character => character.gameObject.SetActive(true)
                );
            var instance = pool.Get();
            instance.pool = pool;
            instance.transform.position = position;
        }

        public void TakeDamage(int damage)
        {
            hitPoint.Value = Mathf.Max(hitPoint.Value - damage, 0);
            if (hitPoint.Value <= 0)
            {
                Dispose();
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
                    case CharacterSkillType.Shotgun:
                    case CharacterSkillType.MachineGun:
                    case CharacterSkillType.WideBullet:
                    case CharacterSkillType.Laser:
                    case CharacterSkillType.Horming:
                        break;
                    case CharacterSkillType.HitPointUp:
                        hitPoint.Value += skill.Power;
                        break;
                    case CharacterSkillType.StrengthUp:
                        strength.Value += skill.Power;
                        break;
                    case CharacterSkillType.CooldownLevelUp:
                        cooldownLevel.Value += skill.Power;
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
