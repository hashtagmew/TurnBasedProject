using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Debug_UIInfo : MonoBehaviour {

	public Camera maincam;

	public Text txtTouchCount;
	public Text txtOrthoSize;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		txtTouchCount.text = "Touches: " + Input.touchCount;
		txtOrthoSize.text = "Ortho Size: " + maincam.orthographicSize.ToString();
	}
}
