using UnityEngine;
using System.Collections;

public class DeploymentScript : MonoBehaviour {

	public GameObject[] DeployPrefab;
	public GameObject DeployTransform;
	public GameObject DeployUI;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

	}
	public void UIappear(){
		DeployUI.SetActive (true);
	}

	public void DeployAM(){

		Instantiate (DeployPrefab[0], DeployTransform.transform.position, DeployTransform.transform.rotation);
		DeployUI.SetActive (false);
	}
	public void DeployAT(){
		
		Instantiate (DeployPrefab[1], DeployTransform.transform.position, DeployTransform.transform.rotation);
		DeployUI.SetActive (false);
	}
	public void DeployMC(){
		
		Instantiate (DeployPrefab[2], DeployTransform.transform.position, DeployTransform.transform.rotation);
		DeployUI.SetActive (false);
	}
	public void DeployDem(){
		
		Instantiate (DeployPrefab[3], DeployTransform.transform.position, DeployTransform.transform.rotation);
		DeployUI.SetActive (false);
	}
	public void DeployMag(){
		
		Instantiate (DeployPrefab[4], DeployTransform.transform.position, DeployTransform.transform.rotation);
		DeployUI.SetActive (false);
	}

}
