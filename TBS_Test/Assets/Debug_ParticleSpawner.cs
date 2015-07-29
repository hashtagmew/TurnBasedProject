using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class Debug_ParticleSpawner : MonoBehaviour {

	public Dictionary<string, GameObject> d_particles;

	public GameObject goDummy;
	public Text txtEntry;
	public Text txtCounter;
	public Text txtList;

	public SlideCam camMain;

	// Use this for initialization
	void Start () {
		d_particles = new Dictionary<string, GameObject>();

		Object[] goTemp = Resources.LoadAll("Particle Effects");

		for (int i = 0; i < goTemp.Length; i++) {
			d_particles.Add(goTemp[i].name, (GameObject)goTemp[i]);
		}
	}
	
	// Update is called once per frame
	void Update () {
		txtCounter.text = "Particle systems: " + goDummy.transform.childCount.ToString();

		if (goDummy.transform.childCount > 0) {
			txtList.text = "";
			for (int i = 0; i < goDummy.transform.childCount; i++) {
				txtList.text += goDummy.transform.GetChild(i).name + "\n";
			}
		}
		else {
			txtList.text = "";
		}
	}

	public void EnableCamera() {
		camMain.enabled = true;
	}

	public void DisableCamera() {
		camMain.enabled = false;
	}

	public void ButtonPressSpawn() {
		SpawnParticle(txtEntry.text);
	}

	public void ButtonPressClear() {
		ClearParticle(txtEntry.text);
	}

	public void SpawnParticle(string particle) {
		if (d_particles.ContainsKey(particle)) {
			GameObject newgo = (GameObject)GameObject.Instantiate(d_particles[particle]);
			newgo.transform.SetParent(goDummy.transform);
		}
		else {
			Debug.Log("Can't find particle system named \"" + particle + "\"");
		}
	}

	public void ClearParticle(string particle) {
		Transform found = goDummy.transform.Find(particle + "(Clone)");

		if (found != null) {
			GameObject.Destroy(found.gameObject);
		}
		else {
			Debug.Log("Can't find particle system named \"" + particle + "\"");
		}
	}
}
