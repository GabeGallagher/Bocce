/* Author Gabriel B. Gallagher November 10, 2017
 *
 * Script handles instantiation of new balls. First, the player throws the Pallino. Then, each subsequent
 * player should throw their Bocce and try to get it as close as pobbile to the Pallino. Players can hit
 * the Pallino or other Bocces, allowing them to get their own Bocces closer, or knock another teams' Bocce
 * away. Essentially, whichever team has the most Bocces closest to the Pallino wins the game.
 * 
 * Script also keeps track of whose turn it is and how many balls have been thrown.
 */

using UnityEngine;
using System.Collections;

public class BallParent : MonoBehaviour
{
    public delegate void OnBallInstantiation();
    public OnBallInstantiation ballInstantiationReporter;

    public GameObject boccePrefabGreen, boccePrefabRed;

    public bool isGreenTurn; //Just determines if it's Green's turn. Could evaluate for red, doesn't matter
                             //as long as the evaluation is consistent.

    public float sphereRadius;

    public bool notInstantiationFailed = true;

    ArrowControl arrow;

    GameObject ball;

    //currently not in use
    void BallInstantiationReporter()
    {
        Debug.Log("New ball instantiated");
    }

    void KillCommandHandler_BallParent()
    {
        //Debug.Log("Ball Parent reading kill command");
        if (!Physics.CheckSphere(transform.position, sphereRadius) && notInstantiationFailed)
        {
            if (!isGreenTurn)
            {
                ball = Instantiate(boccePrefabGreen, transform.position, Quaternion.identity) as GameObject;
                isGreenTurn = true;
            }
            else
            {
                ball = Instantiate(boccePrefabRed, transform.position, Quaternion.identity) as GameObject;
                isGreenTurn = false;
            }
            ball.transform.parent = transform;
            ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
            //ballInstantiationReporter += BallInstantiationReporter;
            arrow = transform.GetChild(0).GetComponent<ArrowControl>();
            arrow.ball = ball.GetComponent<BallControl>();
            arrow.isRotating = true; 
        }
        else
        {
            Debug.Log("Tried to instantiate a new ball, but there is already something at the origin");
            notInstantiationFailed = false;
        }
    }
	
	void Start ()
    {
        ball = transform.GetChild(transform.childCount - 1).gameObject;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
	}
	
	// Update is called once per frame
	void Update ()
    {
        //if (ball.GetComponent<BallControl>().isTossed)
        //{
        //    Debug.Log(ball.name + " is tossed");
        //    float timeSinceToss = Time.timeSinceLevelLoad;
        //    float changeTime = GameObject.Find("Main Camera").GetComponent<CameraControl>().changeTime;
        //    Debug.Log("Change time: " + changeTime);

        //    if (Mathf.Approximately((Time.timeSinceLevelLoad - timeSinceToss), changeTime))
        //    {
        //        ball.GetComponent<BallControl>().killCommandObserver();
        //    }
        //}
	}
}
