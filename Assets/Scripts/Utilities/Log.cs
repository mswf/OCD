using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class Log {

	public enum LogType
	{
		Pickup,
		OutOfBounds,
		Other
	}

	//privates
	private bool _isLogging = false;
	private StreamWriter writer;
	private string _logStartTime;
	private List<string> data;
	private const string filePath = "logs/";


	//getter
	public bool isLogging { get { return _isLogging; } }

	//statics
	private static Log _instance;

	public static Log getInstance()
	{
		if (_instance == null)
		{
			_instance = new Log();
		}

		return _instance;
	}
	
	//functions
	public void startLogging(int profileID, string profileName)
	{
		if (profileID > -1) return; //disables all logging
		if (!_isLogging)
		{
			_isLogging = true;
			_logStartTime = String.Format("{0}-{1}-{2} {3}{4}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute);
			data = new List<string>();
			data.Add(String.Format("Log session started at {0} by ({1}) {2}", DateTime.Now.ToString(), profileID, profileName));
		}
		else
		{
			throw new Exception("New data logging session attempted while previous one is still active.");
		}
	}

	public void stopLogging()
	{
		if (_isLogging)
		{
			_isLogging = false;
			data.Add(string.Format("Log session ended at {0}", DateTime.Now.ToString()));
			
			//create file
			writer = File.AppendText(createFileName(_logStartTime));

			//write data
			for (int i = 0; i < data.Count; i++)
			{
				writer.WriteLine(data[i]);
			}

			//save to file
			writer.Flush();

			//cleanup
			writer.Dispose();
			data = null;
			_logStartTime = null;
		}
	}

	public void addPickupLine(int playerID, Transform player, Transform pickup, float resource)
	{
		if (_isLogging)
		{
			string formatted = string.Format("{0}: {1} by player {2}, position {3}, name {4}, amount {5}", DateTime.Now.ToString(), "Pickup", playerID.ToString(), player.position.ToString(), pickup.name.ToString(), resource.ToString());
			data.Add(formatted);
		}
	}

	public void addOutOfBoundsLine(int playerID, Transform player)
	{
		if (_isLogging)
		{
			string formatted = string.Format("{0}: {1} by player {2}, position {3}", DateTime.Now.ToString(), "OutOfBounds", playerID.ToString(), player.position.ToString());					
			data.Add(formatted);
		}
	}

	public void addLine(string eventName, int playerID, Transform player)
	{
		if (_isLogging)
		{
			string formatted = string.Format("{0}: {1} by player {2}, position {3}", DateTime.Now.ToString(), eventName, playerID.ToString(), player.position.ToString());										
			data.Add(formatted);
		}
	}

	public void addCollisionLine(int playerID, Transform player)
	{
		if (_isLogging)
		{
			string formatted = string.Format("{0}: {1} by player {2}, position {3}", DateTime.Now.ToString(), "Collision", playerID.ToString(), player.position.ToString());
			data.Add(formatted);
		}
	}
	
	public void addLapLine(int playerID, string lapTime, int positionInRace)
	{
		if (_isLogging)
		{
			string formatted = string.Format("{0}: {1} by player {2}, time {3}, position in race {4}", DateTime.Now.ToString(), "Lap", playerID.ToString(), lapTime, positionInRace);
			data.Add(formatted);
		}
	}

	private string createFileName(string startTime)
	{
		string name = filePath + string.Format("Log_{0}.txt", startTime);
		return name;
	}
}
