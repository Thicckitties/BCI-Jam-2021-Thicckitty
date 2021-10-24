using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Kick : MonoBehaviour
{

    public float kickPower;

    public Rigidbody ball;

    public float playerAttachment;

    public Transform launchPoint;

    public bool kickReady;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && kickReady)
        {
            KickBall();
        }
    }


    private void OnTriggerEnter2D(Collider other)
    {
        if(other.tag == "Ball")
        {
            kickReady = true;
        }
    }

    private void OnTriggerExit2D(Collider other)
    {
        if(other.tag == "Ball")
        {
            kickReady = false;
        }
    }


    public void KickBall()
    {
        ball.AddForce(Vector2.up * kickPower);
    }


}
