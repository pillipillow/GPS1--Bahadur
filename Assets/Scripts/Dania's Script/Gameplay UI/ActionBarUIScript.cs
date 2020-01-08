using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActionBarUIScript : MonoBehaviour {

	public int selectedSlot = 1;
	public Image[] slots;
	public Image selector;
	public Image [] overlay;
	private string playerElement;

	public GameObject turret;
	public Image[] turretIcons;
	public Image [] spellOverlay;

	private bool pause = false; //*PLEASE UNCOMMENT THIS ONCE PAUSE SCRIPT UI HAS BEEN PLACED*
	private bool upgrade = false;

	private void Update ()
	{
		//If pause,  it cannot scroll
		pause = PauseMenuScript.paused;  //*PLEASE UNCOMMENT THIS ONCE PAUSE SCRIPT UI HAS BEEN PLACED*
		upgrade = UpgradeMenuScript.upgraded;
		if (pause == true || upgrade == true) 
		{
			//print("The game is pausing");
			return;
		}
		UpdateScrolling ();
		UpdateSelector ();
		if(Player.Instance.getTurretSkill)
		{
			turret.SetActive(true);
		}
		else
		{
			turret.SetActive(false);
		}
	}

	private void UpdateScrolling ()
	{		
		playerElement = Player.Instance.element;

		if (playerElement == "Fire")
		{
			selectedSlot = 1;
			overlay[3].enabled = true;
			overlay[0].enabled = false;
			overlay[1].enabled = true;
			overlay[2].enabled = true;

			turretIcons[0].enabled = true; 
			turretIcons[1].enabled = false;
			turretIcons[2].enabled = false;
			turretIcons[3].enabled = false;
		} 
		else if (playerElement == "Air") 
		{
			selectedSlot = 2;
			overlay[0].enabled = true;
			overlay[1].enabled = false;
			overlay[2].enabled = true;
			overlay[3].enabled = true;

			turretIcons[0].enabled = false; 
			turretIcons[1].enabled = true;
			turretIcons[2].enabled = false;
			turretIcons[3].enabled = false;
		} 
		else if (playerElement == "Earth") 
		{
			selectedSlot = 3;
			overlay[1].enabled = true;
			overlay[2].enabled = false;
			overlay[3].enabled = true;
			overlay[0].enabled = true;

			turretIcons[0].enabled = false; 
			turretIcons[1].enabled = false;
			turretIcons[2].enabled = true;
			turretIcons[3].enabled = false;
		} 
		else if (playerElement == "Water") 
		{
			selectedSlot = 4;
			overlay[2].enabled = true;
			overlay[3].enabled = false;
			overlay[0].enabled = true;
			overlay[1].enabled = true;

			turretIcons[0].enabled = false; 
			turretIcons[1].enabled = false;
			turretIcons[2].enabled = false;
			turretIcons[3].enabled = true;
		}

		if(Player.Instance.spawnTurretCooldown < 0 || Player.Instance.spawnTurretCooldown == 45)
		{
			spellOverlay[0].enabled = false;
		}
		else 
		{
			spellOverlay[0].enabled = true;
		}
	}

	private void UpdateSelector()
	{
		selector.rectTransform.localPosition = slots[selectedSlot - 1].rectTransform.localPosition;
	}
}
