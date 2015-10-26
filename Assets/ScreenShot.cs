using UnityEngine;
using System.Collections;

public class ScreenShot : MonoBehaviour {

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.G))
		{
			Debug.Log("hai(");
			Application.CaptureScreenshot("Screenshot2.png", 2);

		}
	}
}
