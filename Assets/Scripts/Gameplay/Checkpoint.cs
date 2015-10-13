using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Checkpoint : MonoBehaviour {
	public GameLogic gameLogic;
	
	void OnTriggerEnter(Collider collider)
	{
		int i = 0;
		if (int.TryParse(name.Substring(name.IndexOf('_') + 1), out i))
		{
			Vehicle v = collider.GetComponent<Vehicle>();
			if (v != null) gameLogic.hitCheckpoint(v, i);
		}
		else
		{
			Debug.Log("Checkpoint name needs to end with _xx where xx is a number (eg. checkpoint_1)");
		}
	}
}
