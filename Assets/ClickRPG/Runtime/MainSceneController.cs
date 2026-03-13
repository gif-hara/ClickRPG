using ClickRPG.CharacterControllers;
using ClickRPG.StageControllers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClickRPG
{
    public sealed class MainSceneController : MonoBehaviour
    {
        [SerializeField]
        private UserData userData = null!;

        [SerializeField]
        private Character player = null!;

        [SerializeField]
        private UIView[] uiViews = null!;

        [SerializeField]
        private StageActionBundle stageActionBundle = null!;

        [SerializeField]
        private int stageActionStartIndex = 0;

        private async UniTaskVoid Start()
        {
            Application.targetFrameRate = 60;
            var context = new MainSceneContext();
            foreach (var uiView in uiViews)
            {
                uiView.Activate(context);
            }
            player.ApplySkills(userData.Skills);

            await stageActionBundle.InvokeAsync(player, stageActionStartIndex, destroyCancellationToken);
        }
    }
}
