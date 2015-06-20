using UnityEngine;
using System.Collections;

public class GameUnit : MonoBehaviour, ISelectable {

	public Vector2 vGridPosition;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public virtual void OnSelected() {
		//
	}

	public virtual void OnDeselected() {
		//
	}
}
