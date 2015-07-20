using UnityEngine;
using System.Collections;

public class SelectPlayer : MonoBehaviour {

	public float fSpeed = 1.5f;
	private Vector3 vTarget;

	public TileCursor Cursor;
	public MapTile tileGoal;

	bool bUnitSelected;
	// Use this for initialization
	void Start () {
		vTarget = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{

			Debug.Log("Mouse is down");
			//vTarget = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			//vTarget.z = transform.position.z;
			if (Cursor.mapTile != null) {
				vTarget = Cursor.mapTile.gameObject.transform.position;
				vTarget = new Vector3(vTarget.x, 0.1f, vTarget.z);
			}
		}
		transform.position = Vector3.MoveTowards(transform.position, vTarget, fSpeed * Time.deltaTime);
	}

	void OnClick(){

		bUnitSelected = true;
		Debug.Log ("Selected");
	}
}
