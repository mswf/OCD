using UnityEngine;
using System.Collections;

public class Pickup : MonoBehaviour {
	
	public float dist = 1;
	public GameObject ring;
	public GameObject sign;
	public Material onMat;
	public Material offMat;
	private Color onL;
	private Color offL;
	
	protected bool _active = true;
	
	void Awake()
	{
		onL = sign.GetComponent<Light>().color;
	}
	/*
	
	void FixedUpdate () 
	{
		if(sign.GetComponent<Renderer>().isVisible)
		{
			float pulseBase = Mathf.Sin (2f*Mathf.PI * 1f * Time.time);
			float pulseBaseRot = Mathf.Sin (2f*Mathf.PI * .5f * Time.time);
			
			if (sign != null) 
			{
				sign.transform.localRotation = Quaternion.Euler (270f,0,pulseBaseRot*20f);
				sign.transform.localPosition = new Vector3 (0,1f* pulseBase+ 3f,0);
			}
		}

	}
		 * */
	protected void onMaterial()
	{
		ring.GetComponent<Renderer>().material = onMat;
		sign.GetComponent<Renderer>().material = onMat;
		onLight();
	}
	
	protected void offMaterial()
	{
		ring.GetComponent<Renderer>().material = offMat;
		sign.GetComponent<Renderer>().material = offMat;
		offLight();
	}
	
	protected void onLight()
	{
		sign.GetComponent<Light>().color = onL;
	}
	
	protected void offLight()
	{
		sign.GetComponent<Light>().color = new Color(0.0f, 0.0f, 0.0f, 1.0f);
	}
	
	protected void activateTrigger()
	{
		_active = true;
		onMaterial();
	}
}
