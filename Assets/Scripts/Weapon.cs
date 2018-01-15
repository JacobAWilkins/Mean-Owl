using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public int Damage = 10;
	public LayerMask objectsToHit;
	public Transform bulletTrailPrefab;
	public Transform muzzleFlashPrefab;
	public Transform hitPrefab;

	float timeToSpawnEffect = 0;
	public float effectSpawnRate = 10;

	float timeToFire = 0;
	Transform firePoint;

	public string weaponShootSound = "Pistol";

	AudioManager audioManager;

	public float cameraShakeAmount = 0.05f;
	public float cameraShakeLength = 0.1f;
	CameraShake cameraShake;

	// Initialization function
	void Awake () {
		//Finds the fire point child
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("Fire Point not found");
		}
	}

	void Start () {
		cameraShake = GameControl.gameControl.GetComponent<CameraShake> ();
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
		Debug.DrawLine (firePointPos, mousePos);
		if (hit.collider != null) {
			Enemy enemy = hit.collider.GetComponent<Enemy> ();
			if (enemy != null) {
				enemy.DamageEnemy (Damage);
			}
		}
		if (Time.time >= timeToSpawnEffect) {
			Vector3 hitPosition;
			Vector3 hitNormal;
			if (hit.collider == null) {
				hitPosition = (mousePos - firePointPos) * 30;
				hitNormal = new Vector3 (9999, 9999, 9999);
			}
			else {
				hitPosition = hit.point;
				hitNormal = hit.normal;
			}
			Effect (hitPosition, hitNormal);
			timeToSpawnEffect = Time.time + 1 / effectSpawnRate;
		}
	}

	void Effect (Vector3 hitPosition, Vector3 hitNormal) {
		// Controls the image effects of the weapon firing
		Transform bulletTrail = Instantiate (bulletTrailPrefab, firePoint.position, firePoint.rotation) as Transform;
		LineRenderer lineRenderer = bulletTrail.GetComponent<LineRenderer> ();
		if (lineRenderer != null) {
			lineRenderer.SetPosition (0, firePoint.position);
			lineRenderer.SetPosition (1, hitPosition);
		}
		Destroy (bulletTrail.gameObject, 0.04f);

		if (hitNormal != new Vector3 (9999, 9999, 9999)) {
			Transform hitParticle = Instantiate (hitPrefab, hitPosition, Quaternion.FromToRotation (Vector3.right, hitNormal)) as Transform;
			Destroy (hitParticle.gameObject, 1f);
		}

		Transform muzzleFlash = Instantiate (muzzleFlashPrefab, firePoint.position, firePoint.rotation) as Transform;
		muzzleFlash.parent = firePoint;
		float size = Random.Range (0.6f, 0.9f);
		muzzleFlash.localScale = new Vector3 (size, size, size);
		Destroy (muzzleFlash.gameObject, 0.02f);

		// Play gun sound
		audioManager.PlaySound (weaponShootSound);

		// Shake the Camera
		cameraShake.Shake (cameraShakeAmount, cameraShakeLength);
	}

	void flipGun () {
		// Flips the weapon on the y scale
		Vector3 theScale = transform.localScale;
		theScale.y *= -1;
		transform.localScale = theScale;
	}
}
