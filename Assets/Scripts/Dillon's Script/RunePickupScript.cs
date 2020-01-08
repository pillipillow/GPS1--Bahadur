using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunePickupScript : MonoBehaviour 
{
	public int runeHealAmount;
	public GameObject popUp;

	void Start()
	{
		popUp.SetActive(false);
	}


	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			//! For player achievementWhoNeedsHealing failed
			if (PlayerAchievementScript.Instance.achievementWhoNeedsHealing == true)
			{
				PlayerAchievementScript.Instance.achievementWhoNeedsHealing = false;
			}

			if (Player.Instance.playerHP == Player.Instance.playerHPMax - 1)
			{
				Player.Instance.playerHP = Player.Instance.playerHPMax;
				Destroy (this.gameObject);
			}
			else if (Player.Instance.playerHP < Player.Instance.playerHPMax)
			{
				Player.Instance.playerHP = Player.Instance.playerHP + runeHealAmount;
				Destroy (this.gameObject);
			}
			else if(Player.Instance.playerHP >= Player.Instance.playerHPMax)
			{
				popUp.SetActive(true);
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			popUp.SetActive(false);
		}
	}
}
