using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;
using System.Xml;

public enum Axis
{
	x,
	y,
	z
}

public class ProfileHandler
{

	public struct PlayerName
	{
		public string name;
		public int id;
	}

	private string name;
	private int id;
	private Vector3 left;
	private Vector3 right;
	private Axis axis;

	string _filePath = "logs/";
	string fileName = "prof.xml";

	public int getID()
	{
		return id;
	}

	public string getName()
	{
		return name;
	}

	public Vector3 getLeft()
	{
		//return left;
		return new Vector3(0.5f,0.5f,0.5f);
	}

	public Vector3 getRight()
	{
		//return right;
		return new Vector3(0.5f,0.5f,0.5f);
	}

	public string getAxis()
	{
		return axis.ToString();
	}

	public List<PlayerName> getNames()
	{
		List<PlayerName> nameList = new List<PlayerName>();

		if (!fileExists()) return nameList;

		//instantiate new xmldoc
		XmlDocument xmlDoc = new XmlDocument();
		//load from file
		xmlDoc.Load(_filePath + fileName);

		//get all player nodes
		XmlNodeList playerList = xmlDoc.GetElementsByTagName("player");

		//go through all players
		for (int i = 0; i < playerList.Count; i++)
		{
			//add to list
			PlayerName play;
			play.id = int.Parse(playerList[i].Attributes["id"].Value);
			play.name = playerList[i].Attributes["name"].Value;
			nameList.Add(play);
		}

		return nameList;
	}

	public bool loadProfile(int id)
	{
		//check if file exists
		if (!File.Exists(_filePath + fileName))
		{
			throw new Exception(String.Format("File not found at {0}{1}{0}. -Weikie", '"', _filePath + fileName));
		}

		//instantiate new xmldoc
		XmlDocument xmlDoc = new XmlDocument();
		//load from file
		xmlDoc.Load(_filePath + fileName);

		//get all player nodes
		XmlNodeList playerList = xmlDoc.GetElementsByTagName("player");

		//id not yet found
		int index = -1;

		//query through player list, search for index of ID
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].Attributes["id"].Value == id.ToString())
			{
				//id found
				index = i;
				break;
			}
		}

		//id could not be found
		if (index == -1) return false;

		//init vars
		name = playerList[index].Attributes["name"].Value;
		this.id = id;

		//quaternions

		//left
		float x = float.Parse(playerList[index]["settings"]["left"].Attributes["x"].Value);
		float y = float.Parse(playerList[index]["settings"]["left"].Attributes["y"].Value);
		float z = float.Parse(playerList[index]["settings"]["left"].Attributes["z"].Value);
		left = new Vector3(x,y,z);

		//right
		x = float.Parse(playerList[index]["settings"]["right"].Attributes["x"].Value);
		y = float.Parse(playerList[index]["settings"]["right"].Attributes["y"].Value);
		z = float.Parse(playerList[index]["settings"]["right"].Attributes["z"].Value);
		right = new Vector3(x,y,z);

		//axis
		string ax = playerList[index]["settings"]["axis"].Attributes["value"].Value;
		switch (ax)
		{
		case "x":
			axis = Axis.x;
			break;
		case "y":
			axis = Axis.y;
			break;
		case "z":
			axis = Axis.z;
			break;
		}

		return true;
	}

	public bool saveProfile(string name, Vector3 eulerLeft, Vector3 eulerRight, Axis axis, int id = -1)
	{
		if (id == -1)
		{
			id = getLastId() + 1;
			if (id == 0) id++;
		}

		XmlDocument xmlDoc = new XmlDocument();

		//check if file exists
		if (fileExists())
		{
			//load existing
			xmlDoc.Load(_filePath + fileName);
		}
		else
		{
			//create new
			XmlElement pl = xmlDoc.CreateElement("profile");
			xmlDoc.AppendChild(pl);
		}


		XmlElement player = null;
		XmlNodeList playerList = xmlDoc.GetElementsByTagName("player");
		//query through player list, search for index of ID
		for (int i = 0; i < playerList.Count; i++)
		{
			if (playerList[i].Attributes["id"].Value == id.ToString())
			{
				//id found
				player =  (XmlElement) playerList[i];

				//overwrite player
				player.SetAttribute("name", name);
				player.SetAttribute("id", id.ToString());

				//overwrite l/r vectors
				player["settings"]["left"].SetAttribute("x", eulerLeft.x.ToString());
				player["settings"]["left"].SetAttribute("y", eulerLeft.y.ToString());
				player["settings"]["left"].SetAttribute("z", eulerLeft.z.ToString());

				player["settings"]["right"].SetAttribute("x", eulerRight.x.ToString());
				player["settings"]["right"].SetAttribute("y", eulerRight.y.ToString());
				player["settings"]["right"].SetAttribute("z", eulerRight.z.ToString());

				//overwrite axis
				player["settings"]["axis"].SetAttribute("value", axis.ToString());


				break;
			}
		}

		//player does not yet exist, create new
		if (player == null) player = createNewPlayerNode(xmlDoc, name, id, eulerLeft, eulerRight, axis);

		//add player to end of list
		Debug.Log("Player is to be created");
		xmlDoc.FirstChild.AppendChild(player);
		Debug.Log("Player is created");
		//save
		if (!Directory.Exists(_filePath)) Directory.CreateDirectory(_filePath);
		xmlDoc.Save(_filePath + fileName);
		return true;
	}

	private XmlElement createNewPlayerNode(XmlDocument xmlDoc, string name, int id, Vector3 left, Vector3 right, Axis axis)
	{
		//create new player
		XmlElement player = xmlDoc.CreateElement("player");
		player.SetAttribute("name", name);
		player.SetAttribute("id", id.ToString());

		//create new settings
		XmlElement settings = xmlDoc.CreateElement("settings");
		player.AppendChild(settings);

		//create left euler
		XmlElement leftEuler = xmlDoc.CreateElement("left");
		leftEuler.SetAttribute("x", left.x.ToString());
		leftEuler.SetAttribute("y", left.y.ToString());
		leftEuler.SetAttribute("z", left.z.ToString());
		settings.AppendChild(leftEuler);

		//create right euler
		XmlElement rightEuler = xmlDoc.CreateElement("right");
		rightEuler.SetAttribute("x", right.x.ToString());
		rightEuler.SetAttribute("y", right.y.ToString());
		rightEuler.SetAttribute("z", right.z.ToString());
		settings.AppendChild(rightEuler);

		//create axis
		XmlElement axisNode = xmlDoc.CreateElement("axis");
		axisNode.SetAttribute("value", axis.ToString());
		settings.AppendChild(axisNode);

		return player;
	}

	public int getLastId()
	{
		//if file doesnt exists
		if (!fileExists()) return -1;

		//instantiate new xmldoc
		XmlDocument xmlDoc = new XmlDocument();
		//load from file
		xmlDoc.Load(_filePath + fileName);


		int highestID = int.MinValue;

		XmlNodeList playerList = xmlDoc.GetElementsByTagName("player");

		//query through player list, search for highest ID
		for (int i = 0; i < playerList.Count; i++)
		{
			if (int.Parse(playerList[i].Attributes["id"].Value) > highestID)
			{
				//higher id found
				highestID = int.Parse(playerList[i].Attributes["id"].Value);
			}
		}

		//return id of last player in file
		return highestID;
	}

	private bool fileExists()
	{
		if (!File.Exists(_filePath + fileName))
		{
			return false;
		}
		return true;
	}
}
