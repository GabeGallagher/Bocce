/* Author Gabriel B. Gallagher November 6, 2017
 *
 * Script handles the behavior of the back wall. Among other rules, if a Bocce hits the back wall,
 * it cannot be counted for scoring at the end of the round.
 */

using UnityEngine;
using System.Collections;

public class WarningWall : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<BocceControl>())
        {
            collision.gameObject.GetComponent<BallControl>().hitWarningWall = true;
        }
    }
}
