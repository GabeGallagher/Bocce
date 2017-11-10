using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PowerBarControl : MonoBehaviour
{
    public TossBall tossBall;

    public float maxTimeInSeconds;

    Slider powerBar;

    float instantiationTime, powerTime;

    // Use this for initialization
    void Start ()
    {
        tossBall.spaceKeyObserver += SpaceKeyHandler_PowerBarControl;
        powerBar = gameObject.GetComponent<Slider>();
        powerBar.value = 0;
        instantiationTime = Time.timeSinceLevelLoad;
    }

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

    void SpaceKeyHandler_PowerBarControl()
    {
        Debug.Log("Power bar script reads space key down");
        powerBar.value = CalculatePower();
    }

    // Update is called once per frame
    void Update ()
    {
        //float powerTime = Time.timeSinceLevelLoad - instantiationTime;

        if (Input.GetKeyUp(KeyCode.Space))
        {
            Debug.Log("Tossing " + tossBall.gameObject.name);

            if (powerTime >= maxTimeInSeconds)
            {
                tossBall.gameObject.GetComponent<Ball>().Toss(powerBar.maxValue);
            }
            else
            {
                tossBall.gameObject.GetComponent<Ball>().Toss(powerBar.value);
            }
            Destroy(gameObject);
        }
    }
}
