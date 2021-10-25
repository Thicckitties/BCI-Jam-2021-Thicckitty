using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{

    public static class EnemyManager
    {
        public delegate void EnemyManagerEventDelegate(EnemyAIComponent component);
        public static EnemyManagerEventDelegate EnemyRemovedEventDelegate;
        public static EnemyManagerEventDelegate EnemyAddedEventDelegate;

        private static List<EnemyAIComponent> _enemiesSpawned
            = new List<EnemyAIComponent>();

        public static int NumberOfEnemies => _enemiesSpawned.Count;

        public static void AddEnemy(EnemyAIComponent component)
        {
            if (!_enemiesSpawned.Contains(component))
            {
                _enemiesSpawned.Add(component);
                EnemyAddedEventDelegate?.Invoke(component);
            }
        }

        public static void RemoveEnemy(EnemyAIComponent component)
        {
            if (_enemiesSpawned.Contains(component))
            {
                _enemiesSpawned.Remove(component);
                EnemyRemovedEventDelegate?.Invoke(component);
            }
        }
    }
}