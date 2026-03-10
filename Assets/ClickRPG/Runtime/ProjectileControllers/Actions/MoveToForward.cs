using System;
using R3;
using R3.Triggers;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class MoveToForward : IProjectileAction
    {
        [SerializeField, Min(0f)]
        private float speed = 1f;

        public void Invoke(Projectile projectile)
        {
            projectile.UpdateAsObservable()
                .Subscribe((this, projectile), static (_, state) =>
                {
                    var (self, projectile) = state;
                    projectile.Rigidbody2D.position += (Vector2)(projectile.transform.right * self.speed * Time.deltaTime);
                })
                .AddTo(projectile.Scope);
        }
    }
}
