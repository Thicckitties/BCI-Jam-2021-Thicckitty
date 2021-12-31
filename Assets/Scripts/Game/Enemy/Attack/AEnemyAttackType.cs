using UnityEngine;

namespace Thicckitty
{

    public interface IEnemyAttackComponent
    {
        RangedEnemyTypeData RangedEnemyTypeData
        {
            get;
        }

        Transform Transform
        {
            get;
        }

        Vector3 TargetPosition
        {
            get;
        }
    }

    [System.Serializable]
    public enum EnemyAttackType
    {
        TYPE_NONE,
        TYPE_RANGED_ENEMY
    }    
    
    public abstract class AEnemyAttackType
    {
        public delegate void EnemyAttackEventDelegate(AEnemyAttackType type);
        public EnemyAttackEventDelegate EnemyBeginAttack;
        public EnemyAttackEventDelegate EnemyEndAttack;

        public delegate void EnemySetAttackingEventDelegate(AEnemyAttackType type, bool attacking);
        public EnemySetAttackingEventDelegate EnemySetAttacking;
        
        protected readonly IEnemyAttackComponent _component;
        private bool _attacking = false, _enabled = true;

        public bool IsAttacking => _attacking;

        public bool IsEnabled => _enabled;
        
        public AEnemyAttackType(IEnemyAttackComponent component)
        {
            _component = component;
        }
        
        public virtual void HookEvents() { }
        
        public virtual void UnHookEvents() { }

        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }

        public virtual void SetAttacking(bool attacking)
        {
            if (_attacking != attacking)
            {
                EnemySetAttacking?.Invoke(this, attacking);
            }
            _attacking = attacking;
        }

        /// <summary>
        /// Actually begins the attack animation per enemy type.
        /// </summary>
        public void BeginAttack()
        {
            if (IsEnabled
                && IsAttacking)
            {
                EnemyBeginAttack?.Invoke(this);
            }
        }

        /// <summary>
        /// Listens when the attack animation is completed.
        /// </summary>
        public void EndAttack()
        {
            if (IsEnabled
                && IsAttacking)
            {
                EnemyEndAttack?.Invoke(this);
            }
        }

        /// <summary>
        /// Actually applies the attack to the target.
        /// </summary>
        public abstract void ApplyAttack();

        public abstract void OnUpdate(float deltaTime);
        
        public abstract EnemyAttackType GetAttackType();

        public static AEnemyAttackType Create(EnemyAttackType type,
            IEnemyAttackComponent component)
        {
            switch (type)
            {
                case EnemyAttackType.TYPE_RANGED_ENEMY:
                    return new RangedEnemyType(component);
            }
            return null;
        }
    }
}