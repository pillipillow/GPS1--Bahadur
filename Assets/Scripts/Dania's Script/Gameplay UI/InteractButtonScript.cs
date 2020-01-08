using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractButtonScript : MonoBehaviour {

	public GameObject interactButtonPanel;

	// Use this for initialization
	void Start () 
	{
		DisableInteract();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(Input.GetKeyDown(KeyCode.F))
		{
			DisableInteract();
		}
	}

	public void EnableInteract()
	{
		interactButtonPanel.SetActive(true);
	}

	public void DisableInteract()
	{
		interactButtonPanel.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			EnableInteract();
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			DisableInteract();
		}
	}



}
