using UnityEngine;

namespace Thicckitty
{
    public class GameManager : EventsListener
    {
        // TODO: Implementation
        
        protected override bool HookEvents()
        {
            EnemyManager.EnemyRemovedEventDelegate += EnemyRemovedEvent;
            return true;
        }

        protected override bool UnHookEvents()
        {
            EnemyManager.EnemyRemovedEventDelegate -= EnemyRemovedEvent;
            return true;
        }

        private void EnemyRemovedEvent(EnemyAIComponent enemy)
        {
            if (EnemyManager.NumberOfEnemies <= 0
                && EnemySpawnerManager.NumberOfSpawners <= 0)
            {
            }
        }
    }
}