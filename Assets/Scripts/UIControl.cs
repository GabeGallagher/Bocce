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

    GameObject balls;

    public TossBall tossBall;

    void Start ()
    {
        balls = GameObject.Find("Balls");

        if (balls.transform.childCount <= 0)
        {
            Debug.Log("There are no balls on the court");
        }
        else
        {
            int childCount = balls.transform.childCount;
            if (!balls.transform.GetChild(childCount - 1).GetComponent<TossBall>())
            {
                Debug.Log(name + " does not have the TossBall script attached to it");
            }
            else
            {
                tossBall = balls.transform.GetChild(childCount - 1).GetComponent<TossBall>(); 
            }
        }
        tossBall.spaceKeyObserver += SpaceKeyHander_UIControl;
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
            powerBar.GetComponent<PowerBarControl>().tossBall = tossBall; //set the ball associated with
                                                                          //the bar to the currently
                                                                          //selected ball
        }
    }

    void Update ()
    {
	    if (tossBall.gameObject.GetComponent<BallControl>().isDead)
        {
            int childCount = balls.transform.childCount;
            tossBall = balls.transform.GetChild(childCount - 1).GetComponent<TossBall>();
        }
	}
}
