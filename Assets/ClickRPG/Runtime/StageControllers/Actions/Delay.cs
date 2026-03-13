using System;
using System.Threading;
using ClickRPG.CharacterControllers;
using ClickRPG.ValueSelectors;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClickRPG.StageControllers.Actions
{
    [Serializable]
    public sealed class Delay : IStageAction
    {
        [SerializeReference, SubclassSelector]
        private IValueSelector<float> delayTime = default!;

        public UniTask InvokeAsync(Character player, CancellationToken cancellationToken)
        {
            return UniTask.Delay(TimeSpan.FromSeconds(delayTime.Value), cancellationToken: cancellationToken);
        }
    }
}
