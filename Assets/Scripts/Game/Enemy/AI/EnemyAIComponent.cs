using System;
using Thicckitty.SODA;
using UnityEngine;
using UnityEngine.Serialization;

namespace Thicckitty
{
    
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyAIComponent : EventsListener, IGroundDetectionComponent, ISprite3DUpdater
    {
        [SerializeField, UnityEngine.Min(0.0f)]
        private float aiMovementSpeed = 0.0f;
        [SerializeField]
        private GroundDetectionData groundDetectionData;
        
        [SerializeField]
        private Sprite3DUpdaterData sprite3DUpdaterData;
        
        [SerializeField]
        private AIControllerType controllerType;
        [SerializeField]
        private EnemyBackAndForthAIData backAndForthData;
        [SerializeField]
        private EnemyMimicMovementAIData mimicMovementData;

        [SerializeField]
        private Color positionColor = Color.black;

        private Rigidbody _rigidbody;
        
        private GroundDetectionController _groundDetector;
        private AEnemyAIControllerType _enemyControllerType;
        private Sprite3DUpdaterComponent _updaterComponent;
        
        private AEnemyAIControllerType EnemyAIControllerType
        {
            get
            {
                _enemyControllerType ??= AEnemyAIControllerType.Create(this);
                return _enemyControllerType;
            }
        }

        public GroundDetectionController GroundDetector
        {
            get
            {
                _groundDetector ??= new GroundDetectionController(this);
                return _groundDetector;
            }
        }

        public AIControllerType ControllerType => controllerType;

        public EnemyBackAndForthAIData BackAndForthAIData => backAndForthData;

        public EnemyMimicMovementAIData MimicMovementData => mimicMovementData;

        public float AIMovementSpeed => aiMovementSpeed;

        public Color PositionColor => positionColor;

        public Sprite3DUpdaterData UpdaterData => sprite3DUpdaterData;
        public Vector3 MovementDirection => _rigidbody.velocity.normalized;
        
        public Transform Transform => transform;

        public Sprite3DUpdaterComponent UpdaterComponent
        {
            get
            {
                _updaterComponent ??= new Sprite3DUpdaterComponent(this);
                return _updaterComponent;
            }
        }
        
        public GroundDetectionData GroundDetectionData => groundDetectionData;

        protected override void Start()
        {
            base.Start();
            _rigidbody = GetComponent<Rigidbody>();
        }
        
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
            UpdaterComponent?.Update(Time.deltaTime);
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
            GroundDetector.OnDrawGizmos();
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