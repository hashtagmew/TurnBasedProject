using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class NetGameManager : MonoBehaviour {

	public Text txtTurn;
	public Button uiEndTurn;
	public GameObject DeployPanel;
//	public string CurDeployed;

	//public GameObject goLocalNetPlayer;

	public NetPathMap pathmap;
	public NetGameMap map;
	public TileMap Tmap;

	Ray ray;
	RaycastHit rayHit;

	public List<GameUnit> l_guUnits;

	//public GameObject protoUnit;
	
	void Start() {

		if (PhotonNetwork.player.ID == 1) {
			foreach (GameUnit unittemppos in l_guUnits) {
				unittemppos.GetComponent<GameUnit> ().tileX = (int)24;
				unittemppos.GetComponent<GameUnit> ().tileY = (int)18;
			}
		}

		if (PhotonNetwork.player.ID == 2) {
			foreach (GameUnit unittemppos in l_guUnits) {
				unittemppos.GetComponent<GameUnit> ().tileX = (int)2;
				unittemppos.GetComponent<GameUnit> ().tileY = (int)2;
			}
		}

		DeployPanel.SetActive (false);

		for (int y = 0; y < map.iMapVertSize; y++) {
			for (int x = 0; x < map.iMapHorzSize; x++) {
				if (map.GetTile(x, y).l_tfFeatures.Count > 0) {
					if (map.GetTile(x, y).l_tfFeatures[0].iType == FEATURE_TYPE.TREE) {
						pathmap.tiles[x, y] = 1;
					}
					if (map.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.MOUNTAIN){
						pathmap.tiles[x,y] = 1;
					}
					if (map.GetTile(x,y).l_tfFeatures[0].iType == FEATURE_TYPE.WALL){
						pathmap.tiles[x,y] = 1;
					}
				}
			}
		}
	}
	
	// Update is called once per frame
	void Update() {
		Debug.Log (PhotonNetwork.player.ID);

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

		if (PhotonNetwork.room != null) {
			if ((int)PhotonNetwork.room.customProperties["Turn"] == PhotonNetwork.player.ID) {
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
		if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel("net-test");
		}

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
		Vector3 vectemp = new Vector3(0.0f, 0.0f, 0.0f);
		GameObject tempunit = (GameObject)PhotonNetwork.Instantiate("NetGameUnit", vectemp, Quaternion.identity, 0);
		GameUnit tempunit2 = tempunit.GetComponent<GameUnit>();
		tempunit2.LoadUnitStats (unit);
//		tempunit2.tileX = (int)tempunit2.transform.position.x;
//		tempunit2.tileY = (int)tempunit2.transform.position.z;
		l_guUnits.Add(tempunit2);
		DeployPanel.SetActive (false);
	}

	public void UIappear(){
		DeployPanel.SetActive (true);
	}

	public void NetEndTurn() {

		foreach (GameUnit unittemp in l_guUnits) {
			unittemp.GetComponent<GameUnit>().currentPath = null;
			unittemp.GetComponent<GameUnit>().remainingMovement = unittemp.GetComponent<GameUnit>().remainingMovement;
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
