using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {

	public GameObject Pathfinding;
	public float Count;

	public TileCursor mapcursor;
	public SelectionManager mngSelect;
	public UIManager mngUI;

	// Use this for initialization
	void Start () {
		mngSelect = this.GetComponent<SelectionManager>();
		mngUI = this.GetComponent<UIManager>();
		Count = 0;
		Pathfinding.SetActive (false);
	}
	
	// Update is called once per frame
	void Update () {

		Count += Time.deltaTime;
		if (Count <= 1f) 
		{
			Pathfinding.SetActive(true);
		}

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
			//Debug.Log("No tile!");
		}

		if (Input.GetKeyDown(KeyCode.F12)) {
			ChangeScene("particle-effect-tester");
		}

		if (mngSelect.guSelection != null) {
			mngUI.bUnitShow = true;
		}
		else {
			mngUI.bUnitShow = false;
		}

		if (mapcursor.mapTile != null) {
			mngUI.bTerrainShow = true;
		}
		else {
			mngUI.bTerrainShow = false;
		}
	}

	public void ChangeScene(string scene) {
		Application.LoadLevel(scene);
	}
}
