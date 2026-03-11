using System;
using System.Collections.Generic;
using ClickRPG.ProjectileControllers;
using HK;
using R3;
using R3.Triggers;
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClickRPG.CharacterControllers.Brains
{
    [Serializable]
    public sealed class Player : ICharacterBrain
    {
        [SerializeField]
        private Camera worldCamera = null!;

        [SerializeField]
        private Transform muzzleObject = null!;

        [SerializeField]
        private CharacterSkillType[] muzzleSkillTypes = null!;

        [SerializeField]
        private MuzzleElement.DictionaryList muzzleElements = null!;

        public void Attach(Character character, CompositeDisposable scope)
        {
            character.UpdateAsObservable()
                .Subscribe((this, character), static (_, state) =>
                {
                    var (@this, character) = state;
                    var mousePosition = Mouse.current.position.ReadValue();
                    var worldPosition = @this.worldCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, @this.worldCamera.nearClipPlane));

                    var direction = worldPosition - @this.muzzleObject.position;
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    @this.muzzleObject.rotation = Quaternion.Euler(0, 0, angle);

                    if (Mouse.current.leftButton.isPressed)
                    {
                        foreach (var muzzleSkillType in @this.muzzleSkillTypes)
                        {
                            if (character.SkillPowers.TryGetValue(muzzleSkillType, out var power) && power > 0)
                            {
                                var muzzleElement = @this.muzzleElements.Get(muzzleSkillType);
                                if (muzzleElement != null)
                                {
                                    var levelIndex = Mathf.Min(power - 1, muzzleElement.Levels.Length - 1);
                                    var muzzleLevelElement = muzzleElement.Levels[levelIndex];
                                    foreach (var muzzle in muzzleLevelElement.Muzzles)
                                    {
                                        muzzle.Fire(character);
                                    }
                                }
                            }
                        }
                    }
                })
                .AddTo(scope);
        }

        [Serializable]
        public sealed class MuzzleElement
        {
            [field: SerializeField]
            public CharacterSkillType Type { get; private set; } = default!;

            [field: SerializeField]
            public MuzzleLevelElement[] Levels { get; private set; } = null!;

            [Serializable]
            public sealed class DictionaryList : DictionaryList<CharacterSkillType, MuzzleElement>
            {
                public DictionaryList() : base(element => element.Type)
                {
                }
            }
        }

        [Serializable]
        public sealed class MuzzleLevelElement
        {
            [field: SerializeField]
            public List<Muzzle> Muzzles { get; private set; } = new();
        }
    }
}
