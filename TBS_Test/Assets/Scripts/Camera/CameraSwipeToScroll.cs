using UnityEngine;
using System.Collections;

public class CameraSwipeToScroll : MonoBehaviour {

	private Camera camMain;
	private Vector2 vTouchDelta;
	public float fMoveSpeed = 0.15f;
	
	void Start(){
		camMain = this.GetComponent<Camera>();
	}

	void Update() {	
		if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved) {
			vTouchDelta = Input.GetTouch(0).deltaPosition;
			camMain.transform.Translate(-vTouchDelta.x * fMoveSpeed * Time.deltaTime,
			                            -vTouchDelta.y * fMoveSpeed * Time.deltaTime, 0);
		}
	}
}
