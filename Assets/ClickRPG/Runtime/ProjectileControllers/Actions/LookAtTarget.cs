using System;
using ClickRPG.ValueSelectors;
using R3;
using R3.Triggers;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class LookAtTarget : IProjectileAction
    {
        [SerializeReference, SubclassSelector]
        private IValueSelector<float> lookAtSpeed = default!;

        public void Invoke(Projectile projectile)
        {
            projectile.UpdateAsObservable()
                .Subscribe((this, projectile, lookAtSpeed.Value), static (_, state) =>
                {
                    var (self, projectile, lookAtSpeed) = state;
                    if (projectile.Target == null)
                    {
                        return;
                    }
                    var direction = (projectile.Target.transform.position - projectile.transform.position).normalized;
                    var targetRotation = Quaternion.LookRotation(Vector3.forward, direction) * Quaternion.Euler(0, 0, 90);
                    projectile.transform.rotation = Quaternion.RotateTowards(projectile.transform.rotation, targetRotation, lookAtSpeed * Time.deltaTime);
                })
                .AddTo(projectile.Scope);
        }
    }
}
