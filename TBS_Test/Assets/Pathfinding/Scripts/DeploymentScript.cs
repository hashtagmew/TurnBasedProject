using UnityEngine;
using System.Collections;

public class DeploymentScript : MonoBehaviour {

	public GameObject[] DeployPrefab;
	public GameObject DeployTransform;
	public GameObject DeployUI;

	// Use this for initialization
	void Start () {
		DeployPrefab [0].GetComponent<Unit> ().tileX = (int)DeployPrefab [0].transform.position.x;
		DeployPrefab [0].GetComponent<Unit> ().tileY = (int)DeployPrefab [0].transform.position.z;

		DeployPrefab [1].GetComponent<Unit> ().tileX = (int)DeployPrefab [1].transform.position.x;
		DeployPrefab [1].GetComponent<Unit> ().tileY = (int)DeployPrefab [1].transform.position.z;

		DeployPrefab [2].GetComponent<Unit> ().tileX = (int)DeployPrefab [2].transform.position.x;
		DeployPrefab [2].GetComponent<Unit> ().tileY = (int)DeployPrefab [2].transform.position.z;

		DeployPrefab [3].GetComponent<Unit> ().tileX = (int)DeployPrefab [3].transform.position.x;
		DeployPrefab [3].GetComponent<Unit> ().tileY = (int)DeployPrefab [3].transform.position.z;

		DeployPrefab [4].GetComponent<Unit> ().tileX = (int)DeployPrefab [4].transform.position.x;
		DeployPrefab [4].GetComponent<Unit> ().tileY = (int)DeployPrefab [4].transform.position.z;
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
