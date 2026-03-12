using System.Collections.Generic;
using ClickRPG.CharacterControllers;
using ClickRPG.ProjectileControllers.Actions;
using R3;
using UnityEngine;
using UnityEngine.Pool;

namespace ClickRPG.ProjectileControllers
{
    public sealed class Projectile : MonoBehaviour
    {
        [field: SerializeField]
        public Rigidbody2D Rigidbody2D { get; private set; } = null!;

        [SerializeField, Min(1)]
        private int penetrationCount = 1;

        [SerializeReference, SubclassSelector]
        private List<IProjectileAction> actions = null!;

        private ObjectPool<Projectile> pool = null!;

        public CompositeDisposable Scope { get; } = new();

        public Character Owner { get; private set; } = null!;

        public Character Target { get; set; } = null!;

        private int currentPenetrationCount;

        public Projectile Spawn(Character owner, Vector3 position, Quaternion rotation)
        {
            pool ??= new ObjectPool<Projectile>(
                createFunc: () => Instantiate(this),
                actionOnRelease: projectile =>
                {
                    projectile.gameObject.SetActive(false);
                    projectile.Scope.Clear();
                },
                actionOnGet: projectile => projectile.gameObject.SetActive(true)
                );
            var instance = pool.Get();
            instance.pool = pool;
            instance.Owner = owner;
            instance.Target = null;
            instance.currentPenetrationCount = instance.penetrationCount;
            instance.transform.SetPositionAndRotation(position, rotation);
            foreach (var action in instance.actions)
            {
                action.Invoke(instance);
            }
            return instance;
        }

        public void ConsumePenetration()
        {
            currentPenetrationCount--;
            if (currentPenetrationCount <= 0)
            {
                Release();
            }
        }

        public void Release()
        {
            pool.Release(this);
        }
    }
}
