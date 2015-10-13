using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;
using System;

public class OculusLog {

	//privates
	private bool _isLogging = false;
	private StreamWriter writer;
	private string _logStartTime;
	private List<string> data;
	private const string filePath = "logs/";

	//getter
	public bool isLogging { get { return _isLogging; } }

	//statics
	private static OculusLog _instance;

	public static OculusLog getInstance()
	{
		if (_instance == null)
		{
			_instance = new OculusLog();
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
			//dd-mm-yy
			_logStartTime = string.Format("{0}-{1}-{2} {3}{4}", DateTime.Now.Day, DateTime.Now.Month, DateTime.Now.Year, DateTime.Now.Hour, DateTime.Now.Minute);
			data = new List<string>();
			data.Add(string.Format("Device log session started at {0} by ({1}) {2}", DateTime.Now.ToString(), profileID, profileName));
		}
		else
		{
			throw new Exception("New Device logging session attempted while previous one is still active.");
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

	public void logData(Quaternion orientation)
	{
		if (_isLogging)
		{
			string formatted = string.Format("{0}: {1} ({2},{3},{4})", DateTime.Now.ToString(), "Orientation", orientation.x, orientation.y, orientation.z);
			data.Add(formatted);
		}
	}

	private string createFileName(string startTime)
	{
		string name = filePath + string.Format("Device_Log_{0}.txt", startTime);
		return name;
	}
}
