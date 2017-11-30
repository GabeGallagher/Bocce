/* Author Gabriel B. Gallagher November 12, 2017
 *
 * Script allows the Bocce to measure the distance between itself and the palino in order to score the
 * game
 */

using UnityEngine;
using System.Collections;

public class BocceControl : MonoBehaviour
{
    public GameObject pallino;

    public LayerMask layerMask;

    public bool isGreen; //if false, is Red
	
	void Start ()
    {
        GetPallino();
    }

    void GetPallino()
    {
        int count = transform.parent.childCount;
        for (int i = 0; i < count; ++i)
        {
            if (transform.parent.GetChild(i).GetComponent<PallinoControl>())
            {
                pallino = transform.parent.GetChild(i).gameObject;
            }
        }

        if (!pallino)
        {
            Debug.Log("Error getting pallino");
        }
    }

    Vector3 GetRayDirection()
    {
        return new Vector3(pallino.transform.position.x - transform.position.x,
                           pallino.transform.position.y - transform.position.y,
                           pallino.transform.position.z - transform.position.z);
    }

    public float GetDistance()
    {
        Ray ray;

        RaycastHit hit;

        Vector3 rayDirection = GetRayDirection();

        ray = new Ray(transform.position, rayDirection);
        if (Physics.Raycast(ray, out hit, layerMask))
        {
            return hit.distance;
        }
        else
        {
            Debug.Log("Error reporting " + name + " distance");
            return 1000.0f;
        }
    }
}
