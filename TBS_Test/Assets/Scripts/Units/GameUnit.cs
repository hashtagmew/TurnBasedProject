using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameUnit : MonoBehaviour, ISelectable {

	public UNIT_FACTION iFaction;

	public Vector2 vGridPosition;
	public Vector3 vMoveTarget;
	public bool bMoving = false;

	public bool bSelected = false;

	public string sName;

	//public int iLevel;
	//public float fXP;
	//public float fXPtoNext;
	public float fAP;

	public float fHealth;
	//public float fMana;
	public float fMovement;
	//public float fVision;
	public float fSpeed = 1.5f;

	public float fAttack;
	public float fPhysAttack;
	public float fRangAttack;
	public float fMagiAttack;

	public float fResistance;
	public float fDefence;

	public List<Ability> l_abilities;

//	public GameUnit() {
//		vGridPosition = Vector2.zero;
//		
//		sName = "Noname";
//		
//		iLevel = 1;
//		fXP = 0.0f;
//		fXPtoNext = 100.0f;
//		
//		fHealth = 50.0f;
//		fMovement = 6.0f;
//		fVision = 2.0f;
//
//		fAttack = 4.0f;
//		fPhysAttack = 4.0f;
//		fRangAttack = 4.0f;
//		fMagiAttack = 4.0f;
//		
//		fResistance = 2.0f;
//		fDefence = 3.0f;
//		
//		l_abilities = new List<Ability>();
//	}

	// Use this for initialization
	void Start () {
		l_abilities = new List<Ability>();
	}
	
	// Update is called once per frame
	void Update () {
//		if (bMoving) {
//			float fTempX = this.transform.position.x;
//			float fTempZ = this.transform.position.z;
//			
//			if (!FloatApproximation(fTempX, vMoveTarget.x, 0.025f)) {
//				if (fTempX < vMoveTarget.x) {
//					fTempX += fSpeed * Time.deltaTime;
//				}
//				else if (fTempX > vMoveTarget.x) {
//					fTempX -= fSpeed * Time.deltaTime;
//				}
//			}
//			else if (!FloatApproximation(fTempZ, vMoveTarget.z, 0.025f)) {
//				if (fTempZ < vMoveTarget.z) {
//					fTempZ += fSpeed * Time.deltaTime;
//				}
//				else if (fTempZ > vMoveTarget.z) {
//					fTempZ -= fSpeed * Time.deltaTime;
//				}
//			}
//			else {
//				bMoving = false;
////				Vector3 vTemp = this.transform.position;
////				vTemp.x = Mathf.Round(vTemp.x);
////				vTemp.z = Mathf.Round(vTemp.z);
//				this.transform.position = vMoveTarget;
//			}
//
//			if (bMoving) {
//				this.transform.position = new Vector3(fTempX, this.transform.position.y, fTempZ);
//			}
		//}
	}

	public virtual void OnSelected() {
		bSelected = true;
	}

	public virtual void OnDeselected() {
		//
	}

	public void MoveTo(Vector3 dest) {
		vMoveTarget = dest;
		bMoving = true;
	}

	private bool FloatApproximation(float a, float b, float tolerance) {
		return (Mathf.Abs(a - b) < tolerance);
	}
}
