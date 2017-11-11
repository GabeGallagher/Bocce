using UnityEngine;
using System.Collections;

public class BallParent : MonoBehaviour
{
    public GameObject boccePrefab;

    ArrowControl arrow;

    GameObject ball;

    void KillCommandHandler_BallParent()
    {
        Debug.Log(name + " reading kill command");
        ball = Instantiate(boccePrefab, transform.position, Quaternion.identity) as GameObject;
        ball.transform.parent = transform;
        arrow = transform.GetChild(0).GetComponent<ArrowControl>();
        arrow.tossBall = ball.GetComponent<TossBall>();
        arrow.isRotating = true;
    }
	
	void Start ()
    {
        ball = transform.GetChild(transform.childCount - 1).gameObject;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
	}
	
	// Update is called once per frame
	void Update ()
    {
	    
	}
}
