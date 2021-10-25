using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thicckitty
{
    
    [DisallowMultipleComponent]
    public class EnemySpawner : EventsListener
    {
        public delegate void EnemySpawnerEventDelegate(EnemySpawner enemySpawner);
        public EnemySpawnerEventDelegate EnemySpawnedEvent;
        
        [Header("Spawner Variables")]
        [SerializeField, Min(0.01f)]
        private float maxDistanceFromPlayer;
        [SerializeField]
        private GameObject prefab;

        [Header("Gizmos")] 
        [SerializeField, UnityEngine.Min(0.5f)]
        private float pointSize = 0.5f;
        [SerializeField] 
        private Color enemySphereColor;
        
        [Header("SODA References")]
        [SerializeField]
        private SODA.Vector3Reference playerPosition;

        private bool _spawnedPrefab = false;

        protected override void Start()
        {
            base.Start();
            EnemySpawnerManager.AddSpawner(this);
        }

        private void OnDestroy()
        {
            EnemySpawnerManager.RemoveSpawner(this);
        }

        protected override bool HookEvents()
        {
            if (!playerPosition.HasVariable)
            {
                return false;
            }
            playerPosition.VariableChangedEvent += HandlePlayerPositionChanged;
            return true;
        }

        protected override bool UnHookEvents()
        {
            if (playerPosition.HasVariable)
            {
                playerPosition.VariableChangedEvent -= HandlePlayerPositionChanged;
            }
            return true;
        }


        private void HandlePlayerPositionChanged(Vector3 prev, Vector3 next)
        {
            float distance = (next - transform.position).sqrMagnitude;
            float powerOfMaxDist = maxDistanceFromPlayer * maxDistanceFromPlayer;
            if (distance <= powerOfMaxDist)
            {
                if (_spawnedPrefab)
                {
                    return;
                }
                if((_spawnedPrefab = SpawnEnemy()))
                {
                    EnemySpawnedEvent?.Invoke(this);
                }
            }
        }

        private bool SpawnEnemy()
        {
            if (!prefab)
            {
                return true;
            }
            GameObject.Instantiate(
                prefab, transform.position, Quaternion.identity);
            return true;
        }
        
        #if UNITY_EDITOR
        
        private void OnDrawGizmos()
        {
            Gizmos.color = enemySphereColor;
            Gizmos.DrawSphere(transform.position, pointSize);
            Gizmos.DrawWireSphere(transform.position, maxDistanceFromPlayer);
        }
        
        #endif

    }
}

