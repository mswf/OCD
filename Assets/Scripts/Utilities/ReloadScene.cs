using UnityEngine;
using System.Collections;

public class ReloadScene : MonoBehaviour {

	void Update ()
	{
		if (Input.GetKeyDown("y"))
		{
			Application.LoadLevel(Application.loadedLevelName);
			
		}
	}
}
