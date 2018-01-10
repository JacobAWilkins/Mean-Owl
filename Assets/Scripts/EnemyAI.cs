using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Pathfinding;

[RequireComponent (typeof (Rigidbody2D))]
[RequireComponent (typeof (Seeker))]
public class EnemyAI : MonoBehaviour {

	public Transform target; // Target that the AI will follow
	public float updateRate = 2f; // Times per second to update path
	private Seeker seeker; 
	private Rigidbody2D rb;
	public Path path; // The path the AI will move on
	public float speed = 300f; // AI speed per second
	public ForceMode2D fm;
	[HideInInspector]
	public bool pathIsEnded = false;
	public float nextWayPointDistance = 3f; // Distance for AI to continue to the next way point
	private int currentWayPoint = 0; // The Waypoint we are currently moving towards
	private bool searchingForPlayer = false;

	void Start () {
		seeker = GetComponent<Seeker> ();
		rb = GetComponent<Rigidbody2D> ();

		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			return;
		}

		// Start new path to target position and return result of OnPathComplete function
		seeker.StartPath (transform.position, target.position, OnPathComplete);

		StartCoroutine (UpdatePath ());
	}

	IEnumerator SearchForPlayer () {
		GameObject searchResult = GameObject.FindGameObjectWithTag ("Player");
		if (searchResult == null) {
			yield return new WaitForSeconds (0.5f);
			StartCoroutine (SearchForPlayer ());
		}
		else {
			target = searchResult.transform;
			searchingForPlayer = false;
			StartCoroutine (UpdatePath ());
			yield return false;
		}
	}

	IEnumerator UpdatePath () {
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			yield return false;
		}

		seeker.StartPath (transform.position, target.position, OnPathComplete);
		yield return new WaitForSeconds (1f / updateRate);
		StartCoroutine (UpdatePath ());
	}

	public void OnPathComplete (Path _path) {
		if (!_path.error) {
			path = _path;
			currentWayPoint = 0;
		}
	}

	void FixedUpdate () {
		if (target == null) {
			if (!searchingForPlayer) {
				searchingForPlayer = true;
				StartCoroutine (SearchForPlayer ());
			}
			return;
		}

		//TODO: Make enemy look towards player

		if (path == null) {
			return;
		}

		if (currentWayPoint >= path.vectorPath.Count) {
			if (pathIsEnded) {
				return;
			}
			pathIsEnded = true;
			return;
		}
		pathIsEnded = false;

		// Direction to the next waypoint
		Vector3 dir = (path.vectorPath [currentWayPoint] - transform.position).normalized;
		dir *= speed * Time.fixedDeltaTime;

		// Move AI
		rb.AddForce (dir, fm);

		float distance = Vector3.Distance (transform.position, path.vectorPath [currentWayPoint]);
		if (distance < nextWayPointDistance) {
			currentWayPoint++;
			return;
		}
	}

}