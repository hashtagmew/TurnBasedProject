﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetGameManager : MonoBehaviour {

	public Text txtTurn;
	public Button uiEndTurn;

	//public GameObject goLocalNetPlayer;

	public NetPathMap pathmap;
	public NetGameMap map;

	public List<GameUnit> l_guUnits;

	//public GameObject protoUnit;
	
	void Start() {
		//
	}
	
	// Update is called once per frame
	void Update() {
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

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			Vector3 vectemp = new Vector3(0.0f, 0.0f, 0.0f);
			GameObject tempunit = (GameObject)PhotonNetwork.Instantiate("NetGameUnit", vectemp, Quaternion.identity, 0);
			GameUnit tempunit2 = tempunit.GetComponent<GameUnit>();
			l_guUnits.Add(tempunit2);
			tempunit2.tileX = (int)tempunit2.transform.position.x;
			tempunit2.tileY = (int)tempunit2.transform.position.y;
		}
	}

	public void NetEndTurn() {
		int newturn = PhotonNetwork.otherPlayers[0].ID;

		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
		hash.Add("Turn", newturn);
		PhotonNetwork.room.SetCustomProperties(hash);
	}
}
