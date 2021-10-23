using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TestMovement : MonoBehaviour, ITCharacterMovementComponent
{
    [SerializeField, Min(0f)]
    private float movementSpeed;

    private TCharacterMovement _movement;

    public float MovementSpeed => movementSpeed;

    public Transform TransformReference
        => transform;

    public TCharacterMovement MovementImplementation
    {
        get
        {
            _movement ??= new TCharacterMovement(this);
            return _movement;
        }
    }

    private void Update()
    {
        UpdateInputs();
        MovementImplementation.Update(Time.deltaTime);
    }


    private void UpdateInputs()
    {
        Vector3 inputVector = Vector3.zero;

        if(Input.GetKey(KeyCode.W))
        {
            inputVector += TransformReference.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += -TransformReference.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += -TransformReference.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += TransformReference.right;
        }
        MovementImplementation.SetVelocity(
            inputVector.normalized * movementSpeed);
    }
}
