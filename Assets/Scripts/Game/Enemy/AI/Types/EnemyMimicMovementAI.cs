using UnityEngine;

namespace Thicckitty
{
    
    [System.Serializable]
    public enum EnemyAxisSettingsType
    {
        [InspectorName("Normal Axis")]
        AXIS_NORMAL,
        [InspectorName("Lock Axis")]
        AXIS_LOCKED,
        [InspectorName("Opposite Axis")]
        AXIS_OPPOSITE
    }

    [System.Serializable]
    public struct EnemyMimicMovementAIData
    {
        [SerializeField, Min(0.001f)]
        private float distanceThreshold;
        [SerializeField]
        public SODA.Vector3Reference targetPosition;
        [SerializeField]
        private EnemyAxisSettingsType xAxis;
        [SerializeField]
        private EnemyAxisSettingsType yAxis;
        [SerializeField]
        private EnemyAxisSettingsType zAxis;

        public float DistanceThreshold => distanceThreshold;

        public void ApplyLockedValues(ref Vector3 vector)
        {
            vector.x = ApplyToAxis(vector.x, xAxis);
            vector.y = ApplyToAxis(vector.y, yAxis);
            vector.z = ApplyToAxis(vector.z, zAxis);
        }

        private static float ApplyToAxis(in float axisValue, in EnemyAxisSettingsType axis)
        {
            switch (axis)
            {
                case EnemyAxisSettingsType.AXIS_LOCKED:
                    return 0.0f;
                case EnemyAxisSettingsType.AXIS_OPPOSITE:
                    return -axisValue;
            }

            return axisValue;
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
            if (!IsEnabled)
            {
                return;
            }
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

#if UNITY_EDITOR
        public override void OnDrawGizmos() { }

#endif
    }
}