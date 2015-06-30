﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUnit : MonoBehaviour, ISelectable {

	public Vector2 vGridPosition;

	public string sName;

	public int iLevel;
	public float fXP;
	public float fXPtoNext;
	public float fAP;

	public float fHealth;
	public float fMana;
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
