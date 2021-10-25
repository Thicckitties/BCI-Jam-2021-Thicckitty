using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Thicckitty
{

    [System.Serializable]
    public enum EnemyProjectileAimType
    {
        [InspectorName("Random Directions In Front")]
        TYPE_RANDOM_DIRECTIONS_IN_FRONT,
        [InspectorName("Change in Directions")]
        TYPE_CHANGE_IN_DIRECTION,
        [InspectorName("Aim at Position")]
        TYPE_AIM_AT_POSITION
    }
    
    [System.Serializable]
    public struct EnemyProjectileRangedAIData
    {
        [System.Serializable]
        public enum ReferenceType
        {
            [InspectorName("Use Player Position")]
            TYPE_USE_PLAYER_POSITION,
            [InspectorName("Use Transform Position")]
            TYPE_TRANSFORM_POSITION,
            [InspectorName("Use SODA Reference")]
            TYPE_SODA_POSITION
        }
        
        [SerializeField, Min(0.0f)]
        public float rangeAmount;
        [SerializeField, Min(0.0f)]
        public float projectileLaunchedOffset;
        [SerializeField]
        public SODA.Vector3Reference playerPosition;
        
        [SerializeField]
        private TProjectileComponent projectilePrefab;
        
        [SerializeField]
        public EnemyProjectileAimType aimType;
        
        [SerializeField]
        public Transform changeInDirection_DirectionOne;
        [SerializeField]
        public Transform changeInDirection_DirectionTwo;
        
        [SerializeField]
        public ReferenceType referenceType;
        [SerializeField]
        public Transform aimPosition_Transform;
        [SerializeField]
        public SODA.Vector3Reference aimPosition_SODAVec3;

        public GameObject ProjectilePrefab
        {
            get
            {
                if (projectilePrefab)
                {
                    return projectilePrefab.gameObject;
                }

                return null;
            }
        }
    }

    public abstract class ProjectileRangedAIType
    {
        public delegate void ProjectileShootEvent(Vector3 direction);
        public ProjectileShootEvent ShootProjectileEvent;
        
        protected readonly EnemyProjectileRangedAI _ai;
        
        public ProjectileRangedAIType(EnemyProjectileRangedAI ai) 
        {
            _ai = ai;
        }

        public abstract void HookEvents();

        public abstract void UnHookEvents();

        public abstract void Update(float deltaTime);

        public abstract void FixedUpdate(float deltaTime);

        protected void ShootProjectile(Vector3 direction)
        {
            ShootProjectileEvent?.Invoke(direction);
        }

        public static ProjectileRangedAIType Create(EnemyProjectileRangedAI rangedAI)
        {
            switch (rangedAI.RangedAIData.aimType)
            {
                case EnemyProjectileAimType.TYPE_AIM_AT_POSITION:
                    break;
                case EnemyProjectileAimType.TYPE_RANDOM_DIRECTIONS_IN_FRONT:
                    break;
            }
            return null;
        }
    }
    
    public class EnemyProjectileRangedAI : AEnemyAIControllerType
    {
        private ProjectileRangedAIType _rangedAIType;
        public EnemyProjectileRangedAIData RangedAIData
            => _component.RangedAIData;

        public EnemyAIComponent Component => _component;
        
        public EnemyProjectileRangedAI(EnemyAIComponent component) 
            : base(component)
        {
            _rangedAIType = ProjectileRangedAIType.Create(this);
        }

        public override void HookEvents()
        {
            _rangedAIType?.HookEvents();

            if (_rangedAIType != null)
            {
                _rangedAIType.ShootProjectileEvent += ShootProjectile;
            }
        }

        public override void UnHookEvents()
        {
            _rangedAIType?.UnHookEvents();

            if (_rangedAIType != null)
            {
                _rangedAIType.ShootProjectileEvent -= ShootProjectile;
            }
        }

        public override void FixedUpdate(float deltaTime)
        {
            _rangedAIType?.FixedUpdate(deltaTime);
        }

        public override void Update(float deltaTime)
        {
            _rangedAIType?.Update(deltaTime);
        }

        private void ShootProjectile(Vector3 direction)
        {
            if (!RangedAIData.ProjectilePrefab)
            {
                return;
            }

            Vector3 pos = Transform.forward * RangedAIData.projectileLaunchedOffset
                              + Transform.position;
            GameObject projectile = GameObject.Instantiate(RangedAIData.ProjectilePrefab,
                pos, Quaternion.identity);
            if (projectile == null)
            {
                return;
            }
            projectile.transform.LookAt(pos + direction);
        }

#if UNITY_EDITOR
        
        public override void OnDrawGizmos()
        {
        }
        
#endif
        
    }
}