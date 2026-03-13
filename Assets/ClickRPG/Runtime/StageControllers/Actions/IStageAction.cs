using System.Threading;
using ClickRPG.CharacterControllers;
using Cysharp.Threading.Tasks;

namespace ClickRPG.StageControllers.Actions
{
    public interface IStageAction
    {
        UniTask InvokeAsync(Character player, CancellationToken cancellationToken);
    }
}
