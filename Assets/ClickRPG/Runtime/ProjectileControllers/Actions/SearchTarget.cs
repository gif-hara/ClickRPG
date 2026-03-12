using System;
using ClickRPG.CharacterControllers;
using R3;
using R3.Triggers;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class SearchTarget : IProjectileAction
    {
        [SerializeField]
        private Collider2D searchCollider = null!;

        public void Invoke(Projectile projectile)
        {
            searchCollider.OnTriggerStay2DAsObservable()
                .Subscribe((this, projectile), static (collision, state) =>
                {
                    var (self, projectile) = state;
                    if (collision.attachedRigidbody == null)
                    {
                        return;
                    }
                    if (collision.attachedRigidbody.TryGetComponent<Character>(out var target))
                    {
                        if (projectile.Target == null)
                        {
                            projectile.Target = target;
                        }
                        else
                        {
                            var currentDistance = Vector3.Distance(projectile.transform.position, projectile.Target.transform.position);
                            var newDistance = Vector3.Distance(projectile.transform.position, target.transform.position);
                            if (newDistance < currentDistance)
                            {
                                projectile.Target = target;
                            }
                        }
                    }
                })
                .AddTo(projectile.Scope);
        }
    }
}
