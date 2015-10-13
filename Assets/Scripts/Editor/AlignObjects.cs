using UnityEngine;
using System.Collections;
using UnityEditor;


class AlignObjects : MonoBehaviour {

	[MenuItem ("Steff Utilities/Align Pickups to track %&k")]
	private static void alignPickups ()
	{
		GameObject p = GameObject.Find("Pickups");
		if (p == null) return;
		foreach (Transform t in p.transform)
		{
			alignFloor(t, 1f);
			alignWall(t);
		}
	}

	[MenuItem("Steff Utilities/Align Checkpoints to track")]
	private static void alignCheckpoints()
	{
		GameObject p = GameObject.Find("Checkpoints");
		if (p == null) return;
		foreach (Transform t in p.transform)
		{
			alignFloor(t, 5f);
			alignWall(t);
		}
	}


	private static void alignWall(Transform transform)
	{
		RaycastHit hitInfo;					//hitinfo of the raycast
		Ray ray = new Ray(transform.position, -transform.right);
		Ray ray2 = new Ray(transform.position, transform.forward);


		//raycast down from given position to layer 9 (wall)
		if (Physics.Raycast(ray, out hitInfo, 50, 1 << 9))
		{
			//rotate to normal
			transform.localRotation = Quaternion.FromToRotation(transform.right, hitInfo.normal) * transform.rotation;
		}
		else if (Physics.Raycast(ray2, out hitInfo, 50, 1 << 9))
		{
			transform.localRotation = Quaternion.FromToRotation(transform.right, hitInfo.normal) * transform.rotation;
		}
		else 
		{
			//Debug.Log(transform.name + " " + transform.position + " not aligned to wall, no nearby track found.");
		}
	}

	private static void alignFloor(Transform transform, float dist)
	{
		RaycastHit hitInfo;
		Ray ray = new Ray(transform.position + transform.up, -transform.up);

		//raycast down from given position to layer 8 (floor)
		if (Physics.Raycast(ray, out hitInfo, 50, 1 << 8))
		{
			//rotate to normal
			Quaternion q = Quaternion.FromToRotation(transform.up, hitInfo.normal);
			transform.localRotation = Quaternion.Euler(-q.eulerAngles.x, transform.rotation.eulerAngles.y, -q.eulerAngles.z);

			//change position to clip at certain dist
			transform.position = transform.position + transform.up * (dist - hitInfo.distance);
			transform.rotation = Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation;
		}
		else
		{
			Debug.Log(transform.name + " " + transform.position + " not aligned to floor, no nearby track found.");
		}
	}
}
