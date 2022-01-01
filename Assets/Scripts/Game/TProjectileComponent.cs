using System;
using UnityEngine;

namespace Thicckitty
{

    public interface IProjectileHit
    {
        void OnCollideWithProjectile(TProjectileComponent component);
    }
    
    [RequireComponent(typeof(Rigidbody))]
    public class TProjectileComponent : MonoBehaviour
    {
        private Rigidbody _rigidbody;

        public Rigidbody Rigidbody
        {
            get
            {
                _rigidbody ??= GetComponent<Rigidbody>();
                return _rigidbody;
            }
        }

        private void OnCollisionEnter(Collision other)
        {
            IProjectileHit projectileHit 
                = other.rigidbody?.GetComponent<IProjectileHit>();
            projectileHit?.OnCollideWithProjectile(this);
        }
    }
}