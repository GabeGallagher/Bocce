/* Author Gabriel B. Gallagher November 6, 2017
 *
 * Script handles behavior of the balls when they are thrown off of the ledge. The Pallino should respawn
 * and allow the player who threw it a rethrow. If the player throwing the pallino misses the field twice,
 * it goes to the other team. This process repeats until one team gets the pallino in bounds. If the ball
 * is a bocce, then it is added toward the total throws for that team and destroyed. The turn is then
 * passed to the next team.
 */

using UnityEngine;
using System.Collections;

public class Shredder : MonoBehaviour
{
    public GameObject ballsParent, arrow;

    public Camera mainCamera;

    public Canvas uI;

    public int pallinoShredCount;

    GameObject ball;

    private void OnTriggerEnter(Collider trigger)
    {
        //Debug.Log("Triggered by: " + trigger.name);

        if (trigger.GetComponent<BocceControl>()/* && !trigger.GetComponent<PallinoControl>()*/)
        {
            if (trigger.GetComponent<BocceControl>().isGreen)
            {
                ++ballsParent.GetComponent<BallParent>().greenBocceCount;
            }
            else if (!trigger.GetComponent<BocceControl>().isGreen)
            {
                ++ballsParent.GetComponent<BallParent>().redBocceCount;
            }
            else
            {
                Debug.Log("Error finding " + trigger.name + " color");
            }
            Debug.Log("Destroying " + trigger.name);
            Destroy(trigger.gameObject, trigger.GetComponent<BallControl>().delayTime); //Shred ball
            ballsParent.GetComponent<BallParent>().InstantiateNewBocce();
            //Update the focus for the camera to the new ball
            ball = mainCamera.GetComponent<CameraControl>().ball;
            ball = ballsParent.transform.GetChild(ballsParent.transform.childCount - 1).gameObject;
            mainCamera.transform.position = ball.transform.position + 
                mainCamera.GetComponent<CameraControl>().offset;
            //Update the ball in the UIControl
            uI.GetComponent<UIControl>().isDestroyed = true;
        }
        else if (trigger.GetComponent<PallinoControl>())
        {
            Debug.Log("Shredder triggered by pallino");

            Destroy(trigger.gameObject, trigger.GetComponent<BallControl>().delayTime);
            ++pallinoShredCount;

            ballsParent.GetComponent<BallParent>().InstantiateNewPallino();
            for (int i = 0; i < ballsParent.transform.childCount; ++i)
            {
                if (ballsParent.transform.GetChild(i).GetComponent<PallinoControl>())
                {
                    ball = ballsParent.transform.GetChild(i).gameObject;
                }
            }
            //Update ball focus in main camera
            mainCamera.GetComponent<CameraControl>().ball = ball;

            //Update the ball in the UIControl
            uI.GetComponent<UIControl>().pallinoDestroyed = true;

            //Update ball in Arrow
            arrow.GetComponent<ArrowControl>().GetBall();
        }
    }

    void Update()
    {
        //Check in case the camera does not properly get the new ball
        if(!mainCamera.GetComponent<CameraControl>().ball)
        {
            ball = ballsParent.transform.GetChild(ballsParent.transform.childCount - 1).gameObject;
            mainCamera.transform.position = ball.transform.localPosition +
                mainCamera.GetComponent<CameraControl>().offset;
            //Update the focus for the camera to the new ball
            ball = mainCamera.GetComponent<CameraControl>().ball;
        }
    }
}
