using UnityEngine;
using System.Collections;
using UnityEngine.Assertions.Must;

public class Vehicle : IVehicle
{

	private static int _idCount = 0;

	#region Bumpedy bump
	private enum BumpState
	{
		None,
		Left,
		Right
	}
	private BumpState _bumpState;
	private float bumpStrength = 0f;
	#endregion


	public bool onShortcut = false;

	private bool leftRayHit;
	private bool rightRayHit;

	private bool _canGoLeft = true;
	private bool _canGoRight = true;

	private bool _warning = true;
	private bool _warning2 = true;

	public GameObject wheelHolo;
	public GameObject particleSystemWind;
	public ParticleSystem particleSystemLeftWing;
	public ParticleSystem particleSystemRightWing;
	public Transform hovercar;
	public float carRotationSpeed;
	private float carRotation;

	#region ray colors

	private Color VehicleCollisionColor = Color.red;
	private Color WallCollisionColor = Color.green;
	private Color FloorCollisionColor = Color.blue;


	#endregion

	#region sounds
	public AudioClip sideMoveSound;
	public AudioClip collisionSound;
	public AudioClip collisionSound2;
	public AudioClip collisionSound3;
	public AudioClip warningSound;
	public AudioClip warningSound2;
	private AudioSource _aSource;
	#endregion

	public bool finished = false;

	//autopilot handler
	private Player _player;
	private Enemy _enemy;
	public string autoPilotText;

	//camera effects
	public GameObject ovrCamController;
	private Vector3 ovrCamStartPos;
	public float camEffectY = 0.0f;
	public float camEffectZ = -0.9f; // This value was used for the effect

	//shield effect
	public GameObject shieldObj;
	public float shieldFadeSpeed = 2f;

	public float maxSpeed
	{
		get
		{
			return _currentMaxSpeed;    // Gives max speed for the engine sound class
		}
	}

	//rubberbanding stuff
	private float _incrSpeed = 0.0f;
	public float RubberbandSpeed
	{
		set
		{
			_incrSpeed = value;
		}
	}

	//position in race
	private int _positionInRace = 5;
	public int positionInRace
	{
		get
		{
			return _positionInRace;
		}
		set
		{
			_positionInRace = value;
		}
	}

	private Quaternion _oldQuaternion = Quaternion.identity;

	public GameObject wrongAxis;


	#region Out of Bounds
	//out of bounds handler
	public string lapText;
	private int _currLap = 0;
	public int currLap
	{
		get
		{
			return _currLap;
		}
		set
		{
			_currLap = value;
			lapped();
		}
	}
	public int checkpointNumb = 0;
	private bool __isInBounds = true;
	private ParticleSystem _particleSystemWind;

	private bool IsInBounds
	{
		get
		{
			return __isInBounds;
		}
		set
		{
			if (IsInBounds && value == false)
			{
				oobStart();
			}
			else if (!IsInBounds && value == true)
			{
				oobStop();
			}
			__isInBounds = value;
		}
	}

	#endregion

	private void lapped()
	{
		if (isPlayer)
		{
			lapText = "Lap " + currLap + "/" + GameObject.Find("GameLogic").GetComponent<GameLogic>().laps;
			Invoke("clearLapText", 2);
		}
	}

	private void clearLapText()
	{
		lapText = "";
	}

	private void oobStart()
	{
		Invoke("resetPos", 2);
	}

	private void oobStop()
	{
		CancelInvoke("resetPos");
	}

	private void resetPos()
	{
		if (!IsInBounds)
		{
			warpToCheckpoint(checkpointNumb);
		}
	}



