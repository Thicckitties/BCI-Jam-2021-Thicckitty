using System.Numerics;
using UnityEngine;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thicckitty
{
    [System.Serializable]
    public class RangedEnemyTypeData
    {
        [SerializeField]
        public TProjectileComponent projectilePrefab;
        [SerializeField]
        public Transform projectileLaunchPosition;
        [SerializeField, Min(0.0f)]
        private float projectileAdditionalForce = 0.0f;
        [SerializeField, Min(0.0f)]
        private float projectileForceMultiplier = 1.0f;
        [SerializeField]
        private bool hasUpMotion;
        [SerializeField, Range(0.0f, 90.0f)]
        private float upRotation = 0.0f;
        
        [SerializeField]
        private bool hasMaxRange = true;
        [SerializeField, Min(1.0f)]
        private float maxRange = 1.0f;
        
        [SerializeField]
        private bool mustSeeTarget = false;
        [SerializeField, Range(-1.0f, 1.0f)]
        private float minTargetViewThreshold = 0.5f;

        [SerializeField, Min(0.001f)]
        private float minCooldownSeconds = 0.5f;
        [SerializeField, Min(0.001f)]
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
        [SerializeField, Min(0.001f)]
        private float minBetweenShotsCooldown = 0.001f;
        [SerializeField, Min(0.001f)]
        private float maxBetweenShotsCooldown = 0.001f;
        [SerializeField, Min(0.001f)]
        private float betweenShotsCooldown = 0.001f;
        
        
        public bool HasMaxRange => hasMaxRange;
        public float MaxRange => maxRange;
        public bool MustSeeTarget => mustSeeTarget;

        public float MinTargetViewThreshold => minTargetViewThreshold;

        public float UpRotation => hasUpMotion ? upRotation : 0.0f;
        public float AdditionalProjectileForce => projectileAdditionalForce;
        public float ProjectileForceMultiplier => projectileForceMultiplier;
        
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

                if (!_component.IsControlledByAnimations)
                {
                    ApplyAttack();
                    EndAttack();
                }
            }
        }

        public override void ApplyAttack()
        {
            if (IsEnabled
                && IsAttacking)
            {
                // Spawns the projectile and have it face player.
                Transform spawnPosition = RangedEnemyTypeData.projectileLaunchPosition ? RangedEnemyTypeData.projectileLaunchPosition : _component.Transform;
                GameObject instantiated = GameObject.Instantiate(RangedEnemyTypeData.projectilePrefab.gameObject,
                    spawnPosition.position, Quaternion.identity);
                if (instantiated)
                {
                    instantiated.transform.LookAt(_component.TargetPosition);

                    Vector3 diff = _component.TargetPosition - spawnPosition.position;
                    float distanceSquared = diff.sqrMagnitude;
                    TProjectileComponent component = instantiated.GetComponent<TProjectileComponent>();

                    Vector3 rightRotVector = instantiated.transform.right;
                    Quaternion rotation = Quaternion.AngleAxis(
                        RangedEnemyTypeData.UpRotation, rightRotVector.normalized);
                    instantiated.transform.rotation *= rotation;
                    
                    component.Rigidbody.AddForce(instantiated.transform.forward
                        * (Mathf.Sqrt(distanceSquared) * RangedEnemyTypeData.ProjectileForceMultiplier 
                           + RangedEnemyTypeData.AdditionalProjectileForce),
                        ForceMode.Impulse);
                }
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

            if (!IsAttacking
                && UpdateCooldownSeconds(deltaTime))
            {
                SetAttacking(true);
            }
            else
            {
                if (_currentSecondsBetweenShots > 0.0f)
                {
                    _currentSecondsBetweenShots -= deltaTime;
                }
                if (_currentSecondsBetweenShots <= 0.0f)
                {
                    BeginAttack();
                }
            }
        }

        #if UNITY_EDITOR
        
        public override void OnDrawGizmos()
        {
            if (RangedEnemyTypeData.projectileLaunchPosition)
            {
                Gizmos.color = CanAttackTarget() ? Color.red : Color.blue;
                Gizmos.DrawSphere(RangedEnemyTypeData.projectileLaunchPosition.position, 0.5f);

                if (RangedEnemyTypeData.HasMaxRange)
                {
                    Handles.color = Gizmos.color;
                    Handles.DrawWireDisc(RangedEnemyTypeData.projectileLaunchPosition.position,
                        Vector3.up, RangedEnemyTypeData.MaxRange);
                }
            }
        }
        
        #endif

        private bool UpdateCooldownSeconds(float deltaTime)
        {
            if (_currentCooldownSeconds > 0.0f)
            {
                _currentCooldownSeconds -= deltaTime;
            }
            if (_currentCooldownSeconds <= 0.0f)
            {
                _currentCooldownSeconds = 0.0f;
                return true;
            }
            return false;
        }

        public bool CanAttackTarget()
        {
            Vector3 currentPosition = _component.Transform.position;
            Vector3 diff = _component.TargetPosition - currentPosition;
            
            if (RangedEnemyTypeData.HasMaxRange
                && diff.magnitude > RangedEnemyTypeData.MaxRange)
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