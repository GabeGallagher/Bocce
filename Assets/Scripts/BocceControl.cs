using UnityEngine;
using System.Collections;

public class BocceControl : MonoBehaviour
{
    public GameObject pallino;

    public LayerMask layerMask;

    public float distance;

    public bool isGreen; //if false, is Red

    Ray ray;

    RaycastHit hit;
	
	void Start ()
    {
        pallino = FindObjectOfType<PallinoControl>().gameObject;
	}
	
	public float GetDistance ()
    {
        Debug.DrawRay(transform.position, pallino.transform.position, Color.green);
        ray = new Ray(transform.position, pallino.transform.position);
        if (Physics.Raycast(ray, out hit))
        {
            Debug.Log("Distance to pallino: " + hit.distance);
            return hit.distance;
        }
        else
        {
            Debug.Log("Error reporting " + name + " distance");
            return 1000.0f;
        }
    }
}
