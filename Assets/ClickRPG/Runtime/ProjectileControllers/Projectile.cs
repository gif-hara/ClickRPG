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

        [SerializeReference, SubclassSelector]
        private List<IProjectileAction> actions = null!;

        private ObjectPool<Projectile> pool = null!;

        public CompositeDisposable Scope { get; } = new();

        public Character Owner { get; private set; } = null!;

        public Projectile Spawn(Character owner)
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
            foreach (var action in instance.actions)
            {
                action.Invoke(instance);
            }
            return instance;
        }

        public void Release()
        {
            pool.Release(this);
        }
    }
}
