using UnityEngine;
using System.Collections;

public class BlinkingSymbol : MonoBehaviour
{
	private int timer = 0;
	private OculusMenu.Label label;
	[SerializeField]
	private string _symbol;
	public bool isBlinking = false;
	
	private void Start()
	{
		label = GetComponent<OculusMenu.Label>();
		StartBlinking();
	}
	
	private IEnumerator Blink()
	{
		while (true)
		{
			if (timer >= 5)
			{
				if (label.GetText().StartsWith(_symbol))
				{
					label.SetText( label.GetText().Remove(0,1) );
				}
				else if (!label.GetText().StartsWith(_symbol))
				{
					label.SetText( _symbol + label.GetText() );
				}
				timer = 0;
			}
			else timer++;
			yield return new WaitForSeconds(0.1f);
		}
	}

	public void StopBlinking()
	{
		StopCoroutine("Blink");
	}

	public void StartBlinking()
	{
		StartCoroutine(Blink());
	}
	
	public void SetSymbol(string symbol)
	{
		_symbol = symbol;
	}
}
