using UnityEngine;
using System.Collections;

public class WheelHolo : MonoBehaviour {
	public Renderer ShieldBar;
	public Renderer PowerBar;
	public Renderer SpeedGauge;
	
	private float minPos = -0.5f;
	private float maxPos = -1.4f;
	
	private float _shieldCharge = 1;
	
	private float _currentSpeed = 1f;
	
	public float shieldCharge {
		set
		{
			_shieldCharge = value;
			ShieldBar.material.SetTextureOffset("_MainTex", new Vector2(0, minPos - ((minPos - maxPos) * _shieldCharge)));
			PowerBar.material.SetTextureOffset("_MainTex", new Vector2(0, minPos - ((minPos - maxPos) * _shieldCharge)));
		}
	}
	
	public float currentSpeed
	{
		set
		{
			_currentSpeed = value;
			SpeedGauge.sharedMaterial.SetTextureOffset("_MainTex", new Vector2 ((_currentSpeed),0));
		}
	}
	
	void Start()
	{
		
		currentSpeed = 1f;
	}
	
}
