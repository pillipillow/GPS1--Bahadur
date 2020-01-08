using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room1Script : MonoBehaviour {

	CircleCollider2D npcCollider;
	DialogueScript dialogue2;
	InteractButtonScript interactButton;

	public GameObject tutorialBox;

	public GameObject npcShout1;
	DialogueScript dialogue;

	public GameObject roomManager;

	public GameObject floorPad1;
	SteppedScript floor1;

	public GameObject floorPad2;
	SteppedScript floor2;

	public GameObject floorPad3;
	SteppedScript floor3;

	public GameObject floorPad4;
	SteppedScript floor4;

	void Start()
	{
		dialogue = npcShout1.GetComponent<DialogueScript>();
	
		npcCollider = this.GetComponent<CircleCollider2D>();
		npcCollider.enabled = false;

		dialogue2 = this.GetComponent<DialogueScript>();
		interactButton = this.GetComponent<InteractButtonScript>();

		floor1 = floorPad1.GetComponent<SteppedScript>();
		floor2 = floorPad2.GetComponent<SteppedScript>();
		floor3 = floorPad3.GetComponent<SteppedScript>();
		floor4 = floorPad4.GetComponent<SteppedScript>();

		tutorialBox.SetActive(false);
	}

	void Update()
	{
		//roomManager.GetComponent<RoomManagerScript>().doorLocked = true;
		if(dialogue.currentLine == 6)
		{
			npcCollider.enabled = true;
			tutorialBox.SetActive(true);
		}
		if(floor1.step && floor2.step && floor3.step && floor4.step)
		{
			interactButton.DisableInteract();
			if(dialogue2.currentLine == 7)
			{
				tutorialBox.SetActive(false);
				npcCollider.radius = 4;
				dialogue2.requireButtonPress = false;
				dialogue2.currentLine = 10;
			}
			dialogue2.startLine = 10;
			dialogue2.endAtLine = 10;
			if(dialogue2.currentLine == 11)
			{
				roomManager.GetComponent<RoomManagerScript>().doorLocked = false;
				npcCollider.enabled = false;
			}

		}

	}


}
