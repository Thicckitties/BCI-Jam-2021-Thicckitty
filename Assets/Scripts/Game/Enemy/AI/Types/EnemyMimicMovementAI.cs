using UnityEngine;

namespace Thicckitty
{
    
    [System.Serializable, System.Flags]
    public enum EnemyMimicAxisLocked
    {
        [InspectorName("Lock X Axis")]
        AXIS_LOCKED_X = 1 << 0,
        [InspectorName("Lock Y Axis")]
        AXIS_LOCKED_Y = 1 << 1,
        [InspectorName("Lock Z Axis")]
        AXIS_LOCKED_Z = 1 << 2
    }

    [System.Serializable]
    public struct EnemyMimicMovementAIData
    {
        [SerializeField, Min(0.001f)]
        private float distanceThreshold;
        [SerializeField]
        public SODA.Vector3Reference targetPosition;
        [SerializeField]
        private EnemyMimicAxisLocked axisLocked;

        public float DistanceThreshold => distanceThreshold;

        public void ApplyLockedValues(ref Vector3 vector)
        {
            if (axisLocked.HasFlag(EnemyMimicAxisLocked.AXIS_LOCKED_X))
            {
                vector.x = 0.0f;
            }

            if (axisLocked.HasFlag(EnemyMimicAxisLocked.AXIS_LOCKED_Y))
            {
                vector.y = 0.0f;
            }

            if (axisLocked.HasFlag(EnemyMimicAxisLocked.AXIS_LOCKED_Z))
            {
                vector.z = 0.0f;
            }
        }
    }
    
    public class EnemyMimicMovementAI : AEnemyAIControllerType
    {

        private Vector3 _targetPrevPosition;

        private EnemyMimicMovementAIData MimicMovementAIData
            => _component.MimicMovementData;
        
        private Vector3 TargetPosition
            => MimicMovementAIData.targetPosition.Value;
        
        public EnemyMimicMovementAI(EnemyAIComponent component) 
            : base(component)
        {
            _targetPrevPosition = TargetPosition;
        }

        public override void HookEvents() { }

        public override void UnHookEvents() { }

        public override void FixedUpdate(float deltaTime)
        {
            Vector3 difference = TargetPosition - _targetPrevPosition;
            Vector3 direction = difference;
            MimicMovementAIData.ApplyLockedValues(ref direction);
            direction.Normalize();

            float distanceSquared = difference.sqrMagnitude;
            float minimumDistance = MimicMovementAIData.DistanceThreshold
                                    * MimicMovementAIData.DistanceThreshold;
            if (distanceSquared <= minimumDistance)
            {
                return;
            }

            if (_component.GroundDetector.IsOnGround())
            {
                Rigidbody.AddForce(direction * _component.AIMovementSpeed);
            }
            _targetPrevPosition = TargetPosition;
        }

        public override void Update(float deltaTime) { }

        public override void OnDrawGizmos() { }
    }
}