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
	static private Vector2 s_vActivationEffectScrollPos;
	static private Vector2 s_vResolutionEffectScrollPos;
	static private Vector2 s_vPassiveEffectScrollPos;
	static private string s_sLastFile;

	static private bool s_bShowActivationEffects;
	static private bool s_bShowResolutionEffects;
	static private bool s_bShowPassiveEffects;

//	static private Dictionary<string, bool> s_dActivationEffectToggles;
//	static private Dictionary<string, bool> s_dResolutionEffectToggles;
//	static private Dictionary<string, bool> s_dPassiveEffectToggles;
//
//	static private Dictionary<string, float> s_dActivationEffectPower;
//	static private Dictionary<string, float> s_dResolutionEffectPower;
//	static private Dictionary<string, float> s_dPassiveEffectPower;

	static private XDocument s_xmlDoc;

	//Bits
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
		
		ReloadEffects();
		
		s_guiStyleDisabled = new GUIStyle();
		s_guiStyleDisabled.normal.textColor = Color.grey;
		
		s_guiStyleAlert = new GUIStyle();
		s_guiStyleAlert.normal.textColor = Color.red;
		
		window.Show();
	}
	
	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);

		//========= GUI BEGIN 
		GUILayout.BeginHorizontal();
		GUILayout.BeginVertical();

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

		if (GUILayout.Button("R", GUILayout.Width(25))) {
			ReloadEffects();
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
				GUILayout.EndHorizontal();
			}
		}

		GUILayout.EndVertical();

		GUILayout.EndVertical();



		if (s_iType == ABILITY_TYPE.ACTIVE) {
			//=========== GUI START	Active Effects
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			s_bShowActivationEffects = EditorGUILayout.Foldout(s_bShowActivationEffects, "Activation");

			GUILayout.EndHorizontal();
			
			
			if (s_bShowActivationEffects) {
				s_vActivationEffectScrollPos = GUILayout.BeginScrollView(s_vActivationEffectScrollPos);
				GUILayout.Label(new GUIContent("Coming soon", "Tooltip"));
				GUILayout.EndScrollView();
			}
			//=========== GUI END	ACTIVE Effects

			//=========== GUI START	Resolve Effects
			GUILayout.BeginHorizontal();
			s_bShowResolutionEffects = EditorGUILayout.Foldout(s_bShowResolutionEffects, "Resolution");

			GUILayout.EndHorizontal();
			
			
			if (s_bShowResolutionEffects) {
				s_vResolutionEffectScrollPos = GUILayout.BeginScrollView(s_vResolutionEffectScrollPos);
				GUILayout.Label(new GUIContent("Coming soon", "Tooltip"));
				GUILayout.EndScrollView();
			}
			GUILayout.EndVertical();
			//=========== GUI END	RESOLVE Effects
		}
		else if (s_iType != ABILITY_TYPE.NONE) {
			//=========== GUI START	Passive Effects
			GUILayout.BeginVertical("box");
			GUILayout.BeginHorizontal();
			s_bShowPassiveEffects = EditorGUILayout.Foldout(s_bShowPassiveEffects, "Passive");

			GUILayout.EndHorizontal();
			
			
			if (s_bShowPassiveEffects) {
				s_vPassiveEffectScrollPos = GUILayout.BeginScrollView(s_vPassiveEffectScrollPos);
				GUILayout.Label(new GUIContent("Coming soon", "Tooltip"));
				GUILayout.EndScrollView();
			}
			GUILayout.EndVertical();
			//=========== GUI END	Passive Effects
		}
		GUILayout.EndHorizontal();
		//========= GUI END 
		
		GUILayout.EndScrollView();
	}

	static public void LoadAbilityFile(string path) {
		//
	}

	static public void SaveAbilityFile(string path) {
		//
	}

	public static void ReloadEffects() {
//		if (s_dActivationEffectToggles == null) {
//			s_dActivationEffectToggles = new Dictionary<string, bool>();
//		}
//
//		if (s_dResolutionEffectToggles == null) {
//			s_dResolutionEffectToggles = new Dictionary<string, bool>();
//		}
//
//		if (s_dPassiveEffectToggles == null) {
//			s_dPassiveEffectToggles = new Dictionary<string, bool>();
//		}
//		
//		if (s_dActivationEffectPower == null) {
//			s_dActivationEffectPower = new Dictionary<string, float>();
//		}
//
//		if (s_dResolutionEffectPower == null) {
//			s_dResolutionEffectPower = new Dictionary<string, float>();
//		}
//
//		if (s_dPassiveEffectPower == null) {
//			s_dPassiveEffectPower = new Dictionary<string, float>();
//		}
//		
//		s_dActivationEffectToggles.Clear();
//		s_dResolutionEffectToggles.Clear();
//		s_dPassiveEffectToggles.Clear();
//
//		s_dActivationEffectPower.Clear();
//		s_dResolutionEffectPower.Clear();
//		s_dPassiveEffectPower.Clear();
//		
//		foreach (KeyValuePair<string, Ability> pair in EffectBox.s_dEffectLookup) {
//			s_dEffectToggles.Add(abilpair.Key, false);
//			s_dEffectPower.Add(abilpair.Key, 0.0f);
//		}
	}
}
