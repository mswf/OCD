using UnityEngine;
using System.Collections;

public class MenuControls : MonoBehaviour 
{

	//System.Collections.Generic.Dictionary<KeyCode, float> KeyDictionary;
	private const float COOLDOWN = 0.6f;
	private const float KEY_COOLDOWN = 0.2f;
	private float _currentHorizontalCooldown = 0.0f;
	private float _currentVerticalCooldown = 0.0f;



	public bool Left()
	{
		if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01f) _currentHorizontalCooldown = 0.0f;
		
		if (Input.GetAxis("Horizontal") < -0.001f)
		{
			if (_currentHorizontalCooldown < 0.1f)
			{
				_currentHorizontalCooldown = COOLDOWN;
				return true;
			}
			else
			{
				_currentHorizontalCooldown -= Time.deltaTime;
				return false;
			}
		}
		else return false;
	}

	public bool Right()
	{
		if (Mathf.Abs(Input.GetAxis("Horizontal")) < 0.01f) _currentHorizontalCooldown = 0.0f;
		
		if (Input.GetAxis("Horizontal") > 0.001f)
		{
			if (_currentHorizontalCooldown < 0.1f)
			{
				_currentHorizontalCooldown = COOLDOWN;
				return true;
			}
			else
			{
				_currentHorizontalCooldown -= Time.deltaTime;
				return false;
			}
		}
		else return false;
	}
	
	public bool Up()
	{
		if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.01f) _currentVerticalCooldown = 0.0f;
		
		if (Input.GetAxis("Vertical") > 0.01f)
		{
			if (_currentVerticalCooldown < 0.1f)
			{
				_currentVerticalCooldown = COOLDOWN;
				return true;
			}
			else
			{
				_currentVerticalCooldown -= Time.deltaTime;
				return false;
			}
		}
		else return false;
	}

	public bool Down()
	{
		if (Mathf.Abs(Input.GetAxis("Vertical")) < 0.01f) _currentVerticalCooldown = 0.0f;

		if (Input.GetAxis("Vertical") < -0.01f)
		{
			if (_currentVerticalCooldown < 0.1f)
			{
				_currentVerticalCooldown = COOLDOWN;
				return true;
			}
			else
			{
				_currentVerticalCooldown -= Time.deltaTime;
				return false;
			}
		}
		else return false;
	}

	public bool Enter()
	{
		if (Input.GetKeyDown(KeyCode.Return)) return true;
		else return false;
	}

	public bool BackspaceDown()
	{
		if (Input.GetKeyDown(KeyCode.Backspace)) return true;
		else return false;
	}

	public bool Backspace()
	{
		if (Input.GetKey(KeyCode.Backspace)) return true;
		else return false;
	}

}
