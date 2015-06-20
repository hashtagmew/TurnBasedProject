using UnityEngine;
using System.Collections;

public class GameBuilding : GameUnit {

	protected SpriteRenderer sprend;
	
	// Use this for initialization
	void Start () {
		Init();
	}

	public void Init() {
		sprend = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void OnSelected() {
		sprend.color = Color.red;
	}
	
	public override void OnDeselected() {
		sprend.color = Color.white;
	}
}
