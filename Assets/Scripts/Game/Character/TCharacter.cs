using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class TCharacter : MonoBehaviour, IGroundDetectionComponent
    {
        [Header("Movement")]
        [SerializeField, Min(0f)]
        private float movementSpeed;
        [SerializeField]
        private GroundDetectionData groundDetectionData;

        [Header("SODA References")] 
        [SerializeField]
        private SODA.Vector3Reference playerPosition;

        private Vector3 _inputVector = Vector3.zero;
        private Rigidbody _rigidbody;
        private GroundDetectionController _groundDetector;
        
        private Rigidbody Rigidbody
        {
            get
            {
                _rigidbody ??= GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        public Transform Transform => transform;
        public GroundDetectionData GroundDetectionData => groundDetectionData;

        public GroundDetectionController GroundDetector
        {
            get
            {
                _groundDetector ??= new GroundDetectionController(this);
                return _groundDetector;
            }
        }
        
        
         private void Update()
        {
            
        } 

        private void FixedUpdate()
        {
            UpdateInputs();
            if (GroundDetector.IsOnGround())
            {
                Rigidbody.AddForce(
                    _inputVector.normalized * movementSpeed);                
            }
        }

        private void Awake()
        {
            playerPosition.Value = transform.position;
        }

        private void LateUpdate()
        {
            playerPosition.Value = transform.position;
        }

        public void SetInputVector(in Vector3 inputVector)
        {
            _inputVector = inputVector;
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
        } 
        
        #if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            GroundDetector.OnDrawGizmos();
        }

#endif
    }
}
