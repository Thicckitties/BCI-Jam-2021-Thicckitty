using UnityEngine;

namespace Thicckitty
{
    public interface IEnemyStunComponent
    {

        bool IsControlledByAnimations
        {
            get;
        }
    }

    public class EnemyStunAI
    {
        public delegate void EnemyStunEvent(EnemyStunAI ai);

        public EnemyStunEvent StunBeginEvent;
        public EnemyStunEvent StunEndEvent;

        private float _stunTime = 0.0f;
        private bool _stunned = false;

        private IEnemyStunComponent _stunComponent;
        
        public bool IsStunned => _stunned;

        public EnemyStunAI(IEnemyStunComponent stunComponent)
        {
            _stunComponent = stunComponent;
        }
        
        public void OnUpdate(float deltaTime)
        {
            if (!_stunComponent.IsControlledByAnimations
                && IsStunned
                && _stunTime > 0.0f)
            {
                _stunTime -= deltaTime;

                if (_stunTime <= 0.0f)
                {
                    SetStunned(false, 0.0f);
                }
            }
        }

        public void SetStunned(bool stunned, float stunTime)
        {
            if (_stunned != stunned)
            {
                if (stunned)
                {
                    if (!_stunComponent.IsControlledByAnimations)
                    {
                        _stunTime = Mathf.Max(stunTime, 0.001f);
                    }
                    StunBeginEvent?.Invoke(this);
                }
                else
                {
                    _stunTime = 0.0f;
                    StunEndEvent?.Invoke(this);
                }
                _stunned = stunned;
            }
        }
    }
}