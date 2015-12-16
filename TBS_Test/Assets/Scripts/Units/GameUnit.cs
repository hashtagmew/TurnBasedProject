using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

public class GameUnit : Photon.MonoBehaviour, ISelectable {

	public UNIT_FACTION iFaction;

	//public Vector2 vGridPosition;
	public Vector3 vMoveTarget;
	public bool bMoving = false;

	public bool bSelected = false;

	public string sName;
	public string sDescription;

	public float fAP;
	public float fMaxAP;

	public float fHealth;
	public float fMaxHealth;
	//public float fMana;
	public float fMovement;
	public float fMaxMovement;
	public float fVision;
	//public float fVision;
	//public float fSpeed = 1.5f;

	public float fAttack;
	public float fPhysAttack;
	public float fRangAttack;
	public float fMagiAttack;

	public float fResistance;
	public float fDefence;

	public AudioManager mngAudio;

	public Dictionary<string, Ability> d_abilities = new Dictionary<string, Ability>();
	public Dictionary<ABILITY_ELEMENT, float> d_efElementalResistances {
		get;
		private set;
	}

	//Pathfinding stuff
	public UNIT_DIR eGridDirection = UNIT_DIR.DOWN_RIGHT;

	public int tileX;
	public int tileY;

	public NetGameManager netman;
	public TileMap map {
		get;
		private set;
	}

	// Our pathfinding info.  Null if we have no destination ordered.
	public List<Node> currentPath = null;

	public int moveSpeed = 2;
	public float remainingMovement = 0;
	public float resetMovement = 0;
	public float currDirX;
	public float currDirY;
	private Vector2 CurPos;
	private Vector2 NextPos;
	
	private SpriteRenderer myRend;
	
	public Sprite texDirSpriteUR;
	public Sprite texDirSpriteDR;
	public Sprite texDirSpriteUL;
	public Sprite texDirSpriteDL;

	public Soundset ssSoundset = new Soundset();

	//Net stuff
	private Vector3 vCorrectPos;
	private Quaternion qCorrectRot;
	private float fCorrectHealth;
	private float fCorrectMaxHealth;

	private string texNetDirSpriteUR;
	private string texNetDirSpriteDR;
	private string texNetDirSpriteUL;
	private string texNetDirSpriteDL;

	private int iNetSpriteDir;

	// Use this for initialization
	void Start () {
		d_efElementalResistances = new Dictionary<ABILITY_ELEMENT, float>();

		d_efElementalResistances.Add(ABILITY_ELEMENT.KINETIC, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.MAGIC, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.LIGHT, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.DARK, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.EARTH, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.WATER, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.AIR, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.FIRE, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.ELECTRIC, 1.0f);
		d_efElementalResistances.Add(ABILITY_ELEMENT.ICE, 1.0f);

		fAP = fMaxAP;
		fHealth = fMaxHealth;
		fMovement = fMaxMovement;

		mngAudio = GameObject.FindGameObjectWithTag("Managers").GetComponent<AudioManager>();

		//LoadUnitStats(sName);

		this.map = GameObject.FindGameObjectWithTag ("MainMap").GetComponent<TileMap> ();
		if (map == null) {
			Debug.Log ("Path map is null");
		}
		myRend = this.GetComponentInChildren<SpriteRenderer> ();

		if (myRend == null) {
			Debug.LogWarning("myRend was null!");
		}
	}
	
