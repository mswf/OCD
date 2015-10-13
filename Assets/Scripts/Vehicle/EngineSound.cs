using UnityEngine;
using System.Collections;

public class EngineSound : MonoBehaviour 
{
	
	public GameObject[]		audioSources;
	public float			engineStartPitch;
	public float			turbineStartPitch;
	private Vehicle			vehicle;
	
	
	void Awake()
	{
		vehicle = gameObject.GetComponent<Vehicle>();
	}
	
	void Start()
	{
		audioSources[0].GetComponent<AudioSource>().enabled = true;
		audioSources[1].GetComponent<AudioSource>().enabled = true;
		//audioSources[0].GetComponent<AudioSource>().Stop();
		audioSources[0].GetComponent<AudioSource>().pitch = engineStartPitch;
		audioSources[0].GetComponent<AudioSource>().volume = 1f;
		audioSources[1].GetComponent<AudioSource>().pitch = turbineStartPitch;
	}
	
	void Update () 
	{	
		float speed = vehicle.currentSpeed;
		
		//if (speed >= 0.00001f) speed = 0.01f;
		
		float engineValue = Mathf.Abs(speed / vehicle.maxSpeed );// * 0.5f + 0.3f ;// 0.3 >> 0.8
		float turbineValue = Mathf.Abs(speed  / 1.0f);//vehicle.maxSpeed );
		
		if (!float.IsNaN(engineValue)) audioSources[0].GetComponent<AudioSource>().pitch = engineValue;
		audioSources[1].GetComponent<AudioSource>().pitch = turbineValue;
	}
}
