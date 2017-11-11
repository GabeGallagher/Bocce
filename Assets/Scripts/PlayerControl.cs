using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    BallControl ball;

	// Use this for initialization
	void Start ()
    {
        ball = ball.GetComponent<BallControl>();
    }

    public void TossBall()
    {
        //Do something crazy
    }
}
