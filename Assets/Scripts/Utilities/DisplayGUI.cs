using System.Diagnostics;
using UnityEngine;
using System.Collections;
using Debug = UnityEngine.Debug;

public class DisplayGUI : OculusSeteoGui
{

	public Color textColor;
	public float starPosX;
	public float starPosY;
	public float boxSizeX;
	public float boxSizeY;
	public string text = "YOUR TEXT HERE";
	public float autoPilotX = 560;
	public float autoPilotY = 380;
	public float autoPilotW = 170;
	public float autoPilotH = 40;
	
	public float lapX = 520;
	public float lapY = 270;
	public float lapW = 170;
	public float lapH = 40;
	
	
	public Vehicle player;
	private bool showPosition = true;
	
	
	
	
	
	override public void OVGUI()
	//override public void OVGUI()
	{
		/*
		    ( x position on screen 	(INT, Float)
		    y position on screen 	(INT, Float)
		    width of the box		(INT, Float)
		    height of the box		(INT, Float)
		    string/image			(String/Texture)
		    string color			(Color)
		    )
		*/
		//EXAMPLES
		switch (player.positionInRace)
		{
		case 1:
			text = "1st";
			break;
			
		case 2:
			text = "2nd";
			break;
			
		case 3:
			text = "3rd";
			break;
			
		case 4:
			text = "4th";
			break;
			
		case 5:
			text = "5th";
			break;
			
		default:
			break;
		}
		
		//if is not paused
		if (Time.timeScale == 1)
		{
			if (player.finished)
			{
				string finishText = "Finished!";
				WeikiesRiftHack.StereoBox(autoPilotX, autoPilotY, autoPilotW, autoPilotH, ref finishText, textColor);
			}
			
			else
			{
				//position
				if (showPosition)
				{
					WeikiesRiftHack.StereoBox(starPosX, starPosY, boxSizeX, boxSizeY, ref text, textColor);
				}
				
				//autopilot
				if (player.autoPilotText != "")
				{
					WeikiesRiftHack.StereoBox(autoPilotX, autoPilotY, autoPilotW, autoPilotH, ref player.autoPilotText, textColor);
				}
				
				//lap
				if (player.lapText != "")
				{
					WeikiesRiftHack.StereoBox(lapX, lapY, lapW, lapH, ref player.lapText, textColor);
				}
			}
		}
	}
	
	public void disablePosition()
	{
		showPosition = false;
	}
	
	public void enablePosition()
	{
		showPosition = true;
	}
}
