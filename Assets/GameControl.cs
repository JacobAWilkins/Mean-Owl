using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

	public static GameControl gameControl;

	void Start () {
		if (gameControl == null) {
			gameControl = GameObject.FindGameObjectWithTag ("GameController");
	}

	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
	}

}
