using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class NetGameManager : MonoBehaviour {

	public Text txtTurn;
	public Button uiEndTurn;

	public GameObject goLocalNetPlayer;

	
	void Start() {
		//
	}
	
	// Update is called once per frame
	void Update() {
		//Is it our turn?
		if (PhotonNetwork.room == null) {
			Debug.Log("NO ROOM");
		}
		else {
			Debug.Log("PAIRS " + PhotonNetwork.room.customProperties.Count.ToString());
			foreach (DictionaryEntry pair in PhotonNetwork.room.customProperties) {
				Debug.Log(pair.Key);
			}
		}

		if ((int)PhotonNetwork.room.customProperties["Turn"] == PhotonNetwork.player.ID) {
			uiEndTurn.interactable = true;
			txtTurn.text = "My turn";
		}
		else {
			uiEndTurn.interactable = false;
			txtTurn.text = "Their turn";
		}

		if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel("net-test");
		}

		if (Input.GetKeyDown(KeyCode.P) && !goLocalNetPlayer) {
			goLocalNetPlayer = (GameObject)PhotonNetwork.Instantiate("NetPlayer", Vector3.zero, Quaternion.identity, 0);
		}
	}

	public void NetEndTurn() {
		int newturn = PhotonNetwork.otherPlayers[0].ID;

		ExitGames.Client.Photon.Hashtable hash = new ExitGames.Client.Photon.Hashtable();
		hash.Add("Turn", newturn);
		PhotonNetwork.room.SetCustomProperties(hash);
	}
}
