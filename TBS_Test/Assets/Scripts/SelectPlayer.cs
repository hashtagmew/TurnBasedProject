using UnityEngine;
using System.Collections;

public class SelectPlayer : MonoBehaviour {

	public float speed = 1.5f;
	private Vector3 target;

	bool bUnitSelected;
	// Use this for initialization
	void Start () {
		target = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown(0))
		{
			Debug.Log("Mouse is down");
			target = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			target.z = transform.position.z;
		}
		transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
	}

	void OnClick(){

		bUnitSelected = true;
		Debug.Log ("Selected");
	}
}
