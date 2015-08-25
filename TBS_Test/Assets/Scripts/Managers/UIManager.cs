using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIManager : MonoBehaviour {
	
	public Dictionary<string, Transform> d_Panels;

	public Transform tUnitPanel;
	public Transform tTerrainPanel;

	public float fSlideSpeed = 0.6f;

	public bool bUnitShow;
	public bool bTerrainShow;
	
	void Start() {
		d_Panels = new Dictionary<string, Transform>();

		if (UIPanelManager.l_PanelList == null) {
			UIPanelManager.Init();
		}

		GetPanels();
	}

	void Update() {
		if (tUnitPanel != null) {
			if (bUnitShow) {
				if (tUnitPanel.position.x < 0) {
					tUnitPanel.position = new Vector3(tUnitPanel.position.x + fSlideSpeed, tUnitPanel.position.y);
				}
			}
			else {
				if (tUnitPanel.position.x > -210) {
					tUnitPanel.position = new Vector3(tUnitPanel.position.x - fSlideSpeed, tUnitPanel.position.y);
				}
			}
		}

		if (tTerrainPanel != null) {
			if (bTerrainShow) {
				if (tTerrainPanel.position.x > Screen.width) {
					tTerrainPanel.position = new Vector3(tTerrainPanel.position.x - fSlideSpeed, tTerrainPanel.position.y);
				}
			}
			else {
				if (tTerrainPanel.position.x < Screen.width + 110) {
					tTerrainPanel.position = new Vector3(tTerrainPanel.position.x + fSlideSpeed, tTerrainPanel.position.y);
				}
			}
		}

		if (tUnitPanel == null && tTerrainPanel == null) {
			GetPanels();
		}
	}

	void GetPanels() {
		foreach (Transform tran in UIPanelManager.l_PanelList) {
			d_Panels.Add(tran.name.Substring(6), tran);
			Debug.Log(tran.name.Substring(6));
		}
		
		tUnitPanel = d_Panels["UnitInfo"];
		tTerrainPanel = d_Panels["Terrain"];
		
		tUnitPanel.transform.gameObject.SetActive(true);
		tTerrainPanel.transform.gameObject.SetActive(true);
	}
}
