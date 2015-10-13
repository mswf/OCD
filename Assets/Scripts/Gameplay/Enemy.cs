using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Enemy : MonoBehaviour {

	public AI_Difficulty difficulty;
	public Transform player;
	private PickupBehaviour _pickupState;
	private WiggleBehaviour _wiggleState;
	private PlayerPosition _positionState;
	private Vehicle _vehicle;
	
	private Transform rightWing;
	private Transform leftWing;
	private Transform steeringWheel;

	public float wingRotation = 0f;
	public float wingRotationSpeed = 7f;
	private int wingDirection = 0;

	//the middle area the vehicle tries to stay in
	private const float middlePercentage = 40;

	public enum AI_Difficulty
	{
		Easy,
		Normal,
		Hard,
		Y_Key
	}

	private enum PlayerPosition
	{
		None,
		InFront,
		Behind
	}

	private enum PickupBehaviour
	{
		None,
		Collecting,
		Ignoring
	}
	
	private enum WiggleBehaviour
	{
		None,
		Wiggling,
		Ignoring
	}

	void Start()
	{
		_vehicle = GetComponent<Vehicle>();
		
		steeringWheel = transform.Find("CarContents/HoverCar/CarBody_LOD0/SteeringWheelPosition/Car_SteeringWheel");
		leftWing = transform.Find("CarContents/HoverCar/CarBody_LOD0/Wing_Left_LOD0");
		rightWing = transform.Find("CarContents/HoverCar/CarBody_LOD0/Wing_Right_LOD0");
	}
	
	private void RotateWings()
	{
		rightWing.localRotation = Quaternion.Euler(0, 0, wingRotation);
		leftWing.localRotation = Quaternion.Euler(0, 0, wingRotation);

		steeringWheel.localRotation = Quaternion.Euler(0, -wingRotation, 0);

		wingRotation *= .9f;
	}

	void FixedUpdate()
	{

		switch (difficulty)
		{
			case AI_Difficulty.Easy:
				easyAI();
				break;
			case AI_Difficulty.Normal:
				normalAI();
				break;
			case AI_Difficulty.Hard:
				hardAI();
				break;
			case AI_Difficulty.Y_Key:
				yKeyAI();
				//overtakePlayer();
				break;
			default:
				easyAI();
				break;
		}

		handleWingRotation();
		RotateWings();
		//TEMP_INPUT();
	}
	
	private void handleWingRotation()
	{
		if (wingDirection < 0)
		{
			wingRotation += wingRotationSpeed;
		}
		else if (wingDirection > 0)
		{
			wingRotation -= wingRotationSpeed;
		}
		wingDirection = 0;
		RotateWings();
	}
	
	private void easyAI()
	{
		float distToPlayer = Vector3.Distance(transform.position, player.position);

		//check if player is close
		if (distToPlayer < 5)
		{
			//dodge player
			//moveAwayFrom(player);
			_vehicle.bounceAwayFrom(player.transform);

		}
		else
		{
			//if player is not close, check for nearby pickups
			Transform pickup = getClosestPickup();
			if (pickup != null)
			{
				//try to dodge pickups
				moveAwayFrom(pickup);
			}
			else
			{
				//no pickups or player nearby, try to stay in the middle
				goToMiddle();
			}
		}
	}

	private void normalAI()
	{
		float distToPlayer;
		if (player != null) distToPlayer = Vector3.Distance(transform.position, player.position);
		else distToPlayer = 200;		//player is null, failsafe

		//check if player is close
		if (distToPlayer < 5)
		{
			//dodge player
			//moveAwayFrom(player);
			_vehicle.bounceAwayFrom(player.transform);
		}
		else
		{
			//get closest pickup
			Transform pickup = getClosestPickup();;

			//check if state has to change
			if (_pickupState == PickupBehaviour.None)
			{

				//if pickup is nearby
				if (pickup != null)
				{
					float collectPercentage = 50;

					//if RNG decides to collect
					if (Random.value <= collectPercentage / 100)
					{
						//set to collect state
						_pickupState = PickupBehaviour.Collecting;
					}
					else
					{
						//set to dodge pickup state
						_pickupState = PickupBehaviour.Ignoring;
					}

					//reset state after given time
					float resetTimer = 1.5f;
					Invoke("resetPickupState", resetTimer);
				}
			}

			//actual behaviours in each state
			switch (_pickupState)
			{
				case PickupBehaviour.None:
					goToMiddle();
					break;
				case PickupBehaviour.Collecting:
					if (pickup != null) moveTo(pickup);
					else 
					{
						//no pickups are in range anymore, reset state prematurely
						resetPickupState();
						CancelInvoke("resetPickupState");
					}
					break;
				case PickupBehaviour.Ignoring:
					//try to dodge pickups
					if (pickup != null) moveAwayFrom(pickup);
					else
					{
						//no pickups are in range anymore, reset state prematurely
						resetPickupState();
						CancelInvoke("resetPickupState");
					}
					break;
			}
		}
	}

	private void hardAI()
	{
		float distToPlayer;
		if (player != null) distToPlayer = Vector3.Distance(transform.position, player.position);
		else distToPlayer = 200;		//player is null, failsafe

		//check if player is close
		if (distToPlayer < 5)
		{
			//dodge player
			_vehicle.bounceAwayFrom(player.transform);

			//moveAwayFrom(player);
		}
		else
		{
			//get closest pickup
			Transform pickup = getClosestPickup(); ;

			//check if state has to change
			if (_pickupState == PickupBehaviour.None)
			{

				//if pickup is nearby
				if (pickup != null)
				{
					float collectPercentage = 90;

					//if RNG decides to collect
					if (Random.value <= collectPercentage / 100)
					{
						//set to collect state
						_pickupState = PickupBehaviour.Collecting;
					}
					else
					{
						//set to dodge pickup state
						_pickupState = PickupBehaviour.Ignoring;
					}

					//reset state after given time
					float resetTimer = 1.5f;
					Invoke("resetPickupState", resetTimer);
				}
			}

			//actual behaviours in each state
			switch (_pickupState)
			{
				case PickupBehaviour.None:
					goToMiddle();
					break;
				case PickupBehaviour.Collecting:
					if (pickup != null) moveTo(pickup);
					else
					{
						//no pickups are in range anymore, reset state prematurely
						resetPickupState();
						CancelInvoke("resetPickupState");
					}
					break;
				case PickupBehaviour.Ignoring:
					//try to dodge pickups
					if (pickup != null) moveAwayFrom(pickup);
					else
					{
						//no pickups are in range anymore, reset state prematurely
						resetPickupState();
						CancelInvoke("resetPickupState");
					}
					break;
			}
		}
	}

	private void yKeyAI()
	{
		//get closest pickup
		Transform pickup = getSmartestPickup();

		//check if state has to change
		if (_pickupState == PickupBehaviour.None)
		{

			//if pickup is nearby
			if (pickup != null)
			{
				moveTo(pickup);
			}
			else
			{
				goToMiddle();
			}
		}
	}

	private void resetPickupState()
	{
		_pickupState = PickupBehaviour.None;
	}

	private void resetWiggleState()
	{
		_wiggleState = WiggleBehaviour.None;
	}
	
	private Transform getSmartestPickup()
	{
		RaycastHit[] hitInfo;
		Ray ray = new Ray(transform.position + -transform.up + transform.forward * 10, transform.forward);
		//Debug.DrawRay(transform.position + -transform.up, transform.forward, Color.red);
		hitInfo = Physics.SphereCastAll(ray, 10f, 30f, 1 << 10); //radius, dist

		if (hitInfo.Length == 0) return null;
		

		List<RaycastHit> temp = new List<RaycastHit>();

		for (int i = 0; i < hitInfo.Length; i++)
		{
			if (temp.Count == 0) temp.Add(hitInfo[i]);
			else
			{
				for (int n = 0; n < temp.Count; n++)
				{
					if (hitInfo[i].distance < temp[n].distance)
					{
						//insert before n
						temp.Insert(n, hitInfo[i]);
						break;
					}
				}
			}
		}

		Transform target = null;
		string type = "Other";
		float dist = float.MaxValue;

		foreach (RaycastHit hit in temp)
		{
			if (hit.distance < dist)
			{
				if (hit.transform.name.Contains("Unlock"))
				{
					target = hit.transform;
					dist = hit.distance;
					type = "Unlock";
				}
				else if (hit.transform.name.Contains("Speed"))
				{
					target = hit.transform;
					dist = hit.distance;
					type = "Speed";
				}
				else if (hit.transform.name.Contains("Power"))
				{
					if (type == "Other")
					target = hit.transform;
					dist = hit.distance;
					type = "Power";
				}
				else
				{
					if (type == "Other")
					{
						target = hit.transform;
						dist = hit.distance;
					}
				}
			}
		}
		return target;
	}

	private Transform getAllNearbyPickups()
	{
		RaycastHit[] hitInfo;
		Ray ray = new Ray(transform.position + -transform.up + transform.forward * 10, transform.forward);
		Debug.DrawRay(transform.position + -transform.up, transform.forward, Color.red);
		hitInfo = Physics.SphereCastAll(ray, 10f, 30f, 1 << 10); //radius, dist

		if (hitInfo.Length == 0) return null;
		

		List<RaycastHit> temp = new List<RaycastHit>();

		for (int i = 0; i < hitInfo.Length; i++)
		{
			if (temp.Count == 0) temp.Add(hitInfo[i]);
			else
			{
				for (int n = 0; n < temp.Count; n++)
				{
					if (hitInfo[i].distance < temp[n].distance)
					{
						//insert before n
						temp.Insert(n, hitInfo[i]);
						break;
					}
				}
			}
		}
		/*Debug.Log("distances:");
		foreach (RaycastHit hit in temp)
		{
			Debug.Log(hit.distance);
		}*/
		return hitInfo[0].transform;
	}

	private Transform getClosestPickup()
	{
		RaycastHit[] hitInfo;
		Ray ray = new Ray(transform.position + -transform.up + transform.forward * 10, transform.forward);
		Debug.DrawRay(transform.position + -transform.up, transform.forward, Color.red);
		hitInfo = Physics.SphereCastAll(ray, 10f, 30f, 1 << 10); //radius, dist

		if (hitInfo.Length == 0) return null;

		int closestIndex = 0;
		float dist = float.MaxValue;

		for (var i = 0; i < hitInfo.Length; i++)
		{
			if (hitInfo[i].distance < dist)
			{
				closestIndex = i;
				dist = hitInfo[i].distance;
			}
		}

		return hitInfo[closestIndex].transform;
	}

	private void TEMP_INPUT()
	{
		if (Input.GetKey("w"))
		{
			transform.position += transform.forward * 0.5f;
		}
		if (Input.GetKey("a"))
		{
			transform.position -= transform.right * 0.5f;
		}
		if (Input.GetKey("s"))
		{
			transform.position -= transform.forward * 0.5f;
		}
		if (Input.GetKey("d"))
		{
			transform.position += transform.right * 0.5f;
		}
		if (Input.GetKeyDown("q"))
		{
			_vehicle.goLeft(0.5f);
		}
		if (Input.GetKeyDown("e"))
		{
			_vehicle.goRight(0.5f);
		}
	}

	private void moveTo(Transform obj)
	{
		//check stuff
		float pickupRange = 8;
		float dist = _vehicle.calcDist(obj);

		//if obj is to the right and out of pickup range
		if (dist < -pickupRange)
		{
			//go right
			goRight();
		}
		//if obj is to the left and out of pickup range
		else if (dist > pickupRange)
		{
			//go left
			goLeft();
		}
	}

	private void moveAwayFrom(Transform obj)
	{
		//check stuff
		float moveDistance = 20;
		float dist = _vehicle.calcDist(obj);

		//Debug.Log(dist);
		//if obj is to the right and in of pickup range
		if (dist > -moveDistance && dist < 0)
		{
			//go left
			goLeft();
		}
		//if obj is to the left and in of pickup range
		else if (dist < moveDistance && dist > 0)
		{
			//go right
			goRight();
		}
	}

	private void goToMiddle()
	{
		//Debug.Log(Mathf.Sin(Time.time));
		//raycast left + right
		RaycastHit hitInfoLeft;
		RaycastHit hitInfoRight;


		bool leftHit = Physics.Raycast(transform.position, -transform.right, out hitInfoLeft, 50, 1 << 9);
		bool rightHit = Physics.Raycast(transform.position, transform.right, out hitInfoRight, 50, 1 << 9);


		if (!leftHit)
		{
			goRight();
		}
		else if (!rightHit)
		{
			goLeft();
		}
		else
		{
			//sum of distance
			float dist = hitInfoLeft.distance + hitInfoRight.distance;

			//position in lane 0..1 value
			float val = hitInfoLeft.distance / (dist);

			//if to the left of middle area
			if (val < (50 - (middlePercentage / 2)) / 100)
			{
				goRight(true);
			}
			//if to the right of middle area
			else if (val > (50 + (middlePercentage / 2)) / 100)
			{
				goLeft(true);
			}
			//make wiggle
			else
			{
				//if no wiggle state, assign wiggle state
				if (_wiggleState == WiggleBehaviour.None)
				{
					//chance to wiggle or ignore
					if (Random.value < .5f)
					{
						_wiggleState = WiggleBehaviour.Wiggling;
					}
					else
					{
						_wiggleState = WiggleBehaviour.Ignoring;
					}

					//resets wiggle state after a random time between 1 and 3
					Invoke("resetWiggleState", 1+Random.value*2);
				}
				
				//if in wiggle state
				if (_wiggleState == WiggleBehaviour.Wiggling)
				{
					//go left
					if (Mathf.Sin(Time.time * (_vehicle.id + 1)) < 0)
					{
						//if not too close to left wall
						if (val > (50 - ((middlePercentage - 5) / 2)) / 100)
						{
							//go left
							goLeft();
						}
					}
					//go right
					else
					{
						//if not too close to right wall
						if (val < (50 + ((middlePercentage - 5) / 2)) / 100)
						{
							//go right
							goRight();
						}
					}
				}
			}
		}
	}

	

	private void overtakePlayer()
	{
		if (player == null) return;
		
		float front = distSquared(transform.position + transform.forward, player.position);
		float back = distSquared(transform.position - transform.forward, player.position);

		if (front > 1000) return;

		if (front < back)
		{
			if (_positionState != PlayerPosition.InFront) _positionState = PlayerPosition.InFront;
		}
		else
		{
			if (_positionState == PlayerPosition.InFront)
			{
				//play sound
				if (GetComponent<AudioSource>() != null) GetComponent<AudioSource>().Play();
			}
			if (_positionState != PlayerPosition.Behind) _positionState = PlayerPosition.Behind;
		}
	}

	private float distSquared(Vector3 a, Vector3 b)
	{
		return
		(a.x - b.x) * (a.x - b.x) +
		(a.y - b.y) * (a.y - b.y) +
		(a.z - b.z) * (a.z - b.z);
	}

	private void goLeft(float turnStrength = 0.2f)
	{
		_vehicle.goLeft(turnStrength);
		wingDirection--;
	}

	private void goLeft(bool ignoreAnimation, float turnStrength = 0.2f)
	{
		_vehicle.goLeft(turnStrength);
		if (!ignoreAnimation) wingDirection--;
	}

	private void goRight(float turnStrength = 0.2f)
	{
		_vehicle.goRight(turnStrength);
		wingDirection++;
	}

	private void goRight(bool ignoreAnimation, float turnStrength = 0.2f)
	{
		_vehicle.goRight(turnStrength);
		if (!ignoreAnimation) wingDirection++;
	}
}
