using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class EnemyDESTROY : MonoBehaviour
    {
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.other.CompareTag("Enemy"))
            {
              
                Destroy(collision.other);
            }
        }
    }
}
