using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public struct TMovementData
{
    [System.Serializable]
    public enum TMovementType
    {
        [InspectorName("Rigidbody")]
        TYPE_RIGIDBODY,
        [InspectorName("Transform")]
        TYPE_TRANSFORM
    }
    
    [SerializeField]
    public TMovementType movementType;
    [SerializeField]
    public bool useTransformReference;
    [SerializeField]
    public Transform transformReference;
    [SerializeField]
    public Rigidbody rigidbodyReference;
}

public interface ITMovement
{

    TMovementData MovementData
    {
        get;
    }

    Transform ComponentTransform
    {
        get;
    }

    TMovement MovementImplementation
    {
        get;
    }
}



public class TMovement
{

    private readonly ITMovement _movementComponent;
    private Vector3 _velocity = Vector3.zero;

    public Vector3 Velocity => _velocity;

    private Transform Transform
    {
        get
        {
            if (_movementComponent.MovementData.useTransformReference)
            {
                return _movementComponent.MovementData.transformReference;
            }
            return _movementComponent.ComponentTransform;
        }
    }

    public TMovement(ITMovement movementComponent)
    {
        _movementComponent = movementComponent;
    }

    public void Update(float deltaTime)
    {
        switch (_movementComponent.MovementData.movementType)
        {
            case TMovementData.TMovementType.TYPE_TRANSFORM:
            {
                if (Transform)
                {
                    Transform.position += _velocity * deltaTime;
                }
            }
            break;
            case TMovementData.TMovementType.TYPE_RIGIDBODY:
            {
                if (_movementComponent.MovementData.rigidbodyReference)
                {
                    _movementComponent.MovementData.rigidbodyReference.velocity
                        = _velocity;
                }
            }
            break;
        }
        _velocity = Vector3.zero;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }
}
