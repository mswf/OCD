using UnityEngine;
using System.Collections;

public class Adscript : MonoBehaviour {
	
	public Texture[] texList;
	public int changePictureIn;
	public int nextSlide;
	private int timer;
	private int i = 0;
	private bool blending = true;
	private float blend = 0.0f;
	private	int temp1;
	private	int temp2;
	
	void Update () 
	{
		getValue(blending, changePictureIn);
		this.gameObject.GetComponent<Renderer>().material.SetFloat("_Blend", blend);
		
		timer++;
		if (nextSlide <= timer)
		{
			blending = true;
			timer = 0;
		}
		
	}
	
	void changePicture()
	{
		
		i++;
	
		if (i < texList.Length) 
		{
			this.gameObject.GetComponent<Renderer>().material.SetTexture("_BaseTexture", texList[i]);
			if (i + 1 < texList.Length) this.gameObject.GetComponent<Renderer>().material.SetTexture("_OverlayTexture", texList[i+1]);
			else this.gameObject.GetComponent<Renderer>().material.SetTexture("_OverlayTexture", texList[1]);
		}
		else 
		{
			i = 0;
			this.gameObject.GetComponent<Renderer>().material.SetTexture("_BaseTexture", texList[i]);
			this.gameObject.GetComponent<Renderer>().material.SetTexture("_OverlayTexture", texList[i+1]);
		}		
	}
	
	private float getValue(bool active, int frames)
	{
		if (!active)
		{
	    	blend = 0f;
		}
		else
		{
			blend += 1f/frames;
			if (blend > 1) 
			{
				blend = 1f;
				blending = false;
				blend = 0f;
				changePicture();
			}
		}
		return blend;
	}
}

