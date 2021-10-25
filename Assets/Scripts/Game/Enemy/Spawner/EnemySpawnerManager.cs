using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{

    public static class EnemySpawnerManager
    {

        private static List<EnemySpawner> _enemySpawner
            = new List<EnemySpawner>();

        public static int NumberOfSpawners => _enemySpawner.Count;

        public static void AddSpawner(EnemySpawner enemySpawner)
        {
            if (!_enemySpawner.Contains(enemySpawner))
            {
                _enemySpawner.Add(enemySpawner);
                enemySpawner.EnemySpawnedEvent += SpawnerSpawnedPrefabEvent;
            }
        }

        public static void RemoveSpawner(EnemySpawner enemySpawner)
        {
            if (_enemySpawner.Contains(enemySpawner))
            {
                _enemySpawner.Remove(enemySpawner);
                enemySpawner.EnemySpawnedEvent -= SpawnerSpawnedPrefabEvent;
            }
        }

        private static void SpawnerSpawnedPrefabEvent(EnemySpawner enemySpawner)
        {
            RemoveSpawner(enemySpawner);
        }
    }
}