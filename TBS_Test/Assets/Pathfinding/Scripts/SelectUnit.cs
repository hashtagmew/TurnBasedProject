using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectUnit : MonoBehaviour {

	public TileMap map;
	//public Unit unit;
	public bool bSelected = false;

	Ray ray;
	RaycastHit rayHit;

	void Start () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	}

	void Update (){
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);


			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
				Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.green);
				map.selectedUnit = rayHit.collider.transform.parent.gameObject;
				Debug.Log("Unit Selected: " + map.selectedUnit.name);

			}//else{
//				Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.red);
//				map.selectedUnit = null;
//				Debug.Log("No unit");
//			}
//			if(Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"))){
//				map.selectedUnit = map.selectedUnit;
//			}
		}
		if (Input.GetMouseButtonDown (1) && !EventSystem.current.IsPointerOverGameObject ()){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"))) {
				map.selectedUnit = null;
				Debug.Log("No unit");
			}
		}
	}
}

