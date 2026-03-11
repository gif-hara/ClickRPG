using System;
using ClickRPG.ValueSelectors;
using R3;
using R3.Triggers;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class MoveToForward : IProjectileAction
    {
        [SerializeReference, SubclassSelector]
        private IValueSelector<float> speed = default!;

        public void Invoke(Projectile projectile)
        {
            projectile.UpdateAsObservable()
                .Subscribe((this, projectile, speed.Value), static (_, state) =>
                {
                    var (self, projectile, speed) = state;
                    projectile.Rigidbody2D.position += (Vector2)(projectile.transform.right * speed * Time.deltaTime);
                })
                .AddTo(projectile.Scope);
        }
    }
}
