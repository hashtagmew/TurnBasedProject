using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public class MapTile : MonoBehaviour {

	public Vector2 vGridPosition;
	public List<TerrainFeature> l_tfFeatures;

	public TERRAIN_TYPE iType = TERRAIN_TYPE.NONE;
	public string sDisplayName;

	public MeshRenderer meshrenAppearance;

	void Start () {
		l_tfFeatures = new List<TerrainFeature>();

		meshrenAppearance = this.GetComponent<MeshRenderer>();

		Terraform(TERRAIN_TYPE.PLAINS);
	}

	void Update () {
		
	}

	public void Terraform(TERRAIN_TYPE changeto) {
		iType = changeto;

		if (iType == TERRAIN_TYPE.DUST_BOWL) {
			meshrenAppearance.material = Resources.Load("Terrain/dirt") as Material;
		}
		else if (iType == TERRAIN_TYPE.PLAINS) {
			meshrenAppearance.material = Resources.Load("Terrain/grass") as Material;
		}
		else if (iType == TERRAIN_TYPE.RIVER) {
			meshrenAppearance.material = Resources.Load("Terrain/water") as Material;
		}
		else {
			meshrenAppearance.material = Resources.Load("Terrain/none") as Material;
		}

		//Format name of terrain nicely for user display
		sDisplayName = changeto.ToString().ToLower();
		sDisplayName = ToProper(sDisplayName);

		string stemp = "";
		bool buppernext = true;
		for (int i = 0; i < sDisplayName.Length; i++) {
			if (sDisplayName[i] == '_') {
				stemp += ' ';
				buppernext = true;
			}
			else {
				if (buppernext) {
					string upchar = ToProper(sDisplayName[i].ToString());
					stemp += upchar;
					buppernext = false;
				}
				else {
					stemp += sDisplayName[i];
				}
			}
		}

		sDisplayName = stemp;
	}

	private string ToProper(string input) {
		if (input.Length > 1) {
			return input[0].ToString().ToUpper() + input.Substring(1, input.Length - 1);
		}
		else if (input.Length > 0) {
			return input[0].ToString().ToUpper();
		}
		else {
			return "";
		}
	}
}
