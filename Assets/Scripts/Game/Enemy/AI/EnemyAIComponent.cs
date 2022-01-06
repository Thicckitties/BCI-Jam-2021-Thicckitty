using Unity.VisualScripting;
using UnityEngine;

namespace Thicckitty
{
    
    [RequireComponent(typeof(Rigidbody))]
    public class EnemyAIComponent : EventsListener, IGroundDetectionComponent, 
        ISprite3DUpdater, IEnemyAttackComponent, IEnemyStunComponent
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
        private ZigZagMovementAIData zigZagAIData;
        
        [SerializeField]
        private EnemyAttackType attackType;
        [SerializeField]
        private SODA.Vector3Reference targetPosition;
        [SerializeField]
        private RangedEnemyTypeData rangedAttackData;
    
        [SerializeField]
        private Color positionColor = Color.black;
        
        [SerializeField]
        private bool controlledByAnimations;
        [SerializeField]
        private Animator animator;
        [SerializeField]
        private string walkAnimation;
        [SerializeField]
        private string stunBeginAnimation;
        [SerializeField]
        private string stunEndAnimation;
        
        [SerializeField, Min(0.0f)]
        private float stunTime = 0.0f;
        [SerializeField, Min(0.0f)]
        private float ballKnockbackMultiplier = 1.0f;
        [SerializeField, Range(-1.0f, 1.0f)]
        private float ballHitDirectionThreshold = -0.5f;
        [SerializeField, Min(0.0f)]
        private float minBallSpeed = 0.0f;

        
        
        private Rigidbody _rigidbody;
        
        private GroundDetectionController _groundDetector;
        private AEnemyAIControllerType _enemyControllerType;
        private Sprite3DUpdaterBehaviour _updaterBehaviour;
        private AEnemyAttackType _enemyAttackType;
        private EnemyStunAI _enemyStunAI;

        public bool IsControlledByAnimations => controlledByAnimations;

        private AEnemyAttackType EnemyAttackType
        {
            get
            {
                _enemyAttackType ??= AEnemyAttackType.Create(attackType, this);
                return _enemyAttackType;
            }
        }
        
        private AEnemyAIControllerType EnemyAIControllerType
        {
            get
            {
                _enemyControllerType ??= AEnemyAIControllerType.Create(this);
                return _enemyControllerType;
            }
        }

