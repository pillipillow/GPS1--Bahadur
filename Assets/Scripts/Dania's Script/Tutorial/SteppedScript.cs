using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SteppedScript : MonoBehaviour {

	public Sprite stepPad;
	SpriteRenderer floorPad;

	public bool step;

	void Start()
	{
		floorPad = this.GetComponent<SpriteRenderer>();
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if(other.name == "Player")
		{
			if (!step)
			{
				step = true;
				SoundManagerScript.Instance.PlaySFX (AudioClipID.SFX_ACTIVATE_PANEL);
				floorPad.sprite = stepPad;
			}
		}
	}
}
