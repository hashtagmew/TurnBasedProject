using UnityEngine;
using System.Collections;

public class NetGameManager : MonoBehaviour {

	public GameObject goLocalNetPlayer;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.F1)) {
			Application.LoadLevel("net-test");
		}

		if (Input.GetKeyDown(KeyCode.P) && !goLocalNetPlayer) {
			goLocalNetPlayer = (GameObject)PhotonNetwork.Instantiate("NetPlayer", Vector3.zero, Quaternion.identity, 0);
		}
	}
}
