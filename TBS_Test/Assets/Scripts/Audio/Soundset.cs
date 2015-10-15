using UnityEngine;

using System.IO;
using System.Xml;
using System.Xml.Linq;

using System.Collections;
using System.Collections.Generic;

using System.Text;

public class Soundset : MonoBehaviour {

	private Dictionary<string, AudioClip> d_sacSounds;

	// Use this for initialization
	void Start () {
		d_sacSounds = new Dictionary<string, AudioClip>();

		d_sacSounds.Add("Idle", new AudioClip());

		d_sacSounds.Add("Attack_Basic", new AudioClip());
		d_sacSounds.Add("Attack_Magic", new AudioClip());
		d_sacSounds.Add("Attack_Ranged", new AudioClip());

		d_sacSounds.Add("Footstep", new AudioClip());
		d_sacSounds.Add("Footstep_Wet", new AudioClip());
		d_sacSounds.Add("Footstep_Dry", new AudioClip());
		d_sacSounds.Add("Footstep_Fly", new AudioClip());

		d_sacSounds.Add("Hurt", new AudioClip());
		d_sacSounds.Add("Die", new AudioClip());


		d_sacSounds.Add("Cast", new AudioClip());
		d_sacSounds.Add("Execute", new AudioClip());
		d_sacSounds.Add("Execute_Loop", new AudioClip());
		d_sacSounds.Add("Finished", new AudioClip());
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadSound(string file, string key) {
		//
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
