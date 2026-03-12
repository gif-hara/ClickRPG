using System;
using UnityEngine;

namespace ClickRPG.ProjectileControllers.Actions
{
    [Serializable]
    public sealed class SetParentToOwnerSceneView : IProjectileAction
    {
        [SerializeField]
        private bool worldPositionStays = false;

        public void Invoke(Projectile projectile)
        {
            projectile.transform.SetParent(projectile.Owner.SceneView, worldPositionStays);
        }
    }
}
