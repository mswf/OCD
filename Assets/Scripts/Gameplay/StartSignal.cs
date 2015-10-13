using UnityEngine;
using System.Collections.Generic;

public class StartSignal : MonoBehaviour {

	private struct GuiElement
	{
		public showGuiObject show;
		public hideGuiObject hide;
		public bool isShowing;
	}
	private List<GuiElement> elementList = new List<GuiElement>();
	private int currentElement = 0;


	public GameObject[] vehicles;
	public GameObject[] lights;
	public AudioClip[] clips;

	public Material enabledMat;
	public TextMesh countdownText;
	private bool fadedIn = false;
	private bool countdownStarted = false;
	public Texture fadeTexture;
	public float fadeSpeed = 0.1f;
	private float fadeAlpha = 1f;

	private float timer = 0;
	private float timerDuration = 1;
	private float flickerTimer = 0;
	private float flickerRate = 5;

	public DisplayGUI displayGUI;
	public GameObject minimap;

	// Use this for initialization
	void Start () {
		disableVehicles();
		initFlicker();
		hideGuiObjects();
		
		//gameObject.AddComponent<AudioSource>();
		GetComponent<AudioSource>().volume = 1.5f;
		
	}

	private void initFlicker()
	{
		addGuiElement(showMinimap, hideMinimap);
		addGuiElement(displayGUI.enablePosition, displayGUI.disablePosition);
	}

	private void showMinimap()
	{
		minimap.GetComponent<Renderer>().enabled = true;
	}

	private void hideMinimap()
	{
		minimap.GetComponent<Renderer>().enabled = false;
	}

	private void Update()
	{


		if (!countdownStarted)
		{
			//fadeAlpha -= Time.deltaTime;
			if (fadedIn)
			{
				if (showGuiObjects())
				{
					countdownStarted = true;
					startCountdown();
				}
			}
		}
	}

	private bool flicker()
	{
		if (currentElement >= elementList.Count) return true;
		
		//dd = dd + (1 * Time.deltaTime);
		timer += 0.02f;
		if (timer > timerDuration)
		{
			timer = timerDuration;
		}

		flickerTimer += Mathf.Exp(timer);
		if (flickerTimer >= flickerRate)
		{
			flickerTimer = 0;
			GuiElement e = elementList[currentElement];

			if (e.isShowing)
			{
				e.isShowing = false;
				e.hide();
			}
			else
			{
				e.isShowing = true;
				e.show();
			}
			elementList[currentElement] = e;
		}

		if (timer >= timerDuration)
		{
			elementList[currentElement].show();

			GuiElement e = elementList[currentElement];
			e.isShowing = true;
			elementList[currentElement] = e;

			timer = 0;
			currentElement++;
		}

		return false;
	}

	private void hideGuiObjects()
	{
		foreach (GuiElement element in elementList)
		{
			element.hide();
		}
	}

	private bool showGuiObjects()
	{
		//if everything is loaded
		if (flicker())
		{
			return true;
		}	
		return false;
	}

	public void addGuiElement(showGuiObject showFunc, hideGuiObject hideFunc)
	{
		GuiElement e = new GuiElement();
		e.show = showFunc;
		e.hide = hideFunc;
		elementList.Add(e);
	}

	public delegate void showGuiObject();
	public delegate void hideGuiObject();

	private void OnGUI()
	{
		//if is not paused
		if (Time.timeScale == 1) fade();
	}

	private void fade()
	{
		if (countdownStarted) return;
		if (fadeAlpha <= 0) 
		{
			fadedIn = true;
			return;
		}
		
		fadeAlpha -= fadeSpeed * Time.deltaTime;
		fadeAlpha = Mathf.Clamp01(fadeAlpha); //clamp to 0-1

		Color color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, fadeAlpha);
		GUI.color = color;

		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeTexture);
	}

	private void startCountdown()
	{
		countdownText.text = "3";

		Invoke("countdown1", 1);
		//new WaitForSeconds(0.5f);
		Invoke("countdown2", 2.5f);
		//new WaitForSeconds(0.5f);
		Invoke("countdown3", 4);
	}

	private void enableVehicles()
	{
		for (int i = 0; i < vehicles.Length; i++)
		{
			Vehicle car = vehicles[i].GetComponent<Vehicle>();
			Player player = vehicles[i].GetComponent<Player>();
			Enemy enemy = vehicles[i].GetComponent<Enemy>();


			if (car != null) car.enabled = true;
			if (player != null) player.enabled = true;
			if (enemy != null) enemy.enabled = true;
		}
	}

	private void disableVehicles()
	{
		for (int i = 0; i < vehicles.Length; i++)
		{
			Vehicle car = vehicles[i].GetComponent<Vehicle>();
			Player player = vehicles[i].GetComponent<Player>();
			Enemy enemy = vehicles[i].GetComponent<Enemy>();


			if (car != null) car.enabled = false;
			if (player != null) player.enabled = false;
			if (enemy != null) enemy.enabled = false;

		}
	}

	private void countdown1()
	{
		changeStuff(0);
		countdownText.text = "2";
	}

	private void countdown2()
	{
		changeStuff(1);
		countdownText.text = "1";
	}

	private void countdown3()
	{
		changeStuff(2);
		countdownText.text = "";
		enableVehicles();
	}

	private void changeStuff(int index)
	{
		if (lights.Length == 3) 
		{
			lights[index].GetComponent<Renderer>().material = enabledMat;
			if (lights[index].GetComponent<Light>() != null) lights[index].GetComponent<Light>().intensity = 4;
		}
		if (clips.Length == 3)
		{
			GetComponent<AudioSource>().clip = clips[index];
			GetComponent<AudioSource>().Play();
		}
	}
}
