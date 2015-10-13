using UnityEngine;
using System.Collections.Generic;
using System;
using OculusMenu;

public class IPageGenerating: MonoBehaviour
{
	private int max_selectableCount;
	private int _currentSelection;

	protected Menu_V2 menuReference;
	protected List<GameObject> menuItems;
	protected List<Label> menuLables;
	protected List<Button> menuButtons;

	public MenuObject[] menuObjectList;
	public Vector3[] Positions;
	public string[] Texts;

	public const string BUTTON = "Menu/MenuObjects/Button";
	public const string LABEL = "Menu/MenuObjects/Label";

	public void SetReference (Menu_V2 reference)
	{
		menuReference = reference;
	}

	public void Initialize()
	{
		menuItems = new System.Collections.Generic.List<GameObject>();
		menuLables = new System.Collections.Generic.List<Label>();
		menuButtons = new System.Collections.Generic.List<Button>();
		
		for (int i = 0; i < menuObjectList.Length; i++) 
		{
			// Create object
			switch (menuObjectList[i]) 
			{
				case MenuObject.Label:
				{
					CreateItem(LABEL, i);
					break;
				}
				case MenuObject.Button:
				{
					max_selectableCount++;
					CreateItem(BUTTON, i);
					break;
				}
				default:
				{
					throw new System.NotSupportedException("Are you creating an object without a expression?");
				}
			}
		}
		// Set first button to selected;
		menuButtons[0].isButtonSelected = true;
	}

	protected void CreateItem (string item, int i)
	{
		menuItems.Add( Instantiate( Resources.Load(item), new Vector3(0.0f, 1.0f * -i, 0.0f), new Quaternion()) as GameObject );

		if (menuObjectList[i] == MenuObject.Label) 
		{
			menuLables.Add( menuItems[i].GetComponent<Label>() );
			menuLables[menuLables.Count-1].SetText( Texts[i] );
		}
		if (menuObjectList[i] == MenuObject.Button) 
		{
			menuButtons.Add( menuItems[i].GetComponent<Button>() );
			menuButtons[menuButtons.Count-1].SetText( Texts[i] );
		}

		// Set object to this gameObject as the parent
		menuItems[i].transform.parent = gameObject.transform;
		
		// Set object position
		if (Positions[i] != Vector3.zero)
		{
			menuItems[i].transform.position = Positions[i];
		}
		
	}

	/*
	public virtual void PageUpdate()
	{

	}
	*/

	public void Next()
	{
		_currentSelection--;
		if (_currentSelection < 0) _currentSelection = max_selectableCount-1;

		for (int i = 0; i < max_selectableCount; i++) 
		{
			if (_currentSelection == i) menuButtons[i].isButtonSelected = true;
			else menuButtons[i].isButtonSelected = false;
		}
	}

	public void Previous()
	{
		_currentSelection++;
		if (_currentSelection >= max_selectableCount) _currentSelection = 0;

		for (int i = 0; i < max_selectableCount; i++) 
		{
			if (_currentSelection == i) menuButtons[i].isButtonSelected = true;
			else menuButtons[i].isButtonSelected = false;
		}
	}

	public void Select()
	{
		menuButtons[_currentSelection].PressButton();
	}
}
