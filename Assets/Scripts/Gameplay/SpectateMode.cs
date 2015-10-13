using UnityEngine;
using System.Collections;

public class SpectateMode : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GameObject obj = GameObject.Find("enemy");
		if (obj != null) Destroy(obj);
		GameObject player = GameObject.Find("player");
		if (player != null)
		{
			Destroy(player.GetComponent<Player>());//.enabled = false;
			Enemy enemy = player.AddComponent<Enemy>();
			enemy.difficulty = Enemy.AI_Difficulty.Normal;
		}
	}
}
