using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MapTile : MonoBehaviour {

	public Vector2 vGridPosition;
	public List<TerrainFeature> l_tfFeatures;

	public TERRAIN_TYPE iType = TERRAIN_TYPE.NONE;
	public TERRAIN_TYPE iTransitionType = TERRAIN_TYPE.NONE;
	public TERRAIN_ORIENTATION eOrient = TERRAIN_ORIENTATION.UP;
	public string sDisplayName;

	public MeshRenderer meshrenAppearance;
	public GameObject goGrid;

	public bool bGridEnabled = false;

	void OnEnable() {
		meshrenAppearance = this.GetComponent<MeshRenderer>();
	}

	void Start() {
		l_tfFeatures = new List<TerrainFeature>();

		meshrenAppearance = this.GetComponent<MeshRenderer>();
	}

	void Update() {
		if (goGrid) {
			goGrid.SetActive(bGridEnabled);
		}
	}

	public void SimpleTerraform(TERRAIN_TYPE changeto) {
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
		else if (iType == TERRAIN_TYPE.WASTELAND) {
			meshrenAppearance.material = Resources.Load("Terrain/wasteland") as Material;
		}
		else if (iType == TERRAIN_TYPE.DESERT) {
			meshrenAppearance.material = Resources.Load("Terrain/desert") as Material;
		}
		else if (iType == TERRAIN_TYPE.QUAGMIRE) {
			meshrenAppearance.material = Resources.Load("Terrain/swamp") as Material;
		}
		else if (iType == TERRAIN_TYPE.BIOMASS) {
			meshrenAppearance.material = Resources.Load("Terrain/flesh") as Material;
		}
		else if (iType == TERRAIN_TYPE.PAVEMENT) {
			meshrenAppearance.material = Resources.Load("Terrain/tiles") as Material;
		}
		else if (iType == TERRAIN_TYPE.LAVA) {
			meshrenAppearance.material = Resources.Load("Terrain/lava") as Material;
		}
		else {
			meshrenAppearance.material = Resources.Load("Terrain/none") as Material;
		}

		GenerateFormattedDisplayName(changeto);
	}

	public void Terraform(TERRAIN_TYPE changeto, TERRAIN_TYPE transition, TERRAIN_ORIENTATION rot) {
		iType = changeto;
		iTransitionType = transition;

		if (changeto != transition && changeto != TERRAIN_TYPE.NONE && transition != TERRAIN_TYPE.NONE) {
			//Grass/Dirt
			if (iType == TERRAIN_TYPE.DUST_BOWL && transition == TERRAIN_TYPE.PLAINS || 
			    iType == TERRAIN_TYPE.PLAINS && transition == TERRAIN_TYPE.DUST_BOWL) {
				meshrenAppearance.material = Resources.Load("Terrain/sand-to-grass") as Material;
			}
			//Grass/Water
			else if (iType == TERRAIN_TYPE.RIVER && transition == TERRAIN_TYPE.PLAINS || 
			         iType == TERRAIN_TYPE.PLAINS && transition == TERRAIN_TYPE.RIVER) {
				meshrenAppearance.material = Resources.Load("Terrain/grass-to-water") as Material;
			}
			//Lava/Water
			else if (iType == TERRAIN_TYPE.RIVER && transition == TERRAIN_TYPE.LAVA || 
			         iType == TERRAIN_TYPE.LAVA && transition == TERRAIN_TYPE.RIVER) {
				meshrenAppearance.material = Resources.Load("Terrain/lava-to-water") as Material;
			}
			//Sand/Lava
			else if (iType == TERRAIN_TYPE.DUST_BOWL && transition == TERRAIN_TYPE.LAVA || 
			         iType == TERRAIN_TYPE.LAVA && transition == TERRAIN_TYPE.DUST_BOWL) {
				meshrenAppearance.material = Resources.Load("Terrain/sand-to-lava") as Material;
			}
			//Sand/Wasteland
			else if (iType == TERRAIN_TYPE.DUST_BOWL && transition == TERRAIN_TYPE.WASTELAND || 
			         iType == TERRAIN_TYPE.WASTELAND && transition == TERRAIN_TYPE.DUST_BOWL) {
				meshrenAppearance.material = Resources.Load("Terrain/sand-to-wasteland") as Material;
			}
			//Sand/Water
			else if (iType == TERRAIN_TYPE.DUST_BOWL && transition == TERRAIN_TYPE.RIVER || 
			         iType == TERRAIN_TYPE.RIVER && transition == TERRAIN_TYPE.DUST_BOWL) {
				meshrenAppearance.material = Resources.Load("Terrain/sand-to-water") as Material;
			}
			//Wasteland/Lava
			else if (iType == TERRAIN_TYPE.WASTELAND && transition == TERRAIN_TYPE.LAVA || 
			         iType == TERRAIN_TYPE.LAVA && transition == TERRAIN_TYPE.WASTELAND) {
				meshrenAppearance.material = Resources.Load("Terrain/wasteland-to-lava") as Material;
			}
			//Wasteland/Water
			else if (iType == TERRAIN_TYPE.WASTELAND && transition == TERRAIN_TYPE.RIVER || 
			         iType == TERRAIN_TYPE.RIVER && transition == TERRAIN_TYPE.WASTELAND) {
				meshrenAppearance.material = Resources.Load("Terrain/wasteland-to-water") as Material;
			}
			else {
				Debug.Log("No transition existed between " + changeto.ToString() + " and " + transition.ToString() + "!");
				SimpleTerraform(changeto);
			}
		}
		//No transition needed
		else {
			SimpleTerraform(changeto);
		}

		this.transform.localEulerAngles = new Vector3(this.transform.localRotation.eulerAngles.x,
		                               this.transform.localRotation.eulerAngles.y,
		                               (float)rot);
		//this.transform.localEulerAngles.Set(neweuler.x, neweuler.y, neweuler.z);
		//this.transform.eulerAngles.Set(neweuler.x, neweuler.y, neweuler.z);
		//this.transform.Rotate(neweuler);
		//this.transform.eulerAngles
		//this.transform.localRotation.eulerAngles = neweuler;

		//Debug.Log((float)rot);

		this.eOrient = rot;
		this.iTransitionType = transition;
		
		GenerateFormattedDisplayName(changeto);
	}

	private void GenerateFormattedDisplayName(TERRAIN_TYPE changeto) {
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
