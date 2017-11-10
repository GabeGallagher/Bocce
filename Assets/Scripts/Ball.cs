/* Author Gabriel B. Gallagher November 6, 2017
 *
 * Script handles the behavior of the ball excluding user input. Each ball has a Sphere trigger attached
 * to it which is meant to ping whenever another ball is near this ball. This allows us to control
 * collisions and velocity of the balls.
 */

using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour
{
    public float power;

    public float liftAdjustment; // adjusts how much lift should get applied to the ball based on power

    public float stopThreshold; //how slow ball should be moving before kill command is called

    public int delayTime; // amount of time before object gets destroyed if it hits the vanishing plane

    public bool isObjectInCollisionArea; // determines if there is an object that could collide with us

    public bool isOnFloor;

    Rigidbody rBody;

    public Vector3 velocity;

    SphereCollider collisionSphere;

    public void Toss(float force)
    {
        rBody.isKinematic = false;
        GameObject arrow = GameObject.Find("Arrow");
        if (!arrow)
        {
            Debug.Log("No arrow found");
        }
        Debug.Log("Toss force: " + force);
        transform.rotation = arrow.transform.rotation;
        rBody.AddRelativeForce(new Vector3(1.0f, liftAdjustment, 0.0f) * force, ForceMode.Impulse);
    }

    void Start ()
    {
        rBody = GetComponent<Rigidbody>();
        if (!collisionSphere)
        {
            collisionSphere = transform.GetChild(0).GetComponent<SphereCollider>();
        }
        if (stopThreshold <= 0)
        {
            Debug.Log("Error, " + name + " stop threshold is less than or equal to 0");
        }
        //Toss(power);
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

    void Kill()
    {
        rBody.isKinematic = true;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>())
        {
            Debug.Log(name + " collided with " + collision.gameObject.name);
            
            //collision.gameObject.GetComponent<Rigidbody>().isKinematic = false;
        }

        if (collision.gameObject.name == "Floor")
        {
            Debug.Log(name + " is on the floor");
            isOnFloor = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name == "Floor")
        {
            isOnFloor = false;
        }
    }

    private void OnTriggerEnter(Collider trigger)
    {
        if (trigger.name == "VanishingPlane")
        {
            Destroy(gameObject, delayTime);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        velocity = rBody.velocity; //Delete on completion
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

            if (rBody.velocity == Vector3.zero && isObjectInCollisionArea)
            {
                rBody.isKinematic = false;
            }
        }
	}
}
