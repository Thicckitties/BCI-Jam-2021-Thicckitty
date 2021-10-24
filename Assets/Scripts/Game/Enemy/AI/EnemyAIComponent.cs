using System;
using Thicckitty.SODA;
using UnityEngine;
using UnityEngine.Serialization;

namespace Thicckitty
{
    
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyAIComponent : EventsListener
    {
        [SerializeField, UnityEngine.Min(0.0f)]
        private float aiMovementSpeed = 0.0f;

        [SerializeField]
        private AIControllerType controllerType;
        [SerializeField]
        private EnemyBackAndForthAIData backAndForthData;
            
        [SerializeField]
        private Color positionColor = Color.black;


        private AEnemyAIControllerType _enemyControllerType;

        private AEnemyAIControllerType EnemyAIControllerType
        {
            get
            {
                _enemyControllerType ??= AEnemyAIControllerType.Create(this);
                return _enemyControllerType;
            }
        }

        public AIControllerType ControllerType => controllerType;

        public EnemyBackAndForthAIData BackAndForthAIData => backAndForthData;

        public float AIMovementSpeed => aiMovementSpeed;

        public Color PositionColor => positionColor;
        
        protected override bool HookEvents()
        {
            EnemyAIControllerType?.HookEvents();
            return true;
        }

        protected override bool UnHookEvents()
        {
            EnemyAIControllerType?.UnHookEvents();
            return true;
        }

        private void Update()
        {
            EnemyAIControllerType?.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            EnemyAIControllerType?.FixedUpdate(Time.fixedDeltaTime);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
            {
                DrawGizmosInEditor();
            }
            else
            {
                DrawGizmosInGame();
            }
        }

        private void DrawGizmosInEditor()
        {
            switch (controllerType)
            {
                case AIControllerType.CONTROLLER_TYPE_BACK_AND_FORTH:
                {
                    switch (BackAndForthAIData.ReferenceType)
                    {
                        case EnemyBackAndForthAIData.BackAndForthAIReferenceType.TYPE_TRANSFORM_REFERENCES:
                        {
                            Gizmos.color = positionColor;
                            if (BackAndForthAIData.TransformRefA)
                            {
                                Gizmos.DrawSphere(BackAndForthAIData.TransformRefA.position,
                                    BackAndForthAIData.MinDistance);
                            }

                            if (BackAndForthAIData.TransformRefB)
                            {
                                Gizmos.DrawSphere(BackAndForthAIData.TransformRefB.position,
                                    BackAndForthAIData.MinDistance);
                            }
                        } 
                        break;
                        case EnemyBackAndForthAIData.BackAndForthAIReferenceType.TYPE_START_POSITION_OFFSETS:
                        {
                            Vector3 position = transform.position;
                            Vector3 posA = position + BackAndForthAIData.OffsetA;
                            Vector3 posB = position + BackAndForthAIData.OffsetB;
                            
                            Gizmos.color = positionColor;
                            Gizmos.DrawSphere(posA, BackAndForthAIData.MinDistance);
                            Gizmos.DrawSphere(posB, BackAndForthAIData.MinDistance);
                        } 
                        break;
                    }
                } 
                break;
            }
        }

        private void DrawGizmosInGame()
        {
            EnemyAIControllerType?.OnDrawGizmos();
        }
        
#endif
    }
}