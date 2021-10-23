using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class TestMovement : MonoBehaviour, ITMovement
{
    [Header("Movement")]
    [SerializeField, Min(0f)]
    private float movementSpeed;
    [SerializeField]
    private TMovementData movementData;
    
    private TMovement _movement;

    public float MovementSpeed => movementSpeed;

    public Transform ComponentTransform
        => transform;

    public TMovement MovementImplementation
    {
        get
        {
            _movement ??= new TMovement(this);
            return _movement;
        }
    }

    public TMovementData MovementData => movementData;

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
            inputVector += ComponentTransform.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            inputVector += -ComponentTransform.forward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            inputVector += -ComponentTransform.right;
        }
        if (Input.GetKey(KeyCode.D))
        {
            inputVector += ComponentTransform.right;
        }
        MovementImplementation.SetVelocity(
            inputVector.normalized * movementSpeed);
    }
}
