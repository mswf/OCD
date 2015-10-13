using UnityEngine;
using System.Collections;

public class PowerPointboost : Pickup {
	
	public int pointAmount;
	public float reactivateIn;
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
				vehicleComp.pPoints += pointAmount;
				Log.getInstance().addPickupLine(vehicleComp.id, obj.transform, this.transform, pointAmount);
			}
			
		}
	}
}
