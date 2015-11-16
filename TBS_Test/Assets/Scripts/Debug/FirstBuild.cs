using UnityEngine;
using System.Collections;

public class FirstBuild : MonoBehaviour {

	public AudioClip testsound1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(1)) {
			this.GetComponent<AudioSource>().PlayOneShot(testsound1);
		}

		if (Input.GetKeyDown(KeyCode.F1)) {
			GameObject.FindObjectOfType<GameMap>().LoadMap("Assets/Resources/Maps/testfeatures.xml");
		}
	}
}
