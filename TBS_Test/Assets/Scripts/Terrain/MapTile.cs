using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTile : MonoBehaviour {

	public Vector2 vGridPosition;
	public List<TerrainFeature> l_tfFeatures;

	public TERRAIN_TYPE iType = TERRAIN_TYPE.NONE;

	void Start () {
		l_tfFeatures = new List<TerrainFeature>();
	}

	void Update () {
		
	}

	public void Terraform(TERRAIN_TYPE changeto) {
		//
	}
}
