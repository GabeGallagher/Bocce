/* Author Gabriel B. Gallagher November 10, 2017
 *
 * Script handles instantiation of new balls. First, the player throws the Pallino. Then, each subsequent
 * player should throw their Bocce and try to get it as close as pobbile to the Pallino. Players can hit
 * the Pallino or other Bocces, allowing them to get their own Bocces closer, or knock another teams' Bocce
 * away. Essentially, whichever team has the most Bocces closest to the Pallino wins the game.
 * 
 * Script also keeps track of whose turn it is and how many balls have been thrown.
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BallParent : MonoBehaviour
{
    public delegate void OnBeginNewRound();
    public OnBeginNewRound newRoundReporter;

    public GameObject pallinoPrefab, boccePrefabGreen, boccePrefabRed, greenScoreReporter, redScoreReporter;

    public bool isGreenTurn; //Just determines if it's Green's turn. Could evaluate for red, doesn't matter
                             //as long as the evaluation is consistent.

    public int redBocceCount, greenBocceCount, tossesPerRound, winningScore;

    ArrowControl arrow;

    GameObject ball;

    public int greenTeamScore, redTeamScore;

    //What to do when a new round begins
    void BeginNewRoundReporter()
    {
        Debug.Log("Reporting new round");
        GetPallino();
        greenTeamScore = 0;
        redTeamScore = 0;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
    }

    void BeginNewRound(int greenScore, int redScore)
    {
        newRoundReporter += BeginNewRoundReporter;
        //update UI to add scores
        greenScoreReporter.GetComponent<ScoreReporter>().UpdateScore(greenScore);
        redScoreReporter.GetComponent<ScoreReporter>().UpdateScore(redScore);

        //delete all Bocces in arena
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<BallControl>())
            {
                Destroy(transform.GetChild(i).gameObject);
            }
        }
        GameObject pallino = Instantiate(pallinoPrefab, transform.position, Quaternion.identity)
            as GameObject;
        pallino.transform.parent = transform;
        newRoundReporter();
    }

    /* Once each player has tossed their bocces (in a normal round, each player get 4 throws, but this
     * number may be adjusted for testing purposes) the game moves to a score gathering state. Each bocce
     * is tested for its distance to the pallino and sorted based on that distance. Once the list of
     * bocces is sorted, we check the color of the closest bocce to the pallino. Based on the color of the
     * closest bocce, we then see how many bocces of that color are closer to the pallino than the closest
     * bocce of the other color. Once we encounter the closest bocce of the other color, we stop counting,
     * add the count to the total score, and restart the round.
     * 
     * method is called in the KillCommandHandler and should call the begin new round method to reset the
     * board
     */
    void GetScore()
    {
        Debug.Log("Get score called");
        List<GameObject> bocceList = new List<GameObject>();
        List<float> distanceList = new List<float>();

        //go through each bocce on the field and measure its distance to the pallino
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<BocceControl>())
            {
                BocceControl bocce = transform.GetChild(i).GetComponent<BocceControl>();
                //get the distances and build distance list
                bocce.distance = bocce.GetDistance();
                distanceList.Add(bocce.distance);
            }
        }

        //sort the list of children by distance
        distanceList.Sort();

        for (int i = 0; i < distanceList.Count; ++i)
        {
            for (int j = 0; j < transform.childCount; ++j)
            {
                if (transform.GetChild(j).GetComponent<BocceControl>() &&
                    Mathf.Approximately(transform.GetChild(j).GetComponent<BocceControl>().distance,
                    distanceList[i]))
                {
                    bocceList.Add(transform.GetChild(j).gameObject);
                }
            }
        }

        //get the color of the closest item in the list
        if (bocceList[0].GetComponent<BocceControl>().isGreen)
        {
            ++greenTeamScore;
            //go through the sorted list and count how many green bocce are closer to the pallino 
            //than the closest red bocce
            for (int i = 1; i < bocceList.Count; ++i)
            {
                if (bocceList[i].GetComponent<BocceControl>().isGreen)
                {
                    //record that number as the score of the round
                    ++greenTeamScore;
                }
                else
                {
                    //TODO fix possible bugs occuring here when bocces are equidistant from pallino
                    //if two balls of opposite colors are equidistant from the pallino, give both teams a
                    //point and restart the round
                    if (Mathf.Approximately(bocceList[i].GetComponent<BocceControl>().distance,
                        bocceList[0].GetComponent<BocceControl>().distance))
                    {
                        ++greenTeamScore;
                        ++redTeamScore;
                        BeginNewRound(greenTeamScore, redTeamScore);
                    }
                    else
                    {
                        BeginNewRound(greenTeamScore, redTeamScore); 
                    }
                }
            }
        }
        else if (!bocceList[0].GetComponent<BocceControl>().isGreen)
        {
            ++redTeamScore;
            //go through the sorted list and count how many red bocce are closer to the pallino 
            //than the closest green bocce
            for (int i = 1; i < bocceList.Count; ++i)
            {
                if (!bocceList[i].GetComponent<BocceControl>().isGreen)
                {
                    //record that number as the score of the round
                    ++redTeamScore;
                }
                else
                {
                    //TODO fix possible bugs occuring here when bocces are equidistant from pallino
                    if (Mathf.Approximately(bocceList[i].GetComponent<BocceControl>().distance,
                        bocceList[0].GetComponent<BocceControl>().distance))
                    {
                        //if two balls of opposite colors are equidistant from the pallino, give both teams a
                        //point and restart the round
                        ++greenTeamScore;
                        ++redTeamScore;
                        BeginNewRound(greenTeamScore, redTeamScore);
                    }
                    else
                    {
                        BeginNewRound(greenTeamScore, redTeamScore); 
                    }
                }
            }
        }
        else
        {
            Debug.Log("Error reporting closest color");
        }
    }

    void KillCommandHandler_BallParent()
    {
        Debug.Log("Kill Command on: " + name);
        if (greenBocceCount < tossesPerRound && redBocceCount < tossesPerRound)
        {
            InstantiateNewBocce();
        }
        else
        {
            Debug.Log("Moving to scoring");
            GetScore();
        }
    }

    public void InstantiateNewBocce()
    {
        Debug.Log("Instantiating new Bocce");
        if (!isGreenTurn)
        {
            ball = Instantiate(boccePrefabGreen, transform.position, Quaternion.identity)
                as GameObject;
            ball.GetComponent<BocceControl>().isGreen = true;
            isGreenTurn = true;
        }
        else
        {
            ball = Instantiate(boccePrefabRed, transform.position, Quaternion.identity)
                as GameObject;
            ball.GetComponent<BocceControl>().isGreen = false;
            isGreenTurn = false;
        }
        ball.transform.parent = transform;
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
        arrow = transform.GetChild(0).GetComponent<ArrowControl>();
        arrow.ball = ball.GetComponent<BallControl>();
        arrow.isRotating = true;
    }

    void GetPallino()
    {
        for (int i = 0; i < transform.childCount; ++i)
        {
            if (transform.GetChild(i).GetComponent<PallinoControl>())
            {
                ball = transform.GetChild(i).gameObject;
            }
        }
        if (!ball)
        {
            Debug.Log("Error getting ball");
        }
    }

    void Start ()
    {
        GetPallino();
        ball.GetComponent<BallControl>().killCommandObserver += KillCommandHandler_BallParent;
    }
}
