using UnityEngine;
using System.Collections;

//Based on Zach Sutton's code, used with permission

public class UICanvasWrapper : MonoBehaviour {

	public string sStartingPanel;

	void Start() {
		UIPanelManager.Init();
		UIPanelManager.OpenPanel(sStartingPanel);
	}

	void Update() {
	
	}

	public void OpenPanel(string name) {
		UIPanelManager.OpenPanel(name);
	}

	public void PreviousPanel() {
		UIPanelManager.PreviousPanel();
	}

	public void ChangeScene(string name) {
		Application.LoadLevel(name);
	}
}
