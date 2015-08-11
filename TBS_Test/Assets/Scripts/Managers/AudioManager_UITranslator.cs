using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioManager_UITranslator : MonoBehaviour {

	public AudioManager mngAudio;

	public Slider slidAudio;
	public Slider slidMusic;
	
	void OnEnable() {
		slidAudio.onValueChanged.AddListener(delegate {Translate();});
		slidMusic.onValueChanged.AddListener(delegate {Translate();});
	}

	void OnDisable() {
		slidAudio.onValueChanged.RemoveListener(delegate {Translate();});
		slidMusic.onValueChanged.RemoveListener(delegate {Translate();});
	}

	public void Translate() {
		mngAudio.ChangeMusicVolume((int)slidMusic.value);
		mngAudio.ChangeSFXVolume((int)slidAudio.value);
	}
}
