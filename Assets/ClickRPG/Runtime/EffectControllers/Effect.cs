using System;
using HKFeedback;
using UnityEngine;
using UnityEngine.Pool;

namespace ClickRPG.EffectControllers
{
    public sealed class Effect :
        MonoBehaviour,
        IDisposable,
        IProvider<Transform>,
        IProvider<IDisposable>
    {
        Transform IProvider<Transform>.Provide() => transform;

        IDisposable IProvider<IDisposable>.Provide() => this;

        ObjectPool<Effect> pool;

        public Effect Spawn()
        {
            pool ??= new ObjectPool<Effect>(
                createFunc: () => Instantiate(this),
                actionOnRelease: effect =>
                {
                    effect.gameObject.SetActive(false);
                },
                actionOnGet: effect => effect.gameObject.SetActive(true)
                );
            var instance = pool.Get();
            instance.pool = pool;
            return instance;
        }

        public void Dispose()
        {
            if (pool != null)
            {
                pool.Release(this);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}
