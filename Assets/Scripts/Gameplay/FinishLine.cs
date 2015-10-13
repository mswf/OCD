using UnityEngine;
using System.Collections;

public class FinishLine : MonoBehaviour {

	public GameLogic gameLogic;

	void OnTriggerEnter(Collider collider)
	{
		gameLogic.hitLap(collider);
	}
}
