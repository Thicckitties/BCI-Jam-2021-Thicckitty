using UnityEngine;
using UnityEngine.UI;

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
        public Image readyImage;
        public bool flashingNow;
        public bool kickReady;

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

        }

        public void KickBall() //Kicks the ball via addforce
        {
            Vector3 kickPos = new Vector3(transform.position.x, transform.position.y - .2f, transform.position.z);
            ball.AddExplosionForce(kickPower, transform.position, 5);
            KickBallEvent?.Invoke(this);
        }
    }
}
