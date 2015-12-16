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
	public GameObject uiAbilityPanel;
	public GameObject protoAbilityButton;

	public float fDelayCountdown;

	public Ability curAbility = null;
	public GameObject curTarget = null;
	public EXECUTION_STATE curAbilityState = EXECUTION_STATE.NONE;

	private List<GameObject> goAbilityButtons = new List<GameObject>();

	public bool bDeltaAbilityPanel = false;

	public Button depbut1;
	public Button depbut2;
	public Button depbut3;
	public Button depbut4;
	public Button depbut5;
	public Button depbut6;
	public Button depbut7;

	public GAMEPLAY_STATE eLocalState = GAMEPLAY_STATE.WAIT;

	public UNIT_FACTION eLocalFaction = UNIT_FACTION.NONE;

	//public GameObject goLocalNetPlayer;

	//public NetPathMap pathmap;
	public NetGameMap map;
	public TileMap Tmap;

	Ray ray;
	RaycastHit rayHit;

	public List<GameUnit> l_guUnits = new List<GameUnit>();

	public PlayerDeploymentPrefs plyprefs = new PlayerDeploymentPrefs();

	public GameObject goParticleTemp;

	//public GameObject protoUnit;
	
	void Start() {
		AbilityBox.ReloadAbilites ();

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
		eLocalState = GAMEPLAY_STATE.DEPLOYING;
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

	public bool IsLocalTurn() {
		if ((int)PhotonNetwork.room.customProperties ["Turn"] == PhotonNetwork.player.ID) {
			return true;
		}

		return false;
	}

//	[PunRPC]
//	public void ChangeMode(string mode) {
//		Debug.Log ("RPC ChangeMode " + mode);
//		if (mode == "play") {
//			ExitGames.Client.Photon.Hashtable m_PropertiesHash = new ExitGames.Client.Photon.Hashtable ();
//			m_PropertiesHash.Add ("Mode", "play");
//			PhotonNetwork.room.SetCustomProperties (m_PropertiesHash);
//
//			eLocalState = GAMEPLAY_STATE.IDLE;
//		}
//	}

	public void AbilityButtonPressed(GameObject button) {
		Debug.Log (button.GetComponentInChildren<Text> ().text);
		eLocalState = GAMEPLAY_STATE.SELECT_TARGET;
		curAbility = AbilityBox.s_dAbilityLookup [button.GetComponentInChildren<Text> ().text];
	}
	
	// Update is called once per frame
	void Update() {
		if (Input.GetKeyDown(KeyCode.F1)) {
			Debug.ClearDeveloperConsole();
			Application.LoadLevel("net-main-menu");
		}

		if (!PhotonNetwork.isMasterClient) {
			if ((string)PhotonNetwork.room.customProperties["Mode"] == "play" && eLocalState == GAMEPLAY_STATE.DEPLOYING) {
				Debug.Log ("Changing local mode to idle...");
				eLocalState = GAMEPLAY_STATE.IDLE;
				if (this.eLocalFaction == UNIT_FACTION.MAGICAL) {
					this.GetComponent<AudioManager>().ChangeMusic("Magical");
				}
				else {
					this.GetComponent<AudioManager>().ChangeMusic("Mechanical");
				}
			}
			else {
				//Debug.Log((string)PhotonNetwork.room.customProperties["Mode"]);
			}
		}

		if ((string)PhotonNetwork.player.customProperties ["Mode"] == "deploy" && PhotonNetwork.isMasterClient) {
			Debug.Log ("Changing mode to play...");
			if ((bool)PhotonNetwork.player.customProperties ["ReadyDep"] == true) {
				if ((bool)PhotonNetwork.otherPlayers [0].customProperties ["ReadyDep"] == true) {
					DeploBut.gameObject.SetActive (false);
					ReadyBut.gameObject.SetActive (false);

					ExitGames.Client.Photon.Hashtable m_PropertiesHash = new ExitGames.Client.Photon.Hashtable ();
					m_PropertiesHash.Add ("Mode", "play");
					PhotonNetwork.room.SetCustomProperties (m_PropertiesHash);
					eLocalState = GAMEPLAY_STATE.IDLE;

					//this.photonView.RPC("LoadUnitStatsRemote", PhotonTargets.Others, unit);
				}
			}
		}

		//Turn stuff
		if (PhotonNetwork.room != null) {
			if (IsLocalTurn()) {

				//Select
				GameUnit selunit = null;
				if (Tmap.selectedUnit != null) {
					selunit = Tmap.selectedUnit.GetComponent<GameUnit>();
				}

				//Aiming
				if (eLocalState == GAMEPLAY_STATE.SELECT_TARGET) {
					if (curAbility.iTargetType == EFFECT_TARGET.SELF) {
						curTarget = Tmap.selectedUnit;
					}
					//SINGLE TILE
					else if (curAbility.iTargetType == EFFECT_TARGET.SINGLE_EMPTY_TILE) {
						if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
							ray = Camera.main.ScreenPointToRay(Input.mousePosition);
	
							if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("Terrain"))) {
								Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.green);

								//TODO: CHECK IF EMPTY
								curTarget = rayHit.collider.gameObject;
							}
						}

						if (Input.GetMouseButtonDown(1) || Input.touchCount > 2) {
							eLocalState = GAMEPLAY_STATE.IDLE;
						}
					}
					//SINGLE ENEMY
					else if (curAbility.iTargetType == EFFECT_TARGET.SINGLE_ENEMY || curAbility.iTargetType == EFFECT_TARGET.AREA) {
						if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
							ray = Camera.main.ScreenPointToRay(Input.mousePosition);
							
							if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
								Debug.DrawLine(ray.GetPoint(0), rayHit.point, Color.green);

								Debug.Log (rayHit.collider.gameObject.name);
								Debug.Log (rayHit.collider.gameObject.GetComponentInParent<GameUnit>().sName);

								if (!rayHit.collider.gameObject.GetComponentInParent<GameUnit>().photonView.isMine) {
									curTarget = rayHit.collider.gameObject;
								}
							}
						}
						
						if (Input.GetMouseButtonDown(1) || Input.touchCount > 2) {
							eLocalState = GAMEPLAY_STATE.IDLE;
						}
					}
					else {
						Debug.Log(curAbility.iArea);
						Debug.Log(curAbility.fDelayStart);
						Debug.Log(curAbility.fDelayRun);
						Debug.Log(curAbility.fDelayFinish);
						Debug.Log(curAbility.iAccuracy);
						Debug.Log(curAbility.sParticleFinish);

						Debug.LogError("No code for that target exists (" + curAbility.iTargetType.ToString() + ")");
					}
				}

				//Show Abilities
				if (eLocalState == GAMEPLAY_STATE.IDLE && Tmap.selectedUnit != null && selunit.currentPath == null) {
					uiAbilityPanel.SetActive(true);
					if (!bDeltaAbilityPanel) {
						int count = 0;

						if (goAbilityButtons.Count > 0) {
							for (int i = goAbilityButtons.Count - 1; i > -1; i--) {
								GameObject.Destroy(goAbilityButtons[i]);
							}
						}

						goAbilityButtons.Clear();

						foreach (KeyValuePair<string, Ability> pair in selunit.d_abilities) {
							GameObject goTemp = (GameObject)GameObject.Instantiate(protoAbilityButton, Vector3.zero, Quaternion.identity);
							goAbilityButtons.Add(goTemp);
							goTemp.transform.SetParent(uiAbilityPanel.transform);
							RectTransform rectemp = goTemp.GetComponent<RectTransform>();
							rectemp.anchoredPosition = new Vector2(0, 32 - 34 * count);

							goTemp.GetComponentInChildren<Text>().text = pair.Key;

							goTemp.GetComponent<Button>().onClick.AddListener(delegate {
								AbilityButtonPressed(goTemp);
							});

							count++;
						}

						bDeltaAbilityPanel = true;
					}
				}
				else {
					uiAbilityPanel.SetActive(false);
					bDeltaAbilityPanel = false;
				}



				//Execution
				if (eLocalState == GAMEPLAY_STATE.SELECT_TARGET && curTarget != null) {
					eLocalState = GAMEPLAY_STATE.WAIT;
					curAbilityState = EXECUTION_STATE.START;

					if (goParticleTemp != null) {
						GameObject.Destroy(goParticleTemp);
					}
					if (curAbility.sParticleStart != "null" && curAbility.sParticleStart != "") {
						goParticleTemp = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Particle Effects/" + curAbility.sParticleStart), curTarget.transform.position, Quaternion.identity);
					}
					fDelayCountdown = curAbility.fDelayStart;
					//this.GetComponent<AudioManager>().PlayOnce(curAbility.sSoundset);
				}

				if (curAbilityState == EXECUTION_STATE.START) {
					fDelayCountdown -= Time.deltaTime;

					if (fDelayCountdown <= 0) {
						curAbilityState = EXECUTION_STATE.MID;

						if (goParticleTemp != null) {
							GameObject.Destroy(goParticleTemp);
						}
						if (curAbility.sParticleRun != "null" && curAbility.sParticleRun != "") {
							goParticleTemp = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Particle Effects/" + curAbility.sParticleRun), curTarget.transform.position, Quaternion.identity);
						}
						fDelayCountdown = curAbility.fDelayRun;
					}
				}

				if (curAbilityState == EXECUTION_STATE.MID) {
					fDelayCountdown -= Time.deltaTime;
					
					if (fDelayCountdown <= 0) {
						curAbilityState = EXECUTION_STATE.END;

						if (goParticleTemp != null) {
							GameObject.Destroy(goParticleTemp);
						}
						if (curAbility.sParticleFinish != "null" && curAbility.sParticleFinish != "") {
							goParticleTemp = (GameObject)GameObject.Instantiate(Resources.Load<GameObject>("Particle Effects/" + curAbility.sParticleFinish), curTarget.transform.position, Quaternion.identity);
						}

						if (curAbility.d_EffectsResolution.ContainsKey("Damage")) {
							curTarget.GetComponent<GameUnit>().fHealth -= curAbility.d_EffectsResolution["Damage"].fAdjustFloat;
							curTarget.GetPhotonView().RPC("TakeDamage", curTarget.GetPhotonView().owner, curTarget.GetPhotonView().viewID, (int)curAbility.d_EffectsResolution["Damage"].fAdjustFloat);
						}

						fDelayCountdown = curAbility.fDelayFinish;
					}
				}

				if (curAbilityState == EXECUTION_STATE.END) {
					fDelayCountdown -= Time.deltaTime;
					
					if (fDelayCountdown <= 0) {
						curAbilityState = EXECUTION_STATE.NONE;
						
						eLocalState = GAMEPLAY_STATE.IDLE;
						curTarget = null;
						curAbility = null;
						if (goParticleTemp != null) {
							GameObject.Destroy(goParticleTemp);
						}
					}
				}
				
				//EndTurn turn
				if (eLocalState == GAMEPLAY_STATE.IDLE) {
					uiEndTurn.interactable = true;
				}
				else {
					uiEndTurn.interactable = false;
				}

				txtTurn.text = "My turn " + eLocalState.ToString();
			}
			else {
				uiEndTurn.interactable = false;
				txtTurn.text = "Their turn " + eLocalState.ToString();
			}

