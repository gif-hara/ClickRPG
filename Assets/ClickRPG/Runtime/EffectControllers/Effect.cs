using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Cysharp.Threading.Tasks.Triggers;
using HKFeedback;
using HKFeedback.Extensions;
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
        [SerializeReference, SubclassSelector]
        private IFeedback<Effect>[] onEnableFeedbacks = null!;

        Transform IProvider<Transform>.Provide() => transform;

        IDisposable IProvider<IDisposable>.Provide() => this;

        ObjectPool<Effect> pool;

        private CancellationTokenSource lifetimeScope;

        public Effect Spawn()
        {
            pool ??= new ObjectPool<Effect>(
                createFunc: () => Instantiate(this),
                actionOnRelease: effect => effect.gameObject.SetActive(false),
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

        private void OnEnable()
        {
            lifetimeScope = new CancellationTokenSource();
            onEnableFeedbacks.PlayAsync(this, lifetimeScope.Token).Forget();
        }

        private void OnDisable()
        {
            lifetimeScope.Cancel();
        }
    }
}
