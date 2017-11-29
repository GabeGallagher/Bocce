/* Author Gabriel B. Gallagher November 14, 2017
 *
 * Script is attached to a score reporter on the canvas and takes the score from the BallParent scoring 
 * method and displays it in the UI. If score is over a certain threshold, this script loads the win page
 */

using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ScoreReporter : MonoBehaviour
{
    public GameObject ballsParent;

    public LevelManager levelManager;

    public int score, winningScore;

    public bool isGreen;

    Text text;

	void Start ()
    {
        text = GetComponent<Text>();
        score = 0;
        text.text = score.ToString();
    }

    public void UpdateScore(int newScore)
    {
        score += newScore;
        if (score >= ballsParent.GetComponent<BallParent>().winningScore)
        {
            if (isGreen)
            {
                levelManager.LoadLevel("GreenVictory");
            }
            else
            {
                levelManager.LoadLevel("RedVictory");
            }
        }
        text.text = score.ToString();
    }
}
