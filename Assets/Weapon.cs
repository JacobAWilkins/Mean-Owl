using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour {

	public float fireRate = 0;
	public float Damage = 10;
	public LayerMask objectsToHit;

	float timeToFire = 0;
	Transform firePoint;

	// Use this for initialization
	void Start () {
		firePoint = transform.Find ("FirePoint");
		if (firePoint == null) {
			Debug.LogError ("Fire Point not found");
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (fireRate == 0) {
			if (Input.GetButtonDown ("Fire1")) {
				Shoot ();
			}
		} else if (Input.GetButtonDown ("Fire1") && Time.time > timeToFire) {
			timeToFire = Time.time + (1 / fireRate);
			Shoot ();
		}
	}

	void Shoot () {
		Vector2 mousePos = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
		Vector2 firePointPos = new Vector2 (firePoint.position.x, firePoint.position.y);
		RaycastHit2D hit = Physics2D.Raycast (firePointPos, mousePos - firePointPos, 100, objectsToHit);
		Debug.DrawLine (firePointPos, mousePos);
	}
}
