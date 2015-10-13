using UnityEngine;
using System.Collections;

public class Speedboost : Pickup {
	
	public enum Type 
	{
		Increase,
		Decrease
	}
	
	public Type boostType;
	public float boostDuration = 5f;
	public float boostMaxSpeedValue = 10f;
	public float reactivateIn = 5f;
	public float accelerationMultiplier = 1f;
	public AudioClip pickUpSound;
	
	void Start()
	{
		if (pickUpSound != null) gameObject.GetComponent<AudioSource>().clip = pickUpSound; 
	}
	
	void OnTriggerEnter(Collider obj)
	{
		
		if (_active)
		{
			Vehicle vehicleComp = obj.gameObject.GetComponent<Vehicle>();
		
			if(vehicleComp != null)
			{
				GetComponent<AudioSource>().Play();
				_active = false;
				Invoke("activateTrigger", reactivateIn);
				offMaterial();
				
				switch (boostType)
				{
					case Type.Increase:
						vehicleComp.incrAcc(accelerationMultiplier, boostDuration);
						vehicleComp.incrSpeed(boostMaxSpeedValue, boostDuration);
						Log.getInstance().addPickupLine(vehicleComp.id, obj.transform, this.transform, boostMaxSpeedValue);
					break;
				
					case Type.Decrease:
						vehicleComp.decrAcc(accelerationMultiplier, boostDuration);
						vehicleComp.decrSpeed(boostMaxSpeedValue, boostDuration);
					break;
				}
			}
		}
	}
	
}
