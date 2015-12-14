using UnityEngine;

using System.IO;
using System.Xml;
using System.Xml.Linq;

using System.Collections;
using System.Collections.Generic;

using System.Text;

public class Soundset {

	private Dictionary<string, AudioClip> d_sacSounds = new Dictionary<string, AudioClip>();

	public void AddClip(string key, AudioClip clip) {
		d_sacSounds.Add(key, clip);
	}

	public void LoadSound(string file, string key) {
		AddClip(key, Resources.Load<AudioClip>("Audio/" + file));
	}

	public void LoadFromFile(string path) {
		XDocument xmlDoc = XDocument.Load(path);
		
		foreach (XElement xroot in xmlDoc.Elements()) {
			foreach (XElement xlayer1 in xroot.Elements()) {
				if (d_sacSounds.ContainsKey(xlayer1.Name.LocalName)) {
					LoadSound(xlayer1.Value, xlayer1.Name.LocalName);
				}
			}
		}
	}

	public AudioClip GetClip(string key) {
		return d_sacSounds[key];
	}
}
