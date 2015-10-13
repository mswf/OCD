using System.Collections;
using UnityEngine;
using System;

public class Calibration : MonoBehaviour
{

	public enum State
	{
		none,
		neutral,
		axis,
		right,
		left,
		done
	}
	
	private Quaternion _leftOrientation;
	private Quaternion _rightOrientation;
	
	private Axis _axis;
	public 	Axis axis
	{
		get
		{
			return _axis;
		}
		
		set
		{
			_axis = value;
		}
	}
	
	private State _state;
	public State state
	{
		get
		{
			return _state;
		}
		
		set
		{
			_state = value;
		}
	}
	
	
	public Quaternion leftOrientation
	{
		get
		{
			return _leftOrientation;
		}
	}
	public Quaternion rightOrientation
	{
		get
		{
			return _rightOrientation;
		}
	}
	
	void Update()
	{
		Quaternion q = WeikiesRiftHack.GetOrientation();
		//OVRDevice.GetOrientation(0, ref q);
		//OVRDevice.OrientSensor(ref q);
		
		switch (_state)
		{
		case State.neutral:
			WeikiesRiftHack.ResetOrientation();
			break;
			
		case State.axis:
			break;
			
		case State.right:
			_rightOrientation = q;
			break;
			
		case State.left:
			_leftOrientation = q;
			break;
			
		case State.done:
		
			break;
			
		case State.none:
			break;
			
		default:
			throw new Exception("SOMETHING WENT VERY WRONG IN CALIBRATION CLASS");
		}
	}
}
