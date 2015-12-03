using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System.Text;

public class UnitEditor : EditorWindow {

	static private GUIStyle s_guiStyleDisabled;
	static private GUIStyle s_guiStyleAlert;
	
	static private Vector2 s_vScrollPos;
	static private Vector2 s_vAbilityScrollPos;
	static private bool s_bShowAbilities = false;

	static private Dictionary<string, bool> s_dAbilityToggles;
	static private Dictionary<string, float> s_dAbilityPower;

	static private XDocument s_xmlDoc;
	static private string s_sLastFile;

	//Unit bits
	static private string s_sName = "Noname";
	static private string s_sDescription = "A unit that does things.";
	//TODO: Build costs?
	
	static private int s_iLevel = 1;
	//static private float s_fXP = 0.0f;
	//fXPtoNext = 100.0f;
	static private float s_fAP = 3.0f;
	static private float s_fMaxAP = 3.0f;
	
	static private float s_fHealth = 50.0f;
	static private float s_fMaxHealth = 50.0f;
	static private float s_fMana = 0.0f;
	static private float s_fMaxMana = 0.0f;

	static private float s_fMovement = 6.0f;
	static private float s_fVision = 2.0f;

	static private float s_fDeployCost = 1.0f;

	static private float s_fAttack = 1.0f;
//	static private float s_fPhysAttack = 4.0f;
//	static private float s_fRangAttack = 4.0f;
//	static private float s_fMagiAttack = 4.0f;
	
	static private float s_fResistance = 2.0f;
	static private float s_fDefence = 3.0f;

	static private string s_sSpriteFilename = "tile";
	static private string s_sSoundsetFilename = "none";

