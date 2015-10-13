using UnityEngine;
using System.Collections;
using System;

public class PauseFuncTest: OculusStereoGui
{

	public int starPosX;
	public int starPosY;
	public int boxSizeX;
	public int boxSizeY;
	public int offset;
	public Color textColor;
	
	public int picturePosX;
	public int picturePosY;
	public int pictureSizeX;
	public int pictureSizeY;
	
	public Color pictureColor;
	public Texture image;
	public string[] options;
	
	private int currOption = 0;
	private bool isPaused = false;
	private string deviceStatus = "";
	private	string text = "HA";
	
	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.JoystickButton0))
		{
			isPaused = ! isPaused;
		}
		
		
		if (Application.HasProLicense())
		{
			if (!WeikiesRiftHack.IsConnected())
			{
				if (!DeviceInput.isDisabled)
				{
					isPaused = true;
				}
			}
		}
		
		PauseGame();
		
	}
	
	public override void OVGUI()
	{
		/*
		    ( x position on screen 	(INT, Float)
		    y position on screen 	(INT, Float)
		    width of the box		(INT, Float)
		    height of the box		(INT, Float)
		    string/image			(String/Texture)
		    string color			(Color)
		    )
		
		    EXAMPLES
		    //string t = "works";
		    //GuiHelper.StereoBox (400, 300, 200, 50, ref t, Color.red);
		    //GuiHelper.StereoDrawTexture(400, 400, 512, 512, ref image, Color.clear);
		
		*/
		if (isPaused)
		{
		
			for (int i = 0; i < options.Length; i++)
			{
				text = options[i];
				
				if (options[i].Contains("VR"))
				{
					text += deviceStatus;
				}
				
				if (currOption == i)
				{
					//Debug.Log(offset + starPosY * i);
					
					text += " <";
					
					WeikiesRiftHack.StereoBox(starPosX, offset + starPosY * i, boxSizeX, boxSizeY, ref text, Color.red);
				}
				
				else
				{
					WeikiesRiftHack.StereoBox(starPosX, offset + starPosY * i, boxSizeX, boxSizeY, ref text, textColor);
				}
			}
			
			WeikiesRiftHack.StereoDrawTexture(picturePosX, picturePosY, pictureSizeX, pictureSizeY, ref image, pictureColor);
			//if (!isConnected) GuiHelper.StereoDrawTexture(picturePosX, picturePosY, pictureSizeX, pictureSizeY, ref image, pictureColor);
		}
		
	}
	
	private bool confirmInput()
	{
		if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.JoystickButton14))
		{
			return true;
		}
		
		else
		{
			return false;
		}
	}
	
	private bool inputUp()
	{
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.JoystickButton4))
		{
			return true;
		}
		
		return false;
	}
	
	private bool inputDown()
	{
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.JoystickButton6))
		{
			return true;
		}
		
		return false;
	}
	
	private void changeCurrentOption(int count)
	{
		if (inputDown() && (currOption + 1 < count))
		{
			currOption++;
		}
		
		if (inputUp() && (currOption - 1 > -1))
		{
			currOption--;
		}
	}
	
	private void PauseGame()
	{
		if (isPaused)
		{
			Time.timeScale = 0;
			
			if (DeviceInput.isDisabled)
			{
				deviceStatus = "Yes";
			}
			
			else
			{
				deviceStatus = "No";
			}
			
			changeCurrentOption(options.Length);
			
			if (confirmInput())
			{
				switch (currOption)
				{
				case 0:
				{
					if (DeviceInput.isDisabled || WeikiesRiftHack.IsConnected())
					{
						isPaused = false;
					}
					
					break;
				}
				
				case 1:
					resetOrientation();
					break;
					
				case 2:
					DeviceInput.isDisabled = ! DeviceInput.isDisabled;
					break;
					
				case 3:
					loadLevel("MainTrack");
					break;
					
				case 4:
					loadLevel("Menu");
					break;
				}
			}
		}
		
		else
		{
			Time.timeScale = 1;
		}
	}
	
	
	
	private void resetOrientation()
	{
		if (WeikiesRiftHack.IsConnected())
		{
			WeikiesRiftHack.ResetOrientation();
		}
	}
	
	private void loadLevel(string level)
	{
		Application.LoadLevel(level);
	}
}
