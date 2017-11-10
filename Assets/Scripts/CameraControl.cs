/* Author Gabriel B. Gallagher November 6, 2017
 *
 * Script handles the behavior of the camera
 */

using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour
{
    public GameObject ball, vanishingPlane;

    Vector3 offset;

	// Use this for initialization
	void Start ()
    {
        offset = transform.position - ball.transform.localPosition;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (transform.position.y >= vanishingPlane.transform.position.y)
        {
            transform.position = ball.transform.localPosition + offset; 
        }
	}
}
