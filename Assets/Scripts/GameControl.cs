using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

	public static GameControl gameControl;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public Transform spawnPrefab;
	public int spawnDelay = 2;

	void Start () {
		if (gameControl == null) {
			gameControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControl>();
		}
	}

	public IEnumerator respawnPlayer () {
		GetComponent<AudioSource>().Play ();
		yield return new WaitForSeconds (spawnDelay);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy (clone.gameObject, 3f);
	}


	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
		gameControl.StartCoroutine (gameControl.respawnPlayer ());
	}

}
