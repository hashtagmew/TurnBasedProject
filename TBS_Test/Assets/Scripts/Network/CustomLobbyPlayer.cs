//using UnityEngine;
//using UnityEngine.Networking;
//
//using System.Collections;
//
//public class CustomLobbyPlayer : NetworkLobbyPlayer
//{
//	Transform m_LobMng;
//
//
//	// Use this for initialization
//	void Start ()
//	{
//		m_LobMng = GameObject.Find ("GameManager").transform;
//		UIPanelManager.InstantiateGUIElement ("section_LobbyGUI");
//	}
//	
//	// Update is called once per frame
//	void Update ()
//	{
//	
//	}
//
//	public override void OnClientEnterLobby ()
//	{
//		readyToBegin = true;
//		base.OnClientEnterLobby ();
//	}
//}
