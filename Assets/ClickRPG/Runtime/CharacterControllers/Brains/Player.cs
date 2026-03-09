using System;
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

        public void Attach(Character character, CompositeDisposable scope)
        {
            character.UpdateAsObservable()
                .Subscribe(this, static (_, @this) =>
                {
                    var mousePosition = Mouse.current.position.ReadValue();
                    var worldPosition = @this.worldCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, @this.worldCamera.nearClipPlane));

                    var direction = worldPosition - @this.muzzleObject.position;
                    var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    @this.muzzleObject.rotation = Quaternion.Euler(0, 0, angle - 90);
                })
                .AddTo(scope);
        }
    }
}
