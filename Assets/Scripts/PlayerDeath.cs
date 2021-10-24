using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class PlayerDeath : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if(other.CompareTag("Ground"))
            {
                GetComponent<TCharacter>().enabled = false;
                Debug.Log("LoseState");
            }
        }
    }
}
