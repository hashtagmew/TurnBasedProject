using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class SelectUnit : MonoBehaviour {

	public TileMap map;

	public Unit unit;
	public bool bSelected = false;

	Ray ray;
	RaycastHit rayHit;

	public GameObject unit0;
	public GameObject unit1;
	public GameObject SlctTile;
	public bool CanAttack;
	public float distance;
	private float Range = 1.1f;

	public Text txtSelectedName;
	public Text txtSelectedHP;
	//public Text txtSelectedAP;
	public Text txtSelectedPhysAttack;
	public Text txtSelectedRangAttack;
	public Text txtSelectedMagiAttack;
	public Text txtSelectedDefence;
	public Text txtSelectedResistance;

	public UIManager uiMng;

	void Start () {
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
	}

	void Update () {

		distance = Vector3.Distance (unit0.transform.position, unit1.transform.position);

		if (map.selectedUnit != null) {
			SlctTile.transform.position = (map.selectedUnit.transform.position);
		}

		if (distance <= 1.1f) {
			CanAttack = true;
		} else
			CanAttack = false;

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
		if (Input.GetMouseButtonDown (2) && !EventSystem.current.IsPointerOverGameObject ()){
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"))) {
				map.selectedUnit = null;
				SlctTile.transform.position = new Vector3 (-1000,-1000,-1000);
				Debug.Log("No Unit");
			}
		}
		if (Input.GetMouseButtonDown (1) && !EventSystem.current.IsPointerOverGameObject () && map.selectedUnit != null) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
				map.deselectedUnit = rayHit.collider.transform.parent.gameObject;
			}

		}

		if (Input.GetMouseButtonDown (1) && !EventSystem.current.IsPointerOverGameObject () && map.selectedUnit != null && CanAttack == true) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
				Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.green);
				GameUnit attk = map.selectedUnit.GetComponent<GameUnit>();
				GameUnit temphit = rayHit.collider.transform.parent.gameObject.GetComponent<GameUnit>();
				Debug.Log("Unit Hit: " + map.selectedUnit.name);
				temphit.fHealth -= Combat.CombatMechanics.CalculateDamage((int)attk.fPhysAttack, 0, (int)temphit.fDefence, 0);
				map.selectedUnit.GetComponent<AudioSource>().Play();
				map.selectedUnit.GetComponent<AudioSource>().Play();
			}
		}

//		if(Vector3.Distance(map.selectedUnit.transform.position, map.deselectedUnit.transform.position) < Range )
//		{
//			Debug.DrawLine(map.selectedUnit.transform.position, rayHit.point, Color.blue);
//			if(Physics.Raycast(transform.position, (map.deselectedUnit.transform.position - map.selectedUnit.transform.position), out rayHit, Range))
//			{
//
//
//				if(rayHit.transform.tag == "Unit")
//				{
//					// In Range and i can see you!
//				}
//			}
//		}

		if (map.selectedUnit == null) {
			txtSelectedName.text = "None";
			txtSelectedHP.text = "";
			txtSelectedPhysAttack.text = "";
			txtSelectedRangAttack.text = "";
			txtSelectedMagiAttack.text = "";
			txtSelectedDefence.text = "";
			txtSelectedResistance.text = "";

			bSelected = false;
		}
		else {
			txtSelectedName.text = map.selectedUnit.GetComponent<GameUnit>().sName;
			txtSelectedHP.text = map.selectedUnit.GetComponent<GameUnit>().fHealth.ToString() + "/\n" + map.selectedUnit.GetComponent<GameUnit>().fMaxHealth.ToString();
			txtSelectedPhysAttack.text = map.selectedUnit.GetComponent<GameUnit>().fPhysAttack.ToString();
			txtSelectedRangAttack.text = map.selectedUnit.GetComponent<GameUnit>().fRangAttack.ToString();
			txtSelectedMagiAttack.text = map.selectedUnit.GetComponent<GameUnit>().fMagiAttack.ToString();
			txtSelectedDefence.text = map.selectedUnit.GetComponent<GameUnit>().fDefence.ToString();
			txtSelectedResistance.text = map.selectedUnit.GetComponent<GameUnit>().fResistance.ToString();

			bSelected = true;
		}

		uiMng.bUnitShow = bSelected;
	}
}

