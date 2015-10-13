using UnityEngine;
using System.Collections.Generic;
using System;
using OculusMenu;

public enum MenuObject
{
	Label,
	Button
}

public class IPage : MonoBehaviour
{
	private int max_selectableCount;
	private int _currentSelection;

	protected Menu_V2 menuReference;
	protected MenuControls controller;
	public List<GameObject> menuItems;
	protected MenuObject[] menuObjectList;
	protected List<Label> menuLables;
	protected List<Button> menuButtons;
	public PageAnimation animator;


	public void SetReference (Menu_V2 ref1, MenuControls ref2)
	{
		menuReference = ref1;
		controller = ref2;
	}

	public void Initialize()
	{
		animator = GetComponent<PageAnimation>();
		menuLables = new System.Collections.Generic.List<Label>();
		menuButtons = new System.Collections.Generic.List<Button>();


		for (int i = 0; i < menuItems.Count; i++) 
		{
			if (menuItems[i].GetComponent<Label>().ObjectType == MenuObject.Label) 
			{
				menuLables.Add( menuItems[i].GetComponent<Label>() );
			}

			if (menuItems[i].GetComponent<Label>().ObjectType == MenuObject.Button) 
			{
				menuButtons.Add( menuItems[i].GetComponent<Button>() );
				max_selectableCount++;
			}
		}

		// Set first button to selected;
		if (menuButtons.Count > 0) 
		{
			menuButtons[0].isButtonSelected = true;
		}
	}
	
	public virtual void Update()
	{
		Controls();
	}

	public void NextSelection()
	{
		_currentSelection--;
		if (_currentSelection < 0) _currentSelection = max_selectableCount-1;

		for (int i = 0; i < max_selectableCount; i++) 
		{
			if (_currentSelection == i) menuButtons[i].isButtonSelected = true;
			else menuButtons[i].isButtonSelected = false;
		}
		menuReference.audioSource.PlaySoundEffect(menuReference.audioSource.SelectionSound);
	}

	public void PreviousSelection()
	{
		_currentSelection++;
		if (_currentSelection >= max_selectableCount) _currentSelection = 0;

		for (int i = 0; i < max_selectableCount; i++) 
		{
			if (_currentSelection == i) menuButtons[i].isButtonSelected = true;
			else menuButtons[i].isButtonSelected = false;
		}
		menuReference.audioSource.PlaySoundEffect(menuReference.audioSource.SelectionSound);
	}

	public void Select()
	{
		menuButtons[_currentSelection].PressButton();
	}

	private void Controls()
	{
		if( controller.Up() )
		{
			NextSelection();
		}
		
		if( controller.Down() )
		{
			PreviousSelection();
		}
		
		if( controller.Enter() )
		{
			Select();
		}
	}
}
