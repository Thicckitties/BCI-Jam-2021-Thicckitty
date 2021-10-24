using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Kick : MonoBehaviour
{
    //Add U.I when kick is ready
    //Make it A.I compatible
   

    public float kickPower;

    public Rigidbody ball;

    public Transform ballLookAt;

    public Transform launchPoint;

    public Image readyImage;

    public bool kickReady;

    bool stopGrav;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && kickReady)
        {
            KickBall();
            Debug.Log("Play");

            
        }

        if (stopGrav)
        {
            StartCoroutine(StopGravity());
        }

    }

    private void FixedUpdate()
    {
        launchPoint.right = ball.transform.position - launchPoint.position;
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Ball")
        {
            kickReady = true;
            // Blink when ready
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Ball")
        {
            kickReady = false;
            //Stop when false
        }
    }


    public void KickBall()
    {
        ball.AddExplosionForce(kickPower, transform.position, 5);
        stopGrav = true;
    }

    IEnumerator StopGravity()
    {
       yield return new WaitForSeconds(2);

        Debug.Log("Bruh");
        //ball.gravityScale += 0;
    }


}