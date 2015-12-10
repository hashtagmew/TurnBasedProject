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

	public List<Ability> l_abilities;
	public Dictionary<ABILITY_ELEMENT, float> d_efElementalResistances {
		get;
		private set;
	}

	//Pathfinding stuff
	public UNIT_DIR eGridDirection = UNIT_DIR.DOWN_RIGHT;

	public int tileX;
	public int tileY;

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
	
	public GameObject IdleSprite;
	
	private SpriteRenderer myRend;
	
	public Sprite texDirSpriteUR;
	public Sprite texDirSpriteDR;
	public Sprite texDirSpriteUL;
	public Sprite texDirSpriteDL;

	//Net stuff
	private Vector3 vCorrectPos;
	private Quaternion qCorrectRot;

	// Use this for initialization
	void Start () {
		l_abilities = new List<Ability>();
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

		LoadUnitStats(sName);

		this.map = GameObject.FindGameObjectWithTag ("MainMap").GetComponent<TileMap> ();
		if (map == null) {
			Debug.Log ("Path map is null");
		}
		myRend = this.GetComponentInChildren<SpriteRenderer> ();
	}
	
	// Update is called once per frame
	void Update() {

		if (!photonView.isMine) {
			transform.position = Vector3.Lerp (transform.position, this.vCorrectPos, Time.deltaTime);
			transform.rotation = Quaternion.Lerp (transform.rotation, this.qCorrectRot, Time.deltaTime);
		} else {
			Debug.Log("nullPhoton");
		}


		//End turn
		
		
		//Finding Direction the Unit is moving in so that the sprite can be changed
		
		if (currentPath != null && currentPath.Count > 1) {
			CurPos = new Vector2(tileX, tileY);
			NextPos = new Vector2 (currentPath[1].x, currentPath [1].y);
		}
		
		currDirX = CurPos.x - NextPos.x;
		currDirY = CurPos.y - NextPos.y;
		
		//setting direction
		if (currDirX == -1 && currDirY == 0) {
			eGridDirection = UNIT_DIR.UP_RIGHT;
		}
		else if (currDirX == 0 && currDirY == 1) {
			eGridDirection = UNIT_DIR.DOWN_RIGHT;
		}
		else if (currDirX == 0 && currDirY == -1) {
			eGridDirection = UNIT_DIR.UP_LEFT;
		}
		else if (currDirX == 1 && currDirY == 0) {
			eGridDirection = UNIT_DIR.DOWN_LEFT;
		}
		
		//Changing sprite based on direction
		if (eGridDirection == UNIT_DIR.UP_RIGHT) {
			myRend.sprite = texDirSpriteUR;
		}
		if (eGridDirection == UNIT_DIR.DOWN_RIGHT) {
			myRend.sprite = texDirSpriteDR;
		}
		if (eGridDirection == UNIT_DIR.UP_LEFT) {
			myRend.sprite = texDirSpriteUL;
		}
		if (eGridDirection == UNIT_DIR.DOWN_LEFT) {
			myRend.sprite = texDirSpriteDL;
		}
		
		// Draw our debug line showing the pathfinding!
		// NOTE: This won't appear in the actual game view.
		if(currentPath != null) {
			int currNode = 0;
			
			while( currNode < currentPath.Count-1 ) {
				
				Vector3 start = map.TileCoordToWorldCoord( currentPath[currNode].x, currentPath[currNode].y ) + 
					new Vector3(0, -0.5f, 0) ;
				Vector3 end   = map.TileCoordToWorldCoord( currentPath[currNode+1].x, currentPath[currNode+1].y )  + 
					new Vector3(0, -0.5f, 0) ;
				
				Debug.DrawLine(start, end, Color.red);
				
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
		
		// Have we moved our visible piece close enough to the target tile that we can
		// advance to the next step in our pathfinding?
		//Debug.Log (transform.position);
		if (map == null) {
			Debug.Log ("OH GOID");
		} else {
			//Debug.Log ("YEA");
		}
		
		if (Vector3.Distance (transform.position, map.TileCoordToWorldCoord (tileX, tileY)) < 0.1f) {
			AdvancePathing();
		}
		
		// Smoothly animate towards the correct map tile.
		transform.position = Vector3.Lerp(transform.position, map.TileCoordToWorldCoord( tileX, tileY ), 5f * Time.deltaTime);
	}

	// Advances our pathfinding progress by one tile.
	void AdvancePathing() {
		if(currentPath==null)
			return;
		
		if(remainingMovement <= 0)
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
		while(currentPath!=null && remainingMovement > 0) {
			AdvancePathing();
			
		}
		
		// Reset our available movement points.
		
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

	public bool LoadUnitStats(string path) {
		TextAsset taUnit = (TextAsset)Resources.Load<TextAsset>("UnitFiles/" + path);

		if (taUnit == null) {
			Debug.LogWarning("The unit file \"" + path + "\" was not found.");
			return false;
		}

		XDocument xmlDoc = XDocument.Load(new StringReader(taUnit.text));

		foreach (XElement xroot in xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					sName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					sDescription = xlayer1.Value;
				}
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
				else if (xlayer1.Name == "physattack") {
					fPhysAttack = float.Parse(xlayer1.Value);
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
				
//				if (xlayer1.Name == "abilities") {
//					//Make sure only the in-use ones are ticked
//					//					foreach (KeyValuePair<string, bool> pair in s_dAbilityToggles) {
//					//						pair. = false;
//					//					}
//					ReloadAbilities();
//					
//					foreach (XElement xlayer2 in xlayer1.Elements()) {
//						if (AbilityBox.s_dAbilityLookup.ContainsKey(xlayer2.Value)) {
//							s_dAbilityToggles[xlayer2.Value] = true;
//							if (AbilityBox.s_dAbilityLookup[xlayer2.Value].fIntensity > 0) {
//								s_dAbilityPower[xlayer2.Value] = float.Parse(xlayer2.FirstAttribute.Value);
//							}
//						}
//					}
//				}
			}
		}

		return true;
	}

	public void LoadUnitAbilities(string path) {
		//
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
}
