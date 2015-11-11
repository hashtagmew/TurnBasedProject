//using UnityEngine;
//using System.Collections;
//
//using UnityEngine.UI;
//using UnityEngine.Networking;
//
//public class GameServer : MonoBehaviour {
//
//	public Text txtServers;
//	public NetworkManager netmng;
//	//private HostData[] netServerList;
//	//public NetworkConnectionError netError;
//
//	void Start() {
//		//
//	}
//
//	public void StartServer() {
//		netmng.StartHost();
//	}
//
//	public void StartClient() {
//		netmng.ServerChangeScene("net-client");
//	}
//
//	void Update() {
////		if (MasterServer.PollHostList().Length != 0) {
////			HostData[] hostData = MasterServer.PollHostList();
////			int i = 0;
////			while (i < hostData.Length) {
////				txtServers.text = txtServers.text + "Game name: " + hostData[i].gameName + "\n";
////				i++;
////			}
////			MasterServer.ClearHostList();
////		}
//	}
//}
