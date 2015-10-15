using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;
using System.Linq;

using System.IO;

public class UIUnitSelectorArea : MonoBehaviour {

	SortedList<int, Text> sl_txtLines;
	SortedList<int, string> sl_sFiles;
	public Text sPage;
	public int iPage;

	public Scrollbar uiScrollbar;

	bool bChanged = true;
	
	void Start() {
		sl_txtLines = new SortedList<int, Text>();
		sl_sFiles = new SortedList<int, string>();

		//Load all the "line" ui texts
		for (int i = 0; i < this.transform.childCount; i++) {
			if (this.transform.GetChild(i).name.Substring(0, 5) == "Line ") {
				sl_txtLines.Add(i, this.transform.GetChild(i).GetComponent<Text>());
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
			UpdateTextLines();
//			foreach (KeyValuePair<int, Text> pair in sl_txtLines) {
//				Debug.Log(pair.ToString());
//			}
			bChanged = false;
		}
	}

	void LoadFiles() {
		DirectoryInfo dirInfo = new DirectoryInfo(Application.dataPath + "/Resources/UnitFiles/");
		FileInfo[] fileInfo = dirInfo.GetFiles("*.xml");

		sl_sFiles.Clear();

		int f = 0;

		foreach (FileInfo curfile in fileInfo) {
			sl_sFiles.Add(f, curfile.Name);
			f++;
		}

		if (sl_sFiles.Count == 0) {
			Debug.LogError("No unit files found!");
			
			return;
		}

		uiScrollbar.numberOfSteps = (int)System.Math.Ceiling(sl_sFiles.Count / 5f);

		bChanged = true;
	}

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
		//
	}
	
	public void LoadDefender() {
		//
	}
}
