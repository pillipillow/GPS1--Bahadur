using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room3Script : MonoBehaviour {

	DialogueScript dialogue;
	CircleCollider2D npcCollider;
	InteractButtonScript interactButton;

	public GameObject AirGolem;
	GolemBehaviourScript airGolem;
	public GameObject WaterGolem;                            //I know this is most laziest way to do shizz _(:I/L)_
	GolemBehaviourScript waterGolem;
	public GameObject EarthGolem;
	GolemBehaviourScript earthGolem;
	public GameObject FireGolem;
	GolemBehaviourScript fireGolem;

	public GameObject Jar;

	public GameObject roomManager;

	public int closeDoorLine;
	public int activateAirGolemLine;
	public GameObject tutorialBoxAir;
	public GameObject tutorialBoxAir2;

	public int earthGolemLineStart;
	public int earthGolemLineEnd;
	public int activateEarthGolemLine;
	public GameObject tutorialBoxEarth;
	public GameObject tutorialBoxEarth2;

	public int waterGolemLineStart;
	public int waterGolemLineEnd;
	public int activateWaterGolemLine;
	public GameObject tutorialBoxWater;

	public int fireGolemLineStart;
	public int fireGolemLineEnd;
	public int activateFireGolemLine;
	public GameObject tutorialBoxFire;

	public int finalStartLine;
	public int activateJarLine;
	public GameObject arrowPointer;
	public int doorOpenLine;

	// Use this for initialization
	void Start () 
	{
		dialogue = this.GetComponent<DialogueScript>();
		npcCollider = this.GetComponent<CircleCollider2D>();
		interactButton = this.GetComponent<InteractButtonScript>();

		airGolem = AirGolem.GetComponent<GolemBehaviourScript>();
		earthGolem = EarthGolem.GetComponent<GolemBehaviourScript>();
		waterGolem = WaterGolem.GetComponent<GolemBehaviourScript>();
		fireGolem = FireGolem.GetComponent<GolemBehaviourScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if(dialogue.currentLine == closeDoorLine)
		{
			roomManager.GetComponent<RoomManagerScript> ().doorLocked = true;
		}*/
		if(dialogue.currentLine == activateAirGolemLine)
		{
			airGolem.canAct = true;
			tutorialBoxAir.SetActive(true);
		}
		if(Input.GetMouseButton(0) && tutorialBoxAir.activeSelf)
		{
			tutorialBoxAir.SetActive(false);
			tutorialBoxAir2.SetActive(true);
		}
		if(AirGolem == null)
		{
			tutorialBoxAir2.SetActive(false);
			if(dialogue.currentLine == closeDoorLine)
			{
				dialogue.currentLine = earthGolemLineStart;
			}
			dialogue.startLine = earthGolemLineStart;
			dialogue.endAtLine = earthGolemLineEnd;
		}
		if(dialogue.currentLine == activateEarthGolemLine)
		{
			earthGolem.canAct = true;
			tutorialBoxEarth.SetActive(true);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2) || Input.GetAxis ("Mouse ScrollWheel") < 0f && tutorialBoxEarth.activeSelf)
		{
			tutorialBoxEarth.SetActive(false);
			tutorialBoxEarth2.SetActive(true);
		}
		if(EarthGolem == null)
		{
			tutorialBoxEarth.SetActive(false);
			tutorialBoxEarth2.SetActive(false);
			if(dialogue.currentLine == earthGolemLineStart)
			{
				dialogue.currentLine = waterGolemLineStart;
			}
			dialogue.startLine = waterGolemLineStart;
			dialogue.endAtLine = waterGolemLineEnd;
		}
		if(dialogue.currentLine == activateWaterGolemLine)
		{
			waterGolem.canAct = true;
			tutorialBoxWater.SetActive(true);
		}
		if(WaterGolem == null)
		{
			tutorialBoxWater.SetActive(false);
			if(dialogue.currentLine == waterGolemLineStart)
			{
				dialogue.currentLine = fireGolemLineStart;
			}
			dialogue.startLine = fireGolemLineStart;
			dialogue.endAtLine = fireGolemLineEnd;
		}
		if(dialogue.currentLine == activateFireGolemLine)
		{
			tutorialBoxFire.SetActive(true);
			fireGolem.canAct = true;
			fireGolem.canAttack = true;
		}
		if(FireGolem == null)
		{
			tutorialBoxFire.SetActive(false);
			npcCollider.radius = 2.5f;
			dialogue.requireButtonPress = false;
			interactButton.DisableInteract();
			if(dialogue.currentLine == fireGolemLineStart)
			{
				dialogue.currentLine = finalStartLine;
			}
			dialogue.startLine = finalStartLine;
			dialogue.endAtLine = doorOpenLine;
		}
		if(dialogue.currentLine == activateJarLine)
		{
			if(GameObject.Find("Jar(Clone)") == null)
			{
				Instantiate(Jar,this.transform.position + new Vector3(0,1,0),Quaternion.identity);
				roomManager.GetComponent<RoomManagerScript> ().doorLocked = true;
			}
		}
		if(dialogue.currentLine == doorOpenLine)
		{
			arrowPointer.SetActive(true);
			npcCollider.enabled = false;
		}
		if(dialogue.currentLine == 40)
		{
			if(GameObject.Find("Jar(Clone)") == null)
			{
				arrowPointer.SetActive(false);
				roomManager.GetComponent<RoomManagerScript> ().doorLocked = false;
			}
		}

	}
}
		