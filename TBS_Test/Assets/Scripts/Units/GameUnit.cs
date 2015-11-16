using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

public class GameUnit : MonoBehaviour, ISelectable {

	public UNIT_FACTION iFaction;

	public Vector2 vGridPosition;
	public Vector3 vMoveTarget;
	public bool bMoving = false;

	public bool bSelected = false;

	public string sName;
	public string sDescription;

	public float fAP;
	public float fMaxAP;

	public float fHealth;
	public float fMaxHealth;
	//public float fMana;
	public float fMovement;
	public float fMaxMovement;
	public float fVision;
	//public float fVision;
	public float fSpeed = 1.5f;

	public float fAttack;
	public float fPhysAttack;
	public float fRangAttack;
	public float fMagiAttack;

	public float fResistance;
	public float fDefence;

	public List<Ability> l_abilities;
	public Dictionary<ABILITY_ELEMENT, float> d_efElementalResistances {
		get;
		private set;
	}

	// Use this for initialization
	void Start () {
		l_abilities = new List<Ability>();
		d_efElementalResistances = new Dictionary<ABILITY_ELEMENT, float>();

		d_efElementalResistances.Add(ABILITY_ELEMENT.KINETIC, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.MAGIC, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.LIGHT, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.DARK, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.EARTH, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.WATER, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.AIR, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.FIRE, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.ELECTRIC, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.ICE, 1.0f);

		fAP = fMaxAP;
		fHealth = fMaxHealth;
		fMovement = fMaxMovement;

		LoadUnitStats(sName);
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

	void SetResistance(ABILITY_ELEMENT element, float resistpercent) {
		//resistpercent = Mathf.Clamp(resistpercent, -1.0f, 4.0f);
		d_efElementalResistances[element] = resistpercent;
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

	public bool LoadUnitStats(string path) {
		TextAsset taUnit = (TextAsset)Resources.Load<TextAsset>("UnitFiles/" + path);

		if (taUnit == null) {
			Debug.LogWarning("The unit file \"" + path + "\" was not found.");
			return false;
		}

		XDocument xmlDoc = XDocument.Load(new StringReader(taUnit.text));

		foreach (XElement xroot in xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					sName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					sDescription = xlayer1.Value;
				}
				else if (xlayer1.Name == "ap") {
					fAP = float.Parse(xlayer1.Value);
					fMaxAP = fAP;
				}
				else if (xlayer1.Name == "health") {
					fHealth = float.Parse(xlayer1.Value);
					fMaxHealth = fHealth;
				}
				else if (xlayer1.Name == "move") {
					fMovement = float.Parse(xlayer1.Value);
					fMaxMovement = fMovement;
				}
				else if (xlayer1.Name == "vision") {
					fVision = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "physattack") {
					fPhysAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "rangattack") {
					fRangAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "magattack") {
					fMagiAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "defence") {
					fDefence = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "resistance") {
					fResistance = float.Parse(xlayer1.Value);
				}
				
//				if (xlayer1.Name == "abilities") {
//					//Make sure only the in-use ones are ticked
//					//					foreach (KeyValuePair<string, bool> pair in s_dAbilityToggles) {
//					//						pair. = false;
//					//					}
//					ReloadAbilities();
//					
//					foreach (XElement xlayer2 in xlayer1.Elements()) {
//						if (AbilityBox.s_dAbilityLookup.ContainsKey(xlayer2.Value)) {
//							s_dAbilityToggles[xlayer2.Value] = true;
//							if (AbilityBox.s_dAbilityLookup[xlayer2.Value].fIntensity > 0) {
//								s_dAbilityPower[xlayer2.Value] = float.Parse(xlayer2.FirstAttribute.Value);
//							}
//						}
//					}
//				}
			}
		}

		return true;
	}

	public void LoadUnitAbilities(string path) {
		//
	}
}
