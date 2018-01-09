using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public float Damage = 10;
	public LayerMask objectsToHit;
	public Transform bulletTrailPrefab;
	public Transform muzzleFlashPrefab;

	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	float timeToFire = 0;
	Transform firePoint;

	public string weaponShootSound = "Pistol";

	AudioManager audioManager;

	// Initialization function
	void Start () {
		//Finds the fire point child
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("Fire Point not found");
		}

		audioManager = AudioManager.instance;
	}
	
	// Update is called once per frame
	void Update () {
		// Calculates how many times to fire the weapon per second
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else if (Input.GetButton ("Fire1") && Time.time > timeToFire) {
			timeToFire = Time.time + 1 / fireRate;
			Shoot ();
		}

		// Flips gun depending on which direction the player is facing
		if (Camera.main.ScreenToWorldPoint (Input.mousePosition).x < transform.position.x && transform.localScale.y == 1) {
			flipGun ();
		}
		if (Camera.main.ScreenToWorldPoint (Input.mousePosition).x > transform.position.x && transform.localScale.y == -1) {
			flipGun ();
		}
	}

	void Shoot () {
		// Calculates direction to shoot
		Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPos = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPos, mousePos - firePointPos, 100, objectsToHit);
		if (Time.time >= timeToSpawnEffect) {
			Effect ();
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
		Debug.DrawLine (firePointPos, mousePos);
	}

	void Effect () {
		// Controls the image effects of the weapon firing
		Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation);
		Transform clone = Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		clone.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		clone.localScale = new Vector3 (size, size, size);
		Destroy (clone.gameObject, 0.02f);

		// Play gun sound
		audioManager.PlaySound (weaponShootSound);
	}

	void flipGun () {
		// Flips the weapon on the y scale
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}
}
