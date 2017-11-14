/* Author Gabriel B. Gallagher November 10, 2017
 *
 * Script decides which color goes first
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TurnReporter : MonoBehaviour
{
    public Text text;

    public bool isGreenTurn;

    bool GetTeamTurn()
    {
        return GameObject.Find("Balls").GetComponent<BallParent>().isGreenTurn;
    }

	void Start ()
    {
        text = transform.GetChild(0).GetComponent<Text>();
        isGreenTurn = GetTeamTurn();
    }
	
	void Update ()
    {
        isGreenTurn = GetTeamTurn();

        if (isGreenTurn)
        {
            text.text = "Green Team Turn";
            text.color = Color.green;
        }
        else
        {
            text.text = "Red Team Turn";
            text.color = Color.red;
        }
	}
}
