using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossRoomManagerScript : MonoBehaviour {

	public GameObject boss;
	public GameObject djinn;
	public GameObject portal;

	public bool doorLocked;
	public bool bossDefeated;
	public bool upgradeDone;
	public bool triggered;

	void Start ()
	{
		doorLocked = false;
	}

	// Update is called once per frame
	void Update () 
	{
		if (boss == null)
		{
			bossDefeated = true;
		}

		if (djinn == null)
		{
			upgradeDone = true;
		}

		if (bossDefeated && !upgradeDone)
		{
			djinn.SetActive (true);
		}
		else if (bossDefeated && upgradeDone)
		{
			djinn.SetActive (false);
			portal.SetActive (true);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (triggered)
			{
				return;
			}
			Player.Instance.checkPoint = other.transform.position;
			triggered = true;
			doorLocked = true;
		}
	}
}
