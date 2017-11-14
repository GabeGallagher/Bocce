using UnityEngine;
using System.Collections;

public class BocceControl : MonoBehaviour
{
    public GameObject pallino;

    public LayerMask layerMask;

    public bool isGreen; //if false, is Red

    Ray ray;

    RaycastHit hit;
	
	void Start ()
    {
        pallino = FindObjectOfType<PallinoControl>().gameObject;
	}
	
	void Update ()
    {
        Debug.DrawRay(transform.position, pallino.transform.position, Color.green);
        ray = new Ray(transform.position, pallino.transform.position);
        if (Physics.Raycast(ray, out hit))
        {
            //Debug.Log("Distance to pallino: " + hit.distance);
        }
    }
}
