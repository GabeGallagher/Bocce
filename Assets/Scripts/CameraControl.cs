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

    Vector3 offset;

    float callTime;

    bool isChanging = false;

    GameObject GetBallCamFocuses()
    {
        int childCount = GameObject.Find("Balls").transform.childCount;
        isChanging = false;
        return GameObject.Find("Balls").transform.GetChild(childCount - 1).gameObject;
    }

    void KillCommandObserver_CameraControl()
    {
        callTime = Time.timeSinceLevelLoad;
        isChanging = true;
        Debug.Log("Camera reading the kill command");
    }

	// Use this for initialization
	void Start ()
    {
        ball = GetBallCamFocuses();
        offset = transform.position - ball.transform.localPosition;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandObserver_CameraControl;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y >= vanishingPlane.transform.position.y)
        {
            transform.position = ball.transform.localPosition + offset; 
        }

        if (((Time.timeSinceLevelLoad - callTime) >= changeTime) && isChanging)
        {
            ball = GetBallCamFocuses();
        }
	}
}
