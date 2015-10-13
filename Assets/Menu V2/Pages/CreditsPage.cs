using UnityEngine;
using System.Collections;

public class CreditsPage : IPage 
{
	private Credits3D credits;
	private void Start()
	{
		Initialize();
		
		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetButtonMethod(BackButton);
		}

		GameObject go = GameObject.Find("CreditsText");
		credits = go.AddComponent<Credits3D>();
		
	}

	public override void Update ()
	{
		base.Update();

		if (credits.finished)
		{
			BackButton();
		}
	}

	private void BackButton()
	{
		menuButtons[0].PlayButtonPressSound(menuButtons[0].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Main);
		animator.SlideOut();
	}
	
}
