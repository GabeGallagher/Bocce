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

    public int redBocceCount, greenBocceCount;

    public bool notInstantiationFailed = true;

    ArrowControl arrow;

    GameObject ball;

    //currently not in use
    void BallInstantiationReporter()
    {
        Debug.Log("New ball instantiated");
    }

    void GetScore()
    {
        //go through each bocce in this game objects transform

        //get the distance between that bocce and the pallino

        //add the distances to a list

        //sort the list

        //get the color of the closest item in the list

        //go through the sorted list and count how many of that color are closer to the pallino than the
        //closest of the opposite color

        //record that number as the score of the round

        //start a new round
    }

    void KillCommandHandler_BallParent()
    {
        if (greenBocceCount < 4 & redBocceCount < 4)
        {
            if (!Physics.CheckSphere(transform.position, sphereRadius) && notInstantiationFailed)
            {
                InstantiateNewBocce();
            }
            else
            {
                Debug.Log("Tried to instantiate a new ball, but there is already something at the origin");
                notInstantiationFailed = false;
            } 
        }
        else
        {
            Debug.Log("Moving to scoring");
        }
    }

    public void InstantiateNewBocce()
    {
        if (!isGreenTurn)
        {
            ball = Instantiate(boccePrefabGreen, transform.position, Quaternion.identity)
                as GameObject;
            ball.GetComponent<BocceControl>().isGreen = true;
            isGreenTurn = true;
        }
        else
        {
            ball = Instantiate(boccePrefabRed, transform.position, Quaternion.identity)
                as GameObject;
            ball.GetComponent<BocceControl>().isGreen = false;
            isGreenTurn = false;
        }
        ball.transform.parent = transform;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
        arrow = transform.GetChild(0).GetComponent<ArrowControl>();
        arrow.ball = ball.GetComponent<BallControl>();
        arrow.isRotating = true;
    }

    void Start ()
    {
        ball = transform.GetChild(transform.childCount - 1).gameObject;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
	}
	
	void Update ()
    {

	}
}
