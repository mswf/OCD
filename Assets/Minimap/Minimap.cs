using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour 
{

	public GameObject followedObject;
	public Color canvasColor = Color.clear;
	public Color marker = Color.red;
	public Texture2D miniMapTexture;
	private Texture2D texture;
	public int width;
	public int height;

	void Start () 
	{
		texture = miniMapTexture;//new Texture2D(256, 256, TextureFormat.ARGB32, false);

		GetComponent<Renderer>().material.mainTexture = texture;
		//ResetTexture();
	}

	void Update () 
	{
		//ResetTexture();
		texture = miniMapTexture;

		if (followedObject != null)
		{
			int x = Mathf.Abs((int)followedObject.transform.position.x);
			int y = Mathf.Abs((int)followedObject.transform.position.z);

			int x_square = x + width;
			int y_square = y + height;

			Debug.Log("Width: " + texture.width.ToString());
			Debug.Log("Brush: " + x_square.ToString());

			if ( texture.width >= x_square && texture.height >= y_square )
			{

				Color[] colors = new Color[width*height];
				
				for (int i = 0; i < width*height; i++) 
				{
					colors[i] = Color.red;
				}

				texture.SetPixels(x, y, width, height, colors);
				texture.Apply();
			}
		}
	}

	void ResetTexture()
	{

		int y = 0;
		while (y < texture.height) 
		{
			int x = 0;
			while (x < texture.width) 
			{
				texture.SetPixel(x, y, canvasColor);
				++x;
			}
			++y;
		}
		texture.Apply();
	}
}
