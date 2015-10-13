using UnityEngine;
using System.Collections;

public enum Direction
{
	None,
	Up,
	Down,
	Left,
	Right
}

public class Changer : MonoBehaviour {
	//duration of changing
	public float changeDuration = 0; //revert change after x seconds, 0 = forever

	//change over time
	public float changeOverTime = 200; //time in frames? 

	public Direction moveDirection;
	public float moveDist;
	//public Direction rotateDirection;
	//public Vector3 rotation;
	//enable/disable script
	/*public bool scriptsEnabled;
	public GameObject[] objectsContainingScripts;
	public string[] scriptNames;*/

	//if true, then object will be moved into disabled position on start
	public bool objectStatus = true;
	public bool triggerOnce;

	private bool _changing;
	private bool _triggered;
	private int _currTime = 0;

	public void change()
	{
		//if is not already changing AND (can be triggered once and hasnt triggered yet OR can be triggered multiple times)
		 if (!_changing && ((triggerOnce && !_triggered) || !triggerOnce)) 
		 {
			_triggered = true;
			_changing = true;
		 }
		 
	}

	void Start()
	{
		if (objectStatus)
		{
			Vector3 dir;

			switch (moveDirection)
			{
				case Direction.None:
					dir = new Vector3();
					break;
				case Direction.Up:
					dir = Vector3.up;
					break;
				case Direction.Right:
					dir = Vector3.right;
					break;
				case Direction.Down:
					dir = Vector3.down;
					break;
				case Direction.Left:
					dir = Vector3.left;
					break;
				default:
					dir = new Vector3();
					break;
			}

			transform.localPosition += -dir * moveDist;
		}
	}

	void FixedUpdate()
	{
		if (_changing)
		{
			_currTime++;

			if (changeOverTime == 0 || _currTime < changeOverTime)
			{
				//do shit
				changePosition();
				//changeScripts();
				//changeRotation();
				
			}
			else
			{
				
				//make it move back
				if (_currTime > changeOverTime + changeDuration)
				{
					//make it go back
					changePosition(true);
					
				}

				//nothing else needs to be done
				if (changeDuration == 0 || _currTime == changeOverTime*2 + changeDuration - 1)
				{
					_changing = false;
					_currTime = 0;
				}
				
			}
		}
	}

	/*
	private void changeScripts()
	{
		for (int i = 0; i < objectsContainingScripts.Length; i++)
		{
			for (int n = 0; n < scriptNames.Length; n++)
			{
				MonoBehaviour behv = objectsContainingScripts[i].GetComponent(scriptNames[n]) as MonoBehaviour;
				if (behv != null)
				{
					behv.enabled = !behv.enabled;
				}
			}
		}
	}*/
	/*
	private void changeRotation()
	{
		
	}*/

	private void changePosition(bool reverse = false)
	{
		float str = moveDist/changeOverTime;
		Vector3 dir;

		switch (moveDirection)
		{
			case Direction.None:
				dir = new Vector3();
				break;
			case Direction.Up:
				dir = Vector3.up;
				break;
			case Direction.Right:
				dir = Vector3.right;
				break;
			case Direction.Down:
				dir = Vector3.down;
				break;
			case Direction.Left:
				dir = Vector3.left;
				break;
			default:
				dir = new Vector3();
				break;
		}

		if (!reverse) transform.localPosition += dir * str;
		else transform.localPosition -= dir * str;
	}
}
