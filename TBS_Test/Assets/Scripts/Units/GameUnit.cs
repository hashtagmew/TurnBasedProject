using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum UNIT_TYPE {
	NONE = 0,
	INFANTRY,
	ARCHER,
	PIKEMAN,
	CAVALRY,
	SUPPORT,
	IRREGULAR,
	MAGICALBEING,
	MONSTER,
	ANIMAL
}

public class GameUnit : MonoBehaviour, ISelectable {

	public Vector2 vGridPosition;

	public string sName;
	public UNIT_TYPE iType = UNIT_TYPE.NONE;

	public int iLevel;
	public float fXP;
	public float fXPtoNext;

	public float fHealth;
	public float fMovement;
	public float fVision;

	public float fAttack;
	public float fPhysAttack;
	public float fRangAttack;
	public float fMagiAttack;

	public float fResistance;
	public float fDefence;

	public List<Ability> l_abilities;

	public GameUnit() {
		vGridPosition = Vector2.zero;
		
		sName = "Noname";
		iType = UNIT_TYPE.NONE;
		
		iLevel = 1;
		fXP = 0.0f;
		fXPtoNext = 100.0f;
		
		fHealth = 50.0f;
		fMovement = 6.0f;
		fVision = 2.0f;

		fAttack = 4.0f;
		fPhysAttack = 4.0f;
		fRangAttack = 4.0f;
		fMagiAttack = 4.0f;
		
		fResistance = 2.0f;
		fDefence = 3.0f;
		
		l_abilities = new List<Ability>();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnSelected() {
		//
	}

	public virtual void OnDeselected() {
		//
	}
}
