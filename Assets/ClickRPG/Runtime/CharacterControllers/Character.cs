using System;
using System.Collections.Generic;
using ClickRPG.CharacterControllers.Brains;
using Cysharp.Threading.Tasks;
using HKFeedback;
using HKFeedback.Extensions;
using R3;
using SoulLike;
using UnityEngine;
using UnityEngine.Pool;

namespace ClickRPG.CharacterControllers
{
    public sealed class Character :
        MonoBehaviour,
        IDisposable,
        IProvider<Character>,
        IProvider<Transform>,
        IProvider<IDisposable>,
        IProvider<GameObject>
    {
        [field: SerializeField]
        public Transform SceneView { get; private set; } = null!;

        [SerializeReference, SubclassSelector]
        private ICharacterBrain brain = null!;

        [SerializeField]
        private CharacterStatus baseStatus = null!;

        [SerializeField]
        private List<FeedbackElement> feedbackElements = null!;

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

        Character IProvider<Character>.Provide() => this;

        Transform IProvider<Transform>.Provide() => transform;

        IDisposable IProvider<IDisposable>.Provide() => this;

        GameObject IProvider<GameObject>.Provide() => gameObject;

        void Awake()
        {
            brainController.Setup(this, brain);
            hitPoint.Value = baseStatus.HitPoint;
            strength.Value = baseStatus.Strength;
            cooldownLevel.Value = baseStatus.CooldownLevel;

            foreach (var element in feedbackElements)
            {
                var observable = element.EventType switch
                {
                    CharacterEvent.EventType.Died => broker.Receive<CharacterEvent.Died>().AsUnitObservable(),
                    CharacterEvent.EventType.DamageToken => broker.Receive<CharacterEvent.DamageToken>().AsUnitObservable(),
                    _ => throw new ArgumentOutOfRangeException()
                };
                observable
                    .Subscribe((this, element), static (_, state) =>
                    {
                        var (character, feedbackElement) = state;
                        feedbackElement.Feedbacks.PlayAsync(character, character.destroyCancellationToken).Forget();
                    })
                    .RegisterTo(destroyCancellationToken);
            }
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
            broker.Publish(new CharacterEvent.DamageToken(damage));
            if (hitPoint.Value <= 0)
            {
                broker.Publish(new CharacterEvent.Died());
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

        [Serializable]
        public sealed class FeedbackElement
        {
            [field: SerializeField]
            public CharacterEvent.EventType EventType { get; private set; } = default;

            [field: SerializeReference, SubclassSelector]
            public IFeedback<Character>[] Feedbacks { get; private set; } = null!;
        }
    }
}
