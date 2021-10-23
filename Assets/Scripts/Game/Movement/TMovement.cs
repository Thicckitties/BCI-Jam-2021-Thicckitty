using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public interface ITMovement
{

    Transform TransformReference
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


    public TMovement(ITMovement movementComponent)
    {
        _movementComponent = movementComponent;
    }

    public void Update(float deltaTime)
    {
        if(_movementComponent?.TransformReference)
        {
            _movementComponent.TransformReference.position += _velocity * deltaTime;
        }
        _velocity = Vector3.zero;
    }

    public void SetVelocity(Vector3 velocity)
    {
        _velocity = velocity;
    }

    public void AddForce(Vector3 force)
    {
        _velocity += force;
    }
}
