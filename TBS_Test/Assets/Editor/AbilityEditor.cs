using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;

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

	static private bool s_bShowActivationEffects = true;
	static private bool s_bShowResolutionEffects = true;
	static private bool s_bShowPassiveEffects = false;

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
	static private Dictionary<string, bool> s_dEffects;

	static private GameObject s_psCast;
	static private GameObject s_psExecute;
	static private GameObject s_psFinish;

	static private bool s_bGuaranteedHit = false;
	static private bool s_bRangedHitChance = false;
	static private bool s_bMagical = false;
	static private int s_iRange = 1;
	static private float s_fCastDelay = 0.5f;
	static private float s_fExecuteDelay = 0.0f;
	static private float s_fFinishDelay = 1.0f;
	static private int s_iAreaAttackRadius = 1;
	static private EFFECT_TARGET s_eTarget = EFFECT_TARGET.NONE;
	static private string s_sDescription = "Does a thing.";
	static private bool s_bStopOnCollide = false;
	static private TextAsset s_taSoundset;
	static private FEATURE_TYPE s_eFeatureTarget = FEATURE_TYPE.NONE;
	static private bool s_bSetDamageUser = false;
	static private bool s_bSetDamageTarget = false;

	static private int s_iEditorRangedDist = 1;

	static private Dictionary<string, Effect> s_dEffectsActivation = new Dictionary<string, Effect>();
	static private Dictionary<string, Effect> s_dEffectsResolution = new Dictionary<string, Effect>();

	static private float s_fAccuracyBonus = 0.0f;

	//Window
	[MenuItem("Window/Ability Editor %&a")]
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

