using System.Threading;
using ClickRPG.CharacterControllers;
using ClickRPG.StageControllers.Actions;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClickRPG.StageControllers
{
    [CreateAssetMenu(fileName = "StageActionBundle", menuName = "ClickRPG/StageActionBundle")]
    public class StageActionBundle : ScriptableObject
    {
        [SerializeReference, SubclassSelector]
        private IStageAction[] actions = default!;

        public async UniTask InvokeAsync(Character player, int startIndex, CancellationToken cancellationToken)
        {
            for (int i = startIndex; i < actions.Length; i++)
            {
                await actions[i].InvokeAsync(player, cancellationToken);
            }
        }
    }
}
