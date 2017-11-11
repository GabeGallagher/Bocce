/* Author Gabriel B. Gallagher November 7, 2017
 * 
 * Script handles the user input to throw the balls.
 */

using UnityEngine;
using System.Collections;

[RequireComponent (typeof(BallControl))]
public class TossBall : MonoBehaviour
{
    public delegate void OnSpaceKey();
    public OnSpaceKey spaceKeyObserver;

    public GameObject ball;

    void SpaceKeyHandler_TossBall()
    {
        Debug.Log("Space Key Down");
    }

    // Use this for initialization
    void Start ()
    {
        ball = transform.gameObject;
        spaceKeyObserver += SpaceKeyHandler_TossBall;
	}

    void Update()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            spaceKeyObserver();
        }
    }
}
