using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class Stick : MonoBehaviour
    {
        [SerializeField] private Transform parent;

        // Update is called once per frame
        void LateUpdate()
        {
            transform.position = parent.position;
        }
    }
}
