using UnityEngine;

public class MapEditor : MonoBehaviour {

	public GameObject goProtoTile;
	
    public int iRows;
    public int iColumns;
	
    public float fTileWidth = 1.0f;
    public float fTileHeight = 0.1f;

	private float fMapWidth;
	private float fMapHeight;
	private Vector3 vMapPos;
	
    [HideInInspector]
    public Vector3 vCursorPos;
	
    public MapEditor() {
        iColumns = 20;
        iRows = 10;
    }
	
    private void OnDrawGizmosSelected() {
        fMapWidth = iColumns * fTileWidth;
        fMapHeight = iRows * fTileHeight;
        vMapPos = transform.position;

		//Map outline
        Gizmos.color = Color.magenta;
        Gizmos.DrawLine(vMapPos, vMapPos + new Vector3(fMapWidth, 0, 0));
        Gizmos.DrawLine(vMapPos, vMapPos + new Vector3(0, fMapHeight, 0));
        Gizmos.DrawLine(vMapPos + new Vector3(fMapWidth, 0, 0), vMapPos + new Vector3(fMapWidth, fMapHeight, 0));
        Gizmos.DrawLine(vMapPos + new Vector3(0, fMapHeight, 0), vMapPos + new Vector3(fMapWidth, fMapHeight, 0));

		//Grid
        Gizmos.color = Color.grey;
        for (float i = 1; i < iColumns; i++) {
            Gizmos.DrawLine(vMapPos + new Vector3(i * fTileWidth, 0, 0), vMapPos + new Vector3(i * fTileWidth, fMapHeight, 0));
        }
        
        for (float i = 1; i < iRows; i++) {
            Gizmos.DrawLine(vMapPos + new Vector3(0, i * fTileHeight, 0), vMapPos + new Vector3(fMapWidth, i * fTileHeight, 0));
        }

        //Cursor
        Gizmos.color = Color.yellow;    
        Gizmos.DrawWireCube(vCursorPos, new Vector3(fTileWidth, fTileHeight, 1) * 1.1f);
    }
}