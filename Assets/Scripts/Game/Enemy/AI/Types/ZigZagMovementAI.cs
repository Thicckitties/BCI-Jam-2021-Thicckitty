using UnityEngine;

namespace Thicckitty
{
    [System.Serializable]
    public class ZigZagMovementAIData
    {
        [Header("Zig Zag Parameters")]
        [SerializeField, Min(0.01f)]
        public float zigZagAmount = 0.01f;
        [SerializeField]
        public Vector3 zigZagDirection;
        [SerializeField]
        public Vector3 zigZagNormal;
        [SerializeField, Min(0.0f)]
        public float differenceThreshold;

        [Header("Gizmos")] 
        [SerializeField]
        public Color lineColor;
        [SerializeField]
        public Color pointColor;
    }
    
    public class ZigZagMovementAI : AEnemyAIControllerType
    {
        private Vector3 _targetPosition;
        private Vector3 _zigZagDirection;
        private Vector3 _originalForward;
        
        private ZigZagMovementAIData ZigZagMovement
            => _component.ZigZagAIData;

        public ZigZagMovementAI(EnemyAIComponent component)
            : base(component)
        {
            _originalForward = ZigZagMovement.zigZagNormal.normalized;
            _zigZagDirection = ZigZagMovement.zigZagDirection.normalized;
            _targetPosition = component.transform.position
                + _zigZagDirection * ZigZagMovement.zigZagAmount;
        }

        public override void HookEvents() { }

        public override void UnHookEvents() { }

        public override void FixedUpdate(float deltaTime)
        {
            if (!IsEnabled)
            {
                return;
            }
            Vector3 targetDifference = _targetPosition - Transform.position;
            float magnitude = targetDifference.magnitude;
            float differenceThreshold = ZigZagMovement.differenceThreshold;
            if (magnitude <= differenceThreshold)
            {
                _zigZagDirection = Vector3.Reflect(_zigZagDirection, _originalForward);
                _targetPosition = _component.transform.position 
                                  + _zigZagDirection.normalized * ZigZagMovement.zigZagAmount;
                return;
            }

            if (_component.GroundDetector.IsOnGround())
            {
                Rigidbody.AddForce(targetDifference.normalized * _component.AIMovementSpeed);
            }
        }

        public override void Update(float deltaTime)
        { }

        #if UNITY_EDITOR
        
        public override void OnDrawGizmos() { }
        
        #endif
    }
}