using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room6Script : MonoBehaviour {

	public GameObject npcShout1;
	public GameObject npcShout2;
	//public GameObject portal;

	// Use this for initialization
	void Start () 
	{
		npcShout2.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		if(UpgradeMenuScript.upgraded == false)
		{
			//Destroy(npcShout1);
			npcShout2.SetActive(true);
			//Instantiate(portal,new Vector3(this.transform.position.x,this.transform.position.y + 2,0),Quaternion.identity);
		}	
	}
}
