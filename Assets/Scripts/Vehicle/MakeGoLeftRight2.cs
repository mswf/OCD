using UnityEngine;
using System.Collections;

public class MakeGoLeftRight2 : MonoBehaviour
{

	public Transform rightWing;
	public Transform leftWing;
	public Transform steeringWheel;

	public float wingRotation = .7f;
	public float wingRotationSpeed = .1f;

	

	void RotateWings()
	{
		rightWing.localRotation = Quaternion.Euler(0, 0, wingRotation);
		leftWing.localRotation = Quaternion.Euler(0, 0, wingRotation);

		steeringWheel.localRotation = Quaternion.Euler(0, 0, -wingRotation*1.5f);

		wingRotation *= .8f;
	}

	void FixedUpdate()
	{
		
		float valueH = DeviceInput.valueH;
		wingRotation += -valueH * (wingRotationSpeed*2.5f);
		/*
		if (valueH < -0.0001f)
		{
			wingRotation += wingRotationSpeed;
		}
		
		if (valueH > 0.0001f)
		{
			wingRotation -= wingRotationSpeed;			
		}*/
		
		RotateWings();
	}
}