	// Update is called once per frame
	void Update() {
		if (!photonView.isMine) {
//			if (netman.l_guUnits != null && netman.l_guUnits.Count > 0) {
//				foreach (GameUnit unittemp in netman.l_guUnits) {
//					if (unittemp != null) {
//						unittemp.GetComponent<GameUnit>().tileX = (int)unittemp.transform.position.x;
//						unittemp.GetComponent<GameUnit>().tileY = (int)unittemp.transform.position.z;
//					}
//				}
//			}

			//tileX = (int)transform.position.x;
			//tileY = (int)transform.position.z;

			//Post-stream
			this.fHealth = fCorrectHealth;
			this.fMaxHealth = fCorrectMaxHealth;

			transform.position = Vector3.Lerp (transform.position, this.vCorrectPos, Time.deltaTime);
			transform.rotation = Quaternion.Lerp (transform.rotation, this.qCorrectRot, Time.deltaTime);

		} 
		else {
			//Finding Direction the Unit is moving in so that the sprite can be changed
			if (currentPath != null && currentPath.Count > 1) {
				CurPos = new Vector2 (tileX, tileY);
				NextPos = new Vector2 (currentPath [1].x, currentPath [1].y);
			}
		
			currDirX = CurPos.x - NextPos.x;
			currDirY = CurPos.y - NextPos.y;
		
			//setting direction
			if (currDirX == -1 && currDirY == 0) {
				eGridDirection = UNIT_DIR.UP_RIGHT;
			} else if (currDirX == 0 && currDirY == 1) {
				eGridDirection = UNIT_DIR.DOWN_RIGHT;
			} else if (currDirX == 0 && currDirY == -1) {
				eGridDirection = UNIT_DIR.UP_LEFT;
			} else if (currDirX == 1 && currDirY == 0) {
				eGridDirection = UNIT_DIR.DOWN_LEFT;
			}
			else {
				eGridDirection = UNIT_DIR.DOWN_RIGHT;
			}
		
			//Changing sprite based on direction
			if (eGridDirection == UNIT_DIR.UP_RIGHT) {
				if (photonView.isMine) {
					if (myRend.sprite != texDirSpriteUR) {
						myRend.sprite = texDirSpriteUR;
					}
				}
				else {
					if (myRend.sprite.name != texNetDirSpriteUR) {
						myRend.sprite = Resources.Load<Sprite>("UnitSprites/" + texNetDirSpriteUR);
					}
				}
			}
			if (eGridDirection == UNIT_DIR.DOWN_RIGHT) {
				if (photonView.isMine) {
					if (myRend.sprite != texDirSpriteDR) {
						myRend.sprite = texDirSpriteDR;
					}
				}
				else {
					if (myRend.sprite.name != texNetDirSpriteDR) {
						myRend.sprite = Resources.Load<Sprite>("UnitSprites/" + texNetDirSpriteDR);
					}
				}
			}
			if (eGridDirection == UNIT_DIR.UP_LEFT) {
				if (photonView.isMine) {
					if (myRend.sprite != texDirSpriteUL) {
						myRend.sprite = texDirSpriteUL;
					}
				}
				else {
					if (myRend.sprite.name != texNetDirSpriteUL) {
						myRend.sprite = Resources.Load<Sprite>("UnitSprites/" + texNetDirSpriteUL);
					}
				}
			}
			if (eGridDirection == UNIT_DIR.DOWN_LEFT) {
				if (photonView.isMine) {
					if (myRend.sprite != texDirSpriteDL) {
						myRend.sprite = texDirSpriteDL;
					}
				}
				else {
					if (myRend.sprite.name != texNetDirSpriteDL) {
						myRend.sprite = Resources.Load<Sprite>("UnitSprites/" + texNetDirSpriteDL);
					}
				}
			}
		
			// Draw our debug line showing the pathfinding!
			// NOTE: This won't appear in the actual game view.
			if (currentPath != null) {
				int currNode = 0;
			
				while (currNode < currentPath.Count-1) {
				
					Vector3 start = map.TileCoordToWorldCoord (currentPath [currNode].x, currentPath [currNode].y) + 
						new Vector3 (0, -0.5f, 0);
					Vector3 end = map.TileCoordToWorldCoord (currentPath [currNode + 1].x, currentPath [currNode + 1].y) + 
						new Vector3 (0, -0.5f, 0);
				
					Debug.DrawLine (start, end, Color.red);
				
					currNode++;
				}
			}
		
			//		//Reset Movement Cost
			//		if (currentPath == null && remainingMovement <= 0) {
			//			remainingMovement = resetMovement;
			//		}
			//		//Make sure unit stops after remaining steps has been achieved
			//		if (remainingMovement == 0) {
			//			currentPath = null;
			//			remainingMovement = resetMovement;
			//		}
		
			// Have we moved our visible piece close enough to the target tile that we can advance to the next step in our pathfinding?
			if (map == null) {
				Debug.Log ("GameUnit Map was null...");
			}
		
			if (Vector3.Distance (transform.position, map.TileCoordToWorldCoord (tileX, tileY)) < 0.1f) {
				AdvancePathing();
			}
		
			// Smoothly animate towards the correct map tile.
			transform.position = Vector3.Lerp (transform.position, map.TileCoordToWorldCoord (tileX, tileY), 5f * Time.deltaTime);
		}
	}

