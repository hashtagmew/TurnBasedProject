using UnityEngine;
using System.Collections;

public class SelectPlayer : MonoBehaviour {

	public float fSpeed = 1.5f;
	private Vector3 vTarget;

	public TileCursor Cursor;
	public MapTile tileGoal;

	bool bUnitSelected;

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
		transform.position = Vector3.MoveTowards(transform.position, vTarget, fSpeed * Time.deltaTime);
	}

	void OnClick() {
		bUnitSelected = true;
		Debug.Log("Selected " + this.name);
	}
}
