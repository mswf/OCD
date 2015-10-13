using UnityEngine;
using System.Collections;

public class GraphicsPage : IPage 
{
	private string[] _qualityNames;

	private void Start()
	{
		Initialize();
		_qualityNames = QualitySettings.names;
		loadGraphics();
		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetOptionCount(_qualityNames.Length);
			menuButtons[2].SetButtonMethod(BackButton);
			menuButtons[1].SetButtonMethod(SaveGraphicsSettings);
		}
	}

	public override void Update ()
	{
		for (int i = 0; i < menuButtons.Count; i++) 
		{
			if (controller.Left())
			{
				if (menuButtons[i].isButtonSelected)
				{
					menuButtons[0].PlayButtonSelectionSound();
					QualitySettings.DecreaseLevel();
				}
			}
			
			if (controller.Right())
			{
				if (menuButtons[i].isButtonSelected)
				{
					menuButtons[0].PlayButtonSelectionSound();
					QualitySettings.IncreaseLevel();
				}
			}
		}

		menuButtons[0].SetText( "< " + GetQualityName() + " >");

		base.Update();

	}

	private void BackButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Settings);
		animator.SlideIn();
	}

	private void SaveGraphicsSettings()
	{
		saveGraphics();
	}
	
	private string GetQualityName()
	{
		return _qualityNames[QualitySettings.GetQualityLevel()];
	}

	private void saveGraphics()
	{
		PlayerPrefs.SetInt("graphics", QualitySettings.GetQualityLevel());
	}

	private void loadGraphics()
	{
		QualitySettings.SetQualityLevel(PlayerPrefs.GetInt("graphics", 4));
	}
}