	public string GetSoundPath(SUB_SOUNDSET type) {
		string path = "";

		if (type == SUB_SOUNDSET.U_SELECTED) {
			path = this.ssSoundset.d_sacSounds["select"].name;
		}
		else if (type == SUB_SOUNDSET.U_FOOTSTEP) {
			path = this.ssSoundset.d_sacSounds["footstep"].name;
		}
		else if (type == SUB_SOUNDSET.U_ATTACKORDER) {
			path = this.ssSoundset.d_sacSounds["attack"].name;
		}
		else if (type == SUB_SOUNDSET.U_MOVEORDER) {
			path = this.ssSoundset.d_sacSounds["move"].name;
		}
		else {
			return "null";
		}

		return path;
	}

	public void PlaySound(SUB_SOUNDSET type) {
		string file = "";
		if (type == SUB_SOUNDSET.U_SELECTED) {
			file = GetSoundPath(type);
		}
		else if (type == SUB_SOUNDSET.U_FOOTSTEP) {
			file = GetSoundPath(type);
		}
		else if (type == SUB_SOUNDSET.U_ATTACKORDER) {
			file = GetSoundPath(type);
		}
		else if (type == SUB_SOUNDSET.U_MOVEORDER) {
			file = GetSoundPath(type);
		}
		else {
			return;
		}

		if (this.mngAudio != null) {
			mngAudio.PlayOnce(file);
		}
	}

	// Advances our pathfinding progress by one tile.
	void AdvancePathing() {
		if (currentPath == null)
			return;
		
		if (remainingMovement <= 0)
			return;
		
		// Teleport us to our correct "current" position, in case we
		// haven't finished the animation yet.
		transform.position = map.TileCoordToWorldCoord( tileX, tileY );
		
		// Get cost from current tile to next tile
		remainingMovement -= map.CostToEnterTile(currentPath[0].x, currentPath[0].y, currentPath[1].x, currentPath[1].y );
		
		// Move us to the next tile in the sequence
		tileX = currentPath[1].x;
		tileY = currentPath[1].y;
		
		// Remove the old "current" tile from the pathfinding list
		currentPath.RemoveAt(0);
		
		if(currentPath.Count == 1) {
			// We only have one tile left in the path, and that tile MUST be our ultimate
			// destination -- and we are standing on it!
			// So let's just clear our pathfinding info.
			currentPath = null;
		}
	}
	
	public void EndTurn(){
		//pathmap.selectedUnit.GetComponent<Unit>().currentPath = null;
		//remainingMovement = resetMovement;
		//pathmap.selectedUnit.GetComponent<Unit> ().remainingMovement = pathmap.selectedUnit.GetComponent<Unit> ().resetMovement;
	}
	
	// The "Next Turn" button calls this.
	public void NextTurn() {
		
		// Make sure to wrap-up any outstanding movement left over.
		while (currentPath != null && remainingMovement > 0) {
			AdvancePathing();
		}
		
		// Reset our available movement points.
		//
	}

