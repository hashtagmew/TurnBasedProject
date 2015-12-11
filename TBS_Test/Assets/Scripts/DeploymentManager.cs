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

	public Text DepText1;
	public Text DepText2;
	public Text DepText3;
	public Text DepText4;
	public Text DepText5;
	public Text DepText6;
	public Text DepText7;

	public Text txtUnitStats;
	public Text txtUnitName;
	public Text txtUnitDeployCost;
	public Text txtUnitSlot1;
	public Text txtUnitySlot2;
	public Text txtUnitySlot3;
	public Text txtTotalUnits;
	public Text txtDeployPoints;

	public List<TextAsset> l_sUnitNames = new List<TextAsset>();
	public int iListPos = 0;

	public PlayerDeploymentPrefs plyprefs = new PlayerDeploymentPrefs();

	//Unit bits
	public string sName = "";
	public float fHealth = 0.0f;
	public float fMaxHealth = 0.0f;
	public float fPhysAttack = 0.0f;
//	public float fRangAttack = 0.0f;
//	public float fMagiAttack = 0.0f;
	public float fDefence = 0.0f;
	public float fResistance = 0.0f;
	public UNIT_FACTION eFaction = UNIT_FACTION.NONE;

	public int iDeployCost = 0;
	public int iDeployPoints = 7;

	public bool bMenu = false;
	
	void Start() {
		ReloadUnitList ();
		LoadCurrentUnitSlot ();
		plyprefs.LoadPrefs ();
		RecalculatePoints ();
		//LoadUnitStats("infantry_footman");
	}

	void Update() {
		if (bMenu) {
			txtUnitStats.text = "HTP: " + ((int)fMaxHealth).ToString ("D3") + "\tATK: " + ((int)fPhysAttack).ToString ("D3") + "\n" +
				"DEF: " + ((int)fDefence).ToString ("D3") + "\tRES: " + ((int)fResistance).ToString ("D3");
			txtUnitName.text = sName;
			txtUnitDeployCost.text = "Deploy Cost: " + iDeployCost.ToString ();
			txtDeployPoints.text = "Deploy Points: " + iDeployPoints.ToString ();

			txtTotalUnits.text = "";
			//Magical
			if (sldFaction.value == 1f) {
				foreach (KeyValuePair<string, int> str in plyprefs.l_sMagicalUnits) {
					txtTotalUnits.text += str.Key;
					txtTotalUnits.text += "\t";
				}
			}
			//Mech
			if (sldFaction.value == 2f) {
				foreach (KeyValuePair<string, int> str in plyprefs.l_sMechanicalUnits) {
					txtTotalUnits.text += str.Key;
					txtTotalUnits.text += "\t";
				}
			}
			//txtUnitSlot1.text = sName;
			//txtUnitySlot2.text = sName;
			//txtUnitySlot3.text = sName;
		}
	}

	public void RecalculatePoints() {
		iDeployPoints = 7;

		//Magical
		if (sldFaction.value == 1f) {
			foreach (KeyValuePair<string, int> pair in plyprefs.l_sMagicalUnits) {
				iDeployPoints -= pair.Value;
			}
		}
		//Mech
		if (sldFaction.value == 2f) {
			foreach (KeyValuePair<string, int> pair in plyprefs.l_sMechanicalUnits) {
				iDeployPoints -= pair.Value;
			}
		}
	}

	public void ReloadUnitList() {
		TextAsset[] names = Resources.LoadAll<TextAsset> ("UnitFiles/");

		iListPos = 0;
		l_sUnitNames.Clear ();
		l_sUnitNames.AddRange (names);

		//Purge wrong faction info
		//1 MAGICAL		2 MECH
		if (sldFaction.value == 1) {
			for (int i = l_sUnitNames.Count - 1; i > -1; i--) {
				if (!l_sUnitNames[i].text.Contains("<faction>3")) {
					if (!l_sUnitNames[i].text.Contains("<faction>6")) {
						l_sUnitNames.RemoveAt(i);
					}
				}
			}
		} else if (sldFaction.value == 2) {
			for (int i = l_sUnitNames.Count - 1; i > -1; i--) {
				if (!l_sUnitNames[i].text.Contains("<faction>4")) {
					if (!l_sUnitNames[i].text.Contains("<faction>6")) {
						l_sUnitNames.RemoveAt(i);
					}
				}
			}
		}
	}

	public void SavePrefs() {
		plyprefs.SavePrefs ();
	}

	public void LoadCurrentUnitSlot() {
		XDocument xmlDoc = XDocument.Load(new StringReader(l_sUnitNames[iListPos].text));

		foreach (XElement xroot in xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					sName = xlayer1.Value;
				}
				else if (xlayer1.Name == "deploycost") {
					iDeployCost = int.Parse(xlayer1.Value);
					Debug.Log("COST: " + iDeployCost.ToString());
				}
				else if (xlayer1.Name == "faction") {
					eFaction = (UNIT_FACTION)int.Parse(xlayer1.Value);
				}
			}
		}
	}

	public void PrevItem(int slot) {
		if (slot == 0) {
			iListPos--;
			iListPos = Mathf.Clamp(iListPos, 0, l_sUnitNames.Count - 1);

			LoadCurrentUnitSlot();
		}
	}

	public void NextItem(int slot) {
		if (slot == 0) {
			iListPos++;
			iListPos = Mathf.Clamp(iListPos, 0, l_sUnitNames.Count - 1);

			LoadCurrentUnitSlot();
		}
	}

	public void AddUnitToList() {
		if (l_sUnitNames [iListPos].name.Contains ("Commander") || l_sUnitNames [iListPos].name.Contains ("commander")) {
			return;
		}

		if (iDeployCost > iDeployPoints) {
			return;
		} else {
			iDeployPoints -= iDeployCost;
		}

		if (sldFaction.value == 1) {
			plyprefs.l_sMagicalUnits.Add(new KeyValuePair<string, int>(l_sUnitNames[iListPos].name, iDeployCost));
		}

		if (sldFaction.value == 2) {
			plyprefs.l_sMechanicalUnits.Add(new KeyValuePair<string, int>(l_sUnitNames[iListPos].name, iDeployCost));
		}
	}

	public void RemoveUnitFromList() {
		if (l_sUnitNames [iListPos].name.Contains ("Commander") || l_sUnitNames [iListPos].name.Contains ("commander")) {
			return;
		}

		if (sldFaction.value == 1) {
			if (plyprefs.l_sMagicalUnits.Contains(new KeyValuePair<string, int>(l_sUnitNames[iListPos].name, iDeployCost))) {
				plyprefs.l_sMagicalUnits.Remove(new KeyValuePair<string, int>(l_sUnitNames[iListPos].name, iDeployCost));
				iDeployPoints += iDeployCost;
			}
		}

		if (sldFaction.value == 2) {
			if (plyprefs.l_sMechanicalUnits.Contains(new KeyValuePair<string, int>(l_sUnitNames[iListPos].name, iDeployCost))) {
				plyprefs.l_sMechanicalUnits.Remove(new KeyValuePair<string, int>(l_sUnitNames[iListPos].name, iDeployCost));
				iDeployPoints += iDeployCost;
			}
		}
	}

	public void LoadDeploymentUnits(){
		DepText1.text = l_sUnitNames [0].name.ToString();
		DepText2.text = l_sUnitNames [1].name.ToString();
		DepText3.text = l_sUnitNames [2].name.ToString();
		DepText4.text = l_sUnitNames [3].name.ToString();
		DepText5.text = l_sUnitNames [4].name.ToString();
		DepText6.text = l_sUnitNames [5].name.ToString();
		DepText7.text = l_sUnitNames [6].name.ToString();
		
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
					iDeployCost = int.Parse(xlayer1.Value);
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
