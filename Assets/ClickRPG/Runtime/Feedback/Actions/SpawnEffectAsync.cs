using System;
using System.Threading;
using ClickRPG.EffectControllers;
using Cysharp.Threading.Tasks;
using HKFeedback;
using HKFeedback.Extensions;
using UnityEngine;

namespace ClickRPG.Feedback.Actions
{
    [Serializable]
    public class SpawnEffectAsync<TContext> : IFeedback<TContext>
    {
        [SerializeField]
        private Effect effectPrefab = null!;

        [SerializeReference, SubclassSelector]
        private IFeedback<Effect>[] feedbacks = null!;

        public UniTask PlayAsync(TContext context, CancellationToken cancellationToken)
        {
            var effect = effectPrefab.Spawn();
            return feedbacks.PlayAsync(effect, cancellationToken);
        }
    }
}
