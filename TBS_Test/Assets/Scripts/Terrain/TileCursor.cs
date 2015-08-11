using UnityEngine;
using System.Collections;

public class TileCursor : MonoBehaviour {

	public int iXGridPos;
	public int iYGridPos;

	public Ray ray;
	public RaycastHit rayHit;
	
	private Vector3 vTempPos;
	public MapTile mapTile;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"))) {
				vTempPos = rayHit.point;
				//vTempPos.x = (int)vTempPos.x;
				vTempPos.x = rayHit.collider.gameObject.transform.position.x;
				vTempPos.y = 0;
				//vTempPos.z = (int)vTempPos.z;
				vTempPos.z = rayHit.collider.gameObject.transform.position.z;

				this.transform.position = vTempPos;

				Debug.DrawLine(ray.GetPoint(0), rayHit.point);

				mapTile = rayHit.collider.gameObject.GetComponent<MapTile>();
			}
		}

		//vTempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//this.transform.position = vTempPos;
		//this.transform.position = new Vector3(vTempPos.x, 0, vTempPos.z);
		//iXGridPos = (int)vTempPos.x;
		//iYGridPos = (int)vTempPos.z;

		//this.transform.position = new Vector3(iXGridPos, 0, iYGridPos);
	}

	public void TerraformSelection(TERRAIN_TYPE newtype) {
		if (mapTile != null) {
			mapTile.Terraform(newtype);
		}
	}

	public void TerraformSelection(int newtype) {
		if (mapTile != null) {
			mapTile.Terraform((TERRAIN_TYPE)newtype);
		}
	}
}
