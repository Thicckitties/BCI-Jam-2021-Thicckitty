using MiscUtil.Collections.Extensions;
using UnityEngine;
using Debug = System.Diagnostics.Debug;

namespace Thicckitty
{
    [System.Serializable]
    public class EnemyBackAndForthAIData
    {
        [System.Serializable]
        public enum BackAndForthAIReferenceType
        {
            [InspectorName("Transform References")]
            TYPE_TRANSFORM_REFERENCES,
            [InspectorName("Start Position Offsets")]
            TYPE_START_POSITION_OFFSETS
        }

        [System.Serializable]
        public struct ZigZagPosition
        {
            [SerializeField]
            private float x;
            [SerializeField]
            private float z;
            [SerializeField]
            private float yOffset;

            public Vector3 ToOffset(float y)
            {
                return new Vector3(x, y + yOffset, z);
            }
        }
        
        [SerializeField, Min(0.001f)]
        private float minDistance = 0.1f;
        [SerializeField]
        private BackAndForthAIReferenceType referenceType;
        [SerializeField]
        private Transform transformRefA;
        [SerializeField]
        private Transform transformRefB;
        [SerializeField]
        private ZigZagPosition offsetA;
        [SerializeField]
        private ZigZagPosition offsetB;

        public BackAndForthAIReferenceType ReferenceType => referenceType;

        public Transform TransformRefA => transformRefA;
        public Transform TransformRefB => transformRefB;

        public Vector3 GetOffsetA(float y) => offsetA.ToOffset(y);

        public Vector3 GetOffsetB(float y) => offsetB.ToOffset(y);
        
        public float MinDistance => minDistance;
    }
    
    public class EnemyBackAndForthAI : AEnemyAIControllerType
    {
        private Vector3 _startPosition;
        private bool _positionAIsTargetPosition = false;
        private Vector3 TargetPosition
        {
            get
            {
                switch (BackAndForthAIData.ReferenceType)
                {
                    case EnemyBackAndForthAIData.BackAndForthAIReferenceType.TYPE_TRANSFORM_REFERENCES:
                    {
                        if (_positionAIsTargetPosition)
                        {
                            return BackAndForthAIData.TransformRefA?.position
                                   ?? Transform.position;
                        }
                        return BackAndForthAIData.TransformRefB?.position
                               ?? Transform.position;
                    }
                    break;
                    case EnemyBackAndForthAIData.BackAndForthAIReferenceType.TYPE_START_POSITION_OFFSETS:
                    {
                        if (_positionAIsTargetPosition)
                        {
                            return _startPosition + BackAndForthAIData.GetOffsetA(_startPosition.y);
                        }
                        return _startPosition + BackAndForthAIData.GetOffsetB(_startPosition.y);
                    }
                }
                return Transform.position;
            }
        }

        private EnemyBackAndForthAIData BackAndForthAIData
            => _component.BackAndForthAIData;


        public EnemyBackAndForthAI(EnemyAIComponent component)
            : base(component)
        {
            _startPosition = component.transform.position;
        }

        public override void HookEvents() { }

        public override void UnHookEvents() { }
        
        public override void Update(float deltaTime) { }

        public override void FixedUpdate(float deltaTime)
        {
            if (!IsEnabled)
            {
                return;
            }
            Vector3 difference = TargetPosition - Transform.position;
            float squareDistance = difference.sqrMagnitude;
            float minDistanceToThreshold = 
                BackAndForthAIData.MinDistance * BackAndForthAIData.MinDistance;
            if (squareDistance <= minDistanceToThreshold)
            {
                _positionAIsTargetPosition = !_positionAIsTargetPosition;
            }

            difference = TargetPosition - Transform.position;
            Vector3 movementDeltaTime = difference.normalized * _component.AIMovementSpeed;

            if (_component.GroundDetector.IsOnGround())
            {
                Rigidbody.AddForce(movementDeltaTime);
            }
        }
        
        #if UNITY_EDITOR

        public override void OnDrawGizmos()
        {
            Gizmos.color = _component.PositionColor;
            Gizmos.DrawSphere(TargetPosition, BackAndForthAIData.MinDistance);
        }
        
        #endif
    }
}