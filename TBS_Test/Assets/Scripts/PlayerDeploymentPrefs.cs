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
		TextAsset taPly = (TextAsset)Resources.Load<TextAsset> ("playerprefs");
		
		if (taPly == null) {
			Debug.LogWarning ("The player file was not found!");

			return;
		}
		
		XDocument xmlDoc = XDocument.Load (new StringReader(taPly.text));

		foreach (XElement xroot in xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (xlayer1.Name == "name") {
					//sName = xlayer1.Value;
				}
				else if (xlayer1.Name == "magical") {
					foreach (XElement xlayer2 in xlayer1.Elements()) {
						l_sMagicalUnits.Add(new KeyValuePair<string, int>(xlayer2.Value, int.Parse(xlayer2.FirstAttribute.Value)));
					}
				}
				else if (xlayer1.Name == "mechanical") {
					foreach (XElement xlayer2 in xlayer1.Elements()) {
						l_sMechanicalUnits.Add(new KeyValuePair<string, int>(xlayer2.Value, int.Parse(xlayer2.FirstAttribute.Value)));
					}
				}
			}
		}

	}

	public void SavePrefs() {
		XmlWriterSettings xsettings = new XmlWriterSettings();
		xsettings.Encoding = Encoding.ASCII;
		xsettings.OmitXmlDeclaration = true;
		
		using (XmlWriter writer = XmlWriter.Create(Application.persistentDataPath + "/playerprefs.xml", xsettings)) {
		
			writer.WriteStartDocument ();
			writer.WriteStartElement ("TBSPlayer");
			writer.WriteWhitespace ("\n");
		
			//Name
			writer.WriteWhitespace ("\t");
			writer.WriteStartElement ("name");
			writer.WriteValue ("default");
			writer.WriteEndElement ();
			writer.WriteWhitespace ("\n");

			//Magical
			writer.WriteWhitespace ("\t");
			writer.WriteStartElement ("magical");
			writer.WriteWhitespace ("\n");
		
			foreach (KeyValuePair<string, int> str in l_sMagicalUnits) {
				writer.WriteWhitespace ("\t\t");

				writer.WriteStartElement ("unit");
				writer.WriteAttributeString ("cost", str.Value.ToString ());
				writer.WriteValue (str.Key);
				writer.WriteEndElement ();
				writer.WriteWhitespace ("\n");
			}
		
			writer.WriteWhitespace ("\t");
			writer.WriteEndElement ();
			writer.WriteWhitespace ("\n");

			//Mech
			writer.WriteWhitespace ("\t");
			writer.WriteStartElement ("mechanical");
			writer.WriteWhitespace ("\n");
		
			foreach (KeyValuePair<string, int> str in l_sMechanicalUnits) {
				writer.WriteWhitespace ("\t\t");

				writer.WriteStartElement ("unit");
				writer.WriteAttributeString ("cost", str.Value.ToString ());
				writer.WriteValue (str.Key);
				writer.WriteEndElement ();
				writer.WriteWhitespace ("\n");
			}
		
			writer.WriteWhitespace ("\t");
			writer.WriteEndElement ();
			writer.WriteWhitespace ("\n");

			writer.WriteEndElement ();
			writer.WriteEndDocument ();
		
			writer.Close ();
		}
	}
}