	//Window
	[MenuItem("Window/Unit Editor %&u")]
	static void Init() {
		UnitEditor window = (UnitEditor)EditorWindow.GetWindow(typeof(UnitEditor));

		s_xmlDoc = new XDocument();

		ReloadAbilities();
		
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
		if (GUILayout.Button("Load Unit", GUILayout.MinWidth(5.0f))) {
			s_sLastFile = EditorUtility.OpenFilePanel("Load unit", "/Assets/Scripts/Units/UnitFiles/", "xml");
			if (s_sLastFile.Length != 0) {
				GUI.FocusControl("Top");
				LoadUnitFile(s_sLastFile);
			}
		}

		if (GUILayout.Button("Save Unit")) {
			s_sLastFile = EditorUtility.SaveFilePanel("Save unit", "/Assets/Scripts/Units/UnitFiles/", s_sName.ToLower(), "xml");
			if (s_sLastFile.Length != 0) {
				GUI.FocusControl("Top");
				SaveUnitFile(s_sLastFile);
			}
		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		//========= GUI END

		//========= GUI BEGIN 
		GUILayout.BeginVertical("box");
		
		//=========== GUI BEGIN Stats
		//Name
		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		GUI.SetNextControlName("Top");
		GUILayout.Label("Name", EditorStyles.boldLabel);
		s_sName = GUILayout.TextField(s_sName, 28, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Description
		GUILayout.BeginHorizontal();
		GUILayout.Label("Description", EditorStyles.boldLabel);
		s_sDescription = GUILayout.TextArea(s_sDescription, 255, GUILayout.Width(200), GUILayout.Height(60));
		GUILayout.EndHorizontal();

		GUILayout.Space(15);

		//Tier
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("Tier", EditorStyles.boldLabel);
//		s_iLevel = EditorGUILayout.IntField(s_iLevel, GUILayout.Width(200));
//		GUILayout.EndHorizontal();

		//AP
		GUILayout.BeginHorizontal();
		GUILayout.Label("AP", EditorStyles.boldLabel);
		s_fAP = EditorGUILayout.FloatField(s_fAP, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Deploy Cost
		GUILayout.BeginHorizontal();
		GUILayout.Label("Deploy Cost", EditorStyles.boldLabel);
		s_fDeployCost = EditorGUILayout.FloatField(s_fDeployCost, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		//HP
		GUILayout.BeginVertical("box");

		GUILayout.BeginHorizontal();
		GUILayout.Label("Health", EditorStyles.boldLabel);
		s_fHealth = EditorGUILayout.FloatField(s_fHealth, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		GUILayout.BeginHorizontal();
		GUILayout.Label("Max Health", EditorStyles.boldLabel);
		s_fMaxHealth = EditorGUILayout.FloatField(s_fMaxHealth, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Mana
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("Mana", EditorStyles.boldLabel);
//		s_fMana = EditorGUILayout.FloatField(s_fMana, GUILayout.Width(200));
//		GUILayout.EndHorizontal();
//
//		GUILayout.BeginHorizontal();
//		GUILayout.Label("Max Mana", EditorStyles.boldLabel);
//		s_fMaxMana = EditorGUILayout.FloatField(s_fMaxMana, GUILayout.Width(200));
//		GUILayout.EndHorizontal();

		GUILayout.Space(15);

		//Movement
		GUILayout.BeginHorizontal();
		GUILayout.Label("Movement", EditorStyles.boldLabel);
		s_fMovement = EditorGUILayout.FloatField(s_fMovement, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Vision
		GUILayout.BeginHorizontal();
		GUILayout.Label("Vision", EditorStyles.boldLabel);
		s_fVision = EditorGUILayout.FloatField(s_fVision, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Sprite
		GUILayout.BeginHorizontal();
		GUILayout.Label("Sprite", EditorStyles.boldLabel);
		s_sSpriteFilename = GUILayout.TextField(s_sSpriteFilename, 128, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Sound
		GUILayout.BeginHorizontal();
		GUILayout.Label("Soundset", EditorStyles.boldLabel);
		s_sSoundsetFilename = GUILayout.TextField(s_sSoundsetFilename, 128, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		GUILayout.EndVertical();

		//Attack
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		GUILayout.Label("Attack", EditorStyles.boldLabel);
		s_fAttack = EditorGUILayout.FloatField(s_fAttack, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Defence
		GUILayout.BeginHorizontal();
		GUILayout.Label("Defence", EditorStyles.boldLabel);
		s_fDefence = EditorGUILayout.FloatField(s_fDefence, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Resistance
		GUILayout.BeginHorizontal();
		GUILayout.Label("Resistance", EditorStyles.boldLabel);
		s_fResistance = EditorGUILayout.FloatField(s_fResistance, GUILayout.Width(200));
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();
		//=========== GUI END   Stats

		//=========== GUI START	Abilities
		GUILayout.BeginVertical("box");
		GUILayout.BeginHorizontal();
		//GUILayout.Label("Abilities", EditorStyles.boldLabel);
		s_bShowAbilities = EditorGUILayout.Foldout(s_bShowAbilities, "Abilities");

		if (GUILayout.Button ("Refresh")) {
			ReloadAbilities();
		}
		GUILayout.EndHorizontal();
		
		if (s_bShowAbilities) {


			s_vAbilityScrollPos = GUILayout.BeginScrollView (s_vAbilityScrollPos);
			if (s_dAbilityToggles == null) {
				ReloadAbilities ();
			}

			if (s_dAbilityToggles != null && s_dAbilityToggles.Count != 0) {
				foreach (KeyValuePair<string, Ability> abilpair in AbilityBox.s_dAbilityLookup) {
					//Category breaks
					if (abilpair.Key == "Melee Strike") {
						GUILayout.Space (5.0f);
						GUILayout.Label ("Activated");
					}

					if (abilpair.Key == "Guard") {
						GUILayout.Space (5.0f);
						GUILayout.Label ("Passive");
					}

					if (abilpair.Key == "Infantry") {
						GUILayout.Space (5.0f);
						GUILayout.Label ("Types");
					}

					if (abilpair.Key == "Human") {
						GUILayout.Space (5.0f);
						GUILayout.Label ("Races");
					}

					//Draw ability checks
					GUILayout.BeginHorizontal ();
					s_dAbilityToggles [abilpair.Key] = GUILayout.Toggle (s_dAbilityToggles [abilpair.Key], abilpair.Key);
					if (abilpair.Value.iType == ABILITY_TYPE.ACTIVE) {
						s_dAbilityPower [abilpair.Key] = GUILayout.HorizontalSlider (s_dAbilityPower [abilpair.Key], 1.0f, 100.0f, GUILayout.Width (150.0f));
						s_dAbilityPower [abilpair.Key] = EditorGUILayout.FloatField (s_dAbilityPower [abilpair.Key], GUILayout.Width (30.0f));
					} else if (abilpair.Value.iType == ABILITY_TYPE.PASSIVE) {
						if (abilpair.Value.fIntensity > 0) {
							s_dAbilityPower [abilpair.Key] = GUILayout.HorizontalSlider (s_dAbilityPower [abilpair.Key], 1.0f, 100.0f, GUILayout.Width (150.0f));
							s_dAbilityPower [abilpair.Key] = EditorGUILayout.FloatField (s_dAbilityPower [abilpair.Key], GUILayout.Width (30.0f));
						}
					}
					GUILayout.EndHorizontal ();
				}
			}
			GUILayout.EndScrollView ();
		}
		GUILayout.EndVertical();
		//=========== GUI END	Abilities
		
		//========= GUI END
		GUILayout.EndVertical();
		GUILayout.EndScrollView();
	}

	public void LoadUnitFile(string path) {
		s_xmlDoc = XDocument.Load(path);
		
		foreach (XElement xroot in s_xmlDoc.Elements()) {
			//Debug.Log(xroot.Name);
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					s_sName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					s_sDescription = xlayer1.Value;
				}
				else if (xlayer1.Name == "level") {
					s_iLevel = int.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "ap") {
					s_fAP = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "deploycost") {
					s_fDeployCost = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "health") {
					s_fHealth = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "maxhealth") {
					s_fMaxHealth = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "mana") {
					s_fMana = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "maxmana") {
					s_fMaxMana = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "move") {
					s_fMovement = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "vision") {
					s_fVision = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "attack") {
					s_fAttack = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "defence") {
					s_fDefence = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "resistance") {
					s_fResistance = float.Parse(xlayer1.Value);
				}

				if (xlayer1.Name == "abilities") {
					//Make sure only the in-use ones are ticked
//					foreach (KeyValuePair<string, bool> pair in s_dAbilityToggles) {
//						pair. = false;
//					}
					ReloadAbilities();

					foreach (XElement xlayer2 in xlayer1.Elements()) {
						if (AbilityBox.s_dAbilityLookup.ContainsKey(xlayer2.Value)) {
							s_dAbilityToggles[xlayer2.Value] = true;
							if (AbilityBox.s_dAbilityLookup[xlayer2.Value].fIntensity > 0) {
								s_dAbilityPower[xlayer2.Value] = float.Parse(xlayer2.FirstAttribute.Value);
							}
						}
					}
				}
			}
		}
	}

	public void SaveUnitFile(string path) {
		XmlWriterSettings xsettings = new XmlWriterSettings();
		xsettings.Encoding = Encoding.ASCII;
		xsettings.OmitXmlDeclaration = true;

		XmlWriter writer = XmlWriter.Create(s_sLastFile, xsettings);

		writer.WriteStartDocument();
		writer.WriteStartElement("TBSUnit");
		writer.WriteWhitespace("\n");

		//Name
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("name");
		writer.WriteValue(s_sName);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Description
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("description");
		writer.WriteValue(s_sDescription);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		writer.WriteWhitespace("\n");

		//Level
//		writer.WriteWhitespace("\t");
//		writer.WriteStartElement("level");
//		writer.WriteValue(s_iLevel);
//		writer.WriteEndElement();
//		writer.WriteWhitespace("\n");

		//AP
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("ap");
		writer.WriteValue(s_fAP);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Deploy cost
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("deploycost");
		writer.WriteValue(s_fDeployCost);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Health
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("health");
		writer.WriteValue(s_fHealth);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		writer.WriteWhitespace("\t");
		writer.WriteStartElement("maxhealth");
		writer.WriteValue(s_fMaxHealth);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Mana
//		writer.WriteWhitespace("\t");
//		writer.WriteStartElement("mana");
//		writer.WriteValue(s_fMana);
//		writer.WriteEndElement();
//		writer.WriteWhitespace("\n");
//		
//		writer.WriteWhitespace("\t");
//		writer.WriteStartElement("maxmana");
//		writer.WriteValue(s_fMaxMana);
//		writer.WriteEndElement();
//		writer.WriteWhitespace("\n");

		//Move
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("move");
		writer.WriteValue(s_fMovement);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Vision
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("vision");
		writer.WriteValue(s_fVision);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Attack
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("attack");
		writer.WriteValue(s_fAttack);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Defence
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("defence");
		writer.WriteValue(s_fDefence);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Resistance
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("resistance");
		writer.WriteValue(s_fResistance);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		writer.WriteWhitespace("\n");

		//Abilities
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("abilities");
		writer.WriteWhitespace("\n");

		foreach (KeyValuePair<string, bool> pair in s_dAbilityToggles) {
			if (pair.Value == true) {
				writer.WriteWhitespace("\t\t");
				writer.WriteStartElement("ability");
				if (s_dAbilityPower[pair.Key] > 0) {
					writer.WriteAttributeString("strength", s_dAbilityPower[pair.Key].ToString());
				}
				writer.WriteValue(pair.Key);
				writer.WriteEndElement();
				writer.WriteWhitespace("\n");
			}
		}

		writer.WriteWhitespace("\t");
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		writer.WriteEndElement();
		writer.WriteEndDocument();

		writer.Close();
	}

	public static void ReloadAbilities() {
		if (s_dAbilityToggles == null) {
			s_dAbilityToggles = new Dictionary<string, bool>();
		}

		if (s_dAbilityPower == null) {
			s_dAbilityPower = new Dictionary<string, float>();
		}

		s_dAbilityToggles.Clear();
		s_dAbilityPower.Clear();

		foreach (KeyValuePair<string, Ability> abilpair in AbilityBox.s_dAbilityLookup) {
			s_dAbilityToggles.Add(abilpair.Key, false);
			s_dAbilityPower.Add(abilpair.Key, 0.0f);
		}
	}
}