//			if (Input.GetKeyDown(KeyCode.P) && !goLocalNetPlayer) {
//				goLocalNetPlayer = (GameObject)PhotonNetwork.Instantiate("NetPlayer", Vector3.zero, Quaternion.identity, 0);
//			}
		}

		//Debug


		//if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()) {
			//ray = Camera.main.ScreenPointToRay(Input.mousePosition);

//			if (Physics.Raycast(ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer("GameUnit"))) {
//				Debug.Log (PhotonNetwork.player.ID);
//				Photon.MonoBehaviour gotemp = (Photon.MonoBehaviour)rayHit.collider.gameObject;
//
//				if (PhotonNetwork.player.ID == 1 && gotemp.networkView.viewID == 1){
//					Debug.Log("Photon Player Working");
//				}
//			}
		//}
	}

	public void DeployUnit(string unit){	
		//Vector3 vectemp = new Vector3 (0.0f, 0.0f, 0.0f);
		GameObject tempunit = (GameObject)PhotonNetwork.Instantiate ("NetGameUnit", TileCursor.transform.position, Quaternion.identity, 0);
		GameUnit tempunit2 = tempunit.GetComponent<GameUnit> ();

		tempunit2.tileX = (int)TileCursor.transform.position.x;
		tempunit2.tileY = (int)TileCursor.transform.position.z;
		
		tempunit2.LoadUnitStats(unit);
		tempunit2.photonView.RPC("LoadUnitStatsRemote", PhotonTargets.Others, unit);
		tempunit2.eGridDirection = UNIT_DIR.DOWN_RIGHT;

		l_guUnits.Add(tempunit2);
		DeployPanel.SetActive(false);
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
				if (plyprefs.l_sMagicalUnits[0].Key == "null") {
					depbut1.interactable = false;
				}
				if (depbut1.interactable) {
					depbut1.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[0].Key;
				}
				else {
					depbut1.GetComponentInChildren<Text>().text = "None";
				}

				//2
				if (plyprefs.l_sMagicalUnits[1].Key == "null") {
					depbut2.interactable = false;
				}
				if (depbut2.interactable) {
					depbut2.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[1].Key;
				}
				else {
					depbut2.GetComponentInChildren<Text>().text = "None";
				}

				//3
				if (plyprefs.l_sMagicalUnits[2].Key == "null") {
					depbut3.interactable = false;
				}
				if (depbut3.interactable) {
					depbut3.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[2].Key;
				}
				else {
					depbut3.GetComponentInChildren<Text>().text = "None";
				}

				//4
				if (plyprefs.l_sMagicalUnits[3].Key == "null") {
					depbut4.interactable = false;
				}
				if (depbut4.interactable) {
					depbut4.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[3].Key;
				}
				else {
					depbut4.GetComponentInChildren<Text>().text = "None";
				}

				//5
				if (plyprefs.l_sMagicalUnits[4].Key == "null") {
					depbut5.interactable = false;
				}
				if (depbut5.interactable) {
					depbut5.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[4].Key;
				}
				else {
					depbut5.GetComponentInChildren<Text>().text = "None";
				}

				//6
				if (plyprefs.l_sMagicalUnits[5].Key == "null") {
					depbut6.interactable = false;
				}
				if (depbut6.interactable) {
					depbut6.GetComponentInChildren<Text>().text = plyprefs.l_sMagicalUnits[5].Key;
				}
				else {
					depbut6.GetComponentInChildren<Text>().text = "None";
				}

				//7
				if (plyprefs.l_sMagicalUnits[6].Key == "null") {
					depbut7.interactable = false;
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
				if (plyprefs.l_sMechanicalUnits[0].Key == "null") {
					depbut1.interactable = false;
				}
				if (depbut1.interactable) {
					depbut1.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[0].Key;
				}
				else {
					depbut1.GetComponentInChildren<Text>().text = "None";
				}
				
				//2
				if (plyprefs.l_sMechanicalUnits[1].Key == "null") {
					depbut1.interactable = false;
				}
				if (depbut2.interactable) {
					depbut2.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[1].Key;
				}
				else {
					depbut2.GetComponentInChildren<Text>().text = "None";
				}
				
				//3
				if (plyprefs.l_sMechanicalUnits[2].Key == "null") {
					depbut1.interactable = false;
				}
				if (depbut3.interactable) {
					depbut3.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[2].Key;
				}
				else {
					depbut3.GetComponentInChildren<Text>().text = "None";
				}
				
				//4
				if (plyprefs.l_sMechanicalUnits[3].Key == "null") {
					depbut1.interactable = false;
				}
				if (depbut4.interactable) {
					depbut4.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[3].Key;
				}
				else {
					depbut4.GetComponentInChildren<Text>().text = "None";
				}
				
				//5
				if (plyprefs.l_sMechanicalUnits[4].Key == "null") {
					depbut1.interactable = false;
				}
				if (depbut5.interactable) {
					depbut5.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[4].Key;
				}
				else {
					depbut5.GetComponentInChildren<Text>().text = "None";
				}
				
				//6
				if (plyprefs.l_sMechanicalUnits[5].Key == "null") {
					depbut1.interactable = false;
				}
				if (depbut6.interactable) {
					depbut6.GetComponentInChildren<Text>().text = plyprefs.l_sMechanicalUnits[5].Key;
				}
				else {
					depbut6.GetComponentInChildren<Text>().text = "None";
				}
				
				//7
				if (plyprefs.l_sMechanicalUnits[6].Key == "null") {
					depbut1.interactable = false;
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
