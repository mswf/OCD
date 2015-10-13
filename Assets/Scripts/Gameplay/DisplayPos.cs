﻿using UnityEngine;
using System.Collections;

public class DisplayPos : MonoBehaviour {
	
	public Vehicle player;
	private TextMesh text;
	
	void Start()
	{
		text = GetComponent<TextMesh>();
	}
	
	void Update () 
	{
		text.text = player.positionInRace.ToString();
	}
}
