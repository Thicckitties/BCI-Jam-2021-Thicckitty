using UnityEngine;

namespace Thicckitty
{
    [System.Serializable]
    public class RangedEnemyTypeData
    {
        [SerializeField]
        public GameObject projectilePrefab;
        [SerializeField]
        public Transform projectileLaunchPosition;
        
        [SerializeField]
        private bool hasMaxRange = true;
        [SerializeField, Min(1.0f)]
        private float maxRange = 1.0f;
        
        [SerializeField]
        private bool mustSeeTarget = false;
        [SerializeField, Range(-1.0f, 1.0f)]
        private float minTargetViewThreshold = 0.5f;

        [SerializeField, Min(0.0f)]
        private float minCooldownSeconds = 0.5f;
        [SerializeField, Min(0.0f)]
        private float maxCooldownSeconds = 1.0f;

        [SerializeField]
        private bool constantNumberOfProjectiles = true;
        [SerializeField, Min(1)]
        private int minProjectilesToShoot = 1;
        [SerializeField, Min((2))]
        private int maxProjectilesToShoot = 2;
        [SerializeField, Min(1)]
        private int numProjectilesToShoot = 1;

        [SerializeField]
        private bool constantBetweenShotsCooldown = true;
        [SerializeField, Min(0.0f)]
        private float minBetweenShotsCooldown = 0.0f;
        [SerializeField, Min(0.0f)]
        private float maxBetweenShotsCooldown = 0.0f;
        [SerializeField, Min(0.0f)]
        private float betweenShotsCooldown = 0.0f;

        public bool HasMaxRange => hasMaxRange;
        public float MaxRange => maxRange;
        public bool MustSeeTarget => mustSeeTarget;

        public float MinTargetViewThreshold => minTargetViewThreshold;
        
        public float GetRandomCooldown()
        {
            return Random.Range(minCooldownSeconds, maxCooldownSeconds);
        }
        
        public int GetNumberOfProjectilesToShoot()
        {
            if (constantNumberOfProjectiles)
            {
                return Random.Range(minProjectilesToShoot,
                    maxProjectilesToShoot);
            }
            return numProjectilesToShoot;
        }

        public float GetBetweenShotsCooldown()
        {
            if (constantBetweenShotsCooldown)
            {
                return Random.Range(minBetweenShotsCooldown, 
                    maxBetweenShotsCooldown);
            }
            return betweenShotsCooldown;
        }
    }
    
    
    public class RangedEnemyType : AEnemyAttackType
    {
        private float _currentCooldownSeconds = 0.0f;
        private float _currentSecondsBetweenShots = 0.0f;
        
        private int _currentNumberOfProjectiles = 0;

        private RangedEnemyTypeData RangedEnemyTypeData
            => _component.RangedEnemyTypeData;
        
        public RangedEnemyType(IEnemyAttackComponent component) 
            : base(component)
        {
            _currentCooldownSeconds = RangedEnemyTypeData.GetRandomCooldown();
        }

        public override void HookEvents()
        {
            EnemyBeginAttack += HandleAttackBegin;
            EnemyEndAttack += HandleAttackEnd;
        }

        public override void UnHookEvents()
        {
            EnemyBeginAttack -= HandleAttackBegin;
            EnemyEndAttack -= HandleAttackEnd;
        }

        public override void SetAttacking(bool attacking)
        {
            if (IsEnabled
                && attacking != IsAttacking)
            {
                if (attacking)
                {
                    _currentCooldownSeconds = 0.0f;
                    _currentNumberOfProjectiles = RangedEnemyTypeData.GetNumberOfProjectilesToShoot();
                }
                else
                {
                    _currentNumberOfProjectiles = 0;
                    _currentCooldownSeconds = RangedEnemyTypeData.GetRandomCooldown();
                    _currentSecondsBetweenShots = 0.0f;
                }
                base.SetAttacking(attacking);
            }
        }

        private void HandleAttackBegin(AEnemyAttackType type)
        {
            if (IsEnabled
                && IsAttacking)
            {
                _currentSecondsBetweenShots = 0.0f;
            }
        }

        public override void ApplyAttack()
        {
            if (IsEnabled
                && IsAttacking)
            {
                // Spawns the projectile and have it face player.
                Transform spawnPosition = RangedEnemyTypeData.projectileLaunchPosition ? RangedEnemyTypeData.projectileLaunchPosition : _component.Transform;
                GameObject direction = GameObject.Instantiate(RangedEnemyTypeData.projectilePrefab,
                    spawnPosition.position, Quaternion.identity);
                direction.transform.LookAt(_component.TargetPosition);
                
                _currentNumberOfProjectiles--;
            }
        }

        private void HandleAttackEnd(AEnemyAttackType type)
        {
            if (IsEnabled
                && IsAttacking)
            {
                if (_currentNumberOfProjectiles <= 0)
                {
                    SetAttacking(false);
                    return;
                }
                _currentSecondsBetweenShots = RangedEnemyTypeData.GetBetweenShotsCooldown();
            }
        }

        public override void OnUpdate(float deltaTime)
        {
            if (!IsEnabled)
            {
                return;
            }
            if (!CanAttackTarget())
            {
                if (IsAttacking)
                {
                    SetAttacking(false);
                }
                UpdateCooldownSeconds(deltaTime);
                return;
            }

            if (UpdateCooldownSeconds(deltaTime))
            {
                SetAttacking(true);
            }
            else
            {
                if (_currentSecondsBetweenShots > 0.0f)
                {
                    _currentSecondsBetweenShots -= deltaTime;
                    if (_currentSecondsBetweenShots < 0.0f)
                    {
                        BeginAttack();
                    }
                }
            }
        }

        private bool UpdateCooldownSeconds(float deltaTime)
        {
            if (_currentCooldownSeconds > 0.0f)
            {
                _currentCooldownSeconds -= deltaTime;

                if (_currentCooldownSeconds <= 0.0f)
                {
                    _currentCooldownSeconds = 0.0f;
                    return true;
                }
            }
            return false;
        }

        public bool CanAttackTarget()
        {
            Vector3 currentPosition = _component.Transform.position;
            Vector3 diff = _component.TargetPosition - currentPosition;
            if (RangedEnemyTypeData.HasMaxRange
                && diff.sqrMagnitude > RangedEnemyTypeData.MaxRange * RangedEnemyTypeData.MaxRange)
            {
                return false;
            }
            if (!RangedEnemyTypeData.MustSeeTarget)
            {
                return true;
            }
            float dotProduct = Vector3.Dot(_component.Transform.forward,
                diff.normalized);
            return dotProduct >= RangedEnemyTypeData.MinTargetViewThreshold;
        }

        public override EnemyAttackType GetAttackType()
        {
            return EnemyAttackType.TYPE_RANGED_ENEMY;
        }
    }
}