using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using System.Linq;

using System.Xml;
using System.Xml.Linq;

using System.IO;

public class UIUnitSelectorArea : MonoBehaviour {

	SortedList<int, Text> sl_txtLines;
	SortedList<int, string> sl_sFiles;
	//SortedList<int, GameObject> sl_goUnits;
	public Text sPage;
	public int iPage;

	public GameUnit guAttacker;
	public GameUnit guDefender;

	public int iSelection;

	public Scrollbar uiScrollbar;

	bool bChanged = true;
	
	void Start() {
		sl_txtLines = new SortedList<int, Text>();
		sl_sFiles = new SortedList<int, string>();
		//sl_goUnits = new SortedList<int, GameObject>();

		//Load all the "line" ui texts
		for (int i = 0; i < this.transform.childCount; i++) {
			if (this.transform.GetChild(i).name.Substring(0, 5) == "Line ") {
				sl_txtLines.Add(i, this.transform.GetChild(i).GetComponent<Text>());
				//sl_goUnits.Add(i, 
			}
		}

		LoadFiles();
	}

	void Update() {
		iPage = uiScrollbar.numberOfSteps - ((int)(uiScrollbar.value * uiScrollbar.numberOfSteps));
		if (iPage == 0) {
			iPage = 1;
		}
		sPage.text = iPage.ToString() + "/" + uiScrollbar.numberOfSteps.ToString();

		if (bChanged) {
			iSelection = 0;
			UpdateTextLines();
			bChanged = false;
		}

		if (iSelection != 0) {
			foreach (KeyValuePair<int, Text> pair in sl_txtLines) {
				pair.Value.color = Color.black;
			}

			sl_txtLines[iSelection].color = Color.white;
		}
		else {
			foreach (KeyValuePair<int, Text> pair in sl_txtLines) {
				pair.Value.color = Color.black;
			}
		}
	}

	void LoadFiles() {
		Object[] goTemp = Resources.LoadAll("UnitFiles");
		
		sl_sFiles.Clear();
		
		for (int i = 0; i < goTemp.Length; i++) {
			sl_sFiles.Add(i, goTemp[i].name);
		}
		
		if (sl_sFiles.Count == 0) {
			Debug.LogError("No unit files found!");
			
			return;
		}
		
		uiScrollbar.numberOfSteps = (int)System.Math.Ceiling(sl_sFiles.Count / 5f);
		
		bChanged = true;
	}

//	void LoadFiles() {
//		DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Resources/UnitFiles/");
//		FileInfo[] fileInfo = dirInfo.GetFiles("*.xml");
//
//		sl_sFiles.Clear();
//
//		int f = 0;
//
//		foreach (FileInfo curfile in fileInfo) {
//			sl_sFiles.Add(f, curfile.Name);
//			f++;
//		}
//
//		if (sl_sFiles.Count == 0) {
//			Debug.LogError("No unit files found!");
//			
//			return;
//		}
//
//		uiScrollbar.numberOfSteps = (int)System.Math.Ceiling(sl_sFiles.Count / 5f);
//
//		bChanged = true;
//	}

	public void UpdateTextLines() {
		int start = (iPage - 1) * 5;
		int line = 1;
		string stemp = "";

		for (int i = start; i < start + 5; i++) {
			if (sl_sFiles.TryGetValue(i, out stemp)) {
				sl_txtLines[line].text = stemp;
			}
			else {
				sl_txtLines[line].text = "<none>";
			}

			line++;
		}
	}

	public void LoadAttacker() {
		LoadUnit(true);
	}
	
	public void LoadDefender() {
		LoadUnit(false);
	}

	private void LoadUnit(bool isattacker) {
		GameUnit guTemp;

		if (isattacker) {
			guTemp = guAttacker;
		}
		else {
			guTemp = guDefender;
		}

		XDocument s_xmlDoc = XDocument.Load(Application.dataPath + "/Resources/UnitFiles/" + sl_sFiles[iSelection] + ".xml");
		
		foreach (XElement xroot in s_xmlDoc.Elements()) {
			//Debug.Log(xroot.Name);
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					guTemp.sName = xlayer1.Value;
				}
//				else if (xlayer1.Name == "description") {
//					guTemp.d = xlayer1.Value;
//				}
				else if (xlayer1.Name == "ap") {
					guTemp.fAP = float.Parse(xlayer1.Value);
					guTemp.fMaxAP = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "maxhealth") {
					guTemp.fHealth = float.Parse(xlayer1.Value);
					guTemp.fMaxHealth = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "physattack") {
					guTemp.fPhysAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "rangattack") {
					guTemp.fPhysAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "magiattack") {
					guTemp.fPhysAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "defence") {
					guTemp.fDefence = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "resistance") {
					guTemp.fResistance = float.Parse(xlayer1.Value);
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
	}

	public void SelectLine(int line) {
		iSelection = line;
	}
}
