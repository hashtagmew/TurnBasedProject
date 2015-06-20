using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelectorSquare : MonoBehaviour {

	public GameMap mapmng;
	public List<RaycastHit2D> l_rayhits;
	
	void Start () {
		l_rayhits = new List<RaycastHit2D>();
	}

	void Update () {
		//Move
		if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) {
			this.transform.position = this.transform.position + new Vector3(-mapmng.fTileSize, 0, 0);
		}

		if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) {
			this.transform.position = this.transform.position + new Vector3(mapmng.fTileSize, 0, 0);
		}

		if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow)) {
			this.transform.position = this.transform.position + new Vector3(0, mapmng.fTileSize, 0);
		}

		if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow)) {
			this.transform.position = this.transform.position + new Vector3(0, -mapmng.fTileSize, 0);
		}

		//Grid boundaries
		if (this.transform.position.x < 0) {
			this.transform.position = new Vector3(0, this.transform.position.y, 0);
		}

		if (this.transform.position.x > ((mapmng.iMapHorzSize - 1) * mapmng.fTileSize)) {
			this.transform.position = new Vector3((mapmng.iMapHorzSize - 1) * mapmng.fTileSize, this.transform.position.y, 0);
		}

		if (this.transform.position.y > 0) {
			this.transform.position = new Vector3(this.transform.position.x, 0, 0);
		}
		
		if (this.transform.position.y < (((mapmng.iMapVertSize - 1) * mapmng.fTileSize) * -1)) {
			this.transform.position = new Vector3(this.transform.position.x, ((mapmng.iMapVertSize - 1) * mapmng.fTileSize) * -1, 0);
		}

		//Select
		if (Input.GetKeyDown(KeyCode.Space)) {
			//Clean up old
			if (Physics2D.Raycast(new Vector2(this.transform.position.x + 0.16f, this.transform.position.y - 0.16f), Vector3.zero, 0.0f).transform != null) {
				if (l_rayhits.Count > 0) {
					foreach (RaycastHit2D hit in l_rayhits) {
						GameUnit gu = hit.transform.gameObject.GetComponent<GameUnit>();
						if (gu != null) {
							gu.OnDeselected();
						}
					}
				}
			}

			Debug.Log("========");
			l_rayhits.Clear();

			//Add new
			l_rayhits.AddRange(Physics2D.RaycastAll(new Vector2(this.transform.position.x + 0.16f, this.transform.position.y - 0.16f), Vector3.zero, 0.0f));
			if (l_rayhits.Count > 0) {
				foreach (RaycastHit2D hit in l_rayhits) {
					GameUnit gu = hit.transform.gameObject.GetComponent<GameUnit>();
					if (gu != null) {
						gu.OnSelected();
					}

					Debug.Log(hit.transform.gameObject.name);
				}
			}
			else {
				Debug.Log("none");
			}
		}
	}
}
