using System;
using LitMotion;
using LitMotion.Extensions;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class TweenScale : IProjectileAction
    {
        [SerializeField]
        private Transform target = null!;

        [SerializeField]
        private Vector3 from = Vector3.zero;

        [SerializeField]
        private Vector3 to = Vector3.one;

        [SerializeField]
        private float duration = 1.0f;

        [SerializeField]
        private Ease ease = default;

        public void Invoke(Projectile projectile)
        {
            LMotion.Create(from, to, duration)
                .WithEase(ease)
                .BindToLocalScale(target)
                .AddTo(projectile);
        }
    }
}
