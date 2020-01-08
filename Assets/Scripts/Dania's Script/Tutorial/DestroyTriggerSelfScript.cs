using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyTriggerSelfScript : MonoBehaviour {

	public GameObject DestroyNPC;

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			Destroy(DestroyNPC);
			Destroy(gameObject);
		}
	}
}
