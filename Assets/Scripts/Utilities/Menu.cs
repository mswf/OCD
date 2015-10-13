using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Menu : MonoBehaviour
{

	public enum Page
	{
		MainMenu,
		MapSelection,
		Game,
		Difficulty,
		Leaderboard,
		ProfileSelection,
		ProfileCreation,
		OculusOptions,
		Calibration
	}
	// List of 3D Text objects added in the inspector
	private GameObject[] listText;
	public GameObject textPrefab;
	public GameObject pictureOVR;
	public GameObject _OVRPictureLocation;
	public GameObject startLocation;
	public GameObject[] _axisPrefabs;
	public GameObject _axisPictureLocation;
	
	// Current page is set to profile selection
	private Page currPage = Page.ProfileSelection;
	
	private int charLimit = 9;
	private int currOption = 0;
	
	private bool textSpawn = true;
	private bool allowSelect = false;
	private bool creating = false;
	private bool inEdit = false;
	private string nameToEdit = "";
	//private string ps3String = "Sony PLAYSTATION(R)3 Controller";
	//private string xbox360String = "Controller (Xbox 360 Wireless Receiver for Windows)";
	private bool xbox = false;
	private float delay = 0.1f;
	private bool pressEnabled = true;
	private bool ps3 = false;
	private int selectedId = 0;
	private int selectedTrack = 0;
	private int selectedAxis = 0;
	private Profile prof = new Profile();
	private Calibration caliber;
	private HighscoreManager manager = new HighscoreManager();
	private List<string> _tracks = new List<string>();
	public AudioClip[] aClip;
	private AudioSource aSource;
	private GameObject _axisPicture;
	private GameObject _OVRPicture;
	
	
	void Awake()
	{
		Time.timeScale = 1;
	}
	
	private void Start()
	{
	
		addSounds();
		addTrackList();
		
		caliber = gameObject.GetComponent<Calibration>();
		caliber.state = Calibration.State.none;
	}
	
	private void Update()
	{
	
		switch (currPage)
		{
		case Page.MainMenu 		:
			pMainMenu();
			break;
			
		case Page.Game 			:
			pGame();
			break;
			
		case Page.Difficulty 		:
			pDifficulty();
			break;
			
		case Page.MapSelection 		:
			pMapSelection();
			break;
			
		case Page.ProfileSelection 	:
			pProfileSelection();
			break;
			
		case Page.ProfileCreation 	:
			pProfileCreation();
			break;
			
		case Page.OculusOptions 	:
			pOculusOptions();
			break;
			
		case Page.Calibration		:
			pCalibration();
			break;
			
		case Page.Leaderboard 		:
			pLeaderboard();
			break;
		}
		
	}
	
	#region PAGES
	
	private void pMainMenu()
	{
		if (textSpawn)
		{
			textSpawn = false;
			showText(4);
		}
		
		fillText(0, "Start game");
		fillText(1, "Leaderboard");
		fillText(2, "Oculus options");
		fillText(3, "Back to profile selection");
		updateCurrText();
		
		changeCurrentOption(4);
		
		if (inputBack())
		{
			switchPage(Page.ProfileSelection);
		}
		
		switch (currOption)
		{
		case 0:
			if (confirmInput())
			{
				switchPage(Page.Game);
			}
			
			break;
			
		case 1:
			if (confirmInput())
			{
				switchPage(Page.Leaderboard);
			}
			
			break;
			
		case 2:
			if (confirmInput())
			{
				switchPage(Page.OculusOptions);
			}
			
			break;
			
		case 3:
			if (confirmInput())
			{
				switchPage(Page.ProfileSelection);
			}
			
			break;
			
		default:
			Debug.Log("Out of bound in MainMenu");
			break;
		}
	}
	
	private void pGame()
	{
	
		if (textSpawn)
		{
			textSpawn = false;
			showText(3);
		}
		
		fillText(0, "Normal game");
		fillText(1, "Free Look");
		fillText(2, "Back to main menu");
		updateCurrText();
		
		changeCurrentOption(3);
		
		if (inputBack())
		{
			switchPage(Page.MainMenu);
		}
		
		switch (currOption)
		{
		case 0:
		{
			if (confirmInput())
			{
				changeGameMode(0);
				changeScene("MainTrack");
				//switchPage(Page.Difficulty);
			}
		}
		break;
		
		case 1:
		{
			if (confirmInput())
			{
				changeGameMode(1);
				changeScene("MainTrack");
				//switchPage(Page.MapSelection);
			}
		}
		break;
		
		case 2:
			if (confirmInput())
			{
				switchPage(Page.MainMenu);
			}
			
			break;
			
		default:
			Debug.Log("Out of bound in Game");
			break;
		}
	}
	
	private void pDifficulty()
	{
	
		if (textSpawn)
		{
			textSpawn = false;
			showText(5);
		}
		
		fillText(0, "Easy");
		fillText(1, "Medium");
		fillText(2, "Hard");
		fillText(3, "Back to game selection");
		fillText(4, "Current game mode: " + getCurrentGameMode());
		updateCurrText();
		
		changeCurrentOption(4);
		
		if (inputBack())
		{
			switchPage(Page.Game);
		}
		
		switch (currOption)
		{
		case 0:
			if (confirmInput())
			{
				switchPage(Page.MapSelection);
				PlayerPrefs.SetFloat("Difficulty", 1);
			}
			
			break;
			
		case 1:
			if (confirmInput())
			{
				switchPage(Page.MapSelection);
				PlayerPrefs.SetFloat("Difficulty", 2);
			}
			
			break;
			
		case 2:
			if (confirmInput())
			{
				switchPage(Page.MapSelection);
				PlayerPrefs.SetFloat("Difficulty", 3);
			}
			
			break;
			
		case 3:
			if (confirmInput())
			{
				switchPage(Page.Game);
			}
			
			break;
			
		default:
			Debug.Log("Out of bound in Difficulty");
			break;
		}
	}
	
	private void pMapSelection()
	{
	
		if (textSpawn)
		{
			textSpawn = false;
			showText(3);
		}
		
		fillText(0, "Track1");
		fillText(1, "Back to game selection");
		fillText(2, "Current game mode: " + getCurrentGameMode());
		
		updateCurrText();
		
		changeCurrentOption(2);
		
		if (inputBack())
		{
			switchPage(Page.Game);
		}
		
		switch (currOption)
		{
		case 0:
			if (confirmInput())
			{
				changeScene("MainTrack");
			}
			
			break;
			
		case 1:
			if (confirmInput())
			{
				switchPage(Page.Game);
			}
			
			break;
			
		default:
			Debug.Log("Out of bound in MapSelection");
			break;
		}
		
	}
	
	private void pProfileSelection()
	{
	
		List<ProfileHandler.PlayerName> _list = prof.getList();
		string cText = "";
		
		allowSwitch();
		
		if (_list.Count < 1)
		{
			cText = "No profiles available";
		}
		
		else
		{
			cText = "Selected Profile: " + "<" + _list[selectedId].name.ToString() + ">";
		}
		
		if (textSpawn)
		{
			textSpawn = false;
			showText(3);
		}
		
		fillText(0, cText);
		fillText(1, "Create a profile");
		fillText(2, "Exit");
		updateCurrText();
		
		changeCurrentOption(3);
		changeCurrentSelection(_list.Count);
		
		if (inputBack())
		{
			Application.Quit();
		}
		
		switch (currOption)
		{
		case 0:
		{
			if ((confirmInput()) && !(_list.Count < 1))
			{
				prof.loadProfile(selectedId);
				switchPage(Page.MainMenu);
			}
		}
		break;
		
		case 1:
		{
			if (confirmInput())
			{
				switchPage(Page.ProfileCreation);
				nameToEdit = "";
			}
		}
		break;
		
		case 2:
			if (confirmInput())
			{
				Application.Quit();
			}
			
			break;
			
		default:
			Debug.Log("Out of bound in ProfileSelection");
			break;
		}
	}
	
	private void pProfileCreation()
	{
	
		if (textSpawn)
		{
			textSpawn = false;
			showText(5);
		}
		
		fillText(0, "Profile name: " + nameToEdit);
		fillText(1, "Default oculus input");
		fillText(2, "Custom oculus input");
		fillText(3, "Disabled ocolus input");
		fillText(4, "Cancel");
		updateCurrText();
		
		if (inputBack())
		{
			switchPage(Page.ProfileSelection);
		}
		
		if (!inEdit)
		{
			changeCurrentOption(5);
		}
		
		creating = true;
		
		switch (currOption)
		{
		case 0:
		{
			if (confirmInput())
			{
				inEdit = ! inEdit;
			}
			
			if (Input.anyKeyDown && inEdit)
			{
				if (Input.anyKeyDown != confirmInput() && !Input.GetKey(KeyCode.Backspace) && nameToEdit.Length <= charLimit)
				{
					nameToEdit += Input.inputString;
				}
				
				if (Input.GetKey(KeyCode.Backspace))
				{
					if (nameToEdit.Length > 0)
					{
						nameToEdit = nameToEdit.Substring(0, nameToEdit.Length - 1);
					}
				}
			}
		}
		break;
		
		case 1:
		{
			if (confirmInput() && nameToEdit != "")
			{
				defaultCalib(true);
				switchPage(Page.ProfileSelection);
			}
		}
		break;
		
		case 2:
			if (confirmInput() && nameToEdit != "")
			{
				if (isOVRConnected())
				{
					Destroy(_OVRPicture);
					_OVRPicture = null;
					switchPage(Page.Calibration);
				}
			}
			
			break;
			
		case 3:
		{
			if (confirmInput() && !DeviceInput.isDisabled && nameToEdit != "")
			{
				defaultCalib(true);
				DeviceInput.isDisabled = true;
				switchPage(Page.ProfileSelection);
			}
		}
		break;
		
		case 4:
		{
			if (confirmInput())
			{
				switchPage(Page.ProfileSelection);
				creating = false;
			}
		}
		break;
		
		default:
			Debug.Log("Out of bound in Profile Creation");
			break;
		}
		
	}
	
	private void pOculusOptions()
	{
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
			showText(5);
		}
		
		fillText(0, "Calibrate Oculus");
		fillText(1, "Set default oculus settings");
		fillText(2, "Disable oculus");
		fillText(3, "Oculus is disabled: " + status);
		fillText(4, "Back to main menu");
		updateCurrText();
		
		changeCurrentOption(5);
		
		if (inputBack())
		{
			switchPage(Page.MainMenu);
		}
		
		creating = false;
		
		switch (currOption)
		{
		
		case 0:
			if (confirmInput())
			{
				if (isOVRConnected())
				{
					Destroy(_OVRPicture);
					_OVRPicture = null;
					caliber.state = Calibration.State.none;
					switchPage(Page.Calibration);
				}
			}
			
			break;
			
		case 1:
			if (confirmInput())
			{
				defaultCalib(false);
			}
			
			break;
			
		case 2:
		{
			if (confirmInput())
			{
				if (DeviceInput.isDisabled)
				{
					DeviceInput.isDisabled = false;
				}
				
				else
				{
					DeviceInput.isDisabled = true;
				}
			}
		}
		break;
		
		case 3:
			break;
			
		case 4:
			if (confirmInput())
			{
				switchPage(Page.MainMenu);
			}
			
			break;
			
		default:
			Debug.Log("Out of bound in OculusOptions");
			break;
		}
	}
	
	private void pCalibration()
	{
		string tempText = "";
		
		if	(caliber.state == Calibration.State.none)
		{
			tempText = "Start calibrating or cancel with escape button";
		}
		
		else
			if (caliber.state == Calibration.State.neutral)
			{
				tempText = "Resets to neutral position";
			}
			
			else
				if (caliber.state == Calibration.State.axis)
				{
					tempText = "Choose an axis for the controls";
				}
				
				else
					if (caliber.state == Calibration.State.left)
					{
						tempText = "Set the most left value";
					}
					
					else
						if (caliber.state == Calibration.State.right)
						{
							tempText = "Set the most right value";
						}
						
						else
							if (caliber.state == Calibration.State.done)
							{
								tempText = "You have finished calibrating";
							}
							
		//Save calibration into profile and go back to Profile selection
		if (textSpawn)
		{
			textSpawn = false;
			showText(4);
		}
		
		fillText(0, "" + caliber.state.ToString());
		fillText(1, tempText);
		fillText(2, "Confirm to continue(Enter or X)");
		fillText(3, "Press Escape or Circle to cancel");
		//updateCurrText();
		
		if (inputBack())
		{
			switchPage(Page.ProfileSelection);
		}
		
		if (confirmInput())
		{
			if (caliber.state == Calibration.State.done)
			{
				if (creating)
				{
					createProfile(nameToEdit, caliber.axis , caliber.leftOrientation, caliber.rightOrientation);
				}
				
				else
				{
					updateProfile(caliber.axis , caliber.leftOrientation, caliber.rightOrientation);
				}
				
				switchPage(Page.ProfileSelection);
			}
			
			else
			{
				++caliber.state;
			}
		}
		
		if (caliber.state == Calibration.State.axis)
		{
			changeCurrentAxis(3);
			updateSelectedAxis();
		}
		
	}
	
	private void pLeaderboard()
	{
	
		allowSwitch();
		
		List<Highscore> _list = manager.getHighscoreList(_tracks[selectedTrack]);
		
		string trackName = _tracks[selectedTrack];
		
		
		if (textSpawn)
		{
			textSpawn = false;
			showText(7);
		}
		
		fillText(0, "Track score < " + trackName + " >");
		fillText(1, "Back to main menu");
		
		for (int i = 0; i < 5; i++)
		{
			string trackStat = "";
			
			if (i < _list.Count)
			{
				trackStat = _list[i].name + ": ";
				int time = _list[i].totalTime;
				int minute = time / 60;
				int seconds = time - (minute * 60);
				trackStat += minute.ToString() + ":" + seconds.ToString();
			}
			
			fillText(i + 2, trackStat);
		}
		
		updateCurrText();
		
		changeCurrentOption(2);
		
		changeCurrentTrack(_tracks.Count);
		
		if (inputBack())
		{
			switchPage(Page.MainMenu);
		}
		
		switch (currOption)
		{
		case 0:
			break;
			
		case 1:
			if (confirmInput())
			{
				switchPage(Page.MainMenu);
			}
			
			break;
			
		default:
			Debug.Log("Out of bound in Leaderboard");
			break;
		}
	}
	
	#endregion
	
	void createProfile(string name, Axis axis, Quaternion left, Quaternion right)
	{
		prof.createProfile(name, axis, left, right);
	}
	
	void updateProfile(Axis axis , Quaternion _left, Quaternion _right)
	{
		prof.updateCurrentProfile(axis , _left, _right); // Sets player prefs settings with new calibration settings
		prof.updateProfile(); // Gets player prefs settings and saves it to profile file
		prof.loadProfile(selectedId); // Gets the profile file and sets it to the player prefs
	}
	
	void updateSelectedAxis()
	{
		if (selectedAxis == 0)
		{
			caliber.axis = Axis.x;
			PlayerPrefs.SetString("Axis", "x");
			showAxis(0);
		}
		
		if (selectedAxis == 1)
		{
			caliber.axis = Axis.y;
			PlayerPrefs.SetString("Axis", "y");
			showAxis(1);
		}
		
		if (selectedAxis == 2)
		{
			caliber.axis = Axis.z;
			PlayerPrefs.SetString("Axis", "z");
			showAxis(2);
		}
	}
	
	private void showAxis(int index)
	{
		//Quaternion rot = GameObject.Find("Main Camera").transform.rotation;
		
		switch (index)
		{
		case 0:
		{
			if (_axisPicture == null)
			{
				_axisPicture = Instantiate(_axisPrefabs[index], _axisPictureLocation.transform.position, Quaternion.Euler(90.0f, 235.0f, 0.0f)) as GameObject;
			}
		}
		break;
		
		case 1:
		{
			if (_axisPicture == null)
			{
				_axisPicture = Instantiate(_axisPrefabs[index], _axisPictureLocation.transform.position, Quaternion.Euler(90.0f, 235.0f, 0.0f)) as GameObject;
			}
		}
		break;
		
		case 2:
		{
			if (_axisPicture == null)
			{
				_axisPicture = Instantiate(_axisPrefabs[index], _axisPictureLocation.transform.position, Quaternion.Euler(-90.0f, 235.0f, 180.0f)) as GameObject;
			}
		}
		break;
		}
	}
	private void clearAxis()
	{
		Destroy(_axisPicture);
		_axisPicture = null;
	}
	
	void defaultCalib(bool create)
	{
		Axis _axis = Axis.z;
		Quaternion _left = Quaternion.Euler(-0.5f, -0.5f, -0.5f);
		Quaternion _right = Quaternion.Euler(0.5f, 0.5f, 0.5f);
		
		if (!create)
		{
			updateProfile(_axis, _left, _right);
		}
		
		else
		{
			createProfile(nameToEdit, _axis, _left, _right);
		}
	}
	
	#region Text
	void showText(int amount)
	{
		listText = new GameObject[amount];
		Quaternion rot = GameObject.Find("Main Camera").transform.rotation;
		
		for (int i = 0; i < amount; i++)
		{
			listText[i] = Instantiate(textPrefab, startLocation.transform.position + new Vector3(0, -i * 0.2f, 0), rot) as GameObject;
		}
	}
	void fillText(int index, string text)
	{
	
		if (listText[index] != null)
		{
			listText[index].GetComponent<TextMesh>().text = text;
		}
	}
	void updateCurrText()
	{
		string cur = " <";
		
		if (listText[currOption] != null)
		{
			listText[currOption].GetComponent<TextMesh>().text += cur;
		}
	}
	void clearText()
	{
		for (int i = 0; i < listText.Length; i++)
		{
			Destroy(listText[i]);
		}
	}
	#endregion
	
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
	
	private bool confirmInput()
	{
	
		checkController();
		
		if (Input.GetKeyDown(KeyCode.Return) || (Input.GetKeyDown(KeyCode.JoystickButton14) && ps3) || (Input.GetKeyDown(KeyCode.JoystickButton0) && xbox))
		{
			if (currPage == Page.ProfileSelection && currOption == 2)
			{
				switchSound(aClip[0]);
			}
			
			else
				if (currPage == Page.MapSelection && currOption == 0)
				{
					switchSound(aClip[0]);
				}
				
				else
				{
					switchSound(aClip[1]);
				}
				
			playAudio();
			return true;
		}
		
		else
		{
			return false;
		}
	}
	
	private bool inputLeft()
	{
	
		checkController();
		
		float inputX = Input.GetAxis("XboxHorizontal");
		
		if (inputX < 0 && xbox)
		{
			if (pressEnabled)
			{
				pressEnabled = false;
				Invoke("enableNext", delay);
				return true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.LeftArrow) || Input.GetKeyDown(KeyCode.A) || (Input.GetKeyDown(KeyCode.JoystickButton7) && ps3))
		{
			return true;
		}
		
		return false;
	}
	
	private bool inputRight()
	{
		checkController();
		
		float inputX = Input.GetAxis("XboxHorizontal");
		
		if (inputX > 0 && xbox)
		{
			if (pressEnabled)
			{
				pressEnabled = false;
				Invoke("enableNext", delay);
				return true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.JoystickButton5))
		{
			return true;
		}
		
		return false;
	}
	
	private bool inputUp()
	{
		checkController();
		
		float inputY = Input.GetAxis("XboxVertical");
		
		if (inputY > 0 && xbox)
		{
			if (pressEnabled)
			{
				pressEnabled = false;
				Invoke("enableNext", delay);
				return true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.JoystickButton4))
		{
			return true;
		}
		
		return false;
	}
	
	private bool inputDown()
	{
		checkController();
		
		float inputY = Input.GetAxis("XboxVertical");
		
		if (inputY < 0 && xbox)
		{
			if (pressEnabled)
			{
				pressEnabled = false;
				Invoke("enableNext", delay);
				return true;
			}
		}
		
		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.JoystickButton6))
		{
			return true;
		}
		
		return false;
	}
	
	private bool inputBack()
	{
		checkController();
		
		if ((Input.GetKeyDown(KeyCode.JoystickButton13) && ps3) || (Input.GetKeyDown(KeyCode.JoystickButton1) && xbox) || Input.GetKeyDown(KeyCode.Escape))
		{
			return true;
		}
		
		else
		{
			return false;
		}
	}
	
	private bool isOVRConnected()
	{
		if (WeikiesRiftHack.IsConnected())
		{
			return true;
		}
		
		if (_OVRPicture == null)
		{
			_OVRPicture = Instantiate(pictureOVR, _OVRPictureLocation.transform.position, Quaternion.Euler(270.0f, 45.0f, 0.0f)) as GameObject;
		}
		
		return false;
		
	}
	
	private void enableNext()
	{
		pressEnabled = true;
	}
	
	private void changeCurrentOption(int count)
	{
		if (inputDown() && (currOption + 1 < count))
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			currOption++;
		}
		
		if (inputUp() && (currOption - 1 > -1))
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			currOption--;
		}
	}
	
	private void changeCurrentAxis(int count)
	{
		if (inputRight() && (selectedAxis + 1 < count))
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			clearAxis();
			selectedAxis++;
		}
		
		if (inputLeft() && (selectedAxis - 1 > -1))
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			clearAxis();
			selectedAxis--;
		}
	}
	
	private void changeCurrentSelection(int count)
	{
		if (inputRight() && (selectedId + 1 < count) && allowSelect)
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			selectedId++;
		}
		
		if (inputLeft() && (selectedId - 1 > -1) && allowSelect)
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			selectedId--;
		}
	}
	
	private void changeCurrentTrack(int count)
	{
	
		if (inputRight() && (selectedTrack + 1 < count) && allowSelect)
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			selectedTrack++;
		}
		
		if (inputLeft() && (selectedTrack - 1 > -1) && allowSelect)
		{
			if (UnityEngine.Random.value > 0.5)
			{
				switchSound(aClip[2]);
			}
			
			else
			{
				switchSound(aClip[3]);
			}
			
			playAudio();
			selectedTrack--;
		}
	}
	
	private void switchPage(Page page)
	{
		currPage = page;
		clearText();
		textSpawn = true;
		currOption = 0;
	}
	
	private void allowSwitch()
	{
		if (currOption == 0)
		{
			allowSelect = true;
		}
		
		else
		{
			allowSelect = false;
		}
	}
	
	private void changeScene(string name)
	{
		Application.LoadLevel(name);
	}
	
	private void addSounds()
	{
		aSource = gameObject.AddComponent<AudioSource>();
		aSource.clip = aClip[0];
	}
	
	private void addTrackList()
	{
		_tracks.Add("level1");
		_tracks.Add("level2");
		_tracks.Add("level3");
		_tracks.Add("level4");
	}
	
	private void changeGameMode(int mode)
	{
		PlayerPrefs.SetInt("GameMode", mode);
		// if it is zero = Normal
		// if it is one = Tutorial
		// if it is two = FreeLook
	}
	
	private string getCurrentGameMode()
	{
		int temp = PlayerPrefs.GetInt("GameMode");
		
		if (temp == 0)
		{
			return "Normal";
		}
		
		if (temp == 1)
		{
			return "Freelook";
		}
		
		if (temp == 2)
		{
			return "Tutorial";
		}
		
		return "";
	}
	
	void switchSound(AudioClip clip)
	{
		aSource.clip = clip;
	}
	
	void playAudio()
	{
		aSource.Play();
	}
	
}
