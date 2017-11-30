/* Author Gabriel B. Gallagher November 8 2017
 * 
 * Script handles the control of the pointer. The pointer should roughly follow the cursor to give the
 * player a sense of where the ball will go when tossed. Arrow currently only rotates about the Y-axis
 * from -90deg to 90deg 
 */

using UnityEngine;
using System.Collections;

public class ArrowControl : MonoBehaviour
{
    public GameObject ball;

    const float apex = 57.293f; // value of rotation at 0degrees. Lack of accuracy here is what causes the
                                // arrow to stop slightly when it rotates straight ahead

    public bool isRotating = true;

    void GetBall()
    {
        GameObject ballParent = transform.parent.gameObject;
        for (int i = 0; i < ballParent.transform.childCount; ++i)
        {
            if (ballParent.transform.GetChild(i).GetComponent<PallinoControl>())
            {
                ball = ballParent.transform.GetChild(i).gameObject;
            }
        }
        if (!ball)
        {
            Debug.Log("Error finding pallino");
        }
    }

    //What this object should do when the space key is pressed
    void SpaceKeyHander_ArrowControl()
    {
        if (isRotating)
        {
            isRotating = false;
        }
    }

    void Start()
    {
        GetBall();
        ball.GetComponent<BallControl>().spaceKeyObserver += SpaceKeyHander_ArrowControl;
        transform.parent.GetComponent<BallParent>().newRoundReporter += BeginNewRoundObserver_ArrowControl;
    }
    
    void BeginNewRoundObserver_ArrowControl()
    {
        Debug.Log("Arrow observing new round");
        GetBall();
        isRotating = true;
        ball.GetComponent<BallControl>().spaceKeyObserver += SpaceKeyHander_ArrowControl;
        transform.parent.GetComponent<BallParent>().newRoundReporter += BeginNewRoundObserver_ArrowControl;
    }

    /* Gives the distance between the origin and the mouse world point. Since we already know the 
     * length of a (the distance between the z axis and the mouse world point) and b (the distance 
     * between the y axis and the mouse world point) and that angle C will always be 90deg, and that 
     * cos(90) is 0, we simply square a and b and take the square root to get our length.
     */
    float LawOfCosines(float a, float b)
    {
        return Mathf.Sqrt((a * a) + (b * b));
    }

    /* Converts arrow rotation to +/- 90 degrees and returns a float that can be directly assigned to the
     * transform.rotation of the arrow
     */
    float ConvertArrowRotation(float rotation, float zLength)
    {
        float converstionRate = 1.5708f;
        float x = apex - rotation; //subtract rotation from max possible rotation
        if (zLength > 0)
        {
            converstionRate *= -1;
        }
        return x * converstionRate;
    }

    /* Uses ScreenToWorldPoint method to get the 3d coordinates of the cursor, then rotates the arrow to 
     * point toward the cursor, allowing the player to determine the direction the ball will be tossed.
     */
    void ArrowRotation()
    {
        Vector3 p = new Vector3();

        Camera c = Camera.main;

        Vector2 mousePos = new Vector2();

        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;

        p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.farClipPlane));

        float pointA = p.y + 640;   //the origin of the bocce after applying ScreenToWorldPoint algorithm 
                                    //is at about -640 world units on the y axis
        float pointC = p.z;         //The origin of the bocce after applying ScreenToWorldPoint algorithm
                                    //is at 0 world units on the z axis
        if (pointA <= 0)
        {
            pointA = 0; //max rotation of arrow is capped at a 180deg arc
        }

        /* Using the law of sines, we get the angle to rotate the arrow about the y axis. The law of 
         * sines  tells us that b/sinB = c/sinC. Since we are solving for sinB, we isolate it as the 
         * return value, and plug in the arguments. sinC will always be sin90, or 1, so we simply need 
         * to solve b/c.
         */
        float expectedRotation = (pointA / LawOfCosines(pointC, pointA)) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, ConvertArrowRotation(expectedRotation, pointC), 0);
    }
    
    void Update ()
    {
        if (isRotating)
        {
            ArrowRotation(); 
        }
	}
}
