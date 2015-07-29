﻿using UnityEngine;
using System.Collections;

public class CameraPinchToZoom : MonoBehaviour {

	public float fZoomSpeed = 0.1f;
	public Camera cam;

	public float fMinScale = 0.8f;
	public float fMaxScale = 5.0f;

	public float fMinPinchSpeed = 5.0f;

	public float fVariance = 0.0f;

	private float fTouchDelta = 0.0f;
	private Vector2 vLastDistance;
	private Vector2 vCurDistance;

	private float fTouchSpeed0;
	private float fTouchSpeed1;
	
	void Start() {
	
	}

	void Update() {
		//Use delta of touches to detect pinching
		if (Input.touchCount == 2 && Input.GetTouch(0).phase == TouchPhase.Moved && Input.GetTouch(1).phase == TouchPhase.Moved) {
			vCurDistance = Input.GetTouch(0).position - Input.GetTouch(1).position;
			vLastDistance = ((Input.GetTouch(0).position - Input.GetTouch(0).deltaPosition) - (Input.GetTouch(1).position - Input.GetTouch(1).deltaPosition));
			fTouchDelta = vCurDistance.magnitude - vLastDistance.magnitude;
			fTouchSpeed0 = Input.GetTouch(0).deltaPosition.magnitude / Input.GetTouch(0).deltaTime;
			fTouchSpeed1 = Input.GetTouch(1).deltaPosition.magnitude / Input.GetTouch(1).deltaTime;

			if ((fTouchDelta + fVariance <= 1) && (fTouchSpeed0 > fMinPinchSpeed) && (fTouchSpeed1 > fMinPinchSpeed)) {
				cam.orthographicSize = Mathf.Clamp(cam.orthographicSize + fZoomSpeed, fMinScale, fMaxScale);
			}
			
			if ((fTouchDelta + fVariance > 1) && (fTouchSpeed0 > fMinPinchSpeed) && (fTouchSpeed1 > fMinPinchSpeed)) {
				cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - fZoomSpeed, fMinScale, fMaxScale);
			}
		}
	}
}
