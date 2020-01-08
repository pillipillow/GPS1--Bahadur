using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTriggerScript : MonoBehaviour {

	public GameObject DestroyNPC;
	public GameObject SetActiveNPC;

	void Start()
	{
		SetActiveNPC.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			SetActiveNPC.SetActive(true);
			Destroy(DestroyNPC);
			Destroy(gameObject);
		}
	}
}
