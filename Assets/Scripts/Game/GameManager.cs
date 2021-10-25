using UnityEngine;

namespace Thicckitty
{
    [RequireComponent(typeof(AudioSource))]
    public class GameManager : EventsListener
    {
        // TODO: Implementation
    
        [Header("Win Audio")]
        [SerializeField]
        private AudioClip winGameAudio;
        [SerializeField, Range(0.0f, 1.0f)]
        private float winGameVolume = 1.0f;

        private AudioSource _audioSource;

        private AudioSource AudioSource
        {
            get
            {
                _audioSource ??= GetComponent<AudioSource>();
                return _audioSource;
            }
        }
        
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
                WinGame();
            }
        }

        private void WinGame()
        {
            if (AudioSource)
            {
                AudioSource.clip = winGameAudio;
                AudioSource.volume = winGameVolume;
                AudioSource.Play();
            }
        }
    }
}