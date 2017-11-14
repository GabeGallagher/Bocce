using UnityEngine;
using System.Collections;

public class PallinoControl : BallControl
{
    public override void Toss(float force)
    {
        base.Toss(force);
        Debug.Log("Tossed from pallino control");
    }
    // Use this for initialization
    void Start ()
    {
	
	}
	
	// Update is called once per frame
	void Update ()
    {
	
	}
}
