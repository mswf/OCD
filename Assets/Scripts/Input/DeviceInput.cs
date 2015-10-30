using UnityEngine;
using System.Collections;

public class DeviceInput : MonoBehaviour
{

	public enum Axis
	{
		x,
		y,
		z
	}

	public Axis axis;
	public GameObject cameraRig;
	private OVRCameraRig rig;
	private Profile currProfile = new Profile();
	private float _leftMiddle;
	private float _rightMiddle;
	private float _middle;
	private bool inverted = false;

	static public float valueH
	{
		get
		{
			return _orientation;
		}
	}
	static private float _orientation = 0;
	static private bool _isVRControlsDisabled = false;
	static public bool isDisabled
	{
		get
		{
			return _isVRControlsDisabled ;
		}

		set
		{
			_isVRControlsDisabled = value;
		}
	}

	void Start()
	{
		calcDeadZone();
		string stringA = PlayerPrefs.GetString("Axis");

		switch (stringA)
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

		default:
			axis = Axis.x;
			break;
		}

		rig = cameraRig.GetComponent<OVRCameraRig>();

		WeikiesRiftHack.ResetOrientation();
	}


	void Update()
	{
		if (Input.GetKeyDown(KeyCode.O))
		{
			_isVRControlsDisabled = !_isVRControlsDisabled;
		}

		if (Input.GetKeyDown(KeyCode.F))
		{
			inverted = !inverted;
		}
	}

	void FixedUpdate()
	{
		_orientation = 0;
		float multiplier = 3f;

		//OVRDevice.SensorCount > 0
		if (WeikiesRiftHack.IsConnected() && !_isVRControlsDisabled)
		{
			//Quaternion q = WeikiesRiftHack.GetOrientation();

			Quaternion q = rig.centerEyeAnchor.rotation;
			if (inverted)
			{
				q = new Quaternion(-q.x, -q.y, -q.z, q.w);
			}

			switch (axis)
			{
			case Axis.x:
				if (q.x < (_leftMiddle))
				{
					_orientation = q.x - _middle;
				}

				else if (q.x > (_rightMiddle))
				{
					_orientation = q.x + _middle;
				}

				break;

			case Axis.y:
				if (q.y < (_leftMiddle))
				{
					_orientation = q.y - _middle;
				}

				else if (q.y > (_rightMiddle))
				{
					_orientation = q.y + _middle;
				}

				break;

			case Axis.z:
				if (q.z < (_leftMiddle))
				{
					_orientation = -q.z - _middle;
				}

				else if (q.z > (_rightMiddle))
				{
					_orientation = -q.z + _middle;
				}

				break;

			default:
				break;
			}
		}

		float inputX = Input.GetAxis("Horizontal");

		//controller and keyboard
		if (inputX > 0.1f || inputX < -0.1f)
		{
			const float controllerDeadzone = 0.2f;

			//if not in deadzone, set to controller value
			if (inputX < -controllerDeadzone)
			{
				_orientation = inputX + controllerDeadzone;
			}

			else if (inputX > controllerDeadzone)
			{
				_orientation = inputX - controllerDeadzone;
			}

			//limit different
			if (_orientation < -0.6f)
			{
				_orientation = -0.6f;
			}

			else if (_orientation > 0.6f)
			{
				_orientation = 0.6f;
			}
		}

		else
		{
			inputX = Input.GetAxis("KeyboardSide");

			if (inputX > 0.1f || inputX < -0.1f)
			{
				_orientation = inputX;
			}
		}

		_orientation *= multiplier;

		//limit orientation
		if (_orientation < -1.0f)
		{
			_orientation = -1.0f;
		}

		else if (_orientation > 1.0f)
		{
			_orientation = 1.0f;
		}

		//Debug.Log(_orientation);

	}

	void calcDeadZone()
	{
		float _left = 0.0f;
		float _right = 0.0f;

		if (axis == Axis.x)
		{
			_left = currProfile.getLeft().x;
			_right = currProfile.getRight().x;
		}

		if (axis == Axis.y)
		{
			_left = currProfile.getLeft().y;
			_right = currProfile.getRight().y;
		}

		if (axis == Axis.z)
		{
			_left = currProfile.getLeft().z;
			_right = currProfile.getRight().z;
		}

		float c = Mathf.Abs(_left) + Mathf.Abs(_right);
		_middle = _left + (c / 2);
		c = c * 0.1f; //percentage

		_leftMiddle = _middle - c;
		_rightMiddle = _middle + c;


		if (_leftMiddle == 0 && _rightMiddle == 0)
		{
			Debug.Log("calibration not loaded");
			//set default
			_leftMiddle = -0.1f;
			_rightMiddle = 0.1f;
		}
	}
}
