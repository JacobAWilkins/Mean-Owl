using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

	public static GameControl gameControl;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public int spawnDelay = 2;

	void Start () {
		if (gameControl == null) {
			gameControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControl>();
		}
	}

	public IEnumerator respawnPlayer () {
		yield return new WaitForSeconds (spawnDelay);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
	}


	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
		gameControl.StartCoroutine (gameControl.respawnPlayer ());
	}

}
