using UnityEngine;
using System.Collections;

public class CollisionChanger : MonoBehaviour {

	public enum CollisionType
	{
		Shortcut,
		Normal
	}

	public CollisionType swapTo;

	private void OnTriggerEnter(Collider collider)
	{
		Vehicle vehicle = collider.GetComponent<Vehicle>();
		if (vehicle == null) 
		{
			Debug.Log("ERROR cant swap collision layer, vehicle component not found");
		}
		if (swapTo == CollisionType.Normal) vehicle.onShortcut = false;
		else vehicle.onShortcut = true;
	}
}
