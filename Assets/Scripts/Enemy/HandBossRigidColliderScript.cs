using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandBossRigidColliderScript : MonoBehaviour {

	public GameObject promptPanel;
	private HandBehaviourScript handBehaviourScript;

	// Use this for initialization
	void Start () 
	{
		handBehaviourScript = gameObject.GetComponentInParent<HandBehaviourScript> ();
		promptPanel.SetActive (true);
	}

	void Update()
	{
		if (handBehaviourScript.isActive)
		{
			promptPanel.SetActive (false);
		}
	}

	void OnCollisionEnter2D (Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (handBehaviourScript.isActive)
			{
				return;
			}
			handBehaviourScript.canInteract = true;
			promptPanel.SetActive (true);
			return;
		}
	}

	void OnCollisionExit2D (Collision2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			handBehaviourScript.canInteract = false;
		}
	}
}
