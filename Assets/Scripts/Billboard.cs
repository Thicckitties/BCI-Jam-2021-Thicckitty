using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private static Transform cam;

        void Update()
        {
            transform.LookAt(cam);
        }
    }
}
