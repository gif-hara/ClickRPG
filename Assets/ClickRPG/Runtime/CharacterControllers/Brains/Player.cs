using System;
using ClickRPG.ProjectileControllers;
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
        private Transform firePointObject = null!;

        [SerializeField]
        private Projectile projectilePrefab = null!;

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

                    if (Mouse.current.leftButton.wasPressedThisFrame)
                    {
                        var projectile = @this.projectilePrefab.Spawn(character);
                        projectile.transform.position = @this.firePointObject.position;
                        projectile.transform.rotation = @this.firePointObject.rotation;
                    }
                })
                .AddTo(scope);
        }
    }
}
