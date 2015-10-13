using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Xml;

public struct Highscore
{
	public string name;
	public int id;
	public int[] timesInMilliseconds;
	public int totalTime;
}

public class HighscoreManager {
	private const string _filePath = "logs/";
	private const string fileName = "highscore.xml";
	

	/*
	// Debug stuff
	void Update () {
		if (Input.GetKeyDown("z"))
		{
			List<Highscore> list = getHighscoreList("level1");
			Debug.Log("highscore list length: " + list.Count);
		}
		if (Input.GetKeyDown("x"))
		{
			int[] PB = getPersonalBest("level1", 1);
			string txt = "";

			if (PB != null)
			{
				for (int i = 0; i < PB.Length; i++)
				{
					txt += string.Format("Lap{0}: {1},", i, PB[i]);
				}
			}
			Debug.Log("PB: " + txt);
		}
		if (Input.GetKeyDown("c"))
		{
			//saving using list
			List<int> timeList = new List<int>();
			timeList.Add(90);
			timeList.Add(80);
			timeList.Add(50);
			saveHighscore("level1", "weikie", 1, timeList.ToArray());
			
			//saving using array
			//int[] times = new int[3]{50,60,40};
			//saveHighscore("level1", "weikie", 1, times);
			Debug.Log("saving highscore");
		}

	}*/


	public void saveHighscore(string levelName, string playerName, int id, int[] time)
	{
		//get highscore list
		List<Highscore> highscoreList = getHighscoreList(levelName);

		//if new time is in top 10 add score
		if (highscoreList.Count < 10)
		{
			//add
			addScore(levelName, playerName, id, time);
		}
		//if new time is in top 10
		else 
		{
			int total1 = getTotal(time);
			
			if (total1 < highscoreList[9].totalTime)
			{
				//add
				addScore(levelName, playerName, id, time);

			}
			else 
			{
				int PB = getTotal(getPersonalBest(levelName, id, highscoreList));
				//if time is better than personal best
				if (total1 < PB || PB == -1)
				{
					//add
					addScore(levelName, playerName, id, time);
				}
				else
				{
					//dont add
					Debug.Log("didnt save");
				}
			
			}
		}
	}

	/// <summary>
	/// Adds a new score to the highscore list, this function will not check if the entry is valid or not
	/// </summary>
	/// <param name="levelName">Name of the level</param>
	/// <param name="playerName">Name of the player</param>
	/// <param name="playerID">ID of the player</param>
	/// <param name="time">Time in seconds</param>
	private void addScore(string levelName, string playerName, int playerID, int[] time)
	{
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
			XmlElement rootNode = xmlDoc.CreateElement("highscore");
			xmlDoc.AppendChild(rootNode);
		}

		XmlNodeList levelList = xmlDoc.GetElementsByTagName("level");
		XmlElement levelNode = null;

		for (int i = 0; i < levelList.Count; i++)
		{
			//search for level
			if (levelList[i].Attributes["name"].Value == levelName)
			{
				//sets the level node
				levelNode = (XmlElement)levelList[i];
				break;
			}
		}

		//if level doesnt have a highscore list yet
		if (levelNode == null)
		{
			//create empty level
			levelNode = xmlDoc.CreateElement("level");
			levelNode.SetAttribute("name", levelName);
			xmlDoc.FirstChild.AppendChild(levelNode);
		}

		//add data to level
		XmlElement scoreNode = xmlDoc.CreateElement("data");
		scoreNode.SetAttribute("playerName", playerName);
		scoreNode.SetAttribute("playerID", playerID.ToString());
		//scoreNode.SetAttribute("time", time.ToString());
		levelNode.AppendChild(scoreNode);

		//add time to level
		for (int i = 0; i < time.Length; i++)
		{
			XmlElement lapNode = xmlDoc.CreateElement("lap");
			lapNode.SetAttribute("time", time[i].ToString());
			scoreNode.AppendChild(lapNode);
		}

		if (!Directory.Exists(_filePath)) Directory.CreateDirectory(_filePath);
		xmlDoc.Save(_filePath + fileName);
	}

	/// <summary>
	/// Gets the highscore list of requested level
	/// </summary>
	/// <param name="levelName">Name of the level</param>
	/// <returns>List of highscores sorted on fastest time</returns>
	public List<Highscore> getHighscoreList(string levelName)
	{
		List<Highscore> scoreList = new List<Highscore>();
		
		//check if highscore list exists
		if (!fileExists()) return scoreList;

		//instantiate new xmldoc
		XmlDocument xmlDoc = new XmlDocument();
		//load from file
		xmlDoc.Load(_filePath + fileName);

		//get all player nodes
		XmlNodeList levelNodes = xmlDoc.GetElementsByTagName("level");

		//go through all levels
		for (int i = 0; i < levelNodes.Count; i++)
		{
			//search for level
			if (levelNodes[i].Attributes["name"].Value == levelName)
			{
				//return the list
				for (int n = 0; n < levelNodes[i].ChildNodes.Count; n++)
				{
					Highscore score;
					score.name =  levelNodes[i].ChildNodes[n].Attributes["playerName"].Value;
					score.id = int.Parse(levelNodes[i].ChildNodes[n].Attributes["playerID"].Value);

					//get times
					int[] laps = new int[levelNodes[i].ChildNodes[n].ChildNodes.Count];
					int total = 0;
					for (int j = 0; j < levelNodes[i].ChildNodes[n].ChildNodes.Count; j++)
					{
						laps[j] = int.Parse(levelNodes[i].ChildNodes[n].ChildNodes[j].Attributes["time"].Value);
						total += int.Parse(levelNodes[i].ChildNodes[n].ChildNodes[j].Attributes["time"].Value);
					}
					score.timesInMilliseconds = laps;
					score.totalTime = total;

					//add score to list
					scoreList.Add(score);
				}
				//sort
				sortHighscoreList(ref scoreList);
				//level and highscore found, stop looping
				break;
			}
		}

		return scoreList;
	}

	private void sortHighscoreList(ref List<Highscore> scoreList)
	{
		//sorts on time
		scoreList.Sort((score1, score2) => score1.totalTime.CompareTo(score2.totalTime));
	}

	/// <summary>
	/// Gets the personal best time on the requested level
	/// </summary>
	/// <param name="levelName">Name of the level</param>
	/// <param name="id">Player ID</param>
	/// <returns>Time in seconds</returns>
	public int[] getPersonalBest(string levelName, int id)
	{
		List<Highscore> scoreList = getHighscoreList(levelName);

		return getPersonalBest(levelName, id, scoreList);
	}

	private int[] getPersonalBest(string levelName, int id, List<Highscore> scoreList)
	{
		//loop through list
		for (int i = 0; i < scoreList.Count; i++)
		{
			//find id
			if (scoreList[i].id == id)
			{
				//found id
				return scoreList[i].timesInMilliseconds;
			}
		}

		//id not found
		return null;
	}

	private bool fileExists()
	{
		if (!File.Exists(_filePath + fileName))
		{
			return false;
		}
		return true;
	}

	private int getTotal(int[] arr)
	{
		int tot = 0;
		for (int i = 0; i < arr.Length; i++)
		{
			tot += arr[i];
		}
		return tot;
	}
}