//		if (GUILayout.Button("R", GUILayout.Width(25))) {
//			ReloadEffects();
//		}
		GUILayout.EndHorizontal();
		GUILayout.EndVertical();

		GUILayout.BeginVertical("box");

		//Name
		GUILayout.BeginHorizontal("box");
		GUI.SetNextControlName("Top");
		GUILayout.Label("Name", EditorStyles.boldLabel);
		s_sName = GUILayout.TextField(s_sName, 28, GUILayout.Width(200));
		GUILayout.EndHorizontal();

		//Desc
		GUILayout.Label("Description", EditorStyles.boldLabel);
		s_sDescription = GUILayout.TextArea(s_sDescription, GUILayout.Width(200));

		//Type
		GUILayout.BeginHorizontal("box");
		GUILayout.Label("Type", EditorStyles.boldLabel);
		s_iType = (ABILITY_TYPE)EditorGUILayout.EnumPopup(s_iType);
		GUILayout.EndHorizontal();

		if (s_iType != ABILITY_TYPE.NONE) {
			if (s_iType == ABILITY_TYPE.ACTIVE) {
				GUILayout.BeginVertical("box");

				s_bMagical = GUILayout.Toggle(s_bMagical, "Magical");
				s_bSetDamageUser = EditorGUILayout.Toggle("Ignore Caster Attack", s_bSetDamageUser);
				s_bSetDamageTarget = EditorGUILayout.Toggle("Ignore Enemy Defence", s_bSetDamageTarget);
				s_bGuaranteedHit = GUILayout.Toggle(s_bGuaranteedHit, "Guaranteed Hit");
				s_bRangedHitChance = GUILayout.Toggle(s_bRangedHitChance, "Use Ranged Hit Calc");
				s_fAccuracyBonus = EditorGUILayout.FloatField("Accuracy Bonus", s_fAccuracyBonus);

				GUILayout.BeginVertical("Test data", "box");
				GUILayout.Label("Hit Chance: " + 
				                Combat.CombatMechanics.ChanceToHit(s_iEditorRangedDist, (int)s_fAccuracyBonus, 0, s_bGuaranteedHit, s_bRangedHitChance).ToString() + "%");

				GUILayout.BeginHorizontal();

				GUILayout.Label("Distance: ");
				s_iEditorRangedDist = EditorGUILayout.IntSlider(s_iEditorRangedDist, 0, 99);

				GUILayout.EndHorizontal();

//				if (GUILayout.Button("Recalculate")) {
//					Repaint();
//				}

				GUILayout.EndVertical();

				s_iRange = EditorGUILayout.IntField("Ability Range", s_iRange);
				s_eTarget = (EFFECT_TARGET)EditorGUILayout.EnumPopup("Target(s)", s_eTarget);
				if (s_eTarget == EFFECT_TARGET.AREA || s_eTarget == EFFECT_TARGET.CONE) {
					s_iAreaAttackRadius = EditorGUILayout.IntField("Radius", s_iAreaAttackRadius);
				}
				else if (s_eTarget == EFFECT_TARGET.LINE) {
					s_iAreaAttackRadius = EditorGUILayout.IntField("Length", s_iAreaAttackRadius);
					s_bStopOnCollide = EditorGUILayout.Toggle("Stop On Collision", s_bStopOnCollide);
				}
				else if (s_eTarget == EFFECT_TARGET.SINGLE_FEATURE_TILE) {
					s_eFeatureTarget = (FEATURE_TYPE)EditorGUILayout.EnumPopup("Restriction: ", s_eFeatureTarget);
				}

				GUILayout.BeginHorizontal();
				GUILayout.Label("Ability Soundset");
				s_taSoundset = (TextAsset)EditorGUILayout.ObjectField(s_taSoundset, typeof(TextAsset), false);
				if (s_taSoundset != null) {
					if (s_taSoundset.text.Substring(0, 10) != "<Soundset>") {
						Debug.LogWarning("WARNING! " + s_taSoundset.name + " ISN'T A VALID SOUNDSET!");
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.EndVertical();

				//OnActivation
				GUILayout.BeginVertical("box");
				GUILayout.Label("On Activation", EditorStyles.boldLabel);

				s_fCastDelay = EditorGUILayout.FloatField("Delay", s_fCastDelay);

				GUILayout.BeginHorizontal();
				GUILayout.Label("Casting Particle Effect");
				s_psCast = (GameObject)EditorGUILayout.ObjectField(s_psCast, typeof(GameObject), false);
				if (s_psCast != null) {
					if (!IsPrefabAParticleEffect(s_psCast.name)) {
						Debug.LogWarning("WARNING! " + s_psCast.name + " ISN'T A PARTICLE EFFECT!");
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.Label("Current Effects:", EditorStyles.boldLabel);
				if (s_dEffectsActivation.Count > 0) {
					foreach (KeyValuePair<string, Effect> pair in s_dEffectsActivation) {
						GUILayout.BeginHorizontal();

						GUILayout.Label(pair.Key);
						if (pair.Value.bAdjustable) {
							GUILayout.BeginHorizontal("box");
							GUILayout.Label(pair.Value.sAdjustName);
							if (!pair.Value.bAdjustableString) {
								pair.Value.fAdjustFloat = EditorGUILayout.FloatField(pair.Value.fAdjustFloat);
							}
							else {
								pair.Value.sAdjustString = EditorGUILayout.TextField(pair.Value.sAdjustString);
							}
							GUILayout.EndHorizontal();
		                }

						GUILayout.EndHorizontal();
					}
				}

				GUILayout.EndVertical();

				//Execute
				GUILayout.BeginVertical("box");
				GUILayout.Label("On Execute", EditorStyles.boldLabel);


				s_fExecuteDelay = EditorGUILayout.FloatField("Delay", s_fExecuteDelay);

				GUILayout.BeginHorizontal();
				GUILayout.Label("Executing Particle Effect");
				s_psExecute = (GameObject)EditorGUILayout.ObjectField(s_psExecute, typeof(GameObject), false);
				if (s_psExecute != null) {
					if (!IsPrefabAParticleEffect(s_psExecute.name)) {
						Debug.LogWarning("WARNING! " + s_psExecute.name + " ISN'T A PARTICLE EFFECT!");
					}
				}
				GUILayout.EndHorizontal();
				GUILayout.EndVertical();

				//OnResolution
				GUILayout.BeginVertical("box");
				GUILayout.Label("On Resolution", EditorStyles.boldLabel);

				s_fFinishDelay = EditorGUILayout.FloatField("Delay", s_fFinishDelay);

				GUILayout.BeginHorizontal();
				GUILayout.Label("Finished Particle Effect");
				s_psFinish = (GameObject)EditorGUILayout.ObjectField(s_psFinish, typeof(GameObject), false);
				if (s_psFinish != null) {
					if (!IsPrefabAParticleEffect(s_psFinish.name)) {
						Debug.LogWarning("WARNING! " + s_psFinish.name + " ISN'T A PARTICLE EFFECT!");
					}
				}
				GUILayout.EndHorizontal();

				GUILayout.Label("Current Effects:", EditorStyles.boldLabel);
				if (s_dEffectsResolution.Count > 0) {
					foreach (KeyValuePair<string, Effect> pair in s_dEffectsResolution) {
						GUILayout.BeginHorizontal();

						GUILayout.Label(pair.Key);
						if (pair.Value.bAdjustable) {
							GUILayout.BeginHorizontal("box");
							GUILayout.Label(pair.Value.sAdjustName);
							if (!pair.Value.bAdjustableString) {
								pair.Value.fAdjustFloat = EditorGUILayout.FloatField(pair.Value.fAdjustFloat);
							}
							else {
								pair.Value.sAdjustString = EditorGUILayout.TextField(pair.Value.sAdjustString);
							}
							GUILayout.EndHorizontal();
						}

						GUILayout.EndHorizontal();
					}
				}

				GUILayout.EndVertical();
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
				foreach (KeyValuePair<string, Effect> pair in EffectBox.s_dEffectLookup) {
					if (pair.Value.iType == EFFECT_TYPE.ACTIVATION) {
						GUILayout.BeginHorizontal();

						if (GUILayout.Button("+", GUILayout.MaxWidth(20.0f))) {
							if (!s_dEffectsActivation.ContainsKey(pair.Key)) {
								s_dEffectsActivation.Add(pair.Key, pair.Value);
							}
							else {
								Debug.LogError("That key already exists!");
							}
						}
						if (GUILayout.Button("-", GUILayout.MaxWidth(20.0f))) {
							if (s_dEffectsActivation.ContainsKey(pair.Key)) {
								s_dEffectsActivation.Remove(pair.Key);
							}
							else {
								Debug.LogError("That key doesn't exist!");
							}
						}
						GUILayout.Label(pair.Key);

						GUILayout.EndHorizontal();
					}
				}
				GUILayout.EndScrollView();
			}
			//=========== GUI END	ACTIVE Effects

			//=========== GUI START	Resolve Effects
			GUILayout.BeginHorizontal();
			s_bShowResolutionEffects = EditorGUILayout.Foldout(s_bShowResolutionEffects, "Resolution");

			GUILayout.EndHorizontal();
			
			
			if (s_bShowResolutionEffects) {
				s_vResolutionEffectScrollPos = GUILayout.BeginScrollView(s_vResolutionEffectScrollPos);
				foreach (KeyValuePair<string, Effect> pair in EffectBox.s_dEffectLookup) {
					if (pair.Value.iType == EFFECT_TYPE.RESOLUTION) {
						GUILayout.BeginHorizontal();
						
						if (GUILayout.Button("+", GUILayout.MaxWidth(20.0f))) {
							if (!s_dEffectsResolution.ContainsKey(pair.Key)) {
								s_dEffectsResolution.Add(pair.Key, pair.Value);
							}
							else {
								Debug.LogError("That key already exists!");
							}
						}
						if (GUILayout.Button("-", GUILayout.MaxWidth(20.0f))) {
							if (s_dEffectsResolution.ContainsKey(pair.Key)) {
								s_dEffectsResolution.Remove(pair.Key);
							}
							else {
								Debug.LogError("That key doesn't exist!");
							}
						}
						GUILayout.Label(pair.Key);
						
						GUILayout.EndHorizontal();
					}
				}
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
		s_xmlDoc = XDocument.Load(path);

		s_dEffectsActivation.Clear();
		s_dEffectsResolution.Clear();

//		XDocument s_xmlDoc = new XDocument();
//
//		Debug.Log(path);
//		TextAsset taAbility = Resources.Load<TextAsset>(path);
//		XmlDocument xdoc = new XmlDocument();
//		xdoc.LoadXml(taAbility.text);
//		
//		if (taAbility == null) {
//			Debug.LogError("Failed to load level " + path + "!");
//		}
//		
//		//Convert XmlDocument to XDocument
//		using (MemoryStream memStream = new MemoryStream()) {
//			using (XmlWriter tempwrite = XmlWriter.Create(memStream)) {
//				xdoc.WriteContentTo(tempwrite);
//			}
//			memStream.Seek(0, SeekOrigin.Begin);
//			using (XmlReader tempread = XmlReader.Create(memStream)) {
//				s_xmlDoc = XDocument.Load(tempread);
//			}
//		}
//		
//		if (s_xmlDoc == null) {
//			Debug.LogError("Failed to load xml " + path + "!");
//		}
		
		foreach (XElement xroot in s_xmlDoc.Elements()) {
			//Debug.Log(xroot.Name);
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					s_sName = xlayer1.Value;
				}
				else if (xlayer1.Name == "description") {
					s_sDescription = xlayer1.Value;
				}
				else if (xlayer1.Name == "type") {
					s_iType = (ABILITY_TYPE)int.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "feature_restriction") {
					s_eFeatureTarget = (FEATURE_TYPE)int.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "magicalattack") {
					if (xlayer1.Value == "0") {
						s_bMagical = false;
					}
					else {
						s_bMagical = true;
					}
				}
				else if (xlayer1.Name == "setdamage_user") {
					if (xlayer1.Value == "0") {
						s_bSetDamageUser = false;
					}
					else {
						s_bSetDamageUser = true;
					}
				}
				else if (xlayer1.Name == "setdamage_target") {
					if (xlayer1.Value == "0") {
						s_bSetDamageTarget = false;
					}
					else {
						s_bSetDamageTarget = true;
					}
				}
				else if (xlayer1.Name == "guaranteed_hit") {
					if (xlayer1.Value == "0") {
						s_bGuaranteedHit = false;
					}
					else {
						s_bGuaranteedHit = true;
					}
				}
				else if (xlayer1.Name == "use_ranged_calc") {
					if (xlayer1.Value == "0") {
						s_bRangedHitChance = false;
					}
					else {
						s_bRangedHitChance = true;
					}
				}
				else if (xlayer1.Name == "accuracy_bonus") {
					s_fAccuracyBonus = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "range") {
					s_iRange = int.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "target") {
					s_eTarget = (EFFECT_TARGET)int.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "area") {
					s_iAreaAttackRadius = int.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "stoponcollide") {
					if (int.Parse(xlayer1.Value) == 0) {
						s_bStopOnCollide = false;
					}
					else {
						s_bStopOnCollide = true;
					}
				}
				else if (xlayer1.Name == "soundset") {
					s_taSoundset = (TextAsset)Resources.Load("Audio/Soundsets/" + xlayer1.Value);
				}
				else if (xlayer1.Name == "activation_delay") {
					s_fCastDelay = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "activation_particle") {
					s_psCast = (GameObject)Resources.Load("Particle Effects/" + xlayer1.Value);
				}
				else if (xlayer1.Name == "execution_delay") {
					s_fExecuteDelay = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "execution_particle") {
					s_psExecute = (GameObject)Resources.Load("Particle Effects/" + xlayer1.Value);
				}
				else if (xlayer1.Name == "resolution_delay") {
					s_fFinishDelay = float.Parse(xlayer1.Value);
				}
				else if (xlayer1.Name == "resolution_particle") {
					s_psFinish = (GameObject)Resources.Load("Particle Effects/" + xlayer1.Value);
				}
				
				if (xlayer1.Name == "activation_effects") {
					ReloadEffects();
					
					foreach (XElement xlayer2 in xlayer1.Elements()) {
						if (EffectBox.s_dEffectLookup.ContainsKey(xlayer2.Value)) {
							s_dEffectsActivation.Add(xlayer2.Value, EffectBox.s_dEffectLookup[xlayer2.Value]);

							if (EffectBox.s_dEffectLookup[xlayer2.Value].bAdjustable) {
								if (!EffectBox.s_dEffectLookup[xlayer2.Value].bAdjustableString) {
									s_dEffectsActivation[xlayer2.Value].fAdjustFloat = float.Parse(xlayer2.FirstAttribute.Value);
								}
								else {
									s_dEffectsActivation[xlayer2.Value].sAdjustString = xlayer2.FirstAttribute.Value;
								}
							}
						}
					}
				}

				if (xlayer1.Name == "resolution_effects") {
					ReloadEffects();
					
					foreach (XElement xlayer2 in xlayer1.Elements()) {
						if (EffectBox.s_dEffectLookup.ContainsKey(xlayer2.Value)) {
							s_dEffectsResolution.Add(xlayer2.Value, EffectBox.s_dEffectLookup[xlayer2.Value]);

							if (EffectBox.s_dEffectLookup[xlayer2.Value].bAdjustable) {
								if (!EffectBox.s_dEffectLookup[xlayer2.Value].bAdjustableString) {
									s_dEffectsResolution[xlayer2.Value].fAdjustFloat = float.Parse(xlayer2.FirstAttribute.Value);
								}
								else {
									s_dEffectsResolution[xlayer2.Value].sAdjustString = xlayer2.FirstAttribute.Value;
								}
							}
						}
					}
				}
			}
		}
	}

	static public void SaveAbilityFile(string path) {
		XmlWriterSettings xsettings = new XmlWriterSettings();
		xsettings.Encoding = Encoding.ASCII;
		xsettings.OmitXmlDeclaration = true;
		
		XmlWriter writer = XmlWriter.Create(s_sLastFile, xsettings);
		
		writer.WriteStartDocument();
		writer.WriteStartElement("Ability");
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

		//Type
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("type");
		writer.WriteValue((int)s_iType);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		
		writer.WriteWhitespace("\n");

		//Magical
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("magicalattack");
		if (s_bMagical) {
			writer.WriteValue(1);
		}
		else {
			writer.WriteValue(0);
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Ignore user attack
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("setdamage_user");
		if (s_bSetDamageUser) {
			writer.WriteValue(1);
		}
		else {
			writer.WriteValue(0);
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Ignore target def
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("setdamage_target");
		if (s_bSetDamageTarget) {
			writer.WriteValue(1);
		}
		else {
			writer.WriteValue(0);
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		
		//Guaranteed Hit
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("guaranteed_hit");
		if (s_bGuaranteedHit) {
			writer.WriteValue(1);
		}
		else {
			writer.WriteValue(0);
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Ranged
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("use_ranged_calc");
		if (s_bRangedHitChance) {
			writer.WriteValue(1);
		}
		else {
			writer.WriteValue(0);
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Accuracy Bonus
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("accuracy_bonus");
		writer.WriteValue(s_fAccuracyBonus);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Range
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("range");
		writer.WriteValue(s_iRange);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Target
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("target");
		writer.WriteValue((int)s_eTarget);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		if (s_eTarget == EFFECT_TARGET.AREA || s_eTarget == EFFECT_TARGET.CONE || s_eTarget == EFFECT_TARGET.LINE) {
			//Area
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("area");
			writer.WriteValue(s_iAreaAttackRadius);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}

		if (s_eTarget == EFFECT_TARGET.LINE) {
			//Stop on hit
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("stoponcollide");
			if (s_bStopOnCollide) {
				writer.WriteValue(1);
			}
			else {
				writer.WriteValue(0);
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}

		if (s_eTarget == EFFECT_TARGET.SINGLE_FEATURE_TILE) {
			//Feature restriction
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("feature_restriction");
			writer.WriteValue((int)s_eFeatureTarget);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}

		//Soundset
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("soundset");
		writer.WriteValue(s_taSoundset.name);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Activation Delay
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("activation_delay");
		writer.WriteValue(s_fCastDelay);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Execute Delay
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("execution_delay");
		writer.WriteValue(s_fExecuteDelay);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Resolve Delay
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("resolution_delay");
		writer.WriteValue(s_fFinishDelay);
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Activation Particle
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("activation_particle");
		if (s_psCast != null) {
			writer.WriteValue(s_psCast.name);
		}
		else {
			writer.WriteValue("null");
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Execute Particle
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("execution_particle");
		if (s_psExecute != null) {
			writer.WriteValue(s_psExecute.name);
		}
		else {
			writer.WriteValue("null");
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Resolve Particle
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("resolution_particle");
		if (s_psFinish != null) {
			writer.WriteValue(s_psFinish.name);
		}
		else {
			writer.WriteValue("null");
		}
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		
		writer.WriteWhitespace("\n");
		
		//Activation Effects
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("activation_effects");
		writer.WriteWhitespace("\n");
		
		foreach (KeyValuePair<string, Effect> pair in s_dEffectsActivation) {
			writer.WriteWhitespace("\t\t");
			writer.WriteStartElement("effect");
			if (s_dEffectsActivation[pair.Key].bAdjustable) {
				if (!s_dEffectsActivation[pair.Key].bAdjustableString) {
					writer.WriteAttributeString("power", s_dEffectsActivation[pair.Key].fAdjustFloat.ToString());
				}
				else {
					writer.WriteAttributeString("path", s_dEffectsActivation[pair.Key].sAdjustString);
				}
			}
			writer.WriteValue(pair.Key);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}
		
		writer.WriteWhitespace("\t");
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		//Resolution Effects
		writer.WriteWhitespace("\t");
		writer.WriteStartElement("resolution_effects");
		writer.WriteWhitespace("\n");
		
		foreach (KeyValuePair<string, Effect> pair in s_dEffectsResolution) {
			writer.WriteWhitespace("\t\t");
			writer.WriteStartElement("effect");
			if (s_dEffectsResolution[pair.Key].bAdjustable) {
				if (!s_dEffectsResolution[pair.Key].bAdjustableString) {
					writer.WriteAttributeString("power", s_dEffectsResolution[pair.Key].fAdjustFloat.ToString());
				}
				else {
					writer.WriteAttributeString("power", s_dEffectsResolution[pair.Key].sAdjustString);
				}
			}
			writer.WriteValue(pair.Key);
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}
		
		writer.WriteWhitespace("\t");
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");
		
		writer.WriteEndElement();
		writer.WriteEndDocument();
		
		writer.Close();
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

	public bool IsPrefabAParticleEffect(string name) {
		GameObject go = (GameObject)Resources.Load("Particle Effects/" + name);
		if (go != null) {
			return true;
		}

		return false;
	}
}
