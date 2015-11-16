using UnityEngine;
using System.Collections;
using System.Collections.Generic;

using UnityEngine.UI;

public class SoundSampler : MonoBehaviour {

	public AudioSource asSpeaker;

	//Dictionary<string, AudioClip> d_sacClips;

	public AudioClip[] a_acClips;

	public int iPage;
	public int iMaxPage;

	public Text txtClipName;
	public Text txtPage;

	// Use this for initialization
	void Start () {
//		d_sacClips = new Dictionary<string, AudioClip>();
//
//		foreach (AudioClip clip in a_acClips) {
//			d_sacClips.Add(clip.name, clip);
//		}

		iMaxPage = a_acClips.Length;
	}
	
	// Update is called once per frame
	void Update () {
		txtPage.text = iPage.ToString() + "/" + (iMaxPage - 1).ToString();

		txtClipName.text = a_acClips[iPage].name;
	}

	public void PlayClip() {
		//asSpeaker.PlayOneShot(a_acClips[iPage]);
		asSpeaker.clip = a_acClips[iPage];
		asSpeaker.Play();
	}

	public void ChangePage(int move) {
		iPage += move;
		iPage = Mathf.Clamp(iPage, 0, iMaxPage - 1);
	}

	public void LeaveScene() {
		Application.LoadLevel("main-menu");
	}
}
