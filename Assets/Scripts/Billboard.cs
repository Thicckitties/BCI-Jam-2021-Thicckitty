using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class Billboard : MonoBehaviour
    {
        [SerializeField] private static Transform cam;

        private void Start()
        {
            cam = FindObjectOfType<Camera>().transform;
        }

        void Update()
        {
            Vector3 lookPoint = cam.transform.position;
            lookPoint.y = transform.position.y;
            transform.LookAt(lookPoint);
        }
    }
}
