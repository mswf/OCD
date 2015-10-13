using UnityEngine;
using System.Collections;

public class Shieldboost : Pickup {
	
	public int boostShieldValue = 10;
	public float reactivateIn = 5f;
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
				offMaterial();
				Invoke("activateTrigger", reactivateIn);
				vehicleComp.incrShield(boostShieldValue);
				Log.getInstance().addPickupLine(vehicleComp.id, obj.transform, this.transform, boostShieldValue);
			}
		}
	}

}
