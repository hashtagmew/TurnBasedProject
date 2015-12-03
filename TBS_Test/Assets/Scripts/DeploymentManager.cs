using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

public class DeploymentManager : MonoBehaviour {

	public Slider sldFaction;

	public Image imgUnitPicture;

	public Text txtUnitStats;
	public Text txtUnitName;
	public Text txtUnitDeployCost;
	public Text txtUnitSlot1;
	public Text txtUnitySlot2;
	public Text txtUnitySlot3;

	//Unit bits
	public string sName = "";
	public float fHealth = 0.0f;
	public float fMaxHealth = 0.0f;
	public float fPhysAttack = 0.0f;
//	public float fRangAttack = 0.0f;
//	public float fMagiAttack = 0.0f;
	public float fDefence = 0.0f;
	public float fResistance = 0.0f;

	public float fDeployCost = 0.0f;
	
	void Start() {
		LoadUnitStats("infantry_footman");
	}

	void Update() {
		txtUnitStats.text = "HTP: " + ((int)fMaxHealth).ToString("D3") + "\tATK: " + ((int)fPhysAttack).ToString("D3") + "\n" +
			     			"DEF: " + ((int)fDefence).ToString("D3") + "\tRES: " + ((int)fResistance).ToString("D3");
		txtUnitName.text = sName;
		txtUnitDeployCost.text = fDeployCost.ToString();
		//txtUnitSlot1.text = sName;
		//txtUnitySlot2.text = sName;
		//txtUnitySlot3.text = sName;
	}

	public void PrevItem(int slot) {
		//
	}

	public void NextItem(int slot) {
		//
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
//				else if (xlayer1.Name == "description") {
//					sDescription = xlayer1.Value;
//				}
//				else if (xlayer1.Name == "ap") {
//					fAP = float.Parse(xlayer1.Value);
//					fMaxAP = fAP;
//				}
				else if (xlayer1.Name == "health") {
					fHealth = float.Parse(xlayer1.Value);
					fMaxHealth = fHealth;
				}
				else if (xlayer1.Name == "deploycost") {
					fDeployCost = float.Parse(xlayer1.Value);
				}
//				else if (xlayer1.Name == "move") {
//					fMovement = float.Parse(xlayer1.Value);
//					fMaxMovement = fMovement;
//				}
//				else if (xlayer1.Name == "vision") {
//					fVision = float.Parse(xlayer1.Value);
//				}
				else if (xlayer1.Name == "attack") {
					fPhysAttack = float.Parse(xlayer1.Value);
				}
//				else if (xlayer1.Name == "physattack") {
//					fPhysAttack = float.Parse(xlayer1.Value);
//				}
//				else if (xlayer1.Name == "rangattack") {
//					fRangAttack = float.Parse(xlayer1.Value);
//				}
//				else if (xlayer1.Name == "magattack") {
//					fMagiAttack = float.Parse(xlayer1.Value);
//				}
				else if (xlayer1.Name == "defence") {
					fDefence = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "resistance") {
					fResistance = float.Parse(xlayer1.Value);
				}
			}
		}
		
		return true;
	}
}
