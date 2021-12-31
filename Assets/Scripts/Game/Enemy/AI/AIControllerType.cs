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
        [InspectorName("Zig-Zag-Movement")]
        ZIG_ZAG_MOVEMENT
    }

    public abstract class AEnemyAIControllerType
    {
        protected readonly EnemyAIComponent _component;
        private readonly Rigidbody _rigidbody;
        private bool _enabled = true;

        public Rigidbody Rigidbody
            => _rigidbody;

        public Transform Transform
            => _component.transform;

        public bool IsEnabled => _enabled;

        public AEnemyAIControllerType(EnemyAIComponent component)
        {
            _component = component;
            _rigidbody = component.GetComponent<Rigidbody>();
        }

        public abstract void HookEvents();

        public abstract void UnHookEvents();
        
        public abstract void FixedUpdate(float deltaTime);
        public abstract void Update(float deltaTime);

        public void SetEnabled(bool enabled)
        {
            _enabled = enabled;
        }
        
        #if UNITY_EDITOR
        
        public abstract void OnDrawGizmos();

        #endif
        
        public static AEnemyAIControllerType Create(EnemyAIComponent component)
        {
            switch (component.ControllerType)
            {
                case AIControllerType.CONTROLLER_TYPE_BACK_AND_FORTH:
                    return new EnemyBackAndForthAI(component);
                case AIControllerType.CONTROLLER_TYPE_MIMIC_MOVEMENT:
                    return new EnemyMimicMovementAI(component);
                case AIControllerType.ZIG_ZAG_MOVEMENT:
                    return new ZigZagMovementAI(component);
            }
            return null;
        }
    }
}