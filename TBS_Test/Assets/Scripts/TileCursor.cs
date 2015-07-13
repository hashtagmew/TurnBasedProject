using UnityEngine;
using System.Collections;

public class TileCursor : MonoBehaviour {

	public int iXGridPos;
	public int iYGridPos;

	public Ray ray;
	public RaycastHit rayHit;
	
	private Vector3 vTempPos;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0)) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);



			//if (Physics.Raycast(ray.GetPoint(0), Vector3.down, out rayHit, Mathf.Infinity, LayerMask.NameToLayer("Terrain"))) {
			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity)) {//, LayerMask.NameToLayer("Terrain"))) {
				vTempPos = rayHit.point;
				//vTempPos.x = (int)vTempPos.x;
				vTempPos.x = rayHit.collider.gameObject.transform.position.x;
				vTempPos.y = 0;
				//vTempPos.z = (int)vTempPos.z;
				vTempPos.z = rayHit.collider.gameObject.transform.position.z;

				this.transform.position = vTempPos;

				Debug.DrawLine(ray.GetPoint(0), rayHit.point);
			}
		}

		//vTempPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		//this.transform.position = vTempPos;
		//this.transform.position = new Vector3(vTempPos.x, 0, vTempPos.z);
		//iXGridPos = (int)vTempPos.x;
		//iYGridPos = (int)vTempPos.z;

		//this.transform.position = new Vector3(iXGridPos, 0, iYGridPos);
	}
}
