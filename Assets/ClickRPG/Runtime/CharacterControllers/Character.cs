using System;
using System.Collections.Generic;
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

        private readonly Dictionary<Type, ICharacterAbility> abilityMap = new();

        private readonly MessageBroker broker = new();

        public IMessageBroker Broker => broker;

        private readonly CharacterBrainController brainController = new();

        void Awake()
        {
            foreach (var ability in abilities)
            {
                ability.Initialize(this);
                abilityMap[ability.GetType()] = ability;
            }
            brainController.Setup(this, brain);
        }

        public T GetAbility<T>() where T : ICharacterAbility
        {
            return (T)abilityMap[typeof(T)];
        }

        public bool TryGetAbility<T>(out T ability) where T : ICharacterAbility
        {
            if (abilityMap.TryGetValue(typeof(T), out var foundAbility))
            {
                ability = (T)foundAbility;
                return true;
            }
            ability = default!;
            return false;
        }
    }
}
