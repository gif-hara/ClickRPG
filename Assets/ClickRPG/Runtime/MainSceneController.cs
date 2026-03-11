using ClickRPG.CharacterControllers;
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

        private void Start()
        {
            var context = new MainSceneContext();
            foreach (var uiView in uiViews)
            {
                uiView.Activate(context);
            }
            player.ApplySkills(userData.Skills);
        }
    }
}
