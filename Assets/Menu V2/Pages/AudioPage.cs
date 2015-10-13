using UnityEngine;
using System.Collections;

public class AudioPage : IPage 
{
	private float _volume;

	private void Start()
	{
		Initialize();
		loadVolume();

		if (menuButtons.Count > 0)
		{
			menuButtons[2].SetButtonMethod(BackButton);
			menuButtons[1].SetButtonMethod(SaveVolumeSettings);
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
					_volume -= 0.05f;
					if (_volume <= 0.0f) _volume = 0.0f;
					AudioListener.volume = _volume;
				}
			}
			
			if (controller.Right())
			{
				if (menuButtons[i].isButtonSelected)
				{
					menuButtons[0].PlayButtonSelectionSound();
					_volume += 0.05f;
					if (_volume >= 1.0f) _volume = 1.0f;
					AudioListener.volume = _volume;
				}
			}
		}
		menuButtons[0].SetText(AudioListener.volume.ToString("Volume < 0 % >"));
		base.Update();

	}



	private void BackButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Settings);
		animator.SlideIn();
	}

	private void SaveVolumeSettings()
	{
		menuButtons[1].PlayButtonPressSound(menuButtons[1].pressSound);
		saveVolume();
	}
	
	private void saveVolume()
	{
		PlayerPrefs.SetFloat("volume", Mathf.Clamp01(_volume));
	}

	private void loadVolume()
	{
		_volume = PlayerPrefs.GetFloat("volume", 1.0f);
		AudioListener.volume = _volume;
	}
}
