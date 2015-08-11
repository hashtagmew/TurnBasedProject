using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SelectionManager : MonoBehaviour {

	public UNIT_FACTION iPlayerTeam;
	public GameUnit guSelection;

	public Ray ray;
	public RaycastHit rayHit;

	// Use this for initialization
	void Start () {
		guSelection = null;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0) && guSelection == null && !EventSystem.current.IsPointerOverGameObject()) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
				
				Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.red);
				
				guSelection = rayHit.collider.gameObject.GetComponent<GameUnit>();
				Debug.Log(guSelection.ToString());
				if (guSelection.GetComponent<ISelectable>() != null) {
					guSelection.GetComponent<ISelectable>().OnSelected();
				}
			}
		}

		if (Input.GetMouseButton(1)) {
			Deselect();
		}

		if (Input.touchCount == 4) {
			Deselect();
		}

		if (Input.GetMouseButton(0) && guSelection != null && !EventSystem.current.IsPointerOverGameObject()) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			
			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"))) {
				
				Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.yellow);
				
				Vector3 vTempPos = rayHit.point;
				vTempPos.x = rayHit.collider.gameObject.transform.position.x;
				vTempPos.y = 0.2f;
				vTempPos.z = rayHit.collider.gameObject.transform.position.z;

				guSelection.MoveTo(vTempPos);
				Debug.Log(vTempPos.ToString());
			}
		}
	}

	public void Deselect() {
		if (guSelection != null) {
			guSelection.GetComponent<ISelectable>().OnDeselected();
			guSelection = null;
		}
	}
}
