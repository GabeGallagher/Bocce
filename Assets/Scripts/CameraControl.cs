/* Author Gabriel B. Gallagher November 6, 2017
 *
 * Script handles the behavior of the camera
 */

using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public GameObject ball, vanishingPlane;

    public float changeTime; //Time between when a ball is killed and the camera switches to a new ball

    public Vector3 offset; //Designer can adjust this value by moving the camera in editor

    public float callTime;

    BallParent ballParent;

    Vector3 originalPosition;

    int childCount;

    bool isObservingKillCommand = true;

    public bool isChanging = false;

    //Returns which ball the camera should follow
    GameObject GetBallCamFocuses()
    {
        childCount = GameObject.Find("Balls").transform.childCount;
        GameObject updatedBall = GameObject.Find("Balls").transform.GetChild(childCount - 1).gameObject;
        updatedBall.GetComponent<BallControl>().killCommandObserver += KillCommandObserver_CameraControl;
        return updatedBall;
    }

    //what this script should do when a moving ball comes to rest
    void KillCommandObserver_CameraControl()
    {
        if (isObservingKillCommand)
        {
            callTime = Time.timeSinceLevelLoad;
            isObservingKillCommand = false;
        }
        isChanging = true;
    }

    public void GetPallino()
    {
        childCount = ballParent.gameObject.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            if (ballParent.gameObject.transform.GetChild(i).GetComponent<PallinoControl>())
            {
                ball = ballParent.gameObject.transform.GetChild(i).gameObject;
            }
        }
        if (!ball)
        {
            Debug.Log("Error getting pallino in camera");
        }
    }
    
	void Start ()
    {
        ballParent = (BallParent)FindObjectOfType(typeof(BallParent));
        GetPallino();
        originalPosition = transform.position;
        offset = transform.position - ball.transform.localPosition;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandObserver_CameraControl;
        ballParent.newRoundReporter += BeginNewRoundObserver_CameraControl;
    }

    void BeginNewRoundObserver_CameraControl()
    {
        Debug.Log("Camera observing new round");
        GetPallino();
        transform.position = originalPosition;
        offset = transform.position - ball.transform.localPosition;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandObserver_CameraControl;
        ballParent.newRoundReporter += BeginNewRoundObserver_CameraControl;
    }
	
	void Update ()
    {
        if (transform.position.y >= vanishingPlane.transform.position.y)
        {
            transform.position = ball.transform.localPosition + offset; 
        }

        if (isChanging && Time.timeSinceLevelLoad - callTime > changeTime)
        {
            ball = GetBallCamFocuses();
            isObservingKillCommand = true;
            isChanging = false;
        }
	}
}
