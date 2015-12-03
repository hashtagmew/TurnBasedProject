using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UI_GetFaction : MonoBehaviour {

	public string sDisplayName;
	public Slider slider;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (slider.value == 1) {
			sDisplayName = "Magical Faction";
		}
		else if (slider.value == 2) {
			sDisplayName = "Mechanical Faction";
		}
		else if (slider.value == 3) {
			sDisplayName = "Biological Faction";
		}
		else {
			sDisplayName = "Error Faction";
		}

		this.gameObject.GetComponent<Text>().text = sDisplayName;
	}
}
