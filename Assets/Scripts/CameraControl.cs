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

    int childCount;

    bool isObservingKillCommand = true;

    public bool isChanging = false;

    //Returns which ball the camera should follow
    GameObject GetBallCamFocuses()
    {
        Debug.Log("Changing focus");
        childCount = GameObject.Find("Balls").transform.childCount;
        GameObject updatedBall = GameObject.Find("Balls").transform.GetChild(childCount - 1).gameObject;
        Debug.Log("Updated Ball name: " + updatedBall.name);
        updatedBall.GetComponent<BallControl>().killCommandObserver += KillCommandObserver_CameraControl;
        return updatedBall;
    }

    //what this script should do when a moving ball comes to rest
    void KillCommandObserver_CameraControl()
    {
        Debug.Log("Camera read kill command");
        if (isObservingKillCommand)
        {
            callTime = Time.timeSinceLevelLoad;
            isObservingKillCommand = false;
        }
        Debug.Log("Call Time: " + callTime);
        isChanging = true;
    }
    
	void Start ()
    {
        childCount = GameObject.Find("Balls").transform.childCount;
        Debug.Log("Number of children: " + childCount);
        ball = GetBallCamFocuses();
        offset = transform.position - ball.transform.localPosition;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandObserver_CameraControl;
    }
	
	void Update ()
    {
        if (transform.position.y >= vanishingPlane.transform.position.y)
        {
            transform.position = ball.transform.localPosition + offset; 
        }

        if (isChanging && Time.timeSinceLevelLoad - callTime > changeTime)
        {
            Debug.Log("Changing focus");
            ball = GetBallCamFocuses();
            isObservingKillCommand = true;
            isChanging = false;
        }
	}
}
