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
    TossBall tossBall;

    const float apex = 57.293f; // value of rotation at 0degrees. Lack of accuracy here is what causes the
                                // arrow to stop slightly when it rotates straight ahead

    public bool isRotating = true;

    private void Start()
    {
        tossBall = transform.parent.Find("Bocce").GetComponent<TossBall>();
        tossBall.spaceKeyObserver += SpaceKeyHander_ArrowControl;
    }

    void SpaceKeyHander_ArrowControl()
    {
        Debug.Log("Arrow control reads space key down");
        if (isRotating)
        {
            isRotating = false;
        }
    }

    /* Returns the distance between the origin and the mouse world point. Since we already know the length
     * of a (the distance between the z axis and the mouse world point) and b (the distance between the y
     * axis and the mouse world point) and that angle C will always be 90deg, and that cos(90) is 0, we 
     * simply square a and b and take the square root to get our length.
     */
    float LawOfCosines(float a, float b)
    {
        return Mathf.Sqrt((a * a) + (b * b));
    }

    /* Returns the angle to rotate the arrow about the y axis. The law of sines tells us that 
     * b/sinB = c/sinC. Since we are solving for sinB, we isolate it as the return value, and plug in the
     * arguments. sinC will always be sin90, or 1, so we simply need to solve b/c.
     */
    float LawOfSines(float b, float c)
    {
        return b / c;
    }

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

        float expectedRotation = LawOfSines(pointA, LawOfCosines(pointC, pointA)) * Mathf.Rad2Deg;

        transform.rotation = Quaternion.Euler(0, ConvertArrowRotation(expectedRotation, pointC), 0);
    }

    void OnGUI()
    {
        Vector3 p = new Vector3();
        Camera c = Camera.main;
        Event e = Event.current;
        Vector2 mousePos = new Vector2();

        // Get the mouse position from Event.
        // Note that the y position from Event is inverted.
        mousePos.x = Input.mousePosition.x;
        mousePos.y = Input.mousePosition.y;

        p = c.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, c.farClipPlane));

        GUILayout.BeginArea(new Rect(20, 300, 250, 120));
        GUILayout.Label("Screen pixels: " + c.pixelWidth + ":" + c.pixelHeight);
        GUILayout.Label("Mouse position: " + mousePos);
        GUILayout.Label("World position: " + p.ToString("F3"));
        GUILayout.EndArea();

        float pointA = p.y + 640;   //the origin of the bocce after applying ScreenToWorldPoint algorithm 
                                    //is at about -640 world units on the y axis
        float pointC = p.z;         //The origin of the bocce after applying ScreenToWorldPoint algorith
                                    //is conveniently at 0 world units on the z axis

        GUILayout.BeginArea(new Rect(1600, 300, 250, 120));
        if (pointA <= 0)
        {
            pointA = 0;
        }
        GUILayout.Label("length side a: " + pointA); //side on y axis
        GUILayout.Label("length of side b: " + LawOfCosines(pointC, pointA)); //from origin to mouse point
        GUILayout.Label("length side c: " + pointC); //side on z axis
        float expectedRotation = LawOfSines(pointA, LawOfCosines(pointC, pointA)) * Mathf.Rad2Deg;
        GUILayout.Label("Expected arrow rotation: " + expectedRotation);
        GUILayout.Label("Actual rotation: " + ConvertArrowRotation(expectedRotation, pointC));
        //Max apex is 57.293 with about +/- .0025 variation based on aspect ratio and size of window. 
        //Biggest difference is at 4:3 aspect ratio
        //if (expectedRotation > apex)
        //{
        //    apex = expectedRotation;
        //}
        //GUILayout.Label("Apex:" + apex);
        GUILayout.EndArea();
    }

    // Update is called once per frame
    void Update ()
    {
        if (isRotating)
        {
            ArrowRotation(); 
        }
	}
}
