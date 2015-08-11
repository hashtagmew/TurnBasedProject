using UnityEngine;
using UnityEditor;

using System;

[CustomEditor(typeof(MapEditor))]
public class MapEditorLogic : Editor {

    private Vector3 vMouseHitPos;
	private MapEditor mapedTarget;
	private Vector2 vTilePos;
	private GameObject goTileCheck;
	
    private void OnSceneGUI() {
		Event current = Event.current;

        if (this.UpdateHitPosition()) {
            SceneView.RepaintAll();
        }
		
        RecalculateCursor();
		
        if (this.IsMouseOnLayer()) {
            if (current.type == EventType.MouseDown || current.type == EventType.MouseDrag) {
				//Erase
                if (current.button == 1) {
                    Erase();
                    current.Use();
                }
				//Draw
                else if (current.button == 0) {
                    Draw();
                    current.Use();
                }
            }
        }
		
        Handles.BeginGUI();
        GUI.Label(new Rect(10, Screen.height - 90, 150, 100), "Left Click: Draw");
        GUI.Label(new Rect(10, Screen.height - 105, 150, 100), "Right Click: Erase");
		GUI.Label(new Rect(10, Screen.height - 120, 300, 100), "See Tile Picker tab to choose tiles");
        Handles.EndGUI();
    }
	
    private void OnEnable() {
        Tools.current = Tool.View;
        Tools.viewTool = ViewTool.FPS;
    }
	
    private void Draw() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}
		
        vTilePos = this.GetTilePosition();	
        goTileCheck = GameObject.Find(string.Format("Tile_{0}_{1}", vTilePos.x, vTilePos.y));
		
		if (goTileCheck != null && goTileCheck.transform.parent != mapedTarget.transform) {
            return;
        }
		
        if (goTileCheck == null) {
            //goTileCheck = GameObject.CreatePrimitive(PrimitiveType.Cube);
			goTileCheck = GameObject.Instantiate(((MapEditor)target).goProtoTile);
        }
		
		Vector3 vLocalPos = new Vector3((vTilePos.x * mapedTarget.fTileWidth) + (mapedTarget.fTileWidth / 2), (vTilePos.y * mapedTarget.fTileHeight) + (mapedTarget.fTileHeight / 2));
		goTileCheck.transform.position = mapedTarget.transform.position + vLocalPos;
		goTileCheck.transform.localScale = new Vector3(mapedTarget.fTileWidth, mapedTarget.fTileHeight, 0.1f);
		goTileCheck.transform.parent = mapedTarget.transform;
        goTileCheck.name = string.Format("Tile_{0}_{1}", vTilePos.x, vTilePos.y);

		goTileCheck.GetComponent<MapTile>().Terraform(TilePicker.s_iSelection);
    }
	
    private void Erase() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}
		
        Vector2 vTilePos = GetTilePosition();
		GameObject goTemp = GameObject.Find(string.Format("Tile_{0}_{1}", vTilePos.x, vTilePos.y));
		
		if (goTemp != null && goTemp.transform.parent == mapedTarget.transform) {
			UnityEngine.Object.DestroyImmediate(goTemp);
        }
    }
	
    private Vector2 GetTilePosition() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}
		
		Vector3 vTempPos = new Vector3(this.vMouseHitPos.x / mapedTarget.fTileWidth, this.vMouseHitPos.y / mapedTarget.fTileHeight, mapedTarget.transform.position.z);
		
        vTempPos = new Vector3((int)Math.Round(vTempPos.x, 5, MidpointRounding.ToEven), (int)Math.Round(vTempPos.y, 5, MidpointRounding.ToEven), 0);
		
        int iCol = (int)vTempPos.x;
        int iRow = (int)vTempPos.y;

        if (iRow < 0) {
            iRow = 0;
        }

		if (iRow > mapedTarget.iRows - 1) {
			iRow = mapedTarget.iRows - 1;
        }

        if (iCol < 0) {
            iCol = 0;
        }

		if (iCol > mapedTarget.iColumns - 1) {
			iCol = mapedTarget.iColumns - 1;
        }
		
        return new Vector2(iCol, iRow);
    }

    private bool IsMouseOnLayer() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}
		
		if (vMouseHitPos.x > 0 && vMouseHitPos.x < (mapedTarget.iColumns * mapedTarget.fTileWidth) 
		    && vMouseHitPos.y > 0 && vMouseHitPos.y < (mapedTarget.iRows * mapedTarget.fTileHeight)) {
			return true;
		}
		else {
			return false;
		}
    }

    private void RecalculateCursor() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}
		
        Vector2 tilepos = GetTilePosition();
		Vector3 vTempPos = new Vector3(tilepos.x * mapedTarget.fTileWidth, tilepos.y * mapedTarget.fTileHeight, 0);
		
		mapedTarget.vCursorPos = mapedTarget.transform.position + new Vector3(vTempPos.x + (mapedTarget.fTileWidth / 2), vTempPos.y + (mapedTarget.fTileHeight / 2), 0);
    }

    private bool UpdateHitPosition() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}
		
		Plane plane = new Plane(mapedTarget.transform.TransformDirection(Vector3.forward), mapedTarget.transform.position);
        Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
        Vector3 vHit = new Vector3();
        float fHitDist;
		
        if (plane.Raycast(ray, out fHitDist)) {
            vHit = ray.origin + (ray.direction.normalized * fHitDist);
        }

        //Convert from world space to local space
		Vector3 vConvertedPos = mapedTarget.transform.InverseTransformPoint(vHit);
		
        if (vConvertedPos != vMouseHitPos) {
            vMouseHitPos = vConvertedPos;
            return true;
        }
		
        return false;
    }

	public void ClearAll() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}

		for (int i = 0; i < mapedTarget.iColumns; i++) {
			for (int j = 0; j < mapedTarget.iRows; j++) {
				goTileCheck = GameObject.Find(string.Format("Tile_{0}_{1}", i, j));
				if (goTileCheck != null) {
					goTileCheck.GetComponent<MapTile>().Terraform(TERRAIN_TYPE.NONE);
				}
			}
		}
	}

	public void EraseAll() {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}
		
		for (int i = 0; i < mapedTarget.iColumns; i++) {
			for (int j = 0; j < mapedTarget.iRows; j++) {
				goTileCheck = GameObject.Find(string.Format("Tile_{0}_{1}", i, j));
				if (goTileCheck != null) {
					GameObject.DestroyImmediate(goTileCheck);
				}
			}
		}
	}

	public void Resize(int rows, int columns) {
		if (mapedTarget == null) {
			mapedTarget = (MapEditor)this.target;
		}

		if (columns < mapedTarget.iColumns) {
			for (int i = columns; i < mapedTarget.iColumns; i++) {
				for (int j = 0; j < mapedTarget.iRows; j++) {
					goTileCheck = GameObject.Find(string.Format("Tile_{0}_{1}", i, j));
					if (goTileCheck != null) {
						GameObject.DestroyImmediate(goTileCheck);
					}
				}
			}
		}

		if (rows < mapedTarget.iRows) {
			for (int i = 0; i < mapedTarget.iColumns; i++) {
				for (int j = rows; j < mapedTarget.iRows; j++) {
					goTileCheck = GameObject.Find(string.Format("Tile_{0}_{1}", i, j));
					if (goTileCheck != null) {
						GameObject.DestroyImmediate(goTileCheck);
					}
				}
			}
		}

		if (mapedTarget) {
			mapedTarget.iRows = rows;
			mapedTarget.iColumns = columns;
		}
	}

	public MapEditor GetMapEditor() {
		return mapedTarget = (MapEditor)this.target;
	}
}