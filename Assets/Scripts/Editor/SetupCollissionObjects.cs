using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;


// Made by Steff Kempink
// In case of errors or memory leaks, hold Weikie Yeh responsible

class SetupCollissionObjects : MonoBehaviour 
{
	[MenuItem ("Steff Utilities/Setup Collissions %&l")]
	private static void setupObjects ()
	{
		List<GameObject> wallParts = new List<GameObject>();
		FindByPartialName(wallParts, "_wall");
		AddColliders(wallParts, "_wall");
		RemoveRenderers (wallParts);
		
		wallParts.Clear();
		
		List<GameObject> floorParts = new List<GameObject>();
		FindByPartialName(floorParts, "_floor");
		AddColliders(floorParts, "_floor");
		RemoveRenderers(floorParts);
		
		floorParts.Clear();
		
		List<GameObject> rampParts = new List<GameObject>();
		FindByPartialName(rampParts, "_ramp");
		AddColliders(rampParts, "_ramp");
		
		rampParts.Clear();
		
		
		Debug.Log("Collissions have been set up.");
	}
		
	private static void FindByPartialName(List<GameObject> gameObjectsWithName, string searchString)
	{
    	foreach (GameObject obj in Object.FindObjectsOfType(typeof(GameObject)))
    	{
			if (obj.name.Contains(searchString))gameObjectsWithName.Add(obj);
    	}
	}
	
	private static void AddColliders(List<GameObject> targetObjects, string typeString)
	{
		foreach (GameObject obj in 	targetObjects)
		{
			if (obj.GetComponent<MeshCollider>() == null)
			{
				obj.AddComponent<MeshCollider>();
			}

			if(typeString == "_wall") obj.layer = 9;
			else if (typeString == "_floor" || typeString == "_ramp") obj.layer = 8;
			
			if(typeString!= "_ramp") obj.isStatic = true;
		}
	}
	
	private static void RemoveRenderers(List<GameObject> targetObjects)
	{
		foreach (GameObject obj in targetObjects)
		{
			if (obj.GetComponent<MeshRenderer>() == null)
			{
					
			}
			else
			{
				DestroyImmediate(obj.GetComponent<MeshRenderer>());	
			}
		}
	}
}