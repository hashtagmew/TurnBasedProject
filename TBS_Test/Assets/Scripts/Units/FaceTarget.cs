using UnityEngine;
using System.Collections;

public class FaceTarget : MonoBehaviour {
	public GameObject target;
	
	void Update () {

		target = GameObject.FindGameObjectWithTag ("MainCamera");
		if (target != null) {
			transform.rotation = target.transform.rotation;
		}
	}
}
