using UnityEngine;
using System.Collections;

/// <summary>
/// Made this class because I accepted that I will have to forever keep updating the SDK for the rest of my life whenever it is needed
/// And to make my own life easier, all SDK calls will be done from here.
/// </summary>

public class WeikiesRiftHack
{
	public static void ResetOrientation()
	{
		OVRManager.display.RecenterPose();
	}
	
	public static Quaternion GetOrientation()
	{
		OVRPose pose = OVRManager.tracker.GetPose(0);
		return pose.orientation;
	}
	
	public static bool IsConnected()
	{
		return OVRManager.isHmdPresent;
	}
	
	public static void StereoBox(float x, float y, float width, float height, ref string text, Color color)
	{
		//GuiHelper.StereoBox(autoPilotX, autoPilotY, autoPilotW, autoPilotH, ref finishText, textColor);
		
	}
	
	public static void StereoDrawTexture(float x, float y, float width, float height, ref Texture text, Color color)
	{
		//GuiHelper.StereoDrawTexture(picturePosX, picturePosY, pictureSizeX, pictureSizeY, ref image, pictureColor);
	}
	
}
