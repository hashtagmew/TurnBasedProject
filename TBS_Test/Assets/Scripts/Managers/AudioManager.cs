using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	public int iMusicVolume;
	public int iSoundVolume;

	public Dictionary<string, AudioClip> d_acSounds;
	
	void Start() {
		Object[] loadin = Resources.LoadAll("Audio/", typeof(AudioClip));

		d_acSounds = new Dictionary<string, AudioClip>();

		foreach (AudioClip clip in loadin) {
			d_acSounds.Add(clip.name, (AudioClip)clip);
		}

		if (d_acSounds.Count == 0) {
			Debug.LogWarning("No sound effects loaded, intentional?");
		}
	}

	void Update() {
		
	}

	public void ChangeSFXVolume(int newvol) {
		iSoundVolume = newvol;
	}

	public void ChangeMusicVolume(int newvol) {
		iMusicVolume = newvol;
	}
}
