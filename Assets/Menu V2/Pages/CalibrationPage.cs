using UnityEngine;
using System.Collections;

public class CalibrationPage : IPage 
{
	private Calibration _calibrater;
	private string _informationLabel;

	private void Start()
	{
		_calibrater = gameObject.GetComponent<Calibration>();
		_calibrater.state = Calibration.State.none;

		Initialize();
		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetButtonMethod(StepCalibrationButton);
			menuButtons[1].SetButtonMethod(CancelButton);
		}
	}

	public override void Update ()
	{
		base.Update();
		UpdateCalibrationState();
	}

	private void UpdateCalibrationState()
	{
		switch (_calibrater.state) 
		{		
			case Calibration.State.none:
			{
				_informationLabel = "State.None - Waiting for \n calibration to begin.";
				break;
			}
			case Calibration.State.neutral:
			{
				_informationLabel = "State.Neutral - Keeps reseting \n camera orientation until \n pressing next.";
				break;
			}
			case Calibration.State.axis:
			{
				_informationLabel = "State.Axis - Choose an Axis.";
				// SHOULD BE CHANGED
				break;
			}
			case Calibration.State.left:
			{
				_informationLabel = "State.Left - Set the LEFT \n far most value.";
				break;
			}
			case Calibration.State.right:
			{
				_informationLabel = "State.Right - Set the RIGHT \n far most value.";
				break;
			}
			case Calibration.State.done:
			{
				_informationLabel = "State.Done - Press Finish \n to save calibration.";
				menuButtons[0].SetText("Finish");
				break;
			}
			default:
				break;
		}
		menuLables[0].SetText(_informationLabel);
	}

	private void StepCalibrationButton()
	{
		if (_calibrater.state == Calibration.State.done)
		{
			menuButtons[0].PlayButtonPressSound(menuButtons[0].backSound);
			//updateProfile(caliber.axis ,caliber.leftOrientation, caliber.rightOrientation);
			Debug.Log("Calibration was done. Swithced page. NO SAVING CURRENTLY!");
			menuReference.ChangeCurrentPageTo(Menu_V2.Page.Controls);
			animator.SlideIn();
		}
		else
		{
			menuButtons[0].PlayButtonPressSound(menuButtons[0].pressSound);
			_calibrater.state++;
		}

	}
	/* SETTING AXIS WHICH IS NOT TO BE IN FINAL BUILD
	private void UpdateSelectedAxis()
	{
		if (selectedAxis == 0) 
		{
			caliber.axis = Axis.x;
			PlayerPrefs.SetString("Axis", "x");
			showAxis(0);
		}
		if (selectedAxis == 1) 
		{
			caliber.axis = Axis.y;
			PlayerPrefs.SetString("Axis", "y");
			showAxis(1);
		}
		if (selectedAxis == 2) 
		{
			caliber.axis = Axis.z;
			PlayerPrefs.SetString("Axis", "z");
			showAxis(2);
		}
	}
	*/
	private void CancelButton()
	{
		menuButtons[1].PlayButtonPressSound(menuButtons[1].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Controls);
		animator.SlideIn();
	}
}