	void Update()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			toggleAutoPilot();
		}
	}

	void Start ()
	{
		//Add audio source for the side movement
		_aSource = gameObject.AddComponent<AudioSource>();
		//Add clip to the audio source
		if (sideMoveSound != null) _aSource.clip = sideMoveSound;

		//Set the max speed
		_currentMaxSpeed = DEFAULT_MAX_SPEED;
		//Set acceleration speed
		_currentAccelerationSpeed = DEFAULT_ACCELERATION_SPEED;
		//Set minimum side speed
		_currentSideSpeed = MIN_SIDE_SPEED;

		//Vehicle id
		id = _idCount;
		_idCount++;


		//set vehicle layer
		gameObject.layer = 11;

		if (ovrCamController != null)	ovrCamStartPos = ovrCamController.transform.localPosition;

		if (isPlayer)
		{
			_particleSystemWind = particleSystemWind.GetComponent<ParticleSystem>();
		}



	}

	/// <summary>
	/// This function is called from the start function of the player component. It should only be called once on initialization.
	/// </summary>
	public void playerInit()
	{
		isPlayer = true;
		//initAxisCheck();

	}

	private void initAxisCheck()
	{
		testStuff();

		Transform cam = ovrCamController.transform.FindChild("CameraRight");
		wrongAxis = Instantiate(wrongAxis, cam.position, Quaternion.identity) as GameObject;
		wrongAxis.transform.parent = cam;
		wrongAxis.transform.localRotation = Quaternion.Euler(270, 0, 0);
		wrongAxis.transform.position += -wrongAxis.transform.up * 2;
		wrongAxis.SetActive(false);
		Debug.Log("init");
	}

	private void loadAutoPilot()
	{
		//setup autopilot if is player
		if (isPlayer)
		{
			_player = GetComponent<Player>();
			_enemy = GetComponent<Collider>().gameObject.AddComponent<Enemy>();
			_enemy.difficulty = Enemy.AI_Difficulty.Y_Key;
			_enemy.enabled = false;
			//Debug.Log("autopilot loaded");
		}
	}

	public void finishGame()
	{
		enableAutoPilot();
		finished = true;
	}

	public void enableAutoPilot()
	{
		if (_enemy == null) loadAutoPilot();

		_enemy.enabled = true;
		_player.enabled = false;
	}

	public void disableAutoPilot()
	{
		if (_enemy == null) loadAutoPilot();

		_enemy.enabled = false;
		_player.enabled = true;
	}

	public void toggleAutoPilot()
	{
		if (isPlayer)
		{
			if (_enemy == null) loadAutoPilot();

			_enemy.enabled = !_enemy.enabled;
			_player.enabled = !_player.enabled;
		}
	}

	public bool isAutoPilot()
	{
		return (isPlayer && _enemy != null && _enemy.enabled);
	}

	void FixedUpdate ()
	{

		// Update current speed if on the ground
		if (_grounded) speedUpdate();

		// Move vehicle
		goForward();

		// Update particles from the wings
		particleEffectUpdate();

		// Update information on the wheel
		if (isPlayer) hudUpdate();

		// Other updates(No touch)
		sideBoundaryUpdate();
		vehicleFloorAlignment();
		verticalAlignment();
		rotateCar();
		updateBump();
		cameraEffect();

		//autopilot warning
		autoPilotWarning();

		collideVehicle();


		shieldFeedback();


		//Debug.Log(wrongAxis.transform.rotation.eulerAngles);
	}

	//move this function to player
	private void testStuff()
	{
		Quaternion q = Quaternion.identity;
		//OVRDevice.GetOrientation(0, ref q);

		Debug.Log(q);

		float diffX = Mathf.Abs(q.x - _oldQuaternion.x);
		float diffY = Mathf.Abs(q.y - _oldQuaternion.y);
		float diffZ = Mathf.Abs(q.z - _oldQuaternion.z);

		//if the change in orientation is greater for the X and Y axis
		if ((diffX > diffZ || diffY > diffZ) && (diffX > 0.2f || diffY > 0.2f))
		{
			Debug.Log("using wrong axis");
			wrongAxis.SetActive(true);
		}
		else
		{
			Debug.Log("using correct axis");
			wrongAxis.SetActive(false);
		}


		_oldQuaternion = q;

		Invoke("testStuff", 0.5f);
	}

	private void showWrongInput()
	{

	}

	private void hideWrongInput()
	{

	}


	private void shieldFeedback()
	{
		if (shieldObj != null)
		{
			Color c = shieldObj.GetComponent<Renderer>().material.GetColor("_Color");
			if (c.a == 0) return;

			float alpha = c.a - shieldFadeSpeed;
			if (alpha < 0) alpha = 0;

			Color n = new Color(c.r, c.g, c.b, alpha);
			shieldObj.GetComponent<Renderer>().material.color = n;
			//Debug.Log(n);
		}
	}

	private void autoPilotWarning()
	{
		if (isPlayer && isAutoPilot())
		{
			//Debug.Log("checking autopilot message");
			if (!IsInvoking("autoPilotWarningShow") && !IsInvoking("autoPilotWarningHide"))
			{
				//Debug.Log("start autopilot warning showing");
				Invoke("autoPilotWarningShow", .8f);
			}
		}
	}

	private void autoPilotWarningShow()
	{
		if (!IsInvoking("autoPilotWarningHide"))
		{
			//Debug.Log("clearing lap text soon");
			autoPilotText = "Autopilot";
			Invoke("autoPilotWarningHide", .8f);
		}
	}

	private void autoPilotWarningHide()
	{
		autoPilotText = "";
	}

	private void cameraEffect()
	{
		if (!isPlayer) return;

		if (_currentSpeed > DEFAULT_MAX_SPEED)
		{
			float over = DEFAULT_MAX_SPEED - _currentSpeed;
			float val = over/1;

			val = Mathf.Clamp01(Mathf.Abs(val));


			Vector3 adj = new Vector3(0, val*camEffectY, val*camEffectZ);
			ovrCamController.transform.localPosition = ovrCamStartPos + adj;

			ovrCamController.transform.localPosition = Vector3.Slerp(ovrCamController.transform.localPosition, ovrCamStartPos + adj, 0.2f);
		}
		else
		{
			ovrCamController.transform.localPosition = Vector3.Slerp(ovrCamController.transform.localPosition, ovrCamStartPos, 0.2f);
		}
	}

	#region Speed modifiers

	public override void decrSpeed(float amount, float durationInSeconds)
	{
		if (IsInvoking("resetSpeed"))
		{
			CancelInvoke("resetSpeed");
			resetSpeed();
		}

		_currentMaxSpeed -= amount;
		Invoke("resetSpeed", durationInSeconds);

		if (_currentMaxSpeed < 0) _currentMaxSpeed = 0;
	}

	public override void incrSpeed(float amount, float durationInSeconds)
	{
		if (IsInvoking("resetSpeed"))
		{
			CancelInvoke("resetSpeed");
			resetSpeed();
		}

		_currentMaxSpeed += amount;
		Invoke("resetSpeed", durationInSeconds);

		//if (_currentMaxSpeed > ) _currentMaxSpeed = 0;
	}

	protected override void resetSpeed()
	{
		_currentMaxSpeed = DEFAULT_MAX_SPEED;
	}

	public override void decrAcc(float multiplier, float durationInSeconds)
	{
		if (IsInvoking("resetAcc"))
		{
			CancelInvoke("resetAcc");
			resetAcc();
		}

		_currentAccelerationSpeed /= multiplier;
		Invoke("resetAcc", durationInSeconds);
	}

	public override void incrAcc(float multiplier, float durationInSeconds)
	{
		if (IsInvoking("resetAcc"))
		{
			CancelInvoke("resetAcc");
			resetAcc();
		}

		if (_currentAccelerationSpeed == MIN_ACCELERATION_SPEED) _currentAccelerationSpeed *= multiplier;
		Invoke("resetAcc", durationInSeconds);
	}

	protected override void resetAcc()
	{
		_currentAccelerationSpeed = DEFAULT_ACCELERATION_SPEED;
	}

	#endregion

	#region Movement

	//fix harzardous code
	protected override void vehicleFloorAlignment()
	{
		Vector3 orig = transform.position;	//raycast origin
		RaycastHit hitInfo;					//hitinfo of the raycast

		//raycast down from given position to layer 8 (floor)
		if (Physics.Raycast(orig, -transform.up + 0.5f * transform.forward, out hitInfo, RAY_DIST_DOWN - 1, 1<<8))
		{
			_grounded = true;				//is on ground

			//0.08 = good for track, bad for bumps
			//0.04 = bad for track, good for bumps
			float smoothStrength = 0.06f;	//strength to smooth with (0..1), higher = harder bumps

			//rotate to normal
			transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation, smoothStrength);

			//GameObject.Find("HoverCar").gameObject.transform.localPosition += Mathf.Sin(Time.time) * Vector3.up * 0.05f;
			hovercar.Rotate(0.0f, 0.0f, Mathf.Sin(Time.time) * 0.06f);

			//if distance to floor smaller than 4
			if (hitInfo.distance < 4)
			{
				//transform.position = transform.position + transform.up * 0.1f;
				transform.position = transform.position + transform.up * (4 - hitInfo.distance);
			}
			//if distance to floor greater than 5
			if (hitInfo.distance > 5)
			{
				transform.position = transform.position + -transform.up * 0.1f;
			}

			IsInBounds = true;
		}
		else
		{
			_grounded = false;				//is not on ground

			//raycast down while jumping to align rotation while in air
			if (Physics.Raycast(orig, -transform.up + 0.5f * transform.forward, out hitInfo, 200f, 1<<8))
			{
				//rotate to straight
				transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.FromToRotation(transform.up, hitInfo.normal) * transform.rotation, 0.05f);
				IsInBounds = true;
			}
			else
			{
				//Debug.Log("Out of bounds? (floor): " + this.transform.position );
				IsInBounds = false;
			}

			//gravity kicks in
			transform.position = transform.position + -transform.up * GRAVITY_STRENGTH;
		}
	}

	protected void collideVehicle()
	{
		Transform trans = transform;
		Vector3 upVector = trans.up;
		Vector3 rightVector = trans.right;
		Vector3 forwardVector = trans.forward;
		Vector3 position = trans.position;
		Vector3 rayBackPosition = position - upVector*2;
		Vector3 rayBackStartPosition = rayBackPosition + -forwardVector * 2;
		Vector3 rayFrontStartPosition = rayBackPosition + forwardVector * 4;

		Ray frontLeft = new Ray(rayFrontStartPosition, -rightVector);
		Ray backLeft  = new Ray(rayBackStartPosition , -rightVector);

		Ray frontRight = new Ray(rayFrontStartPosition, rightVector);
		Ray backRight =  new Ray(rayBackStartPosition, rightVector);

		Vector3 rightStrength = rightVector*2;

		Debug.DrawRay(rayFrontStartPosition,  rightStrength, VehicleCollisionColor);
		Debug.DrawRay(rayFrontStartPosition, -rightStrength, VehicleCollisionColor);

		Debug.DrawRay(rayBackStartPosition,  rightStrength, VehicleCollisionColor);
		Debug.DrawRay(rayBackStartPosition, -rightStrength, VehicleCollisionColor);


		if (Physics.Raycast(frontLeft, 2.5f, 1 << 11) || Physics.Raycast(backLeft, 2.5f, 1 << 11))
		{
			//collides with other vehicle left side
			bumpLeftWall();
		}
		if (Physics.Raycast(frontRight, 2.5f, 1 << 11) || Physics.Raycast(backRight, 2.5f, 1 << 11))
		{
			//collides with other vehicle right side
			bumpRightWall();
		}
	}


	protected override void verticalAlignment()
	{
		bool hit1;
		bool hit2;
		RaycastHit hitInfo;
		RaycastHit hitInfo2;
		int layer;
		if (!onShortcut)
		{
			layer = 1 << 9;
		}
		else
		{
			layer = 1 << 11;
		}
		Transform trans = transform;
		Vector3 rayOrigin = trans.position - trans.up;
		Vector3 rightDirection = trans.right;

		//raycast to the left from given position to layer 9 (walls)
		hit1 = Physics.Raycast(rayOrigin, -rightDirection, out hitInfo, RAY_DIST_SIDE, layer);
		hit2 = Physics.Raycast(rayOrigin, rightDirection, out hitInfo2, RAY_DIST_SIDE, layer);


		Debug.DrawRay(rayOrigin, -rightDirection * 2, WallCollisionColor);
		Debug.DrawRay(rayOrigin, rightDirection * 2, WallCollisionColor);

		//if left hits and right doesnt OR if both hit and left is closer
		float distanceInfo1 = hitInfo.distance;
		float distanceInfo2 = hitInfo2.distance;
		if ((hit1 && !hit2) || (hit1 && hit2 && distanceInfo1 < distanceInfo2))
		{
			//sets the lookAt to smoothly align the y-rotation
			_lookAt = Quaternion.FromToRotation(rightDirection, hitInfo.normal) * trans.rotation;
			IsInBounds = true;
		}
		//if right hits and left doesnt OR if both hit and right is closer
		else if ((!hit1 && hit2) || (hit1 && hit2 && distanceInfo2 < distanceInfo1))
		{
			_lookAt = Quaternion.FromToRotation(-rightDirection, hitInfo2.normal) * trans.rotation;
			IsInBounds = true;
		}
		//not hitting any sides
		else
		{
			//Debug.Log("Out of bounds? (side)");
			IsInBounds = false;
		}


		lookAtTowardsTo();
	}

	protected override void sideBoundaryUpdate()
	{
		//the min distance of the ray from the side down
		const float MIN_SIDE_DIST = 1.5f;

		//rays from side of vehicle casting down
		Transform trans = transform;
		Vector3 downDirection = -trans.up;
		Vector3 rayOrigin = trans.position + -downDirection;
		Vector3 maxDist = trans.right * Mathf.Max(_currentSideSpeed, MIN_SIDE_DIST);

		Ray leftRay =  new Ray(rayOrigin + -maxDist, downDirection);
		Ray rightRay = new Ray(rayOrigin + maxDist, downDirection);

		leftRayHit = Physics.Raycast(leftRay, RAY_DIST_DOWN, 1 << 8);
		rightRayHit = Physics.Raycast(rightRay, RAY_DIST_DOWN, 1 << 8);

		Debug.DrawRay(rayOrigin + -maxDist, downDirection * 5, FloorCollisionColor);
		Debug.DrawRay(rayOrigin + maxDist, downDirection * 5, FloorCollisionColor);

		if(shield <= 20)
		{
			if (_warning)
			{
				_warning = false;
				switchSound(1);
				_aSource.Play();
			}
			if (shield <= 0)
			{
				if (_warning2)
				{
					_warning2 = false;
					switchSound(2);
					_aSource.Play();
				}
			}
		}

		if (!leftRayHit && _grounded)
		{
			if (_canGoLeft)
			{
				drainShield(SHIELD_DAMAGE);

				if (shield > 20) switchSound(0);
				else
				{
					if (!_aSource.isPlaying)
					{
						if (Random.value > 0.5) switchSound(4);
						else switchSound(5);
					}
				}
				_aSource.Play();

				//disable movement to the left and enable it in 0.5 second
				_canGoLeft = false;
				Invoke("enableGoLeft", 0.5f);

				if (shield == 0) _currentSpeed *= 0.5f;
				if (_currentSpeed < 0.01) _currentSpeed = 0;
				//Reduce the speed of the hover car
			}

			//move slighty to right
			bumpLeftWall();
		}
		else if (!rightRayHit && _grounded)
		{
			if (_canGoRight)
			{
				drainShield(SHIELD_DAMAGE);

				if (shield > 20) switchSound(0);
				else
				{
					if (!_aSource.isPlaying)
					{
						if (Random.value > 0.5) switchSound(4);
						else switchSound(5);
					}
				}
				_aSource.Play();

				//disable movement to the right and enable it in 0.5 second
				_canGoRight = false;
				Invoke("enableGoRight", 0.5f);

				if (shield == 0) _currentSpeed *= 0.5f;
				if (_currentSpeed < 0.01) _currentSpeed = 0;
			}

			//move slighty to left
			bumpRightWall();
		}
	}

	private void bumpLeftWall()
	{
		_bumpState = BumpState.Left;
		bumpStrength = _currentSideSpeed;
		showShield();
	}
	private void bumpRightWall()
	{
		_bumpState = BumpState.Right;
		bumpStrength = _currentSideSpeed;
		showShield();
	}

	private void showShield()
	{
		if (shieldObj == null || shield <= 0) return;
		Color c = shieldObj.GetComponent<Renderer>().material.color;
		Color n = new Color(c.r, c.g, c.b, 1);
		shieldObj.GetComponent<Renderer>().material.color = n;
	}

	private void updateBump()
	{
		if (_bumpState != BumpState.None)
		{
			switch (_bumpState)
			{
			case BumpState.Left:
				goRight(bumpStrength * 0.3f);
				break;
			case BumpState.Right:
				goLeft(bumpStrength * 0.3f);
				break;
			}
			bumpStrength *= 0.95f;

			if (bumpStrength < 0.5f)
			{
				_bumpState = BumpState.None;
			}
		}
	}

	protected override void speedUpdate()
	{

		// Acceleration speed should grow over time to a certain degree

		_currentSpeed += _currentAccelerationSpeed;

		float normalizedSpeed = (_currentSpeed  / _currentMaxSpeed);

		//reverse the 0..1 value
		normalizedSpeed = 1 - normalizedSpeed;

		//v=(max-min)*t+min
		_currentSideSpeed = (MAX_SIDE_SPEED - MIN_SIDE_SPEED) * normalizedSpeed + MIN_SIDE_SPEED;

		//limit max sideway speed
		if (_currentSideSpeed > MAX_SIDE_SPEED)
		{
			_currentSideSpeed = MAX_SIDE_SPEED;
			_currentSideSpeed *= FRICTION;
		}

		//limit min sideway speed
		if (_currentSideSpeed < MIN_SIDE_SPEED)
		{
			_currentSideSpeed = MIN_SIDE_SPEED;
		}

		if (_currentSpeed > _currentMaxSpeed)
		{
			_currentSpeed *= FRICTION;		//speed is affected by friction (0.9f)
		}

	}


	#region Horizontal movement
	public override void goLeft(float amount) //0..1
	{
		if (_grounded && leftRayHit && _canGoLeft)
		{
			if(!_aSource.isPlaying)
			{
				switchSound(3);
				_aSource.Play();
			}
			Transform trans = transform;
			trans.position += -trans.right * _currentSideSpeed * amount;
			rotateCarLeft();
			sideBoundaryUpdate();
		}
	}

	private void enableGoLeft()
	{
		_canGoLeft = true;
	}

	public override void goRight(float amount) //0..1
	{
		if (_grounded && rightRayHit && _canGoRight)
		{
			if (!_aSource.isPlaying)
			{
				switchSound(3);
				_aSource.Play();
			}
			Transform trans = transform;
			trans.position += trans.right * _currentSideSpeed * amount;
			rotateCarRight();
			sideBoundaryUpdate();
		}
	}

	private void enableGoRight()
	{
		_canGoRight = true;
	}

	#endregion

	protected override void goForward()
	{
		Transform trans = transform;
		trans.position += trans.forward * (_currentSpeed + _incrSpeed);
	}

	private void rotateCar()
	{
		hovercar.localRotation = Quaternion.Euler(0.0f,0.0f,carRotation);
		carRotation *= 0.92f;
	}

	private void rotateCarLeft()
	{
		carRotation += carRotationSpeed;
	}

	private void rotateCarRight()
	{
		carRotation -= carRotationSpeed;
	}

	protected override void lookAtTowardsTo()
	{
		if (_lookAt != Quaternion.identity)
		{
			//smooth strength
			float smoothStrength = 0.08f;

			//smooths to given rotation
			Quaternion rotation = transform.rotation;
			transform.rotation = Quaternion.Euler(rotation.eulerAngles.x, Quaternion.Slerp(rotation, _lookAt, smoothStrength).eulerAngles.y, rotation.eulerAngles.z);
		}
	}

	#endregion

	#region Modifiers

	public override void incrShield(int amount)
	{
		shield += amount;
		if (shield > MAX_SHIELD) shield = MAX_SHIELD;
	}

	protected override void pointsToShield(int damage)
	{
		pPoints -= damage;
		if (pPoints < 0) pPoints = 0;
	}

	public override void drainShield(int damage)
	{
		shield -= damage;
		if (shield <= 0)
		{
			int temp = Mathf.Abs(shield);
			pointsToShield(temp);
			shield = 0;
		}
	}

	#endregion

	public void bounceAwayFrom(Transform obj)
	{
		//check stuff
		float dist = calcDist(obj);

		//Debug.Log(dist);
		//if obj is to the right and in of pickup range
		if (dist < 0)
		{
			//go left
			bumpRightWall();
		}
		//if obj is to the left and in of pickup range
		else if (dist > 0)
		{
			//go right
			bumpLeftWall();
		}
	}

	/// <summary>
	///	calc distance with X and Z value
	/// </summary>
	/// <param name="obj">obj to compare dist with</param>
	/// <returns>returns a float, negative for left, positive for right</returns>
	public float calcDist(Transform obj)
	{
		//right
		Transform trans = transform;
		Vector3 a = (trans.position + trans.right);
		Vector3 objPosition = obj.position;
		float distX1 = a.x - objPosition.x;
		float distZ1 = a.z - objPosition.z;


		//left
		Vector3 b = (trans.position - trans.right);
		float distX2 = b.x - objPosition.x;
		float distZ2 = b.z - objPosition.z;


		return
			(distX1 * distX1 + distZ1 * distZ1) -
			(distX2 * distX2 + distZ2 * distZ2);
	}

	private void particleEffectUpdate()
	{
		if (particleSystemWind != null)
		{
			float nAlpha = 0.0f;
			if (currentSpeed > 0.65f) nAlpha = currentSpeed / _currentMaxSpeed;
			if (nAlpha > 0.3f) nAlpha = 0.3f;
			_particleSystemWind.startColor = new Color(1.0f, 1.0f, 1.0f, nAlpha);

			float startSize = 0;
			float color = 0f;
			float emisRate = 0f;

			if (currentSpeed > 1f)
			{
				float norm = currentSpeed / 1f;
				//if speedboosted
				startSize = .75f * norm;
				color = .9f * norm;
				emisRate = 110f * norm;
			}
			else if (currentSpeed > 0.9f)
			{
				float norm = currentSpeed / .9f;
				//if near max speed
				startSize = .6f * norm;
				color = .6f * norm;
				emisRate = 100f * norm;

				if (startSize > .75f) startSize = .75f;
				if (color > .9f) color = .9f;
				if (emisRate > 110) emisRate = 110;
			}
			else if (currentSpeed > 0.2f)
			{
				float norm = currentSpeed / .2f;

				//if riding
				startSize = .45f * norm;
				color = .2f * norm;
				emisRate = 70f * norm;

				if (startSize > .6f) startSize = .6f;
				if (color > .6f) color = .6f;
				if (emisRate > 100) emisRate = 100;

			}

			particleSystemLeftWing.startSize += (startSize - particleSystemLeftWing.startSize) * 0.1f;
			particleSystemRightWing.startSize += (startSize - particleSystemRightWing.startSize) * 0.1f;

			particleSystemLeftWing.startColor = new Color(particleSystemLeftWing.startColor.r, particleSystemLeftWing.startColor.g, particleSystemLeftWing.startColor.b, color);
			particleSystemRightWing.startColor = new Color(particleSystemLeftWing.startColor.r, particleSystemLeftWing.startColor.g, particleSystemLeftWing.startColor.b, color);

			particleSystemLeftWing.emissionRate += (emisRate - particleSystemLeftWing.emissionRate) * 0.1f;
			particleSystemRightWing.emissionRate += (emisRate - particleSystemRightWing.emissionRate) * 0.1f;
		}
	}

	private void hudUpdate()
	{
		if (wheelHolo != null)
		{
			float value = (float)shield / (float)MAX_SHIELD;

			wheelHolo.GetComponent<WheelHolo>().shieldCharge = value;
			wheelHolo.GetComponent<WheelHolo>().currentSpeed = 1-(currentSpeed / 1.3f);
		}
	}

	private void switchSound(int temp)
	{
		switch (temp)
		{
		case 0:
			_aSource.clip = collisionSound;  // Normal collision sound
			break;
		case 1:
			_aSource.clip = warningSound; 	// Alert sound when less then 20 % of shield
			break;
		case 2:
			_aSource.clip = warningSound2; 	// Danger sound when 0 % shield
			break;
		case 3:
			_aSource.clip = sideMoveSound;
			break;
		case 4:
			_aSource.clip = collisionSound2;
			break;
		case 5:
			_aSource.clip = collisionSound3;
			break;
		}
	}

	public void warpToCheckpoint(int number)
	{
		GameObject checkpoint = GameObject.Find("Checkpoint_" + number);
		if (checkpoint != null)
		{
			transform.position = checkpoint.transform.position;
			transform.rotation = checkpoint.transform.rotation;
		}
	}
}
