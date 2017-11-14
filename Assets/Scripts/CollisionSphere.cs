/* Author Gabriel B. Gallagher November 7, 2017
 * 
 * Script that handles the behavior of the sphere trigger attached to each ball. The purpose of this
 * sphere is to simply report when another ball is in the vicinity of the ball that this component is
 * attached to.
 */

using UnityEngine;
using System.Collections;

public class CollisionSphere : MonoBehaviour
{
    void OnTriggerEnter(Collider trigger)
    {
        if (trigger.GetComponent<BallControl>())
        {
            Debug.Log(trigger.name + " entered " + transform.parent.name + "'s collision sphere");
            if (trigger.GetComponent<Rigidbody>().velocity == Vector3.zero)
            {
                transform.parent.gameObject.GetComponent<BallControl>().isObjectInCollisionArea = false;
            }
            else
	        {
                transform.parent.gameObject.GetComponent<BallControl>().isObjectInCollisionArea = true; 
            }
        }
    }

    private void OnTriggerExit(Collider trigger)
    {
        if (trigger.GetComponent<BallControl>())
        {
            Debug.Log(trigger.name + " left " + transform.parent.name + "'s collision sphere");
            transform.parent.gameObject.GetComponent<BallControl>().isObjectInCollisionArea = false;
        }
    }
}
