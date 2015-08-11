using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System.Text;

public class TilePicker : EditorWindow {

	static private Vector2 s_vScrollPos;
	static private MapEditorLogic maped;

	static public TERRAIN_TYPE s_iLastSelection = TERRAIN_TYPE.BIOMASS;
	static public TERRAIN_TYPE s_iSelection = TERRAIN_TYPE.NONE;
	static public MAPED_TOOL s_iTool;

	static public Texture s_texTile;

	static public int iColumns;
	static public int iRows;

	//Window
	[MenuItem("Map Edit/Tile Picker")]
	static void Init() {
		TilePicker window = (TilePicker)EditorWindow.GetWindow(typeof(TilePicker));
		
		window.Show();
	}

	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);

		//
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();

		GUI.SetNextControlName("Top");
		GUILayout.Label("Tile");
		s_iSelection = (TERRAIN_TYPE)EditorGUILayout.EnumPopup(s_iSelection);
		GUILayout.EndHorizontal();

		if (s_iLastSelection != s_iSelection) {
			LoadTileTexture();
		}
		GUILayout.Label(s_texTile, GUILayout.Width(100), GUILayout.Height(100));
		GUILayout.EndVertical();


		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();

		GUILayout.Label("Tools");
		s_iTool = (MAPED_TOOL)EditorGUILayout.EnumPopup(s_iTool);
		GUILayout.EndHorizontal();

		if (GUILayout.Button("Clear All")) {
			GetMapEditorLogic().ClearAll();
		}

		if (GUILayout.Button("Erase All")) {
			GetMapEditorLogic().EraseAll();
		}
		GUILayout.EndVertical();


		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();

		if (GetMapEditorLogic() != null && GetMapEditorLogic().GetMapEditor() != null) {
			GUILayout.Label("Current size: " + GetMapEditorLogic().GetMapEditor().iColumns.ToString() + " - " + GetMapEditorLogic().GetMapEditor().iRows.ToString());
		}
		GUILayout.EndHorizontal();


		GUILayout.BeginHorizontal();
		iColumns = EditorGUILayout.IntField("C", iColumns);
		iColumns = Mathf.Clamp(iColumns, 5, 100);
		iRows = EditorGUILayout.IntField("R", iRows);
		iRows = Mathf.Clamp(iRows, 5, 100);
		GUILayout.EndHorizontal();


		if (GUILayout.Button("Resize")) {
			if (GetMapEditorLogic() != null) {
				GetMapEditorLogic().Resize(iRows, iColumns);
				GUI.FocusControl("Top");
			}
		}

		GUILayout.EndVertical();

		GUILayout.EndScrollView();
	}

	static private MapEditorLogic GetMapEditorLogic() {
		MapEditorLogic[] editors = (MapEditorLogic[])Resources.FindObjectsOfTypeAll(typeof(MapEditorLogic));
		if (editors.Length > 0) {
			return editors[0];
		}
		else {
			return null;
		}
	}

	static private void LoadTileTexture() {
		if (s_iSelection == TERRAIN_TYPE.DUST_BOWL) {
			s_texTile = ((Material)Resources.Load("Terrain/dirt")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.PLAINS) {
			s_texTile = ((Material)Resources.Load("Terrain/grass")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.RIVER) {
			s_texTile = ((Material)Resources.Load("Terrain/water")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.WASTELAND) {
			s_texTile = ((Material)Resources.Load("Terrain/wasteland")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.DESERT) {
			s_texTile = ((Material)Resources.Load("Terrain/desert")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.QUAGMIRE) {
			s_texTile = ((Material)Resources.Load("Terrain/swamp")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.BIOMASS) {
			s_texTile = ((Material)Resources.Load("Terrain/flesh")).GetTexture(0);
		}
		else if (s_iSelection == TERRAIN_TYPE.PAVEMENT) {
			s_texTile = ((Material)Resources.Load("Terrain/tiles")).GetTexture(0);
		}
		else {
			s_texTile = ((Material)Resources.Load("Terrain/none")).GetTexture(0);
		}

		s_iLastSelection = s_iSelection;
	}
}
