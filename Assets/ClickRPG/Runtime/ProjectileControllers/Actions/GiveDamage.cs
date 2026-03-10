using System;
using ClickRPG.CharacterControllers;
using ClickRPG.CharacterControllers.Abilities;
using R3;
using R3.Triggers;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class GiveDamage : IProjectileAction
    {
        [SerializeField, Min(0f)]
        private float power = 1.0f;

        public void Invoke(Projectile projectile)
        {
            projectile.OnTriggerEnter2DAsObservable()
                .Subscribe((this, projectile), static (collision, state) =>
                {
                    var (self, projectile) = state;
                    if (collision.attachedRigidbody == null)
                    {
                        return;
                    }
                    if (collision.attachedRigidbody.TryGetComponent<Character>(out var target) && target.TryGetAbility<CharacterHandleStatus>(out var targetHandleStatus))
                    {
                        var ownerHandleStatus = projectile.Owner.GetAbility<CharacterHandleStatus>();
                        var damage = (int)(ownerHandleStatus.Strength.CurrentValue * self.power);
                        targetHandleStatus.TakeDamage(damage);
                        projectile.Release();
                    }
                })
                .AddTo(projectile.Scope);
        }
    }
}
