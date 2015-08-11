using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

//Based on Zach Sutton's code, used with permission

static public class UIPanelManager {

	static public List<Transform> l_PanelList { get; private set; }

	static public Transform tCurrentPanel { get; private set; }
	static public Stack<Transform> st_PreviousPanel { get; private set; }

	static Transform tCanvas;


	static public void Init() {
		tCanvas = GameObject.Find("Canvas").transform;

		st_PreviousPanel = new Stack<Transform>();
		l_PanelList = new List<Transform>();
		
		foreach (Transform chd in tCanvas.gameObject.GetComponentsInChildren<Transform>(true)) {
			if ((chd.gameObject != tCanvas.gameObject) && chd.name.ToLower().Contains("panel_")) {
				l_PanelList.Add(chd);
				chd.gameObject.SetActive(false);
			}
		}

		tCurrentPanel = null;
	}

	static public void OpenPanel(string newPanel) {
		if (l_PanelList.Exists (x => x.name.Contains ("_" + newPanel))) {
			if (tCurrentPanel != null) {
				tCurrentPanel.gameObject.SetActive(false);
				st_PreviousPanel.Push(tCurrentPanel);
			}
			tCurrentPanel = l_PanelList.Find(x => x.name.Contains("_" + newPanel));
			tCurrentPanel.gameObject.SetActive(true);
			SetBackButton();
		}
		else {
			if (newPanel != "") {
				Debug.LogError(newPanel + " does not exist as a UI Panel!");
			}
		}
	}

	static public void PreviousPanel() {
		tCurrentPanel.gameObject.SetActive(false);
		tCurrentPanel = st_PreviousPanel.Pop();
		tCurrentPanel.gameObject.SetActive(true);
		SetBackButton();
	}

	static public void SetBackButton() {
		foreach (Transform butt in tCurrentPanel.GetComponentsInChildren<Transform>(true)) {
			if (butt.name == "button_Back") {
				butt.gameObject.SetActive(st_PreviousPanel.Count == 0 ? false : true);
			}
		}
	}

	static public Transform getUIElementOnPanel(string name, bool includeInactive = false) {
		if (tCurrentPanel != null) {
			List<Transform> lTemp = new List<Transform> (tCurrentPanel.GetComponentsInChildren<Transform> (includeInactive));
			if (lTemp.Exists (x => x.name.Contains ("_" + name))) {
				return lTemp.Find (x => x.name.Contains ("_" + name));
			} 
			else {
				Debug.LogError(name + " does not Exist as a UI Element in this Panel");
				return null;
			}
		} 
		else {
			Debug.LogError("No Active Panel");
			return null;
		}
	}

	static public Transform[] getUIElementsOfPrefix(string prefix, bool includeInactive = false) {
		if (tCurrentPanel != null) {
			List<Transform> lTemp = new List<Transform>(tCurrentPanel.GetComponentsInChildren<Transform>(includeInactive));
			if (lTemp.Exists (x => x.name.Contains(prefix + "_"))) {
				return lTemp.FindAll (x => x.name.Contains(prefix + "_")).ToArray();
			} 
			else {
				Debug.LogError(prefix + " does not Exist as a UI Element in this Panel");
				return null;
			}

		} 
		else {
			Debug.LogError("No Active Panel");
			return null;
		}
	}

	static public Transform InstantiateGUIElement(string name, string parentPanel = "") {
		GameObject tempObj = Resources.Load<GameObject> ("Prefabs/" + name);

		if (tempObj != null) {
			GameObject createdObj = GameObject.Instantiate<GameObject>(tempObj);
			if (parentPanel == "") {
				createdObj.transform.SetParent(tCurrentPanel, false);
			} 
			else {
				createdObj.transform.SetParent(getUIElementOnPanel(parentPanel), false);
			}
			return createdObj.transform;
		} 
		else {
			Debug.LogError("ERROR: Does Not Exist");
			return null;
		}
	}

	static public bool setButtonUsable (string buttonName, bool interactable) {
		if (tCurrentPanel.GetComponentsInChildren<Button> ().Length != 0) {
			List<Transform> tempList = new List<Transform>(tCurrentPanel.GetComponentsInChildren<Transform>());
			tempList.RemoveAll (x => x.gameObject.GetComponent<Button>() == null && !x.gameObject.name.Contains(buttonName));
			if (tempList.Count > 0) {
				foreach (Transform t in tempList) {
					t.gameObject.GetComponent<Button>().interactable = interactable;
				}
				return true;
			}
		}
		Debug.LogError("Error: No Button called " + buttonName + " in this panel");
		return false;
	}
}