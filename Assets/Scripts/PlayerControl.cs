using UnityEngine;
using System.Collections;

public class PlayerControl : MonoBehaviour
{
    Ball ball;

	// Use this for initialization
	void Start ()
    {
        ball = ball.GetComponent<Ball>();
    }

    public void TossBall()
    {
        //Do something crazy
    }
}
