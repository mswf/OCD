using UnityEngine;
using System.Collections;

public class SettingsPage : IPage 
{
	private void Start()
	{
		Initialize();
		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetButtonMethod(ControlsButton);
			menuButtons[1].SetButtonMethod(GraphicsButton);
			menuButtons[2].SetButtonMethod(AudioButton);
			menuButtons[3].SetButtonMethod(BackButton);
		}
	}

	public override void Update ()
	{
		base.Update();
	}

	private void ControlsButton()
	{
		menuButtons[0].PlayButtonPressSound(menuButtons[0].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Controls);
		animator.SlideOut();
	}
	
	private void GraphicsButton()
	{
		menuButtons[1].PlayButtonPressSound(menuButtons[1].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Graphics);
		animator.SlideOut();
	}
	
	private void AudioButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Audio);
		animator.SlideOut();
	}
	
	private void BackButton()
	{
		menuButtons[3].PlayButtonPressSound(menuButtons[0].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Main);
		animator.SlideIn();
	}

}
