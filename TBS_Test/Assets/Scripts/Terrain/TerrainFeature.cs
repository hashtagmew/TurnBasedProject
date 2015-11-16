using UnityEngine;
using System.Collections;

public class TerrainFeature : MonoBehaviour {

	public FEATURE_TYPE iType = FEATURE_TYPE.NONE;
	private Renderer myRenderer;

	// Use this for initialization
	void Start () {
		myRenderer = this.gameObject.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Terraform(FEATURE_TYPE changeto) {
		iType = changeto;

		if (myRenderer == null) {
			myRenderer = this.gameObject.GetComponent<Renderer>();
		}
		
		if (iType == FEATURE_TYPE.TREE) {
			//this.transform.localScale = new Vector3(0.2f, 0.2f, 5.0f);
			//myRenderer.sharedMaterial = Resources.Load("Terrain/leaves") as Material;
			this.name = "Feature-Tree";

			ChangeModel("tree");
		}
		else if (iType == FEATURE_TYPE.MOUNTAIN) {
			//this.transform.localScale = new Vector3(0.8f, 0.8f, 4.0f);
			myRenderer.sharedMaterial = Resources.Load("Terrain/cliff") as Material;
			this.name = "Feature-Mountain";
		}
		else if (iType == FEATURE_TYPE.CRATER) {
			//this.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
			myRenderer.sharedMaterial = Resources.Load("Terrain/crater") as Material;
			this.name = "Feature-Crater";
		}
		else if (iType == FEATURE_TYPE.WALL) {
			//this.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
			//myRenderer.sharedMaterial = Resources.Load("Terrain/crater") as Material;
			this.name = "Feature-Crater";

			ChangeModel("wall");
		}
		else if (iType == FEATURE_TYPE.HILL) {
			//this.transform.localScale = new Vector3(0.9f, 0.9f, 1.0f);
			//myRenderer.sharedMaterial = Resources.Load("Terrain/crater") as Material;
			this.name = "Feature-Crater";

			ChangeModel("hill");
		}
	}

	void ChangeModel(string name) {
		for (int i = 0; i < gameObject.transform.childCount; i++) {
			gameObject.transform.GetChild(i).gameObject.SetActive(false);
		}

		this.gameObject.transform.FindChild(name).gameObject.SetActive(true);
	}
}
