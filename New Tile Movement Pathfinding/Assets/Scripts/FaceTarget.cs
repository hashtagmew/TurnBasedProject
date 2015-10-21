using UnityEngine;
using System.Collections;

public class FaceTarget : MonoBehaviour {
	public Transform target;
	
	void Update () {
		if (target != null) {
			transform.rotation = target.rotation;
		}
	}
}
