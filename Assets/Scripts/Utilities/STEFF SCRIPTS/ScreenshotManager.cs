using UnityEngine;
using System.Collections;

[AddComponentMenu("Rendering/Make Ingame Screenshots")]

// Made by Steff Kempink
// In case of errors or memory leaks, hold Weikie Yeh responsible

// The script stores screenshots in the base directory
public class ScreenshotManager : MonoBehaviour 
{
	public KeyCode screenshotButton = KeyCode.F6;

	public bool enableHighRes = true;
	public int highResMagnification = 4;

	private int _screenshotCount = 1;
	
	void Start ()
	{
		highResMagnification = highResMagnification < 1 || highResMagnification > 12 ? 1 : highResMagnification;
	}
	
	// Update is called once per frame
	void Update () 
	{
		bool _highRes = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift) ? true : false;
		
		if(Input.GetKeyDown(screenshotButton))	_TakeScreenshot (_highRes);
		
		//if(Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
	}
	
	private void _TakeScreenshot (bool highRes)
	{
		if (highRes && enableHighRes)
		{
			string _screenshotName = "Screenshot_"+  _screenshotCount.ToString() +"_HighRes.png";
			Application.CaptureScreenshot (_screenshotName, highResMagnification);
		}
		else
		{
			string _screenshotName = "Screenshot_"+  _screenshotCount.ToString() +".png";
			Application.CaptureScreenshot (_screenshotName, 1);
		}
		_screenshotCount ++;
	}
}