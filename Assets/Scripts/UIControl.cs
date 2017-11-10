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

    TossBall tossBall;
	
	void Start ()
    {
        balls = GameObject.Find("Balls");

        if (balls.transform.childCount <= 0)
        {
            Debug.Log("There are no balls on the court");
        }
        else
        {
            if (!balls.transform.GetChild(0).GetComponent<TossBall>())
            {
                Debug.Log(name + " does not have the TossBall script attached to it");
            }
            else
            {
                tossBall = balls.transform.GetChild(0).GetComponent<TossBall>(); 
            }
        }

        tossBall.spaceKeyObserver += SpaceKeyHander_UIControl;
	}

    void SpaceKeyHander_UIControl()
    {
        Debug.Log("UI Control script reads space key down");
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
            powerBar.transform.parent = transform;
            powerBar.transform.localPosition = sliderPrefab.transform.position;
            powerBar.GetComponent<PowerBarControl>().tossBall = tossBall; //set the ball associated with
                                                                          //the bar to the currently
                                                                          //selected ball
        }
    }

    void Update ()
    {
	
	}
}
