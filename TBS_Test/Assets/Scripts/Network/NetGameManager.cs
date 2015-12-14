using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NetGameManager : MonoBehaviour {

	public Text txtTurn;
	public Button uiEndTurn;
	public GameObject DeployPanel;
	public GameObject DeploBut;
	public GameObject ReadyBut;
//	public string CurDeployed;
	public GameObject TileCursor;

	public Button depbut1;
	public Button depbut2;
	public Button depbut3;
	public Button depbut4;
	public Button depbut5;
	public Button depbut6;
	public Button depbut7;

	public UNIT_FACTION eLocalFaction = UNIT_FACTION.NONE;

	//public GameObject goLocalNetPlayer;

	public NetPathMap pathmap;
	public NetGameMap map;
	public TileMap Tmap;

	Ray ray;
	RaycastHit rayHit;

	public List<GameUnit> l_guUnits = new List<GameUnit>();

	public PlayerDeploymentPrefs plyprefs = new PlayerDeploymentPrefs();

	//public GameObject protoUnit;
	
	void Start() {
		plyprefs.LoadPrefs();

		if ((int)PhotonNetwork.player.customProperties["Faction"] == 1) {
			eLocalFaction = UNIT_FACTION.MAGICAL;
		}
		else {
			eLocalFaction = UNIT_FACTION.TECHNOLOGICAL;
		}

		//Tmap.selectedUnit = null;
//		if (PhotonNetwork.player.ID == 1) {
//			foreach (GameUnit unittemppos in l_guUnits) {
//				unittemppos.GetComponent<GameUnit> ().tileX = (int)22;
//				unittemppos.GetComponent<GameUnit> ().tileY = (int)22;
//			}
//		}
//
//		if (PhotonNetwork.player.ID == 2) {
//			foreach (GameUnit unittemppos in l_guUnits) {
//				unittemppos.GetComponent<GameUnit> ().tileX = (int)2;
//				unittemppos.GetComponent<GameUnit> ().tileY = (int)2;
//			}
//		}

		DeployPanel.SetActive (false);

		Debug.Log("NetGameManager loaded... " + ray.ToString());

//		for (int y = 0; y < map.iMapVertSize; y++) {
//			for (int x = 0; x < map.iMapHorzSize; x++) {
//				if (map.GetTile(x, y).l_tfFeatures.Count > 0) {
//					if (map.GetTile(x, y).l_tfFeatures[0].iType == FEATURE_TYPE.TREE) {
//						pathmap.tiles[x, y] = 1;
//					}
//					if (map.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.MOUNTAIN){
//						pathmap.tiles[x,y] = 1;
//					}
//					if (map.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.WALL){
//						pathmap.tiles[x,y] = 1;
//					}
//				}
//			}
//		}
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel("net-main-menu");
		}
		
//		Debug.Log (PhotonNetwork.player.ID);

		//Is it our turn?
//		if (PhotonNetwork.room == null) {
//			Debug.Log("Not sure if our turn, no room is present!");
//		}
//		else {
//			Debug.Log("PAIRS " + PhotonNetwork.room.customProperties.Count.ToString());
//			foreach (DictionaryEntry pair in PhotonNetwork.room.customProperties) {
//				Debug.Log(pair.Key);
//			}
//		}

		if ((bool)PhotonNetwork.player.customProperties ["ReadyDep"] == true) {
			if ((bool)PhotonNetwork.otherPlayers [0].customProperties ["ReadyDep"] == true) {
				DeploBut.SetActive(false);
				ReadyBut.SetActive(false);

				ExitGames.Client.Photon.Hashtable m_PropertiesHash = new ExitGames.Client.Photon.Hashtable();
				m_PropertiesHash.Add("Mode", "play");
				PhotonNetwork.room.SetCustomProperties(m_PropertiesHash);
			}
		}

		if (PhotonNetwork.room != null) {
			if ((int)PhotonNetwork.room.customProperties["Turn"] == PhotonNetwork.player.ID && (string)PhotonNetwork.room.customProperties["Mode"] == "play") {
				uiEndTurn.interactable = true;
				txtTurn.text = "My turn";
			}
			else {
				uiEndTurn.interactable = false;
				txtTurn.text = "Their turn";
			}

//			if (Input.GetKeyDown(KeyCode.P) && !goLocalNetPlayer) {
//				goLocalNetPlayer = (GameObject)PhotonNetwork.Instantiate("NetPlayer", Vector3.zero, Quaternion.identity, 0);
//			}
		}

		//Debug


		if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
//				Debug.Log (PhotonNetwork.player.ID);
//				Photon.MonoBehaviour gotemp = (Photon.MonoBehaviour)rayHit.collider.gameObject;
//
//				if (PhotonNetwork.player.ID == 1 && gotemp.networkView.viewID == 1){
//					Debug.Log("Photon Player Working");
//				}
//			}
		}
	}
	public void DeployUnit(string unit){	
		//Vector3 vectemp = new Vector3 (0.0f, 0.0f, 0.0f);
		GameObject tempunit = (GameObject)PhotonNetwork.Instantiate ("NetGameUnit", TileCursor.transform.position, Quaternion.identity, 0);
		GameUnit tempunit2 = tempunit.GetComponent<GameUnit> ();

		tempunit2.tileX = (int)TileCursor.transform.position.x;
		tempunit2.tileY = (int)TileCursor.transform.position.z;
		
		tempunit2.LoadUnitStats (unit);

		l_guUnits.Add (tempunit2);
		DeployPanel.SetActive (false);
	}

	public void DeployUnit(int num) {
		if (eLocalFaction == UNIT_FACTION.MAGICAL) {
			DeployUnit(plyprefs.l_sMagicalUnits[num].Key);
		}

		if (eLocalFaction == UNIT_FACTION.TECHNOLOGICAL) {
			DeployUnit(plyprefs.l_sMechanicalUnits[num].Key);
		}
	}

	public void SetReady() {
		ExitGames.Client.Photon.Hashtable m_PropertiesHash = PhotonNetwork.player.customProperties;
		m_PropertiesHash["ReadyDep"] = true;
		PhotonNetwork.player.SetCustomProperties(m_PropertiesHash);
	}

	public void UIToggle() {
		DeployPanel.SetActive(!DeployPanel.GetActive());

		if (DeployPanel.GetActive()) {
			if (eLocalFaction == UNIT_FACTION.MAGICAL) {
				//1
				if (plyprefs.l_sMagicalUnits.Count >= 1) {
					depbut1.interactable = true;
				}
				if (depbut1.interactable) {
					depbut1.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[0].Key;
				}
				else {
					depbut1.GetComponentInChildren<Text>().text = "None";
				}

				//2
				if (plyprefs.l_sMagicalUnits.Count >= 2) {
					depbut2.interactable = true;
				}
				if (depbut2.interactable) {
					depbut2.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[1].Key;
				}
				else {
					depbut2.GetComponentInChildren<Text>().text = "None";
				}

				//3
				if (plyprefs.l_sMagicalUnits.Count >= 3) {
					depbut3.interactable = true;
				}
				if (depbut3.interactable) {
					depbut3.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[2].Key;
				}
				else {
					depbut3.GetComponentInChildren<Text>().text = "None";
				}

				//4
				if (plyprefs.l_sMagicalUnits.Count >= 4) {
					depbut4.interactable = true;
				}
				if (depbut4.interactable) {
					depbut4.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[3].Key;
				}
				else {
					depbut4.GetComponentInChildren<Text>().text = "None";
				}

				//5
				if (plyprefs.l_sMagicalUnits.Count >= 5) {
					depbut5.interactable = true;
				}
				if (depbut5.interactable) {
					depbut5.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[4].Key;
				}
				else {
					depbut5.GetComponentInChildren<Text>().text = "None";
				}

				//6
				if (plyprefs.l_sMagicalUnits.Count >= 6) {
					depbut6.interactable = true;
				}
				if (depbut6.interactable) {
					depbut6.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[5].Key;
				}
				else {
					depbut6.GetComponentInChildren<Text>().text = "None";
				}

				//7
				if (plyprefs.l_sMagicalUnits.Count >= 7) {
					depbut7.interactable = true;
				}
				if (depbut7.interactable) {
					depbut7.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[6].Key;
				}
				else {
					depbut7.GetComponentInChildren<Text>().text = "None";
				}
			} 
			else {
				//1
				if (plyprefs.l_sMechanicalUnits.Count >= 1) {
					depbut1.interactable = true;
				}
				if (depbut1.interactable) {
					depbut1.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[0].Key;
				}
				else {
					depbut1.GetComponentInChildren<Text>().text = "None";
				}
				
				//2
				if (plyprefs.l_sMechanicalUnits.Count >= 2) {
					depbut2.interactable = true;
				}
				if (depbut2.interactable) {
					depbut2.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[1].Key;
				}
				else {
					depbut2.GetComponentInChildren<Text>().text = "None";
				}
				
				//3
				if (plyprefs.l_sMechanicalUnits.Count >= 3) {
					depbut3.interactable = true;
				}
				if (depbut3.interactable) {
					depbut3.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[2].Key;
				}
				else {
					depbut3.GetComponentInChildren<Text>().text = "None";
				}
				
				//4
				if (plyprefs.l_sMechanicalUnits.Count >= 4) {
					depbut4.interactable = true;
				}
				if (depbut4.interactable) {
					depbut4.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[3].Key;
				}
				else {
					depbut4.GetComponentInChildren<Text>().text = "None";
				}
				
				//5
				if (plyprefs.l_sMechanicalUnits.Count >= 5) {
					depbut5.interactable = true;
				}
				if (depbut5.interactable) {
					depbut5.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[4].Key;
				}
				else {
					depbut5.GetComponentInChildren<Text>().text = "None";
				}
				
				//6
				if (plyprefs.l_sMechanicalUnits.Count >= 6) {
					depbut6.interactable = true;
				}
				if (depbut6.interactable) {
					depbut6.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[5].Key;
				}
				else {
					depbut6.GetComponentInChildren<Text>().text = "None";
				}
				
				//7
				if (plyprefs.l_sMechanicalUnits.Count >= 7) {
					depbut7.interactable = true;
				}
				if (depbut7.interactable) {
					depbut7.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[6].Key;
				}
				else {
					depbut7.GetComponentInChildren<Text>().text = "None";
				}
			}
		}
	}

	public void NetEndTurn() {

		if (l_guUnits.Count > 0) {
			foreach (GameUnit unittemp in l_guUnits) {
				unittemp.GetComponent<GameUnit> ().currentPath = null;
				unittemp.GetComponent<GameUnit> ().remainingMovement = unittemp.GetComponent<GameUnit> ().remainingMovement;
			}
		}
//		tempunit.GetComponent<Unit>().currentPath = null;
//		tempunit.GetComponent<Unit> ().remainingMovement = tempunit.GetComponent<Unit> ().resetMovement;

//		l_guUnits[0].GetComponent<Unit>().currentPath = null;
//		//remainingMovement = resetMovement;
//		l_guUnits[0].GetComponent<Unit> ().remainingMovement = Tmap.selectedUnit.GetComponent<Unit> ().resetMovement;

		if (PhotonNetwork.playerList.Length != 0) {
			int newturn = 0;
			newturn = PhotonNetwork.otherPlayers[0].ID;

			ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable ();
			hash.Add ("Turn", newturn);
			PhotonNetwork.room.SetCustomProperties (hash);
		}
	}
}
