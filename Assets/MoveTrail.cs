using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTrail : MonoBehaviour {

	public int moveSpeed = 230;
	
	// Update is called once per frame
	void Update () {
		// Moves bullet trail
		transform.Translate (Vector3.right * Time.deltaTime * moveSpeed);
		Destroy (gameObject, 1);
	}
}
