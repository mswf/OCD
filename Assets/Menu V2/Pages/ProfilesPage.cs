using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ProfilesPage : IPage 
{
	private List<ProfileHandler.PlayerName> _ProfileNames;

	private void Start()
	{
		_ProfileNames = new List<ProfileHandler.PlayerName>();
		_ProfileNames = menuReference.profile.getList();

		Initialize();
		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetOptionCount(_ProfileNames.Count);
			menuButtons[0].SetButtonMethod(SelectProfileButton);
			menuButtons[1].SetButtonMethod(CreateButton);

		}
	}

	public override void Update ()
	{
		base.Update();

		SelectionControls();

		if (_ProfileNames.Count > 0)
		{
			menuButtons[2].SetButtonMethod(BackButton);
			menuLables[0].SetText( _ProfileNames[ menuButtons[0].GetCurrentOption() ].name );
		}
		else
		{
			menuButtons[2].SetText( "Quit" );
			menuButtons[2].SetButtonMethod(QuitButton);
			menuLables[0].SetText( "No profiles" );
		}
	}

	private void SelectionControls()
	{
		for (int i = 0; i < menuButtons.Count; i++) 
		{
			if (controller.Left())
			{
				if (menuButtons[i].isButtonSelected)
				{
					menuButtons[i].PreviousOption();
				}
			}
			
			if (controller.Right())
			{
				if (menuButtons[i].isButtonSelected)
				{
					menuButtons[i].NextOption();
				}
			}
		}
	}

	private void SelectProfileButton()
	{
		// Check if there is a profile selected and if not play a buzz sound
		
		if (menuButtons[0].GetOptionCount() > 0)
		{
			menuButtons[0].PlayButtonPressSound(menuButtons[0].pressSound);
			//Debug.Log(menuButtons[0].GetCurrentOption());
			menuReference.profile.loadProfile(menuButtons[0].GetCurrentOption() + 1);
			menuReference.ChangeCurrentPageTo(Menu_V2.Page.Main);
			animator.SlideIn();
		}
		else menuButtons[0].PlayButtonPressSound(menuButtons[0].errorSound);
	}

	private void CreateButton()
	{
		menuButtons[1].PlayButtonPressSound(menuButtons[1].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.ProfileCreation);
		animator.SlideOut();
	}

	private void BackButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Main);
	}

	private void QuitButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].backSound);
		Application.Quit();
	}
}
