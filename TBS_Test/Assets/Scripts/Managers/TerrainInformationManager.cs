using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TerrainInformationManager : MonoBehaviour {

	public TileCursor tilecursor;

	public Text txtName;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (tilecursor != null) {
			if (tilecursor.mapTile != null) {
				txtName.text = tilecursor.mapTile.sDisplayName;
			}
		}
	}
}
