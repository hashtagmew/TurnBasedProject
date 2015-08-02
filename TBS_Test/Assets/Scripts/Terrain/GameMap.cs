using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameMap : MonoBehaviour {
	public GameObject TilePrefab;

	public float fTileSize = 0.32f;

	public int iMapHorzSize = 10;
	public int iMapVertSize = 11;
	List <List<MapTile>> map = new List<List<MapTile>>();

	public bool bGridToggle = false;

	void Start() {
		GenerateMap();  
	}

	void Update() {

		if (Input.GetKeyDown(KeyCode.G)) {
			ToggleGrid();
		}
	}

	public void GenerateMap() {
		map = new List<List<MapTile>>();
		for (int i = 0; i < iMapHorzSize; i++) {
			List<MapTile> row = new List<MapTile>();
			for (int j = 0; j < iMapVertSize; j++) {
				MapTile tile = ((GameObject)Instantiate(TilePrefab, 
				                                        new Vector3(i * fTileSize, 0, j * -fTileSize),
				                                         Quaternion.identity)).GetComponent<MapTile>();
				tile.vGridPosition = new Vector2(i, j);
				tile.gameObject.name = "Tile (" + i.ToString("D2") + ", " + j.ToString("D2") + ")";
				tile.transform.parent = this.gameObject.transform;
				row.Add(tile);
			}

			map.Add(row);
		}
	}

	public void ToggleGrid() {
		bGridToggle = !bGridToggle;

		for (int i = 0; i < iMapHorzSize; i++) {
			for (int j = 0; j < iMapVertSize; j++) {
				GetTile(i, j).bGridEnabled = bGridToggle;
			}
		}
	}

	public MapTile GetTile(int x, int y) {
		return map[x][y];
	}
}