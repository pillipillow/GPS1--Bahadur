using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room4Script : MonoBehaviour {

	public GameObject tutorialBoxFire;
	public GameObject tutorialBoxAir;
	public GameObject tutorialBoxEarth;
	public GameObject tutorialBoxWater;

	private string playerElement;
	bool inRoom4;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		playerElement = Player.Instance.element;

		if(inRoom4 == true)
		{
			if(playerElement == "Fire")
			{
				tutorialBoxFire.SetActive(true);
				tutorialBoxAir.SetActive(false);
				tutorialBoxEarth.SetActive(false);
				tutorialBoxWater.SetActive(false);
			}
			else if(playerElement == "Air")
			{
				tutorialBoxFire.SetActive(false);
				tutorialBoxAir.SetActive(true);
				tutorialBoxEarth.SetActive(false);
				tutorialBoxWater.SetActive(false);
			}
			else if(playerElement == "Earth")
			{
				tutorialBoxFire.SetActive(false);
				tutorialBoxAir.SetActive(false);
				tutorialBoxEarth.SetActive(true);
				tutorialBoxWater.SetActive(false);
			}
			else if(playerElement == "Water")
			{
				tutorialBoxFire.SetActive(false);
				tutorialBoxAir.SetActive(false);
				tutorialBoxEarth.SetActive(false);
				tutorialBoxWater.SetActive(true);
			}
		}		
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			inRoom4 = true;
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			inRoom4 = false;
			tutorialBoxFire.SetActive(false);
			tutorialBoxAir.SetActive(false);
			tutorialBoxEarth.SetActive(false);
			tutorialBoxWater.SetActive(false);
		}
	}
}
