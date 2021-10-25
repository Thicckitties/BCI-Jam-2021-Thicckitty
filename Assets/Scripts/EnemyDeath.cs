using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class EnemyDeath : MonoBehaviour
    {
        public float knockBackForce;
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {

        }

        private void OnCollisionEnter(Collision collision)
        {
            if (collision.other.CompareTag("Ball"))
            {
                //gameObject.GetComponent<EnemyAIComponent>().enabled = false;
                gameObject.GetComponent<Rigidbody>().AddForce(transform.forward * 10000f);
                //Destroy(gameObject);

            }
        }
    }
}
