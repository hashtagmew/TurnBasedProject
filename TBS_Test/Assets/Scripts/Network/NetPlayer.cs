using UnityEngine;
using System.Collections;

public class NetPlayer : Photon.MonoBehaviour {
	
	private Vector3 vCorrectPos;
	private Quaternion qCorrectRot;

	public AudioClip acSample;

	// Use this for initialization
	void Start () {
		//
	}

	void OnPhotonInstantiate(PhotonMessageInfo info) {
		// e.g. store this gameobject as this player's charater in PhotonPlayer.TagObject
		info.sender.TagObject = this.gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		if (!photonView.isMine) {
			transform.position = Vector3.Lerp(transform.position, this.vCorrectPos, Time.deltaTime);
			transform.rotation = Quaternion.Lerp(transform.rotation, this.qCorrectRot, Time.deltaTime);
		}

		if (Input.GetKeyDown(KeyCode.Alpha1)) {
			PlaySample();
		}

		if (Input.touchCount > 0 || Input.GetMouseButton(0)) {
			this.transform.position = new Vector3(this.transform.position.x + (3 * Time.deltaTime), this.transform.position.y, this.transform.position.z);
		}

		//PC TEST
		if (Input.GetKey(KeyCode.S)) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (-3 * Time.deltaTime));
		}

		if (Input.GetKey(KeyCode.A)) {
			this.transform.position = new Vector3(this.transform.position.x + (-3 * Time.deltaTime), this.transform.position.y, this.transform.position.z);
		}

		if (Input.GetKey(KeyCode.D)) {
			this.transform.position = new Vector3(this.transform.position.x + (3 * Time.deltaTime), this.transform.position.y, this.transform.position.z);
		}

		if (Input.GetKey(KeyCode.W)) {
			this.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z + (3 * Time.deltaTime));
		}
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//Local player
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);
		}
		else {
			//Net player
			this.vCorrectPos = (Vector3)stream.ReceiveNext();
			this.qCorrectRot = (Quaternion)stream.ReceiveNext();
		}
	}

	[PunRPC]
	void PlaySample() {
		this.GetComponent<AudioSource>().PlayOneShot(acSample);
	}

	void OnGUI() {
		if (PhotonNetwork.connectionStateDetailed == PeerState.Joined) {
			if (GUILayout.Button("All")) {
				this.photonView.RPC("PlaySample", PhotonTargets.All);
			}

			if (GUILayout.Button("Remote")) {
				this.photonView.RPC("PlaySample", PhotonTargets.Others);
			}
		}
	}
}
