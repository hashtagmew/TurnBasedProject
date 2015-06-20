using UnityEngine;
using System.Collections;

public class DogSquad : GameUnit {

	private SpriteRenderer sprend;

	// Use this for initialization
	void Start () {
		sprend = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnSelected() {
		sprend.color = Color.blue;
	}

	public override void OnDeselected() {
		sprend.color = Color.white;
	}
}
