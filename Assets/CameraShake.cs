﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraShake : MonoBehaviour {

	public Camera mainCamera;
	float shakeAmount = 0;

	void Awake () {
		if (mainCamera == null) {
			mainCamera = Camera.main;
		}
	}

	public void Shake (float amount, float length) {
		shakeAmount = amount;
		InvokeRepeating ("BeginShake", 0, 0.01f);
		Invoke ("StopShake", length);
	}

	void BeginShake () {
		if (shakeAmount > 0) {
			Vector3 cameraPosition = mainCamera.transform.position;
			float offsetX = Random.value * shakeAmount * 2 - shakeAmount;
			float offsetY = Random.value * shakeAmount * 2 - shakeAmount;

			cameraPosition.x += offsetX;
			cameraPosition.y += offsetY;

			mainCamera.transform.position = cameraPosition;
		}
	}

	void StopShake () {
		CancelInvoke ("BeginShake");
		mainCamera.transform.localPosition = Vector3.zero;
	}
}
