using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;
using System.Collections.Generic;
using System;

public class ProfileCreationPage : IPage 
{
	private bool isEditing = false;
	private string _name = "Enter Name";
	private const int nameCharacterLimit = 15;
	private int timer = 0;

	private void Start()
	{
		Initialize();
		if (menuButtons.Count > 0)
		{
			menuButtons[0].SetButtonMethod(EnterNameButton);
			menuButtons[1].SetButtonMethod(CreateButton);
			menuButtons[2].SetButtonMethod(CancelButton);
		}
	}

	public override void Update ()
	{
		if (isEditing)
		{
			EditingControls();
			EditingBlink();
			if (_name.Length == nameCharacterLimit) menuLables[1].SetText("Name has a maximum \n of " + nameCharacterLimit + " letters.");
			else if (_name.Length < 1) menuLables[1].SetText("Name must contain \n atleast one character.");
			else menuLables[1].SetText("");
		}
		else
		{
			base.Update();
			menuButtons[0].SetText("> " + _name + " <");
			if (_name.Length < 1) menuLables[1].SetText("Name must contain \n atleast one character.");
			else menuLables[1].SetText("");
		}
	}

	private void EnterNameButton()
	{
		isEditing =! isEditing;
		if (isEditing) menuButtons[0].GetBlinkingSymbol().GetComponent<BlinkingSymbol>().StopBlinking();
		else menuButtons[0].GetBlinkingSymbol().GetComponent<BlinkingSymbol>().StopBlinking();
	}

	private void CreateButton()
	{
		if (_name.Length > 0)
		{
			menuButtons[1].PlayButtonPressSound(menuButtons[1].pressSound);
			menuReference.profile.createProfile(_name, Axis.z, new Quaternion(), new Quaternion());
			menuReference.ChangeCurrentPageTo(Menu_V2.Page.Profiles);
			animator.SlideOut();
		}
	}

	private void CancelButton()
	{
		menuButtons[2].PlayButtonPressSound(menuButtons[2].backSound);
		menuReference.ChangeCurrentPageTo(Menu_V2.Page.Profiles);
		animator.SlideIn();
	}

	private void EditingControls()
	{

		if (Input.anyKeyDown != controller.Enter() && !controller.BackspaceDown())
		{
			if (_name.Length <= nameCharacterLimit-1)
			{
				if (Regex.IsMatch(Input.inputString, @"([A-Za-z0-9 -]+)"))
				{
					_name += Input.inputString;
					menuButtons[0].SetText(_name + "_");
				}
			}
		}

		if(controller.BackspaceDown()) 
		{
			if (_name.Length > 0) _name = _name.Substring(0, _name.Length - 1);
			menuButtons[0].SetText(_name + "_");
		}

		if(controller.Enter())
		{
			Select();
		}
	}

	private void EditingBlink()
	{
		if (timer >= 60)
		{
			if (!menuButtons[0].GetText().EndsWith("_"))
			{
				menuButtons[0].SetText(_name + "_");
			}
			else if (menuButtons[0].GetText().EndsWith("_"))
			{
				menuButtons[0].SetText(menuButtons[0].GetText().Remove(menuButtons[0].GetText().Length-1, 1));
			}
			timer = 0;
		}
		else timer++;
	}
	
}
