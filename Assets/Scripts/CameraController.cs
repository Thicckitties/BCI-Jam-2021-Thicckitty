using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Thicckitty
{
    public class CameraController : MonoBehaviour
    {
        [Header("Vert Angle Bounds")]
        [SerializeField] private float maxAngle; //Less than 180
        [SerializeField] private float minAngle; //Greater than -180

        // Start is called before the first frame update
        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
        }

        public void VertRotation(float inputAngle)
        {
            inputAngle = Mathf.Clamp(inputAngle, - 90, 90); 
            float angle = transform.rotation.eulerAngles.x - inputAngle;
            transform.rotation = Quaternion.Euler(new Vector3(StupidClamp(angle, minAngle, maxAngle), transform.rotation.eulerAngles.y, transform.rotation.eulerAngles.z));
        }
        
        private float StupidClamp(float angle, float min, float max)
        {
            float returnAngle = angle;
            if (returnAngle > 180 && min > (returnAngle - 360))
            {
                returnAngle = min;
            }
            else
            {
                if (returnAngle < 180 && returnAngle > max)
                {

                    returnAngle = max;
                }
            }

            return returnAngle;
        }
    }
}
