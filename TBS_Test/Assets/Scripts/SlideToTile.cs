using UnityEngine;
using System.Collections;

public class SlideToTile : MonoBehaviour {

	public Vector2 vGoal;
	public Vector2 vGridPos;

	// Use this for initialization
	void Start () {
		
	}

	public void Step() {
		if (vGoal != vGridPos) {
			if (vGoal.x < vGridPos.x) {
				this.transform.position = new Vector3(this.transform.position.x + 1, this.transform.position.y, this.transform.position.z);
				//vGridPos = new Vector2
			}
			else if (vGoal.x > vGridPos.x) {
				this.transform.position = new Vector3(this.transform.position.x - 1, this.transform.position.y, this.transform.position.z);
			}

			if (vGoal.y < vGridPos.y) {
				//
			}
			else if (vGoal.y > vGridPos.y) {
				//
			}
		}
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			Step();
		}
	}
}
