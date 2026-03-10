using ClickRPG.CharacterControllers.Abilities;
using ClickRPG.CharacterControllers.Brains;
using SoulLike;
using UnityEngine;

namespace ClickRPG.CharacterControllers
{
    public sealed class Character : MonoBehaviour
    {
        [SerializeReference, SubclassSelector]
        private ICharacterBrain brain = null!;

        [SerializeReference, SubclassSelector]
        private ICharacterAbility[] abilities = null!;

        private readonly MessageBroker broker = new();

        public IMessageBroker Broker => broker;

        private readonly CharacterBrainController brainController = new();

        void Awake()
        {
            foreach (var ability in abilities)
            {
                ability.Initialize(this);
            }
            brainController.Setup(this, brain);
        }
    }
}
