/*_________________________________________
 * Button.cs
 * ________________________________________ */

using UnityEngine;
using System.Collections;
using OculusMenu;

public class Button : Label 
{
	private bool _isSelected = false;
	[SerializeField]
	private bool _isPressable = true;

	public bool isButtonSelected
	{
		get { return _isSelected; }
		set { 
				_isSelected = value;
				ChangeButtonState();
			}
	}

	private bool _grayOut = false;
	public bool grayOut
	{
		get { return _grayOut;}
		set { _grayOut = value;
			SetGrayOut();
			}
	}

	public bool isPressable
	{
		get { return _isPressable; }
		set { _isPressable = value; }
	}

	public GameObject feedbackCursor;
	private GameObject _cursor;

	[SerializeField]
	private Color _textColorSelected;
	[SerializeField]
	private Color _textColor;
	[SerializeField]
	private Color _unavailableTextColor;

	private MenuSounds _aSource;
	public AudioClip pressSound;
	public AudioClip errorSound;
	public AudioClip backSound;
	public AudioClip selectionSound;
	
	[SerializeField]
	private int _optionCount = 0;
	private int _currentOption = 0;

	public delegate void ButtonMethod();
	private ButtonMethod ptr_buttonMethod;

	protected override void Awake ()
	{
		base.Awake ();
		SetButtonMethod(null);
		_type = MenuObject.Button;
		_aSource = GameObject.Find("Menu").GetComponent<MenuSounds>();
	}

	public void SetButtonMethod(ButtonMethod method)
	{
		ptr_buttonMethod = method;
	}

	public void PlayButtonPressSound(AudioClip clip)
	{
		if (clip != null)
		{
			_aSource.PlaySoundEffect(clip);
		}
	}

	public void PlayButtonErrorSound()
	{
		if (errorSound != null)
		{
			_aSource.PlaySoundEffect(errorSound);
		}
	}

	public void PlayButtonSelectionSound()
	{
		if (selectionSound != null)
		{
			_aSource.PlaySoundEffect(selectionSound);
		}
	}

	// Give feedback which button is currently selected
	private void ChangeButtonState()
	{
		if (isButtonSelected)
		{
			if (!grayOut) SetTextColor(_textColorSelected);
			else SetTextColor(_unavailableTextColor);
			SpawnCursor();
		}
		else
		{
			if (!grayOut) SetTextColor(_textColor);
			else SetTextColor(_unavailableTextColor);
			RemoveCursor();
		}
	}

	public bool PressButton()
	{
		if (isPressable)
		{
			if (ptr_buttonMethod != null)
			{
				ptr_buttonMethod();
				return true;
			}
			else
			{

				return false;
			}
		}
		else 
		{
			PlayButtonErrorSound();
			return false;
		}
	}

	public GameObject GetBlinkingSymbol()
	{
		return _cursor;
	}

	public void SetOptionCount(int count)
	{
		_optionCount = count;
	}

	public int GetOptionCount()
	{
		return _optionCount;
	}

	public int GetCurrentOption()
	{
		return _currentOption;
	}

	public void NextOption()
	{
		PlayButtonSelectionSound();
		_currentOption++;
		if (_currentOption > GetOptionCount()-1) _currentOption = GetOptionCount()-1;
	}

	public void PreviousOption()
	{
		PlayButtonSelectionSound();
		_currentOption--;
		if (_currentOption < 0) _currentOption = 0;
	}

	private void SetGrayOut()
	{
		if (grayOut)
		{
			SetTextColor(_unavailableTextColor);
		}
		else
		{
			SetTextColor(_textColor);
		}
	}

	private void SpawnCursor()
	{
		if (feedbackCursor != null) 
		{
			Vector3 pos = this.transform.position;
			pos = new Vector3(pos.x - 0.2f, pos.y + 0.08f, pos.z);
			_cursor = Instantiate(feedbackCursor, pos, this.transform.rotation) as GameObject;
			_cursor.transform.parent = gameObject.transform;
			_cursor.GetComponent<Label>().SetCharacterSize(0.07f);
			if (!grayOut) _cursor.GetComponent<Label>().SetTextColor(_textColorSelected);
			else _cursor.GetComponent<Label>().SetTextColor(_unavailableTextColor);
		}
	}

	private void RemoveCursor()
	{
		if (_cursor != null) 
		{
			Destroy(_cursor);
		}
	}
}
