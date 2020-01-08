using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTimerScript : MonoBehaviour 
{
	//! Just a script in case others want to use
	public float deathTimer;

	void Update () 
	{
		Destroy (this.gameObject, deathTimer);
	}
}
