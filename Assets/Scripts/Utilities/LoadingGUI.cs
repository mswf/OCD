using UnityEngine;
using System.Collections;

public class LoadingGUI : OculusStereoGui 
{
	public Color textColor;
	public int starPosX;
	public int starPosY;
	public int boxSizeX;
	public int boxSizeY;
	public string loadingText = "Loading... ";
	private bool _load = false;
	public bool isLoading { get{return _load;} set{_load = value;} }

	public override void OVGUI()
	{
		if (isLoading) 
		{
			//GuiHelper.StereoBox(starPosX, starPosY, boxSizeX, boxSizeY, ref loadingText, textColor);
		}
	}

	public void SetLoadingText(float progress)
	{
		loadingText = "Loading... " + progress.ToString() + "%";
	}
}
