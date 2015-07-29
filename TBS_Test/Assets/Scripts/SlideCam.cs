﻿using UnityEngine;
using System.Collections;

public class SlideCam : MonoBehaviour {

	Camera camMain;
	Vector3 vTempMove;
	Vector3 vTempTurn;
	float fMoveSpeed = 0.15f;
	float fZoomSpeed = 0.1f;

	public float fMinScale = 0.8f;
	public float fMaxScale = 5.0f;

	// Use this for initialization
	void Start() {
		camMain = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update() {
		vTempMove = this.transform.position;
		vTempTurn = this.transform.rotation.eulerAngles;

		//Strafe
		if (Input.GetKey(KeyCode.A)) {
			vTempMove += this.transform.right * -fMoveSpeed;
		}
		else if (Input.GetKey(KeyCode.D)) {
			vTempMove += this.transform.right * fMoveSpeed;
		}

		if (Input.GetKey(KeyCode.W)) {
			vTempMove += this.transform.up * fMoveSpeed;
		}
		else if (Input.GetKey(KeyCode.S)) {
			vTempMove += this.transform.up * -fMoveSpeed;
		}

		//Zoom
		if (Input.mouseScrollDelta.y > 0) {
			camMain.orthographicSize = Mathf.Clamp(camMain.orthographicSize - fZoomSpeed, fMinScale, fMaxScale);
		}
		else if (Input.mouseScrollDelta.y < 0) {
			camMain.orthographicSize = Mathf.Clamp(camMain.orthographicSize + fZoomSpeed, fMinScale, fMaxScale);
		}

		this.transform.position = vTempMove;
		this.transform.eulerAngles = vTempTurn;
	}
}