        private EnemyStunAI EnemyStunAI
        {
            get
            {
                _enemyStunAI ??= new EnemyStunAI(this);
                return _enemyStunAI;
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

        public ZigZagMovementAIData ZigZagAIData => zigZagAIData;

        public float AIMovementSpeed => aiMovementSpeed;

        public Color PositionColor => positionColor;

        public Sprite3DUpdaterData UpdaterData => sprite3DUpdaterData;
        public Vector3 MovementDirection => _rigidbody.velocity.normalized;

        public RangedEnemyTypeData RangedEnemyTypeData => rangedAttackData;
        public Transform Transform => transform;
        public Vector3 TargetPosition => targetPosition.Value;
        

        public Sprite3DUpdaterBehaviour UpdaterBehaviour
        {
            get
            {
                _updaterBehaviour ??= new Sprite3DUpdaterBehaviour(this);
                return _updaterBehaviour;
            }
        }
        
        public GroundDetectionData GroundDetectionData => groundDetectionData;

        protected override void Start()
        {
            base.Start();
            _rigidbody = GetComponent<Rigidbody>();
            EnemyManager.AddEnemy(this);
        }

        private void OnDestroy()
        {
            EnemyManager.RemoveEnemy(this);
        }

        protected override bool HookEvents()
        {
            EnemyAIControllerType?.HookEvents();
            EnemyAttackType?.HookEvents();

            if (EnemyAttackType != null)
            {
                EnemyAttackType.EnemySetAttacking += HandleEnemySetAttacking;
            }
            EnemyStunAI.StunBeginEvent += HandleStunBegin;
            EnemyStunAI.StunEndEvent += HandleStunEnd;
            return true;
        }

        protected override bool UnHookEvents()
        {
            EnemyAIControllerType?.UnHookEvents();
            EnemyAttackType?.UnHookEvents();
            if (EnemyAttackType != null)
            {
                EnemyAttackType.EnemySetAttacking -= HandleEnemySetAttacking;
            }
            EnemyStunAI.StunBeginEvent -= HandleStunBegin;
            EnemyStunAI.StunEndEvent -= HandleStunEnd;
            return true;
        }

        private void HandleEnemySetAttacking(AEnemyAttackType type, bool attacking)
        {
            EnemyAIControllerType?.SetEnabled(!attacking);
        }

        private void Update()
        {
            UpdateAnimations();
            
            EnemyStunAI.OnUpdate(Time.deltaTime);
            EnemyAIControllerType?.Update(Time.deltaTime);
            UpdaterBehaviour?.Update(Time.deltaTime);
            EnemyAttackType?.OnUpdate(Time.deltaTime);
        }

        private void UpdateAnimations()
        {
            if (animator
                && !EnemyStunAI.IsStunned)
            {
                animator.Play(walkAnimation);
            }
        }
        
        private void FixedUpdate()
        {
            EnemyAIControllerType?.FixedUpdate(Time.fixedDeltaTime);
        }

        /// <summary>
        /// Sets the enemy stun to be ended via an animation trigger.
        /// </summary>
        public void SetStunEnd_AnimationTrigger()
        {
            EnemyStunAI.SetStunned(false, 0.0f);
        }

        /// <summary>
        /// Called when the enemy has begun to get stunned.
        /// </summary>
        private void HandleStunBegin(EnemyStunAI ai)
        {
            if (animator)
            {
                animator.Play(stunBeginAnimation);
            }
            EnemyAIControllerType.SetEnabled(false);
            EnemyAttackType?.SetEnabled(false);
        }

        /// <summary>
        /// Called when the enemy has ended its stun period.
        /// </summary>
        private void HandleStunEnd(EnemyStunAI ai)
        {
            if (animator
                && !controlledByAnimations)
            {
                animator.Play(stunEndAnimation);
            }
            if (EnemyAIControllerType is EnemyBackAndForthAI backNForth)
            {
                backNForth.RecalculatePositions();
            }
            EnemyAIControllerType.SetEnabled(true);
            EnemyAttackType?.SetEnabled(true);
        }

        private void OnCollisionEnter(Collision collision)
        {
            Rigidbody ballRb = collision.rigidbody;
            if (ballRb 
                && ballRb.gameObject.CompareTag("Ball"))
            {
                var velocity = ballRb.velocity;
                if (collision.contactCount <= 0)
                {
                    return;
                }
                ContactPoint collisionPoint = collision.GetContact(0);
                Vector3 diff = transform.position - collisionPoint.point;
                diff.y = 0.0f;
                float dotProductBetweenHitPointNVelocity = Vector3.Dot(diff.normalized,
                    velocity.normalized);
                if (dotProductBetweenHitPointNVelocity <= ballHitDirectionThreshold
                    || velocity.magnitude <= minBallSpeed)
                {
                    return;
                }
                EnemyStunAI.SetStunned(true, stunTime);
                _rigidbody.AddForce(velocity.normalized
                                    * ballKnockbackMultiplier * velocity.magnitude, ForceMode.Impulse);
            }
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
            EnemyAttackType?.OnDrawGizmos();
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
                            Vector3 posA = position + BackAndForthAIData.GetOffsetA(position.y);
                            Vector3 posB = position + BackAndForthAIData.GetOffsetB(position.y);
                            
                            Gizmos.color = positionColor;
                            Gizmos.DrawSphere(posA, BackAndForthAIData.MinDistance);
                            Gizmos.DrawSphere(posB, BackAndForthAIData.MinDistance);
                        } 
                        break;
                    }
                } 
                break;
                case AIControllerType.ZIG_ZAG_MOVEMENT:
                {
                    Vector3 currentForward = zigZagAIData.zigZagNormal.normalized;
                    Vector3 zigZagDirection = zigZagAIData.zigZagDirection.normalized;
                    Vector3 currentPosition = Transform.position;
            
                    for (int i = 0; i < 5; i++)
                    {
                        Vector3 endPosition = currentPosition
                                              + zigZagDirection * zigZagAIData.zigZagAmount;
                        
                        Gizmos.color = ZigZagAIData.lineColor;
                        Gizmos.DrawLine(currentPosition, endPosition);
                        Gizmos.color = ZigZagAIData.pointColor;
                        Gizmos.DrawSphere(endPosition, ZigZagAIData.differenceThreshold);
                        
                        zigZagDirection = Vector3.Reflect(zigZagDirection, currentForward);
                        currentPosition = endPosition;
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