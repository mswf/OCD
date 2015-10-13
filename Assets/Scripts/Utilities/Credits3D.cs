using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Credits3D : MonoBehaviour {

	/*
	private struct ScrollText
	{
		public string str;
		public Vector2 pos;
	}
	*/
	//private List<ScrollText> credits;
	//public float spacingY = 20f;
	public float textScrollSpeed = 0.2f;
	public bool finished = false;

	//public string[] creditsText;

	private TextMesh mesh;
	public Material material;


	private void Start()
	{
		mesh = gameObject.GetComponent<TextMesh>();
		Debug.Log(material);
		//gameObject.AddComponent<TextOutline>();
		mesh.anchor = TextAnchor.UpperCenter;
		mesh.alignment = TextAlignment.Center;
		mesh.characterSize = 0.03f;
		mesh.font = Resources.Load("oceanicdrift") as Font;
		//Init();
		Init2();
	}

	/*
	private void OnGUI()
	{
		Draw();
	}

	private void Init()
	{
		credits = new List<ScrollText>();
		foreach (string str in creditsText)
		{
			addCreditsLine(str);
		}
	}
	*/

	private void Init2()
	{
		//credits = new List<ScrollText>();

		addCreditsLine("Credits:");
		addCreditsLine("");
		addCreditsLine("Programming:");
		addCreditsLine("Valentinas Rimeika");
		addCreditsLine("Weikie Yeh");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("3D modelling:");
		addCreditsLine("Steff Kempink");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("Audio:");
		addCreditsLine("Miso Finne");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("Textures:");
		addCreditsLine("Steff Kempink");
		addCreditsLine("Nieck de Graaff");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("Level Design:");
		addCreditsLine("Steff Kempink");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("Level Balancing:");
		addCreditsLine("Steff Kempink");
		addCreditsLine("Weikie Yeh");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("AI Programming:");
		addCreditsLine("Weikie Yeh");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("Music by Dan-O at DanoSongs.com.");
		addCreditsLine("Undiscovered Oceans");
		addCreditsLine("");
		addCreditsLine("");
		addCreditsLine("Special Thanks to:");
		addCreditsLine("OculusVR for creating the Oculus Rift");

		StartCoroutine(TextScroll());
	}
	
	IEnumerator TextScroll()
	{
		while (!finished)
		{
			gameObject.transform.Translate(0.0f, textScrollSpeed * Time.deltaTime, 0.0f);// = new Vector3();
			yield return null;
		}
		StopCoroutine("TextScroll");
	}

	/*
	public void Draw()
	{

		for (int i = 0; i < credits.Count; i++)
		{
			GUI.Label(new Rect(credits[i].pos.x, credits[i].pos.y, Screen.width / 2, 200), credits[i].str);
			ScrollText st = credits[i];
			st.pos += new Vector2(0, -(textScrollSpeed * Time.deltaTime));
			credits[i] = st;
		}

		if (credits[credits.Count-1].pos.y < -50f)
		{
			finished = true;
		}
	}
	*/
	private void addCreditsLine(string str)
	{
		//ScrollText st = new ScrollText();
		//st.str = str;
		//Vector2 v = new Vector2(0, Screen.height + (spacingY * credits.Count));
		//st.pos = v;
		//credits.Add(st);
		mesh.text += str;
		mesh.text += "\n";
	}
}
