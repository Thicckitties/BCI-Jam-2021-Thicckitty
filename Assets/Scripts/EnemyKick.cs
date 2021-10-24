using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class EnemyKick : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private Rigidbody playerRB;

        [SerializeField] private float kickRange = 1f;
        [SerializeField] private float kickPower = 200f;

        // Start is called before the first frame update
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
            if((player.position - transform.position).magnitude < kickRange)
            {
                playerRB.AddExplosionForce(kickPower, transform.position, 5);
            }
        }
    }
}