	void SetResistance(ABILITY_ELEMENT element, float resistpercent) {
		//resistpercent = Mathf.Clamp(resistpercent, -1.0f, 4.0f);
		d_efElementalResistances[element] = resistpercent;
	}

	public virtual void OnSelected() {
		bSelected = true;
	}

	public virtual void OnDeselected() {
		//
	}

	public void MoveTo(Vector3 dest) {
		vMoveTarget = dest;
		bMoving = true;
	}

	private bool FloatApproximation(float a, float b, float tolerance) {
		return (Mathf.Abs(a - b) < tolerance);
	}

	[PunRPC]
	public void TakeDamage(int checkid, int amount) {
		Debug.Log("ID IS " + checkid.ToString() + " THIS UNIT " + this.photonView.viewID);

		if (this.photonView.viewID == checkid) {
			this.fHealth -= (float)amount;
			Debug.Log(this.gameObject.name + " took " + amount.ToString() + " damage!");
		}
	}

	[PunRPC]
	public void LoadUnitStatsRemote(string path) {
		netman = GameObject.FindGameObjectWithTag ("Managers").GetComponent<NetGameManager> ();

		Debug.Log ("REMOTE CALL" + path);
		LoadUnitStats (path);
		//LoadUnitSprites();
		netman.l_guUnits.Add (this);
	}

	public bool LoadUnitStats(string path) {
		TextAsset taUnit = (TextAsset)Resources.Load<TextAsset>("UnitFiles/" + path);

		if (taUnit == null) {
			Debug.LogWarning("The unit file \"" + path + "\" was not found.");
			return false;
		}

		XDocument xmlDoc = XDocument.Load(new StringReader(taUnit.text));

		foreach (XElement xroot in xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				//info
				if (xlayer1.Name == "name") {
					sName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					sDescription = xlayer1.Value;
				}
				//stats
				else if (xlayer1.Name == "ap") {
					fAP = float.Parse(xlayer1.Value);
					fMaxAP = fAP;
				}
				else if (xlayer1.Name == "health") {
					fHealth = float.Parse(xlayer1.Value);
					fMaxHealth = fHealth;
				}
				else if (xlayer1.Name == "move") {
					fMovement = float.Parse(xlayer1.Value);
					fMaxMovement = fMovement;
				}
				else if (xlayer1.Name == "vision") {
					fVision = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "attack") {
					fPhysAttack = float.Parse(xlayer1.Value);
					fAttack = fPhysAttack;
				}
				else if (xlayer1.Name == "physattack") {
					fPhysAttack = float.Parse(xlayer1.Value);
					//fAttack = fPhysAttack;
				}
				else if (xlayer1.Name == "rangattack") {
					fRangAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "magattack") {
					fMagiAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "defence") {
					fDefence = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "resistance") {
					fResistance = float.Parse(xlayer1.Value);
				}
				//sprites
				else if (xlayer1.Name == "spriteUL") {
					string stemp = xlayer1.Value;
					texDirSpriteUL = Resources.Load<Sprite>("UnitSprites/" + stemp);
					texNetDirSpriteUL = texDirSpriteUL.name;

					if (texDirSpriteUL == null) {
						Debug.Log(this.name + "'s UL sprite was null!");
					}
				}
				else if (xlayer1.Name == "spriteUR") {
					string stemp = xlayer1.Value;
					texDirSpriteUR = Resources.Load<Sprite>("UnitSprites/" + stemp);
					texNetDirSpriteUR = texDirSpriteUR.name;
				}
				else if (xlayer1.Name == "spriteDL") {
					string stemp = xlayer1.Value;
					texDirSpriteDL = Resources.Load<Sprite>("UnitSprites/" + stemp);
					texNetDirSpriteDL = texDirSpriteDL.name;
				}
				else if (xlayer1.Name == "spriteDR") {
					string stemp = xlayer1.Value;
					texDirSpriteDR = Resources.Load<Sprite>("UnitSprites/" + stemp);
					texNetDirSpriteDR = texDirSpriteDR.name;
				}
				//soundset
				else if (xlayer1.Name == "soundset") {
					LoadSoundset(xlayer1.Value);
				}
				
				if (xlayer1.Name == "abilities") {
					foreach (XElement xlayer2 in xlayer1.Elements()) {
						if (AbilityBox.s_dAbilityLookup.ContainsKey(xlayer2.Value)) {
							d_abilities.Add(xlayer2.Value, AbilityBox.s_dAbilityLookup[xlayer2.Value]);
						}
					}
				}
			}
		}

