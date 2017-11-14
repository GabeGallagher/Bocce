/* Author Gabriel B. Gallagher November 9, 2017
 * 
 * Script controls the power bar, which counts the time the player holds the space bar and gives a power
 * amount relative to how long the player holds down the space bar. It then calls the toss method using
 * that power number.
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerBarControl : MonoBehaviour
{
    public BallControl ball;

    public float maxTimeInSeconds;

    Slider powerBar;

    float instantiationTime, powerTime;

    //What this object should do when the space key is pressed
    void SpaceKeyHandler_PowerBarControl()
    {
        powerBar.value = CalculatePower();
    }

    // Use this for initialization
    void Start ()
    {
        Transform ballsParent = GameObject.Find("Balls").transform;
        int childCount = ballsParent.childCount;
        ball = ballsParent.GetChild(childCount - 1).GetComponent<BallControl>();
        if (!ball)
        {
            Debug.Log("unable to get ball object");
        }
        ball.spaceKeyObserver += SpaceKeyHandler_PowerBarControl;
        powerBar = gameObject.GetComponent<Slider>();
        powerBar.value = 0;
        instantiationTime = Time.timeSinceLevelLoad;
    }

    //timeSinceLevelLoad should be the moment the player releases the space bar
    float CalculatePower()
    {
        powerTime = Time.timeSinceLevelLoad - instantiationTime;
        if (powerTime == maxTimeInSeconds)
        {
            return powerBar.maxValue;
        }
        else
	    {
            return powerBar.value = (powerTime / maxTimeInSeconds) * 100; 
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            if (powerTime >= maxTimeInSeconds)
            {
                ball.Toss(powerBar.maxValue);
            }
            else
            {
                ball.Toss(powerBar.value);
            }
            Destroy(gameObject);
        }
    }
}
