using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public TileCursor mapcursor;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (mapcursor.mapTile != null) {
			if (Input.GetKeyDown(KeyCode.F1)) {
				mapcursor.mapTile.Terraform(TERRAIN_TYPE.NONE);
			}

			if (Input.GetKeyDown(KeyCode.F2)) {
				mapcursor.mapTile.Terraform(TERRAIN_TYPE.DUST_BOWL);
			}

			if (Input.GetKeyDown(KeyCode.F3)) {
				mapcursor.mapTile.Terraform(TERRAIN_TYPE.RIVER);
			}

			if (Input.GetKeyDown(KeyCode.F4)) {
				mapcursor.mapTile.Terraform(TERRAIN_TYPE.PLAINS);
			}
		}
		else {
			Debug.Log("No tile!");
		}
	}
}
