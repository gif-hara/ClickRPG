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
    }
}
