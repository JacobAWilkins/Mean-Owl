using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

	public enum SpawnState {
		SPAWNING,
		WAITING,
		COUNTING
	};

	[System.Serializable]
	public class Wave {
		public string name;
		public Transform enemy;
		public int count;
		public float rate;
	}

	public Wave [] waves;
	public Transform[] spawnPoints;
	private SpawnState spawnState = SpawnState.COUNTING;
	private int nextWave = 0;
	public float timeBetweenWaves = 5f;
	float waveCountdown;
	float searchCountdown = 1f;

	void Start () {
		waveCountdown = timeBetweenWaves;
	}

	void Update () {
		if (spawnState == SpawnState.WAITING) {
			if (!EnemyIsAlive ()) {
				WaveCompleted ();
			}
			else {
				return;
			}
		}

		if (waveCountdown <= 0) {
			if (spawnState != SpawnState.SPAWNING) {
				StartCoroutine (SpawnWave (waves [nextWave]));
			}
		}
		else {
			waveCountdown -= Time.deltaTime;
		}
	}

	void WaveCompleted () {
		spawnState = SpawnState.COUNTING;
		waveCountdown = timeBetweenWaves;
		if (nextWave + 1 > waves.Length - 1) {
			nextWave = 0;
		}
		else {
			nextWave++;
		}
	}

	bool EnemyIsAlive () {
		searchCountdown -= Time.deltaTime;
		if (searchCountdown <= 0f) {
			searchCountdown = 1f;
			if (GameObject.FindGameObjectWithTag("Enemy") == null) {
				return false;
			}
		}
		return true;
	}

	IEnumerator SpawnWave (Wave _wave) {
		spawnState = SpawnState.SPAWNING;

		for (int i = 0; i < _wave.count; i++) {
			SpawnEnemy (_wave.enemy);
			yield return new WaitForSeconds (1f / _wave.rate);
		}

		spawnState = SpawnState.WAITING;

		yield break;
	}

	void SpawnEnemy (Transform _enemy) {
		Transform _spawnPoint = spawnPoints [Random.Range (0, spawnPoints.Length)];
		Instantiate (_enemy, _spawnPoint.position, _spawnPoint.rotation);
	}

}
