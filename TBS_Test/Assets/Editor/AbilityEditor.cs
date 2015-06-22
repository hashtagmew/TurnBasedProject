using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System.Text;

public class AbilityEditor : EditorWindow {

	static private GUIStyle s_guiStyleDisabled;
	static private GUIStyle s_guiStyleAlert;
	
	static private Vector2 s_vScrollPos;
	static private string s_sLastFile;

	static private string s_sName = "Goggles";
	static private ABILITY_TYPE s_iType = ABILITY_TYPE.NONE;

	//Menu items
	[MenuItem("Window/Ability Editor %&a")]
	private static void NewMenuOption() {
		Init(); 
	}

	//Window
	public static void Init() {
		AbilityEditor window = (AbilityEditor)EditorWindow.GetWindow(typeof(AbilityEditor));
		
		//s_xmlDoc = new XDocument();
		
		//ReloadAbilities();
		
		s_guiStyleDisabled = new GUIStyle();
		s_guiStyleDisabled.normal.textColor = Color.grey;
		
		s_guiStyleAlert = new GUIStyle();
		s_guiStyleAlert.normal.textColor = Color.red;
		
		window.Show();
	}
	
	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);

		//========= GUI BEGIN 
		//File Select
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal("box");
		if (GUILayout.Button("Load Ability")) {
			s_sLastFile = EditorUtility.OpenFilePanel("Load unit", "/Assets/Scripts/Units/UnitFiles/", "xml");
			if (s_sLastFile.Length != 0) {
				GUI.FocusControl("Top");
				LoadAbilityFile(s_sLastFile);
			}
		}
		
		if (GUILayout.Button("Save Ability")) {
			s_sLastFile = EditorUtility.SaveFilePanel("Save unit", "/Assets/Scripts/Units/UnitFiles/", s_sName.ToLower(), "xml");
			if (s_sLastFile.Length != 0) {
				GUI.FocusControl("Top");
				SaveAbilityFile(s_sLastFile);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");

		//Name
		GUILayout.BeginHorizontal("box");
		GUI.SetNextControlName("Top");
		GUILayout.Label("Name", EditorStyles.boldLabel);
		s_sName = GUILayout.TextField(s_sName, 28, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Type
		GUILayout.BeginHorizontal("box");
		GUILayout.Label("Type", EditorStyles.boldLabel);
		s_iType = (ABILITY_TYPE)EditorGUILayout.EnumPopup(s_iType);
		GUILayout.EndHorizontal();

		if (s_iType != ABILITY_TYPE.NONE) {
			if (s_iType == ABILITY_TYPE.ACTIVE) {
				//OnActivation
				GUILayout.BeginHorizontal("box");
				GUILayout.Label("On Activation", EditorStyles.boldLabel);
				//
				GUILayout.EndHorizontal();

				//OnResolution
				GUILayout.BeginHorizontal("box");
				GUILayout.Label("On Resolution", EditorStyles.boldLabel);
				//
				GUILayout.EndHorizontal();
			}
			else {
				//Passive Effect
				GUILayout.BeginHorizontal("box");
				GUILayout.Label("On Passive", EditorStyles.boldLabel);
				//
				GUILayout.EndHorizontal();
			}
		}

		GUILayout.EndVertical();
		//========= GUI END 

		GUILayout.EndScrollView();
	}

	static public void LoadAbilityFile(string path) {
		//
	}

	static public void SaveAbilityFile(string path) {
		//
	}
}
