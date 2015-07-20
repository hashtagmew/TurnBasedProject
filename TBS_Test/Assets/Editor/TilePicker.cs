using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System.Text;

public enum MAP_TILE {
	NONE = 0,
	DIRT,
	PLAINS,
	WASTELAND
}

public class TilePicker : EditorWindow {

	static private Vector2 s_vScrollPos;
	
	static public MAP_TILE s_iSelection = 0;

	//Window
	[MenuItem("Map Edit/Tile Picker")]
	static void Init() {
		TilePicker window = (TilePicker)EditorWindow.GetWindow(typeof(TilePicker));
		
		window.Show();
	}

	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);

		GUILayout.BeginVertical();
		GUILayout.BeginHorizontal("box");
		GUILayout.Label("Tile");
		s_iSelection = (MAP_TILE)EditorGUILayout.EnumPopup(s_iSelection);
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		GUILayout.EndScrollView();
	}
}
