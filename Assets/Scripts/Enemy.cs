using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {

	[System.Serializable]
	public class EnemyStats {
		public int maxHealth = 100;
		public int damage = 40;
		private int _currentHealth;

		public int currentHealth {
			get { return _currentHealth; }
			set { _currentHealth = Mathf.Clamp (value, 0, maxHealth); }
		}

		public void Init () {
			currentHealth = maxHealth;
		}
	}

	public EnemyStats enemyStats = new EnemyStats ();
	public Transform deathParticles;

	public float shakeAmount = 0.1f;
	public float shakeLength = 0.1f;

	[SerializeField]
	private StatusIndicator statusIndicator;

	void Start () {
		enemyStats.Init ();

		if (statusIndicator != null) {
			statusIndicator.SetHealth (enemyStats.currentHealth, enemyStats.maxHealth);
		}
	}

	public void DamageEnemy (int damage) {
		enemyStats.currentHealth -= damage;

		if (enemyStats.currentHealth <= 0) {
			GameControl.KillEnemy (this);
		}

		if (statusIndicator != null) {
			statusIndicator.SetHealth (enemyStats.currentHealth, enemyStats.maxHealth);
		}
	}

	void OnCollisionEnter2D (Collision2D _colliderInfo) {
		Player _player = _colliderInfo.collider.GetComponent<Player> ();
		if (_player != null) {
			_player.DamagePlayer (enemyStats.damage);
			DamageEnemy (999999);
		}
	}
}
