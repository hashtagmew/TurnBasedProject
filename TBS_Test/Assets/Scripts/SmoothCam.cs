using UnityEngine;
using System.Collections;

public class SmoothCam : MonoBehaviour {
	
	public float fDampTime = 0.15f;
	public float fHorzMargin = 6.3f;
	public float fVertMargin = 6.3f;


	private Vector3 velocity = Vector3.zero;
	public Transform target;
	
	// Update is called once per frame
	void Update() {
		if (target != null) {
			Vector3 point = GetComponent<Camera>().WorldToViewportPoint(target.position);
			Vector3 delta = target.position - GetComponent<Camera>().ViewportToWorldPoint(new Vector3(0.5f, 0.5f, point.z));
			Vector3 destination = transform.position + delta;
			transform.position = Vector3.SmoothDamp(transform.position, destination, ref velocity, fDampTime);
		}

		if (this.transform.position.x < fHorzMargin) {
			this.transform.position = new Vector3(fHorzMargin, this.transform.position.y, this.transform.position.z);
		}

		if (this.transform.position.y > fVertMargin) {
			this.transform.position = new Vector3(this.transform.position.x, fVertMargin, this.transform.position.z);
		}
	}
}