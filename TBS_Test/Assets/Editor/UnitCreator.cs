using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using System.Text;

public class UnitCreator : EditorWindow {

	//static public List<GameUnit> sl_units;
	//static public GameUnit s_curunit;

	static private GUIStyle s_guiStyleDisabled;
	static private GUIStyle s_guiStyleAlert;

	static private bool s_bCleanRuntime;
	static private Vector2 s_vScrollPos;

	static private XDocument s_xmlDoc;
	static private string s_sLastFile;

	//Unit bits
	static private string s_sName = "Noname";
	static private UNIT_TYPE s_iType = UNIT_TYPE.NONE;
	
	static private int s_iLevel = 1;
	static private float s_fXP = 0.0f;
	//fXPtoNext = 100.0f;
	
	static private float s_fHealth = 50.0f;
	static private float s_fMovement = 6.0f;
	static private float s_fVision = 2.0f;
	
	static private float s_fPhysAttack = 4.0f;
	static private float s_fRangAttack = 4.0f;
	static private float s_fMagiAttack = 4.0f;
	
	static private float s_fResistance = 2.0f;
	static private float s_fDefence = 3.0f;

	//Menu items
	[MenuItem("Window/Unit Editor %&u")]
	private static void NewMenuOption() {
		Init(); 
	}

	//Window
	public static void Init() {
		UnitCreator window = (UnitCreator)EditorWindow.GetWindow(typeof(UnitCreator));

		s_xmlDoc = new XDocument();
		//s_curunit = new GameUnit();
		
		s_guiStyleDisabled = new GUIStyle();
		s_guiStyleDisabled.normal.textColor = Color.grey;
		
		s_guiStyleAlert = new GUIStyle();
		s_guiStyleAlert.normal.textColor = Color.red;
		
		window.Show();
	}

	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);

		//File Select
		//========= GUI BEGIN
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal("box");
		if (GUILayout.Button("Load Unit")) {
			s_sLastFile = EditorUtility.OpenFilePanel("Load unit", "/Assets/Scripts/Units/UnitFiles/", "xml");
			if (s_sLastFile.Length != 0) {
				LoadUnitFile(s_sLastFile);
			}
		}

		if (GUILayout.Button("Save Unit")) {
			s_sLastFile = EditorUtility.SaveFilePanel("Save unit", "/Assets/Scripts/Units/UnitFiles/", "unit", "xml");
			if (s_sLastFile.Length != 0) {
				SaveUnitFile(s_sLastFile);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		//========= GUI END

		//========= GUI BEGIN 
		GUILayout.BeginVertical("box");
		
		//=========== GUI BEGIN Stats
		GUILayout.BeginHorizontal("box");
		GUILayout.Label("Name", EditorStyles.boldLabel);
		s_sName = GUILayout.TextField(s_sName, 28,  GUILayout.Width(200));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal("box");
		GUILayout.Label("Name", EditorStyles.boldLabel);
		s_sName = GUILayout.TextField(s_sName, 28,  GUILayout.Width(200));
		GUILayout.EndHorizontal();
		//=========== GUI END   Stats
		
		//========= GUI END
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	public GameUnit LoadUnitFile(string path) {
		//GameUnit newunit = new GameUnit();

		return null;
	}

	public bool SaveUnitFile(string path) {
		//GameUnit newunit = new GameUnit();
		
		return true;
	}
}
