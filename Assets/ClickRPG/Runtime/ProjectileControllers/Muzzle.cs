using System;
using ClickRPG.CharacterControllers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClickRPG.ProjectileControllers
{
    [Serializable]
    public sealed class Muzzle
    {
        [SerializeField]
        private Transform firePoint = null!;

        [SerializeField]
        private Projectile projectilePrefab = null!;

        [SerializeField, Min(0f)]
        private float coolTime = 1.0f;

        private bool canFire = true;

        public void Fire(Character owner)
        {
            if (!canFire)
            {
                return;
            }

            FireAsync(owner).Forget();
        }

        private async UniTask FireAsync(Character owner)
        {
            var projectile = projectilePrefab.Spawn(owner);
            projectile.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            canFire = false;
            await UniTask.Delay(TimeSpan.FromSeconds(coolTime), cancellationToken: owner.destroyCancellationToken);
            canFire = true;
        }
    }
}
