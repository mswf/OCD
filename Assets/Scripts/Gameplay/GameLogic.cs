using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameLogic : MonoBehaviour
{


	struct LapData
	{
		public int lapCount;
		public int checkpointNumber;
		public List<int> lapTime;
	}
	
	private Dictionary<int, LapData> data;
	private System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();
	public int laps = 2;
	private int _playerID = -1;
	
	// Use this for initialization
	void Start()
	{
		data = new Dictionary<int, LapData>();
		
		int mode = PlayerPrefs.GetInt("GameMode");
		
		if (mode == 1)
		{
			GameObject pl = GameObject.Find("player");
			Destroy(pl.GetComponent<Player>());
			Enemy enemy = pl.AddComponent<Enemy>();
			enemy.difficulty = Enemy.AI_Difficulty.Y_Key;
		}
	}
	
	void Update()
	{
		Quaternion q = WeikiesRiftHack.GetOrientation();
		//OVRDevice.OrientSensor(ref q);
		//OVRDevice.GetOrientation(0, ref q);
		OculusLog.getInstance().logData(q);
		
		
	}
	
	public void pauseStopwatch()
	{
		sw.Stop();
	}
	
	public void resumeStopwatch()
	{
		sw.Start();
	}
	
	public void hitLap(Collider collider)
	{
		Vehicle vehicle = collider.GetComponent<Vehicle>();
		
		if (vehicle != null)
		{
			if (data.Count == 0)
			{
				int id = 0;
				string name = "DEBUG";
				sw.Reset();
				sw.Start();
				
				if (PlayerPrefs.HasKey("ID"))
				{
					//ID of the profile
					id = PlayerPrefs.GetInt("ID");
					
					//Name of the profile
					name = PlayerPrefs.GetString("Name");
				}
				
				
				Log.getInstance().startLogging(id, name);
				OculusLog.getInstance().startLogging(id, name);
			}
			
			//check if ID is already in data
			if (data.ContainsKey(vehicle.id))
			{
				//increment if not in data
				LapData d = data[vehicle.id];
				int time = (int)sw.ElapsedMilliseconds;
				
				d.lapCount++;
				vehicle.currLap++;
				
				//if is not first lap
				if (d.lapTime.Count > 0)
				{
					//calc time for current lap time by reducing current time by previous lap times
					for (int i = 0; i < d.lapTime.Count; i++)
					{
						time -= d.lapTime[i];
					}
				}
				
				//add lap time
				d.lapTime.Add(time);
				data[vehicle.id] = d;
				
				int pos = 0;
				
				//get position in race
				foreach (KeyValuePair<int, LapData> item in data)
				{
					if (item.Value.lapCount >= d.lapCount && item.Value.checkpointNumber >= d.checkpointNumber)
					{
						pos++;
					}
				}
				
				//if its not player, check if AI needs to be increased/decreased in difficulty
				if (!vehicle.isPlayer)
				{
					//if enemy is behind player
					if (d.lapCount <= data[_playerID].lapCount)
					{
						int totTime = 0;
						
						for (int i = 0; i < data[_playerID].lapTime.Count; i++)
						{
							totTime += data[_playerID].lapTime[i];
						}
						
						//positive indicates player is ahead, negative means player is behind
						int diff = totTime - (int)sw.ElapsedMilliseconds;
						
						//Debug.Log(diff);
						//if difference bigger than 10 sec
						if (-diff > 10 * 1000)
						{
							//set AI to Ykey AI
							collider.GetComponent<Enemy>().difficulty = Enemy.AI_Difficulty.Y_Key;
							//Debug.Log("changed to ykey");
						}
						
						//if difference bigger than 5 sec
						else
							if (-diff > 5 * 1000)
							{
								//set AI to hard AI
								collider.GetComponent<Enemy>().difficulty = Enemy.AI_Difficulty.Hard;
								//Debug.Log("changed to hard");
							}
					}
				}
				
				
				//log data
				Log.getInstance().addLapLine(vehicle.id, time.ToString(), pos);
				//Debug.Log(int.Parse(time.ToString()) / 1000);
			}
			
			else
			{
				//is not in data, add to data
				LapData d = new LapData();
				d.lapCount = 0;
				d.checkpointNumber = 0;
				d.lapTime = new List<int>();
				data.Add(vehicle.id, d);
				vehicle.currLap = 1;
				
				if (vehicle.isPlayer)
				{
					_playerID = vehicle.id;
				}
			}
			
			//Debug.Log("Checkpoint hit, current lap:" + data[vehicleComp.id]);
			
			//if id has driven the given amount of laps
			if (data[vehicle.id].lapCount == laps)
			{
				//Debug.Log(string.Format("Vehicle {0} has finished {1} laps", vehicleComp.id, laps));
				
				//do something
				if (vehicle.isPlayer)
				{
					//disable player controls and make it go auto steer mode (AI?)
					vehicle.finishGame();
					Log.getInstance().stopLogging();
					OculusLog.getInstance().stopLogging();
					//Debug.Log("game finished");
					
					
					//set highscore
					HighscoreManager hs = new HighscoreManager();
					string name = PlayerPrefs.HasKey("Name") ? PlayerPrefs.GetString("Name") : "Debug";
					hs.saveHighscore(Application.loadedLevelName, name, vehicle.id, data[vehicle.id].lapTime.ToArray());
				}
				
				else
				{
					//AI finished all its laps, disable AI?
				}
			}
		}
	}
	
	public void hitCheckpoint(Vehicle vehicle, int checkpointNumber)
	{
		if (data == null || !data.ContainsKey(vehicle.id))
		{
			return;
		}
		
		//if is player, rubberband
		
		//update checkpoint number
		LapData d = data[vehicle.id];
		
		//if skipped checkpoint, or go backwards, go fix
		//went backwards
		if (checkpointNumber < d.checkpointNumber && checkpointNumber != 1)
		{
			vehicle.warpToCheckpoint(d.checkpointNumber);
			return;
		}
		
		d.checkpointNumber = checkpointNumber;
		data[vehicle.id] = d;
		
		//set position
		int currPosition = getPosition(vehicle.id);
		vehicle.positionInRace = currPosition;
		vehicle.checkpointNumb = checkpointNumber;
		
		//rubberband
		if (!vehicle.isPlayer)
		{
			int playerPos = getPosition(_playerID);
			
			int diff = currPosition - playerPos + 1;
			
			if (diff > 0)
			{
				vehicle.RubberbandSpeed = diff * 0.2f;
			}
			
			else
			{
				vehicle.RubberbandSpeed = 0f;
			}
		}
	}
	
	public int getPosition(int vehicleID)
	{
		int pos = 0;
		
		if (!data.ContainsKey(vehicleID))
		{
			return -1;
		}
		
		LapData d = data[vehicleID];
		
		//get position in race
		foreach (KeyValuePair<int, LapData> item in data)
		{
			if (item.Value.lapCount > d.lapCount ||
					(item.Value.lapCount == d.lapCount && item.Value.checkpointNumber >= d.checkpointNumber))
			{
				pos++;
			}
		}
		
		return pos;
	}
}
