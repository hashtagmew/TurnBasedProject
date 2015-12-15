using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class AudioManager_UITranslator : MonoBehaviour {

	public AudioManager mngAudio;

	public Slider slidAudio;
	public Slider slidMusic;

	public float fDeltaSlide;
	
	void OnEnable() {
		mngAudio.LoadSavedVolumes();

		slidAudio.value = mngAudio.iSoundVolume;
		slidMusic.value = mngAudio.iMusicVolume;
		Debug.Log("Set sfx to " + mngAudio.iSoundVolume.ToString());
		Debug.Log("Set music to " + mngAudio.iMusicVolume.ToString());

		fDeltaSlide = slidAudio.value;

		slidAudio.onValueChanged.AddListener(delegate {Translate();});
		slidMusic.onValueChanged.AddListener(delegate {Translate();});
	}

	void Update() {
		if (Input.touchCount == 0 && !Input.GetMouseButton(0)) {
			//Debug.Log("NOTOUCH");
			if (fDeltaSlide != slidAudio.value) {
				mngAudio.PlayTestAudio();
				fDeltaSlide = slidAudio.value;
			}
		}
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
