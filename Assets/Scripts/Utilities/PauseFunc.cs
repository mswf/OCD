using UnityEngine;
using System.Collections;
using System.Timers;

public class PauseFunc : MonoBehaviour
{

	private GameObject[] listText;
	public GameObject textPrefab;
	public GameObject pictureOVR;
	public GameObject _OVRPictureLocation;
	public GameObject startLocation;
	
	private GameObject _OVRPicture;
	//public Texture image;
	private int currOption = 0;
	
	private bool isPaused = false;
	private bool textSpawn = true;
	
	private bool xbox = false;
	private int time = 100;
	private Timer _timer;
	private bool pressEnabled = true;
	private bool ps3 = false;
	
	void Awake()
	{
		_timer = new Timer(time);
		_timer.Elapsed += new ElapsedEventHandler(enableNext);
	}
	
	void Update()
	{
	
		if (!WeikiesRiftHack.IsConnected())
		{
			if (!DeviceInput.isDisabled)
			{
				isPaused = true;
				spawnPicture();
			}
		}
		
		checkController();
		
		
		if (Input.GetKeyDown(KeyCode.Escape) || (Input.GetKeyDown(KeyCode.JoystickButton0) && ps3) || (Input.GetKeyDown(KeyCode.JoystickButton7) && xbox))
		{
			isPaused = ! isPaused;
		}
		
		if (isPaused)
		{
			Time.timeScale = 0;
			
			string status = "";
			
			if (DeviceInput.isDisabled)
			{
				status = "Yes";
			}
			
			else
			{
				status = "No";
			}
			
			if (textSpawn)
			{
				textSpawn = false;
				showText(4);
			}
			
			fillText(0, "Resume");
			fillText(1, "Reset Orientation");
			fillText(2, "Oculus is Disabled: " + status);
			fillText(3, "Back to main menu");
			updateCurrText();
			
			changeCurrentOption(4);
			
			switch (currOption)
			{
			case 0:
				if (confirmInput())
				{
					if (DeviceInput.isDisabled || WeikiesRiftHack.IsConnected())
					{
						clearText();
						textSpawn = true;
						isPaused = false;
						destroyPicture();
					}
					
					else
					{
						spawnPicture();
					}
				}
				
				break;
				
			case 1:
				if (confirmInput())
				{
					resetOrientation();
				}
				
				break;
				
			case 2:
				if (confirmInput())
				{
					DeviceInput.isDisabled = ! DeviceInput.isDisabled;
				}
				
				break;
				
			case 3:
				if (confirmInput())
				{
					loadMenu();
				}
				
				break;
			}
		}
		
		else
		{
			Time.timeScale = 1;
			clearText();
			textSpawn = true;
			destroyPicture();
		}
	}
	
	private void showText(int amount)
	{
		listText = new GameObject[amount];
		Quaternion rot = GameObject.Find("CameraRight").transform.rotation;
		
		for (int i = 0; i < amount; i++)
		{
			listText[i] = Instantiate(textPrefab, startLocation.transform.position + new Vector3(0, -i * 0.2f, 0), rot) as GameObject;
		}
	}
	
	private void fillText(int index, string text)
	{
		if (listText[index] != null)
		{
			listText[index].GetComponent<TextMesh>().text = text;
		}
	}
	
	private void updateCurrText()
	{
		if (listText[currOption] != null)
		{
			listText[currOption].GetComponent<TextMesh>().text += " <";
		}
	}
	
	private void clearText()
	{
		if (listText != null)
		{
			for (int i = 0; i < listText.Length; i++)
			{
				if (listText[i] != null)
				{
					Destroy(listText[i]);
				}
			}
		}
	}
	
	private void spawnPicture()
	{
		if (_OVRPicture == null)
		{
			_OVRPicture = Instantiate(pictureOVR, _OVRPictureLocation.transform.position, Quaternion.Euler(-90.0f, 90.0f, 0.0f)) as GameObject;
		}
	}
	
	private void destroyPicture()
	{
		if (_OVRPicture != null)
		{
			Destroy(_OVRPicture);
			_OVRPicture = null;
		}
	}
	
	private bool confirmInput()
	{
		if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown(KeyCode.JoystickButton14) && ps3) || (Input.GetKeyDown(KeyCode.JoystickButton0) && xbox))
		{
			return true;
		}
		
		else
		{
			return false;
		}
	}
	
	private bool inputUp()
	{
		float inputY = Input.GetAxis("XboxVertical");
		
		if (inputY > 0 && xbox)
		{
			if (pressEnabled)
			{
				pressEnabled = false;
				enableTimer();
				return true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || (Input.GetKeyDown(KeyCode.JoystickButton4) && ps3))
		{
			return true;
		}
		
		return false;
	}
	
	private bool inputDown()
	{
		float inputY = Input.GetAxis("XboxVertical");
		
		if (inputY < 0 && xbox)
		{
			if (pressEnabled)
			{
				pressEnabled = false;
				enableTimer();
				return true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || (Input.GetKeyDown(KeyCode.JoystickButton6) && ps3))
		{
			return true;
		}
		
		return false;
	}
	
	private void enableTimer()
	{
		_timer.Start();
	}
	
	private void enableNext(object source, ElapsedEventArgs e)
	{
		_timer.Stop();
		pressEnabled = true;
	}
	
	private void checkController()
	{
		string controller = "";
		
		if (Input.GetJoystickNames().Length != 0)
		{
			controller = Input.GetJoystickNames()[0].ToString();
		}
		
		if (controller.Contains("Sony"))
		{
			xbox = false;
			ps3 = true;
		}
		
		if (controller.Contains("Xbox"))
		{
			xbox = true;
			ps3 = false;
		}
	}
	
	private void changeCurrentOption(int count)
	{
		if (inputDown() && (currOption + 1 < count))
		{
			currOption++;
		}
		
		if (inputUp() && (currOption - 1 > -1))
		{
			currOption--;
		}
	}
	
	private void resetOrientation()
	{
		if (WeikiesRiftHack.IsConnected())
		{
			WeikiesRiftHack.ResetOrientation();
		}
	}
	
	private void loadMenu()
	{
		Application.LoadLevel("Menu");
	}
}
