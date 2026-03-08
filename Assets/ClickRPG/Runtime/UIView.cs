using UnityEngine;

namespace ClickRPG
{
    public abstract class UIView : MonoBehaviour
    {
        public abstract void Activate(MainSceneContext context);
    }
}
