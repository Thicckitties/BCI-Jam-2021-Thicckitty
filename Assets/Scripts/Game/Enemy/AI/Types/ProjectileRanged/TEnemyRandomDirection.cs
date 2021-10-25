using UnityEngine;

namespace Thicckitty
{
    public class TEnemyRandomDirection : ProjectileRangedAIType
    {
        private Vector3 _currentForwardDirection;
        private Vector3 _startForwardDirection;

        private float _randomDirectionCooldown = 0.0f;

        public TEnemyRandomDirection(EnemyProjectileRangedAI ai) : base(ai)
        {
            _currentForwardDirection = _startForwardDirection = ai.Transform.forward;
        }

        public override void HookEvents() { }

        public override void UnHookEvents() { }

        public override void Update(float deltaTime)
        {
            if (_randomDirectionCooldown > 0.0f)
            {
                _randomDirectionCooldown -= deltaTime;

                if (_randomDirectionCooldown <= 0.0f)
                {
                    _randomDirectionCooldown = 0.0f;
                }
            }

            float distanceToPlayerSquared = (_ai.RangedAIData.playerPosition.Value
                                             - _ai.Transform.position).sqrMagnitude;
            float rangeSquared = _ai.RangedAIData.rangeAmount * _ai.RangedAIData.rangeAmount;
            if (distanceToPlayerSquared > rangeSquared)
            {
                return;
            }

            if (_randomDirectionCooldown <= 0.0f)
            {
                ShootProjectile(_currentForwardDirection);
                _currentForwardDirection = RandomDirection(_startForwardDirection);
            }
        }

        public override void FixedUpdate(float deltaTime) { }
        
        public static Vector3 RandomDirection(Vector3 forwardVector)
        {
            float randomYaw = Random.Range(-45, 45);
            float randomPitch = Random.Range(-45, 45);
            
            Quaternion rotationToQuaternion 
                = Quaternion.Euler(0.0f, randomYaw, randomPitch);
            Vector3 newDirection = rotationToQuaternion * forwardVector;
            return newDirection.normalized;
        }
    }
}