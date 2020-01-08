using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakJarScript : MonoBehaviour 
{
	[HideInInspector] public bool random = true;
	public Sprite SpriteJarOne;
	public Sprite SpriteJarTwo;
	public Sprite SpriteJarThree;
	public GameObject HealthRune;

	private int dropChance;
	private int spriteType;
	private SpriteRenderer spriteRenderer;

	void Start ()
	{
		random = true;
		spriteRenderer = this.GetComponent <SpriteRenderer> ();
		SetJarSprite ();
	}

	void SetJarSprite ()
	{
		spriteType = Random.Range (1, 4);
		if (spriteType == 1)
		{
			this.spriteRenderer.sprite = SpriteJarOne;
		}
		else if (spriteType == 2)
		{
			this.spriteRenderer.sprite = SpriteJarTwo;
		}
		else if (spriteType == 3)
		{
			this.spriteRenderer.sprite = SpriteJarThree;
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "PlayerBullet")
		{
			if (random == true)
			{
				dropChance = Random.Range (1, 3);
				if (dropChance == 1)
				{
					SoundManagerScript.Instance.PlaySFX(AudioClipID.SFX_ITEM_DROP);
					Instantiate (HealthRune, this.transform.position, Quaternion.identity);
					transform.DetachChildren();
				}
				Destroy (this.gameObject);
			}
			else
			{
				Instantiate (HealthRune, this.transform.position, Quaternion.identity);
				transform.DetachChildren();
				Destroy (this.gameObject);
			}
		}
	}
}
