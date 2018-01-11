using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControl : MonoBehaviour {

	public static GameControl gameControl;
	public Transform playerPrefab;
	public Transform spawnPoint;
	public Transform spawnPrefab;
	public int spawnDelay = 2;
	public string spawnSoundName;
	public string gunCock;
	public CameraShake cameraShake;

	private AudioManager audioManager;

	void Awake () {
		if (gameControl == null) {
			gameControl = GameObject.FindGameObjectWithTag ("GameController").GetComponent<GameControl>();
		}
		audioManager = AudioManager.instance;
	}

	public IEnumerator _respawnPlayer () {
		audioManager.PlaySound (spawnSoundName);
		yield return new WaitForSeconds (spawnDelay);
		audioManager.PlaySound (gunCock);
		Instantiate (playerPrefab, spawnPoint.position, spawnPoint.rotation);
		Transform clone = Instantiate (spawnPrefab, spawnPoint.position, spawnPoint.rotation);
		Destroy (clone.gameObject, 3f);
	}


	public static void KillPlayer (Player player) {
		Destroy (player.gameObject);
		gameControl.StartCoroutine (gameControl._respawnPlayer ());
	}

	public static void KillEnemy (Enemy enemy) {
		gameControl._KillEnemy (enemy);
	}
	public void _KillEnemy (Enemy _enemy) {
		Transform clone = Instantiate (_enemy.deathParticles, _enemy.transform.position, Quaternion.identity) as Transform;
		Destroy (clone.gameObject, 5f);
		cameraShake.Shake (_enemy.shakeAmount, _enemy.shakeLength);
		Destroy (_enemy.gameObject);
	}

}
