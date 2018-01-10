using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	[System.Serializable]
	public class PlayerStats {
		public int maxHealth = 100;
		private int _currentHealth;
		public int currentHealth {
			get { return _currentHealth; }
			set { _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public void Init () {
			currentHealth = maxHealth;
		}
	}

	public PlayerStats playerStats = new PlayerStats ();
	public int fallBoundary = -20;

	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start () {
		playerStats.Init ();

		if (statusIndicator != null) {
			statusIndicator.SetHealth (playerStats.currentHealth, playerStats.maxHealth);
		}
	}

	void Update () {
		if (transform.position.y <= fallBoundary) {
			DamagePlayer (9999999);
		}
	}

	public void DamagePlayer (int damage) {
		playerStats.currentHealth -= damage;
		if (playerStats.currentHealth <= 0) {
			GameControl.KillPlayer (this);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth (playerStats.currentHealth, playerStats.maxHealth);
		}
	}

}