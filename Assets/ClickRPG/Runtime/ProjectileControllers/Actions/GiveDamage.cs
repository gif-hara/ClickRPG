using System;
using ClickRPG.CharacterControllers;
using R3;
using R3.Triggers;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class GiveDamage : IProjectileAction
    {
        [SerializeField]
        private Collider2D triggerCollider = null!;

        [SerializeField, Min(0f)]
        private float power = 1.0f;

        public void Invoke(Projectile projectile)
        {
            triggerCollider.OnTriggerEnter2DAsObservable()
                .Subscribe((this, projectile), static (collision, state) =>
                {
                    var (self, projectile) = state;
                    if (collision.attachedRigidbody == null)
                    {
                        return;
                    }
                    if (collision.attachedRigidbody.TryGetComponent<Character>(out var target))
                    {
                        var damage = (int)(projectile.Owner.Strength.CurrentValue * self.power);
                        target.TakeDamage(damage);
                        projectile.ConsumePenetration();
                    }
                })
                .AddTo(projectile.Scope);
        }
    }
}
