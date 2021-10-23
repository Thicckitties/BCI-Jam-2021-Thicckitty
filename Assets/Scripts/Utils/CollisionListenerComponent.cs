using System;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[DisallowMultipleComponent]
public class CollisionListenerComponent : MonoBehaviour
{
    public delegate void CollisionEventDelegate(CollisionListenerComponent listenerComponent, Collider collider);

    public CollisionEventDelegate OnCollisionEnterEvent;
    public CollisionEventDelegate OnCollisionExitEvent;
    public CollisionEventDelegate OnTriggerEnterEvent;
    public CollisionEventDelegate OnTriggerExitEvent;

    private void OnCollisionEnter(Collision other)
    {
        OnCollisionEnterEvent?.Invoke(this, other.collider);
    }

    private void OnCollisionExit(Collision other)
    {
        OnCollisionExitEvent?.Invoke(this, other.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        OnTriggerEnterEvent?.Invoke(this, other);
    }

    private void OnTriggerExit(Collider other)
    {
        OnTriggerExitEvent?.Invoke(this, other);
    }
}