		return true;
	}

	void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
		if (stream.isWriting) {
			//Local player
			stream.SendNext(transform.position);
			stream.SendNext(transform.rotation);

			stream.SendNext(fHealth);
			stream.SendNext(fMaxHealth);
			stream.SendNext(sName);

			stream.SendNext(texDirSpriteUL.name);
			stream.SendNext(texDirSpriteDL.name);
			stream.SendNext(texDirSpriteUR.name);
			stream.SendNext(texDirSpriteDR.name);

			int tempint = (int)eGridDirection;
			stream.SendNext(tempint);
		}
		else {
			//Net player
			this.vCorrectPos = (Vector3)stream.ReceiveNext();
			this.qCorrectRot = (Quaternion)stream.ReceiveNext();

			this.fHealth = (float)stream.ReceiveNext();
			this.fMaxHealth = (float)stream.ReceiveNext();
			this.sName = (string)stream.ReceiveNext();

			this.texNetDirSpriteUL = (string)stream.ReceiveNext();
			this.texNetDirSpriteDL = (string)stream.ReceiveNext();
			this.texNetDirSpriteUR = (string)stream.ReceiveNext();
			this.texNetDirSpriteDR = (string)stream.ReceiveNext();

			int tempint = (int)stream.ReceiveNext();
			this.eGridDirection = (UNIT_DIR)tempint;
		}
	}

	public void LoadSoundset(string path) {
		TextAsset taSoundset = Resources.Load<TextAsset>("Audio/Soundsets/" + path);
		//AudioClip tempclip;

		XDocument xmlDoc = XDocument.Load(new StringReader(taSoundset.text));
		
		foreach (XElement xroot in xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "type") {
					SOUNDSET_TYPE temptype = (SOUNDSET_TYPE)(int.Parse(xlayer1.Value));

					if (temptype != SOUNDSET_TYPE.UNIT) {
						Debug.LogError("GameUnit attempted to load a non-unit soundset!");
						return;
					}
				}
				else {
					if (Resources.Load<AudioClip>("Audio/" + (string)xlayer1.Value) == null  && xlayer1.Value != "null") {
						Debug.Log("Can't find sound: Audio/" + xlayer1.Value);
					}
					else {
						Debug.Log("Loaded Audio/" + xlayer1.Value);
					}
				}

				if (xlayer1.Name == "select" && xlayer1.Value != "null") {
					ssSoundset.AddClip("select", Resources.Load<AudioClip>("Audio/" + (string)xlayer1.Value));
				}
				else if (xlayer1.Name == "move" && xlayer1.Value != "null") {
					ssSoundset.AddClip("move", Resources.Load<AudioClip>("Audio/" + (string)xlayer1.Value));
				}
				else if (xlayer1.Name == "attack" && xlayer1.Value != "null") {
					ssSoundset.AddClip("attack", Resources.Load<AudioClip>("Audio/" + (string)xlayer1.Value));
				}
				else if (xlayer1.Name == "footstep" && xlayer1.Value != "null") {
					ssSoundset.AddClip("footstep", Resources.Load<AudioClip>("Audio/" + (string)xlayer1.Value));
				}
			}
		}
	}
}
