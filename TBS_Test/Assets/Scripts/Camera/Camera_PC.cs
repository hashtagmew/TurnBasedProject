using UnityEngine;
using System.Collections;

public class Camera_PC : MonoBehaviour {

	private Camera camMain;
	private Vector3 vTempMove;
	private Vector3 vTempTurn;
	public float fMoveSpeed = 0.15f;
	public float fZoomSpeed = 0.1f;

	public float fMinScale = 0.8f;
	public float fMaxScale = 5.0f;

	// Use this for initialization
	void Start() {
		camMain = this.GetComponent<Camera>();
	}
	
	// Update is called once per frame
	void Update() {
		if (Application.isMobilePlatform) {
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
}
