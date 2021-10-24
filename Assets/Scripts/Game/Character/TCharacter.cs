using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody))]
    public class TCharacter : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField, Min(0f)]
        private float movementSpeed;

        [Header("SODA References")] 
        [SerializeField]
        private SODA.Vector3Reference playerPosition;
        
        private Rigidbody _rigidbody;
        
        private Rigidbody Rigidbody
        {
            get
            {
                _rigidbody ??= GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        private void Update()
        {
            UpdateInputs();
        }

        private void Awake()
        {
            playerPosition.Value = transform.position;
        }

        private void LateUpdate()
        {
            playerPosition.Value = transform.position;
        }

        private void UpdateInputs()
        {
            Vector3 inputVector = Vector3.zero;

            if(Input.GetKey(KeyCode.W))
            {
                inputVector += transform.forward;
            }
            if (Input.GetKey(KeyCode.S))
            {
                inputVector += -transform.forward;
            }
            if (Input.GetKey(KeyCode.A))
            {
                inputVector += -transform.right;
            }
            if (Input.GetKey(KeyCode.D))
            {
                inputVector += transform.right;
            }
            Rigidbody.AddForce(
                inputVector.normalized * movementSpeed);
        }
    }
}
