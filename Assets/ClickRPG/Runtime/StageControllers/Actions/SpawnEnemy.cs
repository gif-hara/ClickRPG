using System;
using System.Threading;
using ClickRPG.CharacterControllers;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ClickRPG.StageControllers.Actions
{
    [Serializable]
    public sealed class SpawnEnemy : IStageAction
    {
        [SerializeField]
        private Character enemyPrefab = null!;

        [SerializeField, Min(0f)]
        private float radius = 5.0f;

        [SerializeField, Min(1f)]
        private int spawnCount = 1;

        public UniTask InvokeAsync(Character player, CancellationToken cancellationToken)
        {
            for (int i = 0; i < spawnCount; i++)
            {
                var spawnPosition = (Vector2)player.transform.position + UnityEngine.Random.insideUnitCircle * radius;
                enemyPrefab.Spawn(spawnPosition);
            }
            return UniTask.CompletedTask;
        }
    }
}
