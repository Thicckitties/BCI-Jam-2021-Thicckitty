using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class EnemyDESTROY : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Enemy")
            {
               // other.GetComponent<EnemyAIComponent>().enabled = false;
//                 Debug.Log("Yes");
                Destroy(other.transform.parent.gameObject);
            }
        }
    }
}
