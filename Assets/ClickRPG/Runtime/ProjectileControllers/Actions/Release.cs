using System;
using R3;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class Release : IProjectileAction
    {
        [SerializeField, Min(0f)]
        private float delay = 0f;

        public void Invoke(Projectile projectile)
        {
            if (delay <= 0f)
            {
                projectile.Release();
                return;
            }

            Observable.Timer(TimeSpan.FromSeconds(delay))
                .Subscribe(projectile, static (_, projectile) => projectile.Release())
                .AddTo(projectile.Scope);
        }
    }
}
