using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Rigidbody))]
public class TCharacter : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float movementSpeed;

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
