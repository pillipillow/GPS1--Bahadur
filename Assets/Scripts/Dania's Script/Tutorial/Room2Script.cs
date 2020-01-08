using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room2Script : MonoBehaviour {

	public GameObject tutorialBox;

	public GameObject NPC;
	DialogueScript dialogue;
	
	// Use this for initialization
	void Start () 
	{
		dialogue = NPC.GetComponent<DialogueScript>();
		
		tutorialBox.SetActive(false);	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(dialogue.currentLine == 18)
		{
			tutorialBox.SetActive(true);
		}	
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			tutorialBox.SetActive(false);
		}
	}

}
