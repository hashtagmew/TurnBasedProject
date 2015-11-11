//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//
//using UnityEngine.UI;
//using UnityEngine.Networking;
//
//public class CustomNetworkManager : NetworkLobbyManager
//{
//	//public NetworkLobbyManager m_NetMng;
//	public string m_ssLobbyName {get; set;}
//	public string m_ssPlayerName {get;set;}
//	public string m_ssLocalAddress = "";
//
//	public List<Vector2> m_lGUIPositions;
//	public int m_iConnectedPlayers = 0;
//
//	public GameObject m_LobbyGUIPrefab;
//
//	public Text txtDebug;
//	public string sLastError;
//
//	List<string> m_lLocalNetworkAddresses = new List<string>();
//
//
//	bool m_bScanning = false;
//	bool m_bNewScan = false;
//
//	string m_ssLocalNetwork;
//
//	public int m_iLocalAddress;
//		
//
//	void Start ()
//	{
//		m_lGUIPositions = new List<Vector2> ();
//		m_lGUIPositions.AddRange (new Vector2[] {
//			new Vector2 (-100, 40),
//			new Vector2 (100, 40),
//			new Vector2 (-100, -80),
//			new Vector2 (100, -80)
//		});
//		m_ssLocalAddress = Network.player.ipAddress;
//		this.networkAddress = m_ssLocalAddress;
//
//		setLocalNetwork();
//		LocalNetworkScan();
//	}
//
//	public void OnError(NetworkMessage msg)
//	{
//		Debug.Log("\t\tERROR " + MsgType.MsgTypeToString(msg.msgType) + " : " + msg.reader.ReadBytesAndSize());
//		txtDebug.text = "ERROR " + MsgType.MsgTypeToString(msg.msgType)+ ": ";
//
//		if (m_bScanning)
//		{
//			m_bNewScan = true;
//		}
//	}
//	public void OnConnect(NetworkMessage msg)
//	{
//		Debug.Log("\t\tConnected");
//		txtDebug.text = "Connected";
//		m_bScanning = false;
//
//		UIPanelManager.OpenPanel("ClientConnected");
//	}
//	public void OnDisconnect(NetworkMessage msg)
//	{
//		Debug.Log("\t\tDisconnected");
//		//txtDebug.text = "Disconnected";
//	}
//
//	public void StartLobby ()
//	{
//		if (m_ssLobbyName == "" || m_ssPlayerName == "")
//		{
//			return;
//		}
//		base.StartHost ();
//		UIPanelManager.OpenPanel("");
//	}
//
//	public void StartClient()
//	{
//		base.StartClient();
//
//		if (client != null)
//		{
//			client.RegisterHandler(MsgType.Error, OnError);
//			client.RegisterHandler(MsgType.Connect, OnConnect);
//			client.RegisterHandler(MsgType.Disconnect, OnDisconnect);
//		}
//		ScanForServer();
//
//
//	}
//
//	public void setLobbyName ()
//	{
//		InputField temp = UIPanelManager.getUIElementOnPanel ("HostName").GetComponent<InputField> ();
//		if (temp != null)
//		{
//			m_ssLobbyName = temp.text;
//		} else
//		{
//			Debug.Log ("No Element named HostName on this Panel");
//		}
//		return;			
//	}
//
//	void Update ()
//	{
//		if (client != null)
//		{
//			if (client.isConnected)
//			{
//				m_bScanning = false;
//			}
//			else if (m_bScanning)
//			{
//				if (m_bNewScan)
//				{
//					//ConnectToIP ("192.168.43.40");
//					ConnectToIP(m_lLocalNetworkAddresses[0]);
//					m_bNewScan = false;
//					m_lLocalNetworkAddresses.Add(m_lLocalNetworkAddresses[0]);
//					m_lLocalNetworkAddresses.RemoveAt(0);
//				}
//			}
//		}
//	}
//
//	public override void OnClientConnect (NetworkConnection conn)
//	{
//		Debug.Log ("Player Connected");
//
//		m_iConnectedPlayers ++;
//
//		txtDebug.text = "Player connected (Total: " + m_iConnectedPlayers.ToString() + ")";
//
//		base.OnClientConnect (conn);
//		if (m_iConnectedPlayers >= 4)
//		{
//			CheckReadyToBegin();
//		}
//	}
//
//	public override void OnClientDisconnect (NetworkConnection conn)
//	{
//		Debug.Log ("Player Disconnected");
//		m_iConnectedPlayers --;
//		txtDebug.text = "Player Disconnected";
//		base.OnClientDisconnect (conn);
//	}
//
//	public void InstantiatePlayerPanel (NetworkConnection conn)
//	{
//
//	}
//
//	public void setLocalNetwork()
//	{
//		m_ssLocalNetwork = m_ssLocalAddress.Substring(0, m_ssLocalAddress.LastIndexOf(".") + 1);
//		m_iLocalAddress = int.Parse(m_ssLocalAddress.Substring(m_ssLocalAddress.LastIndexOf(".") + 1));
//	}
//
//	public void LocalNetworkScan()
//	{
//		for (int i = 1; i < 256; i++)
//		{
//			if (i != m_iLocalAddress)
//			{
//				//Debug.Log (m_ssLocalNetwork + i.ToString ());
//				m_lLocalNetworkAddresses.Add(m_ssLocalNetwork + i.ToString());
//			}
//			else
//			{
//				//Debug.Log ("Adding Local Host");
//				m_lLocalNetworkAddresses.Add("localhost");
//			}
//		}
//	}
//
//	public void ScanForServer()
//	{
//		if (!client.isConnected)
//		{
//		Debug.Log("Starting To Connect...");
//		txtDebug.text = "Starting To Connect...";
//		m_bScanning = true;
//		m_bNewScan = true;
//		}
//	}
//
//	public void ConnectToIP(string ipAdd)
//	{
//		txtDebug.text += "\nAttempting " + ipAdd + "...";
//		Debug.Log("\t Attempting to connect to " + ipAdd + " on server port " + networkPort + "...");
//		client.Connect(ipAdd, networkPort);
//
//	}
//}
