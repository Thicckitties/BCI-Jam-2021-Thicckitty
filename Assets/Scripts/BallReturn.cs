using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class BallReturn : MonoBehaviour
    {
        [SerializeField] private Transform returnPoint;
        private Rigidbody rb;

        private void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        private void ReturnBall()
        {
            rb.velocity = new Vector3(0, 0, 0);
            transform.position = new Vector3(returnPoint.position.x, returnPoint.position.y, returnPoint.position.z + 1.0f);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ground"))
            {
                ReturnBall();
            }
        }
    }
}
