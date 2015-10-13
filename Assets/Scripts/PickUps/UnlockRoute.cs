using UnityEngine;
using System.Collections;

public class UnlockRoute : Pickup {
	public int feeAmount;
	public float reactivateIn;
	public GameObject[] objectsToAffect;
	public AudioClip pickUpSound;
	private AudioSource aSource;
	
	void Start()
	{
		aSource = gameObject.AddComponent<AudioSource>();
		if (pickUpSound != null) aSource.clip = pickUpSound;
	}
	
	void OnTriggerEnter(Collider obj)
	{
		if (_active)
		{
			Vehicle vehicleComp = obj.gameObject.GetComponent<Vehicle>();
			
			if(vehicleComp != null)	
			{
				if (!(vehicleComp.pPoints - feeAmount < 0))
				{
					aSource.Play();
					_active = false;
					Invoke("activateTrigger", reactivateIn);
					offMaterial();
					
					vehicleComp.pPoints -= feeAmount;

					//go through all objects, change their stuff
					for (int i = 0; i < objectsToAffect.Length; i++)
					{
						objectsToAffect[i].GetComponent<Changer>().change();
					}

					Log.getInstance().addPickupLine(vehicleComp.id, obj.transform, this.transform, feeAmount);
				}
			}
			
		}
	}



}
