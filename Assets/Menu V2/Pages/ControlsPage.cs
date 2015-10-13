using UnityEngine;
using System.Collections;

public class ControlsPage : IPage 
{
	private void Start()
	{
		Initialize();

		if (menuButtons.Count > 0)
		{
			if (DeviceInput.isDisabled) EnableControllerSupport();
			else EnableOculusSupport();
			menuButtons[1].SetButtonMethod(CalibrationButton);
			menuButtons[2].SetButtonMethod(BackButton);
		}
	}

	public override void Update ()
	{
		if (controller.Left())
		{
			if (menuButtons[0].isButtonSelected)
			{
				menuButtons[0].PreviousOption();
				UpdateChange();
			}
		}
			
		if (controller.Right())
		{
			if (menuButtons[0].isButtonSelected)
			{
				menuButtons[0].NextOption();
				UpdateChange();
			}
		}



		base.Update();
	}

	private void UpdateChange()
	{
		if (menuButtons[0].GetCurrentOption() == 0)
		{
			EnableControllerSupport();
		}

		if (menuButtons[0].GetCurrentOption() == 1)
		{
			EnableOculusSupport();
		} 
	}

	private void CalibrationButton()
	{
		menuButtons[1].PlayButtonPressSound(menuButtons[1].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Calibration);
		animator.SlideOut();
	}

	private void BackButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Settings);
		animator.SlideIn();
	}

	private void EnableControllerSupport()
	{
		menuButtons[0].SetText("< Controller >");
		menuButtons[1].isPressable = false;
		menuButtons[1].grayOut = true;
		DeviceInput.isDisabled = true;
	}

	private void EnableOculusSupport()
	{
		menuButtons[0].SetText("< Oculus Rift >");
		menuButtons[1].isPressable = false;
		menuButtons[1].grayOut = true;
		DeviceInput.isDisabled = false;
	}
}
