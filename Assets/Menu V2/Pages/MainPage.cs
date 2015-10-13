using UnityEngine;
using System.Collections;

public class MainPage : IPage 
{
	private LoadingGUI gui;

	private void Start()
	{
		GetCamera();
		Initialize();
		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetButtonMethod(StartButton);
			menuButtons[1].SetButtonMethod(SettingsButton);
			menuButtons[2].SetButtonMethod(ProfilesButton);
			menuButtons[3].SetButtonMethod(LeaderboardButton);
			menuButtons[4].SetButtonMethod(CreditsButton);
			menuButtons[5].SetButtonMethod(QuitButton);
		}
		menuLables[1].SetText("Current Profile: " + menuReference.profile.getProfileName() );
	}

	public override void Update ()
	{
		base.Update();
	}

	private void StartButton()
	{
		menuButtons[0].PlayButtonPressSound(menuButtons[0].pressSound);
		StartCoroutine(LoadLevel("MainTrack"));
	}

	private void SettingsButton()
	{
		menuButtons[1].PlayButtonPressSound(menuButtons[1].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Settings);
		animator.SlideOut();
	}

	private void ProfilesButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Profiles);
		animator.SlideOut();
	}

	private void LeaderboardButton()
	{
		menuButtons[3].PlayButtonPressSound(menuButtons[3].pressSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Leaderboard);
		animator.SlideOut();
	}

	private void CreditsButton()
	{
		menuButtons[5].PlayButtonPressSound(menuButtons[5].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Credits);
		animator.SlideOut();
	}

	private void QuitButton()
	{
		menuButtons[4].PlayButtonPressSound(menuButtons[4].backSound);
		Application.Quit();
	}

	private void GetCamera()
	{
		gui = GameObject.Find("OVRCameraController").GetComponent<LoadingGUI>();
	}

	IEnumerator LoadLevel(string levelName)
	{
		gui.isLoading = true;

		AsyncOperation async = Application.LoadLevelAsync(levelName);

		while(!async.isDone)
		{
			//Debug.Log(async.progress * 100);
			gui.SetLoadingText( async.progress * 100 );
			yield return null;
		}

	}
}
