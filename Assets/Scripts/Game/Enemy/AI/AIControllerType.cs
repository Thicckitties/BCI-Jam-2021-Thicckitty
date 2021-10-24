using UnityEngine;

namespace Thicckitty
{
    [System.Serializable]
    public enum AIControllerType
    {
        [InspectorName("Back and Forth")]
        CONTROLLER_TYPE_BACK_AND_FORTH,
        [InspectorName("Mimic Transform Movement")]
        CONTROLLER_TYPE_MIMIC_MOVEMENT,
        [InspectorName("Spray Bottle Range")]
        SPRAY_BOTTLE_RANGE
    }

    public abstract class AEnemyAIControllerType
    {
        protected readonly EnemyAIComponent _component;
        private readonly Rigidbody _rigidbody;

        protected Rigidbody Rigidbody
            => _rigidbody;

        protected Transform Transform
            => _component.transform;

        public AEnemyAIControllerType(EnemyAIComponent component)
        {
            _component = component;
            _rigidbody = component.GetComponent<Rigidbody>();
        }

        public abstract void HookEvents();

        public abstract void UnHookEvents();
        
        public abstract void FixedUpdate(float deltaTime);
        public abstract void Update(float deltaTime);

        #if UNITY_EDITOR
        
        public abstract void OnDrawGizmos();

        #endif
        
        public static AEnemyAIControllerType Create(EnemyAIComponent component)
        {
            switch (component.ControllerType)
            {
                case AIControllerType.CONTROLLER_TYPE_BACK_AND_FORTH:
                    return new EnemyBackAndForthAI(component);
            }
            return null;
        }
    }
}