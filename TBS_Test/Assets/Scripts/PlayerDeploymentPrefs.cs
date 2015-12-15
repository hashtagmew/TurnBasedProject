using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Linq;
using System.IO;
using System.Text;

public class PlayerDeploymentPrefs {
	
	public List<KeyValuePair<string, int>> l_sMagicalUnits = new List<KeyValuePair<string, int>>();
	public List<KeyValuePair<string, int>> l_sMechanicalUnits = new List<KeyValuePair<string, int>>();

	public PlayerDeploymentPrefs() {
		//LoadPrefs ();
	}

	public void LoadPrefs() {
		//TextAsset taPly = (TextAsset)Resources.Load<TextAsset> ("playerprefs");
		string sTemp = "";
		int iTemp = 0;

		if (PlayerPrefs.HasKey("name")) {
			Debug.Log("LOAD PLY PREFS WITH NAME " + PlayerPrefs.GetString("name"));
		}
		else {
			//Make default
			PlayerPrefs.SetString("name", "default");

			//Magic
			PlayerPrefs.SetString("magical_slot1_name", "magic commander");
			PlayerPrefs.SetInt ("magical_slot1_cost", 1);

			PlayerPrefs.SetString("magical_slot2_name", "arcane manifestation");
			PlayerPrefs.SetInt ("magical_slot2_cost", 1);

			PlayerPrefs.SetString("magical_slot3_name", "arcane manifestation");
			PlayerPrefs.SetInt ("magical_slot3_cost", 1);

			PlayerPrefs.SetString("magical_slot4_name", "demons");
			PlayerPrefs.SetInt ("magical_slot4_cost", 1);

			PlayerPrefs.SetString("magical_slot5_name", "magicians");
			PlayerPrefs.SetInt ("magical_slot5_cost", 1);

			PlayerPrefs.SetString("magical_slot6_name", "dragon");
			PlayerPrefs.SetInt ("magical_slot6_cost", 2);

			PlayerPrefs.SetString("magical_slot7_name", "null");
			PlayerPrefs.SetInt ("magical_slot7_cost", 0);

			//Mech
			PlayerPrefs.SetString("mechanical_slot1_name", "mechanical commander");
			PlayerPrefs.SetInt ("mechanical_slot1_cost", 1);
			
			PlayerPrefs.SetString("mechanical_slot2_name", "armoured troops");
			PlayerPrefs.SetInt ("mechanical_slot2_cost", 1);
			
			PlayerPrefs.SetString("mechanical_slot3_name", "armoured troops");
			PlayerPrefs.SetInt ("mechanical_slot3_cost", 1);
			
			PlayerPrefs.SetString("mechanical_slot4_name", "skimmer");
			PlayerPrefs.SetInt ("mechanical_slot4_cost", 1);
			
			PlayerPrefs.SetString("mechanical_slot5_name", "dropship");
			PlayerPrefs.SetInt ("mechanical_slot5_cost", 1);
			
			PlayerPrefs.SetString("mechanical_slot6_name", "colossal walker");
			PlayerPrefs.SetInt ("mechanical_slot6_cost", 2);
			
			PlayerPrefs.SetString("mechanical_slot7_name", "null");
			PlayerPrefs.SetInt ("mechanical_slot7_cost", 0);

			PlayerPrefs.Save();
		}

		for (int i = 1; i < 8; i++) {
			if (PlayerPrefs.HasKey("magical_slot" + i.ToString() + "_name")) {
				sTemp = PlayerPrefs.GetString("magical_slot" + i.ToString() + "_name");
				if (PlayerPrefs.HasKey("magical_slot" + i.ToString() + "_cost")) {
					iTemp = PlayerPrefs.GetInt("magical_slot" + i.ToString() + "_cost");
				}

				l_sMagicalUnits.Add(new KeyValuePair<string, int>(sTemp, iTemp));
			}

			if (PlayerPrefs.HasKey("mechanical_slot" + i.ToString() + "_name")) {
				sTemp = PlayerPrefs.GetString("mechanical_slot" + i.ToString() + "_name");
				if (PlayerPrefs.HasKey("mechanical_slot" + i.ToString() + "_cost")) {
					iTemp = PlayerPrefs.GetInt("mechanical_slot" + i.ToString() + "_cost");
				}
				
				l_sMechanicalUnits.Add(new KeyValuePair<string, int>(sTemp, iTemp));
			}
		}

		
//		if (taPly == null) {
//			Debug.LogWarning ("The player file was not found!");
//
//			return;
//		}
//		
//		XDocument xmlDoc = XDocument.Load (new StringReader(taPly.text));
//
//		foreach (XElement xroot in xmlDoc.Elements()) {
//			foreach (XElement xlayer1 in xroot.Elements()) {
//				if (xlayer1.Name == "name") {
//					//sName = xlayer1.Value;
//				}
//				else if (xlayer1.Name == "magical") {
//					foreach (XElement xlayer2 in xlayer1.Elements()) {
//						l_sMagicalUnits.Add(new KeyValuePair<string, int>(xlayer2.Value, int.Parse(xlayer2.FirstAttribute.Value)));
//					}
//				}
//				else if (xlayer1.Name == "mechanical") {
//					foreach (XElement xlayer2 in xlayer1.Elements()) {
//						l_sMechanicalUnits.Add(new KeyValuePair<string, int>(xlayer2.Value, int.Parse(xlayer2.FirstAttribute.Value)));
//					}
//				}
//			}
//		}
	}

	public void SavePrefs() {
		for (int i = 0; i < 6; i++) {
			Debug.Log("SAVE SLOT " + i.ToString());
			Debug.Log("COUNT " + l_sMagicalUnits.Count.ToString());

			if (l_sMagicalUnits.Count < i) {
				PlayerPrefs.SetString("magical_slot" + (i+1).ToString() + "_name", l_sMagicalUnits[i].Key);
				PlayerPrefs.SetInt("magical_slot" + (i+1).ToString() + "_cost", l_sMagicalUnits[i].Value);
          	}
			else {
				PlayerPrefs.SetString("magical_slot" + (i+1).ToString() + "_name", "");
				PlayerPrefs.SetInt("magical_slot" + (i+1).ToString() + "_cost", 0);
			}

			if (l_sMechanicalUnits.Count < i) {
				PlayerPrefs.SetString("mechanical_slot" + (i+1).ToString() + "_name", l_sMechanicalUnits[i].Key);
				PlayerPrefs.SetInt("mechanical_slot" + (i+1).ToString() + "_cost", l_sMagicalUnits[i].Value);
			}
			else {
				PlayerPrefs.SetString("mechanical_slot" + (i+1).ToString() + "_name", "");
				PlayerPrefs.SetInt("mechanical_slot" + (i+1).ToString() + "_cost", 0);
			}
		}

		PlayerPrefs.Save();
//		XmlWriterSettings xsettings = new XmlWriterSettings();
//		xsettings.Encoding = Encoding.ASCII;
//		xsettings.OmitXmlDeclaration = true;
//		
//		using (XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/playerprefs.xml", xsettings)) {
//		
//			writer.WriteStartDocument ();
//			writer.WriteStartElement ("TBSPlayer");
//			writer.WriteWhitespace ("\n");
//		
//			//Name
//			writer.WriteWhitespace ("\t");
//			writer.WriteStartElement ("name");
//			writer.WriteValue ("default");
//			writer.WriteEndElement ();
//			writer.WriteWhitespace ("\n");
//
//			//Magical
//			writer.WriteWhitespace ("\t");
//			writer.WriteStartElement ("magical");
//			writer.WriteWhitespace ("\n");
//		
//			foreach (KeyValuePair<string, int> str in l_sMagicalUnits) {
//				writer.WriteWhitespace ("\t\t");
//
//				writer.WriteStartElement ("unit");
//				writer.WriteAttributeString ("cost", str.Value.ToString ());
//				writer.WriteValue (str.Key);
//				writer.WriteEndElement ();
//				writer.WriteWhitespace ("\n");
//			}
//		
//			writer.WriteWhitespace ("\t");
//			writer.WriteEndElement ();
//			writer.WriteWhitespace ("\n");
//
//			//Mech
//			writer.WriteWhitespace ("\t");
//			writer.WriteStartElement ("mechanical");
//			writer.WriteWhitespace ("\n");
//		
//			foreach (KeyValuePair<string, int> str in l_sMechanicalUnits) {
//				writer.WriteWhitespace ("\t\t");
//
//				writer.WriteStartElement ("unit");
//				writer.WriteAttributeString ("cost", str.Value.ToString ());
//				writer.WriteValue (str.Key);
//				writer.WriteEndElement ();
//				writer.WriteWhitespace ("\n");
//			}
//		
//			writer.WriteWhitespace ("\t");
//			writer.WriteEndElement ();
//			writer.WriteWhitespace ("\n");
//
//			writer.WriteEndElement ();
//			writer.WriteEndDocument ();
		
//			writer.Close ();
//		}
	}
}
