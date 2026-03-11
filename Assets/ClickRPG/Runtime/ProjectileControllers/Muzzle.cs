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

        [SerializeField, Min(1)]
        private int spawnCount = 1;

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
            for (int i = 0; i < spawnCount; i++)
            {
                var projectile = projectilePrefab.Spawn(owner);
                projectile.transform.SetPositionAndRotation(firePoint.position, firePoint.rotation);
            }
            canFire = false;
            var coolTime = Mathf.Max(this.coolTime - owner.CooldownLevel.CurrentValue * (1 / 60.0f), 0f);
            await UniTask.Delay(TimeSpan.FromSeconds(coolTime), cancellationToken: owner.destroyCancellationToken);
            canFire = true;
        }
    }
}
