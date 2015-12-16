using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour {

	public int iMusicVolume;
	public int iSoundVolume;

	private AudioSource asMusic;
	public AudioSource asTestSound;

	public Dictionary<string, AudioClip> d_acSounds;

	void Awake() {
		LoadSavedVolumes();
	}
	
	void Start() {
		AudioClip[] loadin = Resources.LoadAll<AudioClip>("Audio/");

		d_acSounds = new Dictionary<string, AudioClip>();

		foreach (AudioClip clip in loadin) {
			d_acSounds.Add(clip.name, clip);
		}

		if (d_acSounds.Count == 0) {
			Debug.LogWarning("No sound effects loaded, intentional?");
		}

		if (Application.loadedLevelName == "net-main-menu") {
			asMusic = this.GetComponent<AudioSource>();
		}
		else {
			asMusic = this.GetComponentInChildren<AudioSource>();
		}

		LoadSavedVolumes();
	}

	public void PlayTestAudio() {
		asTestSound.volume = GetSFXVolume();
		asTestSound.Play();
	}

	void Update() {
		if (asMusic != null) {
			asMusic.volume = GetMusicVolume();
		}
	}

	public void PlayOnce(string path) {
		this.GetComponent<AudioSource> ().PlayOneShot (d_acSounds [path]);
	}

	public void LoadSavedVolumes() {
		if (PlayerPrefs.HasKey("Volume_SFX")) {
			iSoundVolume = PlayerPrefs.GetInt("Volume_SFX");
		}
		else {
			PlayerPrefs.SetInt("Volume_SFX", iSoundVolume);
			PlayerPrefs.Save();
		}

		if (PlayerPrefs.HasKey("Volume_Music")) {
			iMusicVolume = PlayerPrefs.GetInt("Volume_Music");
		}
		else {
			PlayerPrefs.SetInt("Volume_Music", iMusicVolume);
			PlayerPrefs.Save();
		}
	}

	public float GetSFXVolume() {
		return ((float)iSoundVolume) / 100.0f;
	}

	public float GetMusicVolume() {
		return ((float)iMusicVolume) / 100.0f;
	}

	public void ChangeSFXVolume(int newvol) {
		iSoundVolume = newvol;
		if (PlayerPrefs.HasKey("Volume_SFX")) {
			PlayerPrefs.SetInt("Volume_SFX", iSoundVolume);
			PlayerPrefs.Save();
		}
	}

	public void ChangeMusicVolume(int newvol) {
		iMusicVolume = newvol;
		if (PlayerPrefs.HasKey("Volume_Music")) {
			PlayerPrefs.SetInt("Volume_Music", iMusicVolume);
			PlayerPrefs.Save();
		}
	}

	public void ChangeMusic(string file) {
		this.asMusic.clip = Resources.Load<AudioClip>("Audio/Music/" + file);
		this.asMusic.loop = true;
		this.asMusic.Play();
	}
}
