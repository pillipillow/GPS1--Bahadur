using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractTombScript : MonoBehaviour 
{
	//! Relics removed for now
	/*
	private bool canOpenTomb;
	private bool tombOpened;
	private Transform player;
	public GameObject relic;

	void Start () 
	{
		canOpenTomb = false;
		tombOpened = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (canOpenTomb == true && tombOpened == false) 
		{
			if (Input.GetKeyUp (KeyCode.F)) 
			{
				relic.SetActive (true);
				tombOpened = true;
				canOpenTomb = false;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.CompareTag ("Player") && tombOpened == false) 
		{
			player = other.transform;
			canOpenTomb = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) 
	{
		if (other.CompareTag ("Player") && tombOpened == false) 
		{
			player = null;
			canOpenTomb = false;
		}
	}
	*/
}
