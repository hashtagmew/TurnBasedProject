﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

using UnityEngine.UI;

public class SelectUnit : MonoBehaviour {

	public TileMap map;
	//public Unit unit;
	public bool bSelected = false;

	Ray ray;
	RaycastHit rayHit;

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
		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);


			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
				Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.green);
				map.selectedUnit = rayHit.collider.transform.parent.gameObject;
				Debug.Log("Unit Selected: " + map.selectedUnit.name);

			}
		}

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
