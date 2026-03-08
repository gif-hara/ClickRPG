using UnityEngine;

namespace ClickRPG
{
    public sealed class MainSceneController : MonoBehaviour
    {
        [SerializeField]
        private UIView[] uiViews = null!;

        private void Start()
        {
            var context = new MainSceneContext();
            foreach (var uiView in uiViews)
            {
                uiView.Activate(context);
            }
        }
    }
}
