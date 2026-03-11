using System;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class SetAngleRandom : IProjectileAction
    {
        [SerializeField, Min(0f)]
        private float range = 0.0f;

        public void Invoke(Projectile projectile)
        {
            var randomAngle = UnityEngine.Random.Range(-range, range);
            projectile.transform.Rotate(0f, 0f, randomAngle);
        }
    }
}
