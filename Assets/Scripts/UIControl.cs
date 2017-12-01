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

    public GameObject ballsParent;

    public BallControl ball;

    public bool isDestroyed = false, pallinoDestroyed = false; //Checks if a balls were shredded

    void GetPallino()
    {
        int childCount = ballsParent.transform.childCount;
        for (int i = 0; i < childCount; ++i)
        {
            if (ballsParent.transform.GetChild(i).GetComponent<PallinoControl>())
            {
                ball = ballsParent.transform.GetChild(i).GetComponent<BallControl>();
            }
        }
        if (!ball)
        {
            Debug.Log("Error getting pallino in UI control");
        }
    }

    void BeginNewRoundObserver_UIControl()
    {
        Debug.Log("UI observing new round");
        GetPallino();
        ball.spaceKeyObserver += SpaceKeyHander_UIControl;
        ballsParent.GetComponent<BallParent>().newRoundReporter += BeginNewRoundObserver_UIControl;
    }

    void Start ()
    {
        GetPallino();
        ball.spaceKeyObserver += SpaceKeyHander_UIControl;
        ballsParent.GetComponent<BallParent>().newRoundReporter += BeginNewRoundObserver_UIControl;
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
            powerBar.GetComponent<PowerBarControl>().ball = ball; //set the ball associated with bar to the 
                                                                  //currently selected ball
        }
    }

    void Update ()
    {
        if (isDestroyed)
        {
            GetBall();
            isDestroyed = false;
        }

        if (pallinoDestroyed)
        {
            GetPallino();
            pallinoDestroyed = false;
        }

        if (ball.gameObject.GetComponent<BallControl>().isDead)
        {
            GetBall();
        }
    }

    void GetBall()
    {
        for (int i = 0; i < ballsParent.transform.childCount; ++i)
        {
            if (ballsParent.transform.GetChild(i).GetComponent<BallControl>())
            {
                ball = ballsParent.transform.GetChild(i).GetComponent<BallControl>();
            }
        }
        if (!ball)
        {
            Debug.Log("Error getting ball in UI Control");
        }
    }
}
