using UnityEngine;
using System.Collections.Generic;

//Deprecated pathfinding beta unit

public class Unit : MonoBehaviour {

	// tileX and tileY represent the correct map-tile position
	// for this piece.  Note that this doesn't necessarily mean
	// the world-space coordinates, because our map might be scaled
	// or offset or something of that nature.  Also, during movement
	// animations, we are going to be somewhere in between tiles.
	public int tileX;
	public int tileY;

	public UNIT_DIR eGridDirection = UNIT_DIR.DOWN_RIGHT;

	public TileMap map { get; private set; }

	// Our pathfinding info.  Null if we have no destination ordered.
	public List<Node> currentPath = null;

	// How far this unit can move in one turn. Note that some tiles cost extra.
	//int moveSpeed = 2;
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

	void Start() {
		this.map = GameObject.FindGameObjectWithTag ("MainMap").GetComponent<TileMap> ();
		myRend = this.GetComponentInChildren<SpriteRenderer> ();
	}

	void Update() {

//		//End turn
		

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
		map.selectedUnit.GetComponent<Unit>().currentPath = null;
		//remainingMovement = resetMovement;
		map.selectedUnit.GetComponent<Unit> ().remainingMovement = map.selectedUnit.GetComponent<Unit> ().resetMovement;
	}

	// The "Next Turn" button calls this.
	public void NextTurn() {

		// Make sure to wrap-up any outstanding movement left over.
		while(currentPath!=null && remainingMovement > 0) {
			AdvancePathing();

		}

		// Reset our available movement points.

	}
}
