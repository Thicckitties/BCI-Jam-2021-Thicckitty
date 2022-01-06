using System;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Thicckitty
{
    public class Kick : MonoBehaviour
    {
        public delegate void KickBallEventDelegate(Kick kickScript);
        public KickBallEventDelegate KickBallEvent;        
        
        //Add U.I when kick is ready
        //Make it A.I compatible
       
        public float kickPower;
        public Rigidbody ball;
        public Transform ballLookAt;
        public Transform launchPoint;
        public GameObject player;
        public Camera camera;
        public Image readyImage;
        public Transform imageTwist;
        public bool flashingNow;
        public bool kickReady;

        [Header("Camera Rotation Kick Values")]
        [SerializeField, Min(0.0f)]
        private float maxKickAngle = 40.0f;
        [SerializeField]
        private float minCameraRotation = 85.0f;
        [SerializeField]
        private float maxCameraRotation = 110.0f;
        
        [Header("Shake Camera Settings")]

       public float strengh;
       public int vibration;
       public float duration;
       public float randomDirection;
  
        private float t; //Time
        private Rigidbody playerR;

        private bool stopGrav;


        private void Start()
        {
           
            playerR = player.GetComponent<Rigidbody>(); // Grabs rigidbody from player and stores it s velocity to check if they are standing still
            readyImage.color = Color.white;
        }

        void  Update()
        {
            Vector3 vel = playerR.velocity;
            //Debug.Log(vel.magnitude);
            //Debug.Log(kickReady);
            if (Input.GetKeyDown(KeyCode.Space) && kickReady && vel.magnitude <= 0.1f) //check if player is standing still and ready to kick
            { 
                KickBall();
                Debug.Log("Play");

                
            }

            if(flashingNow) //Flashing effect
            {
                Flashing();
                //Debug.Log("Fuck");
            }

        }

        private void FixedUpdate()
        {
            //launchPoint.right = ball.transform.position - launchPoint.position; // For arrow, may not add. Don't remove just not being used.
        }


        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Ball")
            {
                kickReady = true;
                flashingNow = true;

                // Blink when ready
            }
        }

        private void OnTriggerStay(Collider other)
        {
            if (!kickReady)
            {
                if (other.tag == "Ball")
                {
                    kickReady = true;
                  
                }
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.tag == "Ball")
            {
                kickReady = false;
                flashingNow = false;
                readyImage.color = Color.white;
                //Stop when false
            }
        }

        public void Flashing() //Lerps between colors
        {
            t += Time.deltaTime;
            readyImage.color = Color.Lerp(Color.white, Color.red, Mathf.Abs(Mathf.Sin(t * 3)));
            imageTwist.DOShakeRotation(0.5f, 0.3f, 1, 2, false);
        }

        public void KickBall() //Kicks the ball via addforce
        {
#if DOMS_OLD_CODE
            // Original Values for Kick:
            // 250 for Kick Power

            Vector3 kickPos = new Vector3(transform.position.x, transform.position.y - .2f, transform.position.z);
            ball.AddExplosionForce(kickPower, transform.position, 5);
            ball.AddForce();
#else
            Vector3 kickDirection = CalculateKickDirection();
            ball.AddForce(kickDirection * kickPower, ForceMode.Impulse);
            
            KickBallEvent?.Invoke(this);
#endif
            Tween();
        }

        /// <summary>
        /// Calculates the kick direction based on the direction of the camera.
        /// The lower the camera is relative to the player, the higher the ball gets kicked.
        /// </summary>
        private Vector3 CalculateKickDirection()
        {
            // Calculates the angle to do the angle axis degrees based on
            // minimum and maximum camera angle values if a camera exists.
            // Otherwise, just get a random angle axis.
            float angleAxisDegrees;
            if (camera)
            {
                float dotProductWithUp = Vector3.Dot(camera.transform.forward,
                    Vector3.up);
                float camRotation = Mathf.Rad2Deg * Mathf.Acos(dotProductWithUp);
                float interpolated = Mathf.Clamp01((camRotation - minCameraRotation) 
                                                   / (maxCameraRotation - minCameraRotation));
                angleAxisDegrees = (1.0f - interpolated) * -maxKickAngle;
            }
            else
            {
                angleAxisDegrees = Random.Range(0.0f, -maxKickAngle);
            }

            // Sends ball in forward direction of kick transform (child of player).
            // Angle Axis applies a rotation upwards so that kick can have some arc.
            var trans = transform;
            Quaternion rotation = Quaternion.AngleAxis(angleAxisDegrees, trans.right)
                                  * trans.rotation;
            return (rotation * Vector3.forward).normalized;
        }


        public void Tween()
        {
            camera.DOShakePosition(duration, strengh, vibration, randomDirection, true);
        }

#if UNITY_EDITOR

        private void OnDrawGizmos()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawRay(transform.position, CalculateKickDirection() * kickPower);
        }

#endif
    }
}
