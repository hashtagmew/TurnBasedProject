using UnityEngine;
using System.Collections;

public class SelectPlayer : MonoBehaviour {

	public float fSpeed = 1.5f;
	private Vector3 vTarget;

	public TileCursor Cursor;
	public MapTile tileGoal;

	//bool bUnitSelected;

	private float fTempX;
	private float fTempZ;

	void Start() {
		vTarget = transform.position;
	}

	void Update() {
		if (Input.GetMouseButtonDown(0)) {
			if (Cursor.mapTile != null) {
				vTarget = Cursor.mapTile.gameObject.transform.position;
				vTarget = new Vector3(vTarget.x, 0.15f, vTarget.z);
			}
		}

		fTempX = this.transform.position.x;
		fTempZ = this.transform.position.z;

		if (!FloatApproximation(fTempX, vTarget.x, 0.1f)) {
			if (fTempX < vTarget.x) {
				fTempX += fSpeed * Time.deltaTime;
			}
			else if (fTempX > vTarget.x) {
				fTempX -= fSpeed * Time.deltaTime;
			}
		}
		else {
			if (!FloatApproximation(fTempZ, vTarget.z, 0.1f)) {
				if (fTempZ < vTarget.z) {
					fTempZ += fSpeed * Time.deltaTime;
				}
				else if (fTempZ > vTarget.z) {
					fTempZ -= fSpeed * Time.deltaTime;
				}
			}
		}

		this.transform.position = new Vector3(fTempX, this.transform.position.y, fTempZ);

		//transform.position = Vector3.MoveTowards(transform.position, vTarget, fSpeed * Time.deltaTime);
	}

	void OnClick() {
		//bUnitSelected = true;
		Debug.Log("Selected " + this.name);
	}

	private bool FloatApproximation(float a, float b, float tolerance) {
		return (Mathf.Abs(a - b) < tolerance);
	}
}
