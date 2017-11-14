/* Author Gabriel B. Gallagher November 30 2016
 * 
 * Script manages changes between levels
 */

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{
    public float autoLoadNextLevelAfter;

    void Start()
    {
    	if (autoLoadNextLevelAfter==0)
        {
    		Debug.Log("Level auto load disabled");
    	}
        else
        {
			Invoke("LoadNextLevel", autoLoadNextLevelAfter);
		}
    }

    public void LoadLevel(string name)
    {
		Debug.Log("New level load:" + name);
		Application.LoadLevel(name);
    }

	public void QuitRequest()
    {
		Application.Quit();
    }
}