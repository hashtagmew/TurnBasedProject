//using UnityEngine;
//using UnityEngine.Networking;
//
//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.IO;
//
//using UnityEngine.UI;
//
//public class ClientHandler : MonoBehaviour {
//
//	NetworkClient m_Client;
//	public NetworkLobbyManager m_LobMngr;
//
//	bool m_bScanning = false;
//	bool m_bNewScan = false;
//
//	string m_ssIPAddress;
//	string m_ssLocalNetwork;
//
//	public int m_iLocalAddress;
//
//	public int m_iExceptionCount;
//	//private System.IO.TextWriter m_writer;
//	private StreamWriter m_writer;
//
//	public Text txtDebug;
//	public string sLastError;
//
//	List<string> m_lLocalNetworkAddresses = new List<string> ();
//
//	public int m_iPort = 17032;
//
//	//Error catching
//	void OnEnable() {
//		Application.logMessageReceived += HandleLog;
//	}
//
//	void OnGUI() {
//		GUILayout.Label(string.Format("Faults: {0}", m_iExceptionCount));
//		GUILayout.Label(sLastError);
//	}
//
//	private void HandleLog(string condition, string stackTrace, LogType type) {
//		if (type == LogType.Exception) {
//			m_iExceptionCount++;
//			//m_writer.WriteLine("Exception - {0}: {1}\n{2}", type, condition, stackTrace);
//			using (StreamWriter writer = new StreamWriter("loggerman.txt", true)) {
//				writer.WriteLine(System.DateTime.Now.ToLongTimeString() + " - {0}: {1}\n{2}\n", type, condition, stackTrace);
//			}
//		}
//
//		if (type == LogType.Error) {
//			m_iExceptionCount++;
//			//m_writer.WriteLine("Error - {0}: {1}\n{2}", type, condition, stackTrace);
//			sLastError = type.ToString() + "/" + condition + "/" + stackTrace;
//			using (StreamWriter writer = new StreamWriter("loggerman.txt", true)) {
//				writer.WriteLine(System.DateTime.Now.ToLongTimeString() + " - {0}: {1}\n{2}\n", type, condition, stackTrace);
//			}
//		}
//	}
//
//	void OnDisable() {
//		Application.logMessageReceived -= HandleLog;
//	}
//	//End error catching
//
//	public void OnError (NetworkMessage msg)
//	{
//		Debug.Log ("\t\tERROR: " + msg.reader.ReadString ());
//		txtDebug.text = "ERROR " + msg.msgType.ToString() + ": " + msg.reader.ToString();
//
//		if (m_bScanning)
//		{
//			m_bNewScan = true;
//		}
//	}
//
//
//	public void OnConnect (NetworkMessage msg)
//	{
//		Debug.Log ("\t\tConnected");
//		txtDebug.text = "Connected";
//		m_bScanning = false;
//
//		UIPanelManager.OpenPanel ("ClientConnected");
//	}
//	public void OnDisconnect (NetworkMessage msg)
//	{
//		Debug.Log ("\t\tDisconnected");
//		//txtDebug.text = "Disconnected";
//	}
//
//	public void OnCRC (NetworkMessage msg)
//	{
//		Debug.Log("\t\tCRC Message");
//	}
//	// Use this for initialization
//	void Start ()
//	{
//		ConnectionConfig m_Config = new ConnectionConfig ();
//
//		m_Config.ConnectTimeout = 200;
//		m_Config.DisconnectTimeout = 1000;
//		m_Client.Configure (m_Config, 1);
//
//		m_Client.RegisterHandler (MsgType.Error, OnError);
//		m_Client.RegisterHandler (MsgType.Connect, OnConnect);
//		m_Client.RegisterHandler (MsgType.Disconnect, OnDisconnect);
//		//m_Client.RegisterHandler (MsgType.CRC, OnCRC);
//		
//
//		m_ssIPAddress = Network.player.ipAddress;
//
//		setLocalNetwork ();
//		LocalNetworkScan ();
//	}
//
//	
//	
//	// Update is called once per frame
//	void Update ()
//	{
//		if (m_Client.isConnected)
//		{
//			m_bScanning = false;
//		} else if (m_bScanning)
//			{
//				if (m_bNewScan)
//				{
//				//ConnectToIP ("192.168.43.40");
//					ConnectToIP (m_lLocalNetworkAddresses [0]);
//					m_bNewScan = false;
//					m_lLocalNetworkAddresses.Add (m_lLocalNetworkAddresses [0]);
//					m_lLocalNetworkAddresses.RemoveAt (0);
//				}
//			}
//	}
//
//	public void setLocalNetwork ()
//	{
//		m_ssLocalNetwork = m_ssIPAddress.Substring (0, m_ssIPAddress.LastIndexOf (".") + 1);
//		m_iLocalAddress = int.Parse (m_ssIPAddress.Substring (m_ssIPAddress.LastIndexOf (".") + 1));
//	}
//
//	public void LocalNetworkScan ()
//	{
//		for (int i = 1; i < 256; i++)
//		{
//			if (i != m_iLocalAddress)
//			{
//				//Debug.Log (m_ssLocalNetwork + i.ToString ());
//				m_lLocalNetworkAddresses.Add (m_ssLocalNetwork + i.ToString ());
//			} else
//			{
//				//Debug.Log ("Adding Local Host");
//				m_lLocalNetworkAddresses.Add ("localhost");
//			}
//		}
//	}
//
//	public void ScanForServer ()
//	{
//		//if (!m_Client.isConnected)
//		//{
//			Debug.Log ("Starting To Connect...");
//			txtDebug.text = "Starting To Connect...";
//			m_bScanning = true;
//			m_bNewScan = true;
//		//}
//	}
//
//	public void ConnectToIP (string ipAdd)
//	{
//		txtDebug.text += "\nAttempting " + ipAdd + "...";
//		Debug.Log ("\t Attempting to connect to " + ipAdd + " on server port " + m_iPort + "...");
//		m_Client.Connect (ipAdd, m_iPort);
//		
//	}
//}
