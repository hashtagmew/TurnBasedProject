using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class ClickableTile : MonoBehaviour {

	public int tileX;
	public int tileY;
	public TileMap map;
	public NetGameManager mngNetGame;

	void Start() {
		mngNetGame = GameObject.FindGameObjectWithTag ("Managers").GetComponent<NetGameManager> ();
	}

	void OnMouseUp() {
		//Debug.Log ("Click!");

		if(EventSystem.current.IsPointerOverGameObject())
			return;

		//Should we move?
		if (map.selectedUnit != null) {
			if (mngNetGame.eLocalState == GAMEPLAY_STATE.IDLE && mngNetGame.IsLocalTurn()) {
				map.GeneratePathTo (tileX, tileY);
			}
		}
	}

}
