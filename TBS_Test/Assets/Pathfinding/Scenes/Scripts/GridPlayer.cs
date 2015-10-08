using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GridPlayer : Pathfinding
{
	private TileCursor T_C;

    public Camera playerCam;
//    public Camera Maincamera;

	private Vector3 vTempPos;

	public Ray ray;
	public RaycastHit rayHit;

    public GUIStyle bgStyle;


	void Update () 
    {
        FindPath();
        if (Path.Count > 0)
        {
            MoveMethod();
        }
	}

    private void FindPath()
    {

//        if (Input.GetButtonDown ("Fire1") && Input.mousePosition.x > (Screen.width / 10) * 7F && Input.mousePosition.y < (Screen.height / 10) * 3.5F) {
//			//Call to the player map
//			Ray ray = Maincamera.ScreenPointToRay (Input.mousePosition);
//			//RaycastHit rayHit;
//			if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Terrain"))) {
//				vTempPos = rayHit.point;
//
//				vTempPos.x = rayHit.collider.gameObject.transform.position.x;
//				vTempPos.y = 0.2f;
//				//vTempPos.z = (int)vTempPos.z;
//				vTempPos.z = rayHit.collider.gameObject.transform.position.z;
//
//				if (Physics.Raycast (ray, out rayHit, Mathf.Infinity)) {             
//					FindPath (transform.position, rayHit.point);
//				}  
//			}
		if (Input.GetButtonDown ("Fire1")) {
			//Call minimap
			Ray ray = playerCam.ScreenPointToRay (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, 0));

			if (Physics.Raycast (ray, out rayHit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Terrain"))) {
				vTempPos = rayHit.point;
				
				vTempPos.x = rayHit.collider.gameObject.transform.position.x;
				vTempPos.y = 0.2f;
				//vTempPos.z = (int)vTempPos.z;
				vTempPos.z = rayHit.collider.gameObject.transform.position.z;
				
				if (Physics.Raycast (ray, out rayHit, Mathf.Infinity)) {             
					FindPath (transform.position, rayHit.point);
				}  
			}
		}
	}

    private void MoveMethod()
    {
        if (Path.Count > 0)
        {
            Vector3 direction = (Path[0] - transform.position).normalized;

            transform.position = Vector3.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * 3F);
            if (transform.position.x < Path[0].x + 0.4F && transform.position.x > Path[0].x - 0.4F && transform.position.z > Path[0].z - 0.4F && transform.position.z < Path[0].z + 0.4F)
            {
                Path.RemoveAt(0);
            }
			if (Path.Count <= 0){
				Vector3 vTemp = this.transform.position;
				vTemp.x = Mathf.Round(vTemp.x);
				vTemp.z = Mathf.Round(vTemp.z);
				this.transform.position = Vector3.Lerp(this.transform.position, vTempPos, 10f);
			} 

            RaycastHit[] hit = Physics.RaycastAll(transform.position + (Vector3.up * 20F), Vector3.down, 100);
            float maxY = -Mathf.Infinity;
            foreach (RaycastHit h in hit)
            {
                if (h.transform.tag == "Untagged")
                {
                    if (maxY < h.point.y)
                    {
                        maxY = h.point.y;
                    }
                }
            }
            transform.position = new Vector3(transform.position.x, maxY + 0.2F, transform.position.z);
        }
    }

    void OnGUI()
    {
        GUI.Label(new Rect(0, 0, Screen.width, Screen.height), "", bgStyle);
    }
}
