/* Author Gabriel B. Gallagher November 6, 2017
 *
 * Script handles the behavior of the ball excluding user input. Each ball has a Sphere trigger attached
 * to it which is meant to ping whenever another ball is near this ball. This allows us to control
 * collisions and velocity of the balls.
 */

using UnityEngine;
using System.Collections;

public class BallControl : MonoBehaviour
{
    public delegate void OnSpaceKey();
    public OnSpaceKey spaceKeyObserver;

    public delegate void OnKillCommand();
    public OnKillCommand killCommandObserver;

    public float liftAdjustment; // adjusts how much lift should get applied to the ball based on power

    public float stopThreshold; //how slow ball should be moving before kill command is called

    public int delayTime; // amount of time before object gets destroyed if it hits the vanishing plane

    public bool isObjectInCollisionArea; // determines if there is an object that could collide with us

    public bool isOnFloor, isCounted = false, hitWarningWall = false;

    Rigidbody rBody;

    public Vector3 velocity;

    SphereCollider collisionSphere;

    public bool isTossed = false; //ensures that the kill command is only called once from this object

    public bool isDead;

    bool killCommandEnabled = false;

    //What this object should do when the space key is pressed
    void SpaceKeyHandler_BallControl()
    {
        //Debug.Log("Space Key Down");
    }

    //What this object should do when a moviing ball comes to rest
    public void KillCommandHandler_BallControl()
    {
        Debug.Log(name + " is dead");
    }

    //Throw the ball
    public void Toss(float force)
    {
        rBody.isKinematic = false;
        isTossed = true;
        GameObject arrow = GameObject.Find("Arrow");
        if (!arrow)
        {
            Debug.Log("No arrow found");
        }
        transform.rotation = arrow.transform.rotation;
        rBody.AddRelativeForce(new Vector3(1.0f, liftAdjustment, 0.0f) * force, ForceMode.Impulse);
    }

    void Start ()
    {
        rBody = GetComponent<Rigidbody>();
        spaceKeyObserver += SpaceKeyHandler_BallControl;
        killCommandObserver += KillCommandHandler_BallControl;

        isDead = false;

        if (!collisionSphere)
        {
            collisionSphere = transform.GetChild(0).GetComponent<SphereCollider>();
        }
        if (stopThreshold <= 0)
        {
            Debug.Log("Error, " + name + " stop threshold is less than or equal to 0");
        }
    }

    //Returns the "absolute" average of velocity
    float GetAverageVelocity(Vector3 velocity)
    {
        float x = GetAbsoluteVelocity(velocity.x);
        float y = GetAbsoluteVelocity(velocity.y);
        float z = GetAbsoluteVelocity(velocity.z);
        return (x + y + z) / 3;
    }

    //Takes a floating point velocity from a single axis and returns its absolute value
    float GetAbsoluteVelocity(float f)
    {
        float absoluteVelocity = f;
        if (absoluteVelocity < 0)
        {
            absoluteVelocity *= -1;
        }
        return absoluteVelocity;
    }

    //Bring the moving ball to rest
    void Kill()
    {
        rBody.isKinematic = true;
        isDead = true;

        if (rBody.isKinematic && !killCommandEnabled)
        {
            killCommandObserver();
            killCommandEnabled = true;

            if (!isCounted)
            {
                if (gameObject.GetComponent<BocceControl>().isGreen)
                {
                    ++transform.parent.GetComponent<BallParent>().greenBocceCount;
                }
                else if (!gameObject.GetComponent<BocceControl>().isGreen)
                {
                    ++transform.parent.GetComponent<BallParent>().redBocceCount;
                }
                else
                {
                    Debug.Log("is Pallino");
                }
                isCounted = true;
            }
        }
        else
        {
            Debug.Log("Failed to kill " + name);
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isOnFloor = true;
        }

        if (collision.gameObject.name == "Back")
        {
            hitWarningWall = true;
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isOnFloor = false;
        }
    }
    
    void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            spaceKeyObserver();
        }

        if (!rBody.isKinematic)
        {
            float avgVel = GetAverageVelocity(rBody.velocity);

            if (avgVel <= stopThreshold && isOnFloor)
            {
                Kill();
            }
        }

        if (rBody.isKinematic)
        {
            float avgVel = GetAverageVelocity(rBody.velocity);

            if (rBody.velocity == Vector3.zero && isObjectInCollisionArea && isTossed)
            {
                rBody.isKinematic = false;
            }
        }

        if (rBody.velocity == Vector3.zero)
        {
            isObjectInCollisionArea = false;
        }
	}
}
