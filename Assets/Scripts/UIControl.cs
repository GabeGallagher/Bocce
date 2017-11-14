/* Author Gabriel B. Gallagher November 9 2017
 * 
 * Script controls the UI, which currently just means instantiating the power slider when the player
 * presses the spacebar
 */

using UnityEngine;
using System.Collections;

public class UIControl : MonoBehaviour
{
    public GameObject sliderPrefab;

    GameObject ballsParent;

    public BallControl ball;

    public bool isDestroyed = false; //Checks to see if a ball was destroyed by the shredder

    void Start ()
    {
        ballsParent = GameObject.Find("Balls");

        if (ballsParent.transform.childCount <= 0)
        {
            Debug.Log("There are no balls on the court");
        }
        else
        {
            int childCount = ballsParent.transform.childCount;
            if (!ballsParent.transform.GetChild(childCount - 1).GetComponent<BallControl>())
            {
                Debug.Log(name + " does not have the BallControl script attached to it");
            }
            else
            {
                ball = ballsParent.transform.GetChild(childCount - 1).GetComponent<BallControl>(); 
            }
        }
        ball.spaceKeyObserver += SpaceKeyHander_UIControl;
	}

    //What this object should do when the space key is pressed
    void SpaceKeyHander_UIControl()
    {
        int count = 0;
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).tag == "PowerBar")
            {
                ++count;
            }
        }

        if (count == 0)
        {
            GameObject powerBar = Instantiate(sliderPrefab, transform.position, Quaternion.identity)
                as GameObject;
            //powerBar.transform.parent = transform;
            powerBar.transform.SetParent(transform, false);
            powerBar.transform.localPosition = sliderPrefab.transform.position;
            powerBar.GetComponent<PowerBarControl>().ball = ball; //set the ball associated with
                                                                          //the bar to the currently
                                                                          //selected ball
        }
    }

    void Update ()
    {
	    if (ball.gameObject.GetComponent<BallControl>().isDead)
        {
            int childCount = ballsParent.transform.childCount;
            ball = ballsParent.transform.GetChild(childCount - 1).GetComponent<BallControl>();
        }

        if (isDestroyed)
        {
            int childCount = ballsParent.transform.childCount;
            ball = ballsParent.transform.GetChild(childCount - 1).GetComponent<BallControl>();
            isDestroyed = false;
        }
	}
}
