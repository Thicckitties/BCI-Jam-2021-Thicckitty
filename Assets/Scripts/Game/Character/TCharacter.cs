using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class TCharacter : EventsListener, IGroundDetectionComponent, ISprite3DUpdater
    {
        [Header("Movement")]
        [SerializeField, Min(0f)]
        private float movementSpeed;
        [SerializeField, Min(0.01f)]
        private float velocityMagnitude = 0.01f;
        [SerializeField]
        private GroundDetectionData groundDetectionData;

        [Header("Camera")]
        [SerializeField]
        private CameraController cameraController;
        [SerializeField, Min(0.01f)]
        private float xSense;
        [SerializeField, Min(0.01f)]
        private float ySense;

        [Header("Behavior")] 
        [SerializeField]
        private Kick kick;
        
        [Header("Visuals")] 
        [SerializeField]
        private Sprite3DUpdaterData sprite3DUpdaterData;
        
        [Header("Animations")]
        [SerializeField]
        private TCharacterAnimationComponent animator;
        [SerializeField]
        private string idleAnimation;
        [SerializeField]
        private string walkingAnimation;
        [SerializeField]
        private string kickAnimation;

        
        [Header("SODA References")] 
        [SerializeField]
        private SODA.Vector3Reference playerPosition;

        private Vector3 _inputVector = Vector3.zero;
        private Vector2 _cameraVector = Vector2.zero;
        private Rigidbody _rigidbody;
        private GroundDetectionController _groundDetector;
        private Sprite3DUpdaterComponent _spriteUpdater;

        private bool _kicking = false;
        
        private Rigidbody Rigidbody
        {
            get
            {
                _rigidbody ??= GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        public Sprite3DUpdaterData UpdaterData => sprite3DUpdaterData;

        public Vector3 MovementDirection => _rigidbody.velocity.normalized;
        
        public Transform Transform => transform;

        public Sprite3DUpdaterComponent UpdaterComponent
        {
            get
            {
                _spriteUpdater ??= new Sprite3DUpdaterComponent(this);
                return _spriteUpdater;
            }
        }
        
        public GroundDetectionData GroundDetectionData => groundDetectionData;

        public GroundDetectionController GroundDetector
        {
            get
            {
                _groundDetector ??= new GroundDetectionController(this);
                return _groundDetector;
            }
        }
        
        private void Awake()
        {
            playerPosition.Value = transform.position;
        }

        protected override bool HookEvents()
        {
            if (kick
                && animator)
            {
                kick.KickBallEvent += HandleKickBall;
                animator.FinishedKickEvent += HandleKickBallFinished;
                return true;
            }
            return false;
        }

        protected override bool UnHookEvents()
        {
            if (kick
                && animator)
            {
                kick.KickBallEvent -= HandleKickBall;
                animator.FinishedKickEvent -= HandleKickBallFinished;
                return true;
            }
            return false;
        }

        private void HandleKickBall(Kick kick)
        {
            if (!_kicking)
            {
                if (animator)
                {
                    animator.Animator.Play(kickAnimation);
                }
                _kicking = true;
            }
        }

        private void HandleKickBallFinished()
        {
            if (_kicking)
            {
                _kicking = false;
            }
        }

        private void Update()
        {
            UpdateInputs();
            UpdateAnimations();
            UpdateCamera(Time.deltaTime);
            
            UpdaterComponent?.Update(Time.deltaTime);
        }

        private void FixedUpdate()
        {
            if (GroundDetector.IsOnGround())
            {
                Rigidbody.AddForce(
                    _inputVector.normalized * movementSpeed);                
            }
        }

        private void UpdateCamera(float deltaTime)
        {
            transform.Rotate(0, xSense * _cameraVector.x, 0);

            if (cameraController)
            {
                cameraController?.VertRotation(ySense * _cameraVector.y);
            }
        }

        private void LateUpdate()
        {
            playerPosition.Value = transform.position;
        }

        public void SetInputVector(in Vector3 inputVector)
        {
            _inputVector = inputVector;
        }

        private void UpdateAnimations()
        {
            bool isMoving = _rigidbody.velocity.sqrMagnitude 
                            > (velocityMagnitude * velocityMagnitude);
            if (animator
                && !_kicking)
            {
                string animation = isMoving ? walkingAnimation : idleAnimation;
                animator.Animator.Play(animation);
            }
        }

        private void UpdateInputs()
        {
            _inputVector = Vector3.zero;
            if(Input.GetKey(KeyCode.W))
            {
                _inputVector += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                _inputVector += -transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                _inputVector += -transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                _inputVector += transform.right;
            }
            _cameraVector.x = Input.GetAxis("Mouse X");
            _cameraVector.y = Input.GetAxis("Mouse Y");
        }
        
        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            GroundDetector.OnDrawGizmos();
        }

#endif
    }
}
