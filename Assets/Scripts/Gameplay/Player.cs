using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour
{

	private Vehicle _vehicle;

	void Start ()
	{
		_vehicle = GetComponent<Vehicle>();
		_vehicle.playerInit();
		//_vehicle.isPlayer = true;
	}

	void Update ()
	{
		float valueH = DeviceInput.valueH;


		if (valueH < -0.1f) _vehicle.goLeft(-valueH);

		if (valueH > 0.1f) _vehicle.goRight(valueH);

		if (Input.GetKeyDown(KeyCode.F))
		{
			WeikiesRiftHack.ToggleInvert();
		}
	}
}
