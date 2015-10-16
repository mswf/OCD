using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Checkpoint : MonoBehaviour
{
	public GameLogic gameLogic;
	private int checkpointNumber;

	void Awake()
	{
		checkpointNumber = 0;
		if (!int.TryParse(name.Substring(name.IndexOf('_') + 1), out checkpointNumber))
		{
			Debug.Log("Checkpoint name needs to end with _xx where xx is a number (eg. checkpoint_1)");
		}
	}

	void OnTriggerEnter(Collider collider)
	{
		Vehicle vehicle = collider.GetComponent<Vehicle>();
		if (vehicle != null) gameLogic.hitCheckpoint(vehicle, checkpointNumber);
	}
}
