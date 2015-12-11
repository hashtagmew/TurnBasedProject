using UnityEngine;
using UnityEditor;

using System.Collections;
using System.Collections.Generic;

using System.Xml;
using System.Xml.Linq;
using System.Linq;

using UnityEngine.UI;

using System.Text;

public class SoundSetEditor : EditorWindow {

	static private Vector2 s_vScrollPos;
	static private SOUNDSET_TYPE s_eType = SOUNDSET_TYPE.NONE;
	static private string s_sLastFile = "";

	static private AudioClip s_acUnitSelect;
	static private AudioClip s_acUnitMove;
	static private AudioClip s_acAttack;
	static private AudioClip s_acFootstep;

	static private AudioClip s_acAbilityCast;
	static private bool s_bExecuteLoop;
	static private AudioClip s_acAbilityExecute;
	static private AudioClip s_acAbilityFinish;

	//Window
	[MenuItem("Window/Sound Set Editor %&s")]
	public static void Init() {
		SoundSetEditor window = (SoundSetEditor)EditorWindow.GetWindow(typeof(SoundSetEditor));
		
		window.Show();
	}
	
	public void OnGUI() {
		s_vScrollPos = GUILayout.BeginScrollView(s_vScrollPos);

		GUI.SetNextControlName("Top");
		s_eType = (SOUNDSET_TYPE)EditorGUILayout.EnumPopup("Soundset type", s_eType);

		GUILayout.BeginVertical("box");

		if (s_eType == SOUNDSET_TYPE.UNIT) {
			GUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("Unit Selected", "Played when the unit is selected"));
			s_acUnitSelect = (AudioClip)EditorGUILayout.ObjectField(s_acUnitSelect, typeof(AudioClip), false);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("Unit Move Confirm", "Played when the unit is told to move"));
			s_acUnitMove = (AudioClip)EditorGUILayout.ObjectField(s_acUnitMove, typeof(AudioClip), false);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("Unit Attack Confirm", "50% chance to be played when the unit attacks"));
			s_acAttack = (AudioClip)EditorGUILayout.ObjectField(s_acAttack, typeof(AudioClip), false);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label(new GUIContent("Unit Footstep", "Played when unit moves"));
			s_acFootstep = (AudioClip)EditorGUILayout.ObjectField(s_acFootstep, typeof(AudioClip), false);
			GUILayout.EndHorizontal();
		}
		else if (s_eType == SOUNDSET_TYPE.ABILITY) {
			GUILayout.BeginHorizontal();
			GUILayout.Label("Ability Initial Cast");
			s_acAbilityCast = (AudioClip)EditorGUILayout.ObjectField(s_acAbilityCast, typeof(AudioClip), false);
			GUILayout.EndHorizontal();
			
			GUILayout.BeginHorizontal();
			GUILayout.Label("Ability Executing");
			s_acAbilityExecute = (AudioClip)EditorGUILayout.ObjectField(s_acAbilityExecute, typeof(AudioClip), false);
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.Label("Ability Finished");
			s_acAbilityFinish = (AudioClip)EditorGUILayout.ObjectField(s_acAbilityFinish, typeof(AudioClip), false);
			GUILayout.EndHorizontal();
		}
		else {
			GUILayout.Label("N/A");
		}

		GUILayout.EndVertical();

		GUILayout.BeginHorizontal("box");
		if (GUILayout.Button("Load Soundset", GUILayout.MinWidth(5.0f))) {
			s_sLastFile = EditorUtility.OpenFilePanel("Load unit", "/Assets/Scripts/Units/UnitFiles/", "xml");
			if (s_sLastFile.Length != 0) {
				GUI.FocusControl("Top");
				LoadSoundset(s_sLastFile);
			}
		}
		
		if (GUILayout.Button("Save Soundset")) {
			s_sLastFile = EditorUtility.SaveFilePanel("Save unit", "/Assets/Scripts/Units/UnitFiles/", "sname", "xml");
			if (s_sLastFile.Length != 0) {
				GUI.FocusControl("Top");
				SaveSoundset(s_sLastFile);
			}
		}
		GUILayout.EndHorizontal();

		GUILayout.EndScrollView();
	}

	public void SaveSoundset(string path) {
		XmlWriterSettings xsettings = new XmlWriterSettings();
		xsettings.Encoding = Encoding.ASCII;
		xsettings.OmitXmlDeclaration = true;
		
		XmlWriter writer = XmlWriter.Create(s_sLastFile, xsettings);
		
		writer.WriteStartDocument();
		writer.WriteStartElement("Soundset");
		writer.WriteWhitespace("\n");

		writer.WriteWhitespace("\t");
		writer.WriteStartElement("type");
		writer.WriteValue(((int)s_eType).ToString());
		writer.WriteEndElement();
		writer.WriteWhitespace("\n");

		if (s_eType == SOUNDSET_TYPE.UNIT) {
			//Select
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("select");
			if (s_acUnitSelect != null) {
				writer.WriteValue(s_acUnitSelect.name);
			}
			else {
				writer.WriteValue("null");
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
			
			//Move
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("move");
			if (s_acUnitMove != null) {
				writer.WriteValue(s_acUnitMove.name);
			}
			else {
				writer.WriteValue("null");
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			//Attack
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("attack");
			if (s_acAttack != null) {
				writer.WriteValue(s_acAttack.name);
			}
			else {
				writer.WriteValue("null");
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			//Footstep
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("footstep");
			if (s_acAttack != null) {
				writer.WriteValue(s_acFootstep.name);
			}
			else {
				writer.WriteValue("null");
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}

		if (s_eType == SOUNDSET_TYPE.ABILITY) {
			//Cast
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("cast");
			if (s_acAbilityCast != null) {
				writer.WriteValue(s_acAbilityCast.name);
			}
			else {
				writer.WriteValue("null");
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
			
			//Move
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("execute");
			if (s_acAbilityExecute != null) {
				writer.WriteValue(s_acAbilityExecute.name);
			}
			else {
				writer.WriteValue("null");
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");

			//Finish
			writer.WriteWhitespace("\t");
			writer.WriteStartElement("finish");
			if (s_acAbilityFinish != null) {
				writer.WriteValue(s_acAbilityFinish.name);
			}
			else {
				writer.WriteValue("null");
			}
			writer.WriteEndElement();
			writer.WriteWhitespace("\n");
		}
		
		writer.WriteEndElement();
		writer.WriteEndDocument();
		
		writer.Close();
	}
	
	public void LoadSoundset(string path) {
		XDocument s_xmlDoc = XDocument.Load(path);
		
		foreach (XElement xroot in s_xmlDoc.Elements()) {
			//Debug.Log(xroot.Name);
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "type") {
					s_eType = (SOUNDSET_TYPE)(int.Parse(xlayer1.Value));
				}

				if (s_eType == SOUNDSET_TYPE.UNIT) {
					if (xlayer1.Name == "select") {
						string stemp = xlayer1.Value;
						s_acUnitSelect = Resources.Load("Audio/" + stemp) as AudioClip;
					}
					else if (xlayer1.Name == "move") {
						string stemp = xlayer1.Value;
						s_acUnitMove = Resources.Load("Audio/" + stemp) as AudioClip;
					}
					else if (xlayer1.Name == "attack") {
						string stemp = xlayer1.Value;
						s_acAttack = Resources.Load("Audio/" + stemp) as AudioClip;
					}
					else if (xlayer1.Name == "footstep") {
						string stemp = xlayer1.Value;
						s_acFootstep = Resources.Load("Audio/" + stemp) as AudioClip;
					}
				}

				if (s_eType == SOUNDSET_TYPE.ABILITY) {
					if (xlayer1.Name == "cast") {
						string stemp = xlayer1.Value;
						s_acAbilityCast = Resources.Load("Audio/" + stemp) as AudioClip;
					}
					else if (xlayer1.Name == "deploycost") {
						string stemp = xlayer1.Value;
						s_acAbilityExecute = Resources.Load("Audio/" + stemp) as AudioClip;
					}
					else if (xlayer1.Name == "health") {
						string stemp = xlayer1.Value;
						s_acAbilityFinish = Resources.Load("Audio/" + stemp) as AudioClip;
					}
				}
			}
		}
	}

}
