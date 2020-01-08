using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalScript : MonoBehaviour {

	public GameObject tempPanel;

	// Use this for initialization
	void Start () 
	{
		tempPanel.SetActive (false);
	}
	
	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			tempPanel.SetActive (true);
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			tempPanel.SetActive(false);
		}
	}
}
