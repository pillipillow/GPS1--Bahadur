using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpRelicScript : MonoBehaviour 
{
	//! Relics removed for now
	/*
	//! The types of relics upgrades the player can get
	enum RelicType 
	{
		CritChance,
		CritDamage,
		StunEffect,
		SlowEffect,
		TormentDamage
	}

	private int randomRelicNo;
	private RelicType relicType;
	private Transform player;
	private bool canDestroy;
	public string relicName;

	void Start () 
	{
		//! To randomize the type of relic the player gets
		randomRelicNo = Random.Range (1, 6);

		if (randomRelicNo == 1) 
		{
			relicType = RelicType.CritChance;
		}
		else if (randomRelicNo == 2)
		{
			relicType = RelicType.CritDamage;
		}
		else if (randomRelicNo == 3)
		{
			relicType = RelicType.StunEffect;
		}
		else if (randomRelicNo == 4)
		{
			relicType = RelicType.SlowEffect;
		}
		else if (randomRelicNo == 5)
		{
			relicType = RelicType.TormentDamage;
		}
	}

	//! Not sure why this is called again here but I'll leave it at this for now
	void Awake () 
	{
		randomRelicNo = Random.Range (1, 6);

		if (randomRelicNo == 1) 
		{
			relicType = RelicType.CritChance;
		}
		else if (randomRelicNo == 2)
		{
			relicType = RelicType.CritDamage;
		}
		else if (randomRelicNo == 3)
		{
			relicType = RelicType.StunEffect;
		}
		else if (randomRelicNo == 4)
		{
			relicType = RelicType.SlowEffect;
		}
		else if (randomRelicNo == 5)
		{
			relicType = RelicType.TormentDamage;
		}

		relicName = relicType.ToString ();
		canDestroy = false;
	}

	void Update () 
	{
		if (Player.Instance.relicStatAdded == true && canDestroy == true) 
		{
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.CompareTag ("Player")) 
		{
			player = other.transform;
			canDestroy = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) 
	{
		if (other.CompareTag ("Player")) 
		{
			player = null;
			canDestroy = false;
		}
	}
	*/
}
