using System;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    
    [RequireComponent(typeof(Collider))]
    [DisallowMultipleComponent]
    public class CollisionListenerComponent : MonoBehaviour
    {
        public delegate void TriggerEventDelegate(CollisionListenerComponent listenerComponent, Collider collider);
        public delegate void CollisionEventDelegate(CollisionListenerComponent listener, Collision collision);
        
        public CollisionEventDelegate OnCollisionEnterEvent;
        public CollisionEventDelegate OnCollisionExitEvent;
        public TriggerEventDelegate OnTriggerEnterEvent;
        public TriggerEventDelegate OnTriggerExitEvent;

        private void OnCollisionEnter(Collision other)
        {
            OnCollisionEnterEvent?.Invoke(this, other);
        }

        private void OnCollisionExit(Collision other)
        {
            OnCollisionExitEvent?.Invoke(this, other);
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
}
