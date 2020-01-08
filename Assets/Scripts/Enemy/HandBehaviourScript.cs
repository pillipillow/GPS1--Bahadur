using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class HandBoss
{
	public Sprite idleSprite;
	public Sprite clenchSprite;
	public Sprite explodeSprite;
}

public class HandBehaviourScript : MonoBehaviour {

	public enum ELEMENT_TYPE 
	{
		Fire = 0,
		Water, 
		Earth,
		Air,

		TOTAL
	};
	public ELEMENT_TYPE elementType;

	public HandBoss handBoss;
	public float handHealth;
	public float circleAttackCooldown;
	public float explosionAttackCooldown;
	public float circleAttackChargeUp;
	public bool isActive;
	public bool canInteract;
	public GameObject handProjectile;
	public GameObject handExplosion;

	public Sprite fireIdleSprite;
	public Sprite fireClenchSprite;
	public Sprite fireExplodeSprite;
	public Sprite waterIdleSprite;
	public Sprite waterClenchSprite;
	public Sprite waterExplodeSprite;
	public Sprite airIdleSprite;
	public Sprite airClenchSprite;
	public Sprite airExplodeSprite;
	public Sprite earthIdleSprite;
	public Sprite earthClenchSprite;
	public Sprite earthExplodeSprite;

	private SpriteRenderer spriteRenderer;

	private string weakElement;
	private Transform player;
	private Vector3 currentPos;
	public float circleAttackTimer;
	private float explosionAttackTimer;
	private int offsetAngle;

	//for color manipulation
	private Color oriColor;


	// Use this for initialization
	void Start () 
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		currentPos = this.transform.position;
		oriColor = spriteRenderer.color;
		spriteRenderer.color = Color.gray;
		circleAttackTimer = 0;

		//set weakness and sprites
		if (elementType.ToString()== "Fire")
		{
			weakElement = "Water";
			handBoss.idleSprite = fireIdleSprite;
			handBoss.clenchSprite = fireClenchSprite;
			handBoss.explodeSprite = fireExplodeSprite;
		} 

		else if (elementType.ToString() == "Water")
		{
			weakElement = "Earth";
			handBoss.idleSprite = waterIdleSprite;
			handBoss.clenchSprite = waterClenchSprite;
			handBoss.explodeSprite = waterExplodeSprite;
		} 

		else if (elementType.ToString()== "Air")
		{
			weakElement = "Fire";
			handBoss.idleSprite = airIdleSprite;
			handBoss.clenchSprite = airClenchSprite;
			handBoss.explodeSprite = airExplodeSprite;
		}

		else if (elementType.ToString() == "Earth")
		{
			weakElement = "Air";
			handBoss.idleSprite = earthIdleSprite;
			handBoss.clenchSprite = earthClenchSprite;
			handBoss.explodeSprite = earthExplodeSprite;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!isActive)
		{
			if (canInteract == true)
			{
				if (Input.GetKeyDown(KeyCode.F))
				{
					spriteRenderer.color = oriColor;
					spriteRenderer.sprite = handBoss.idleSprite;
					isActive = true;
				}
			}
			return;
		}
		circleAttackTimer += Time.deltaTime;
		explosionAttackTimer += Time.deltaTime;

		if (circleAttackTimer >= circleAttackCooldown)
		{
			circleAttackTimer = 0;
			StartCoroutine (CircleAttack ());
		}

		if (explosionAttackTimer >= explosionAttackCooldown)
		{
			explosionAttackTimer = 0;
			ExplosionAttack ();
		}

		CheckHandDeath ();
	}

	IEnumerator CircleAttack()
	{
		spriteRenderer.sprite = handBoss.clenchSprite;

		yield return new WaitForSeconds (circleAttackChargeUp);

		spriteRenderer.sprite = handBoss.explodeSprite;

		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 18; j++)
			{
				offsetAngle = 20 * j + (10 * i);
				Debug.Log ("Offset Angle" + offsetAngle);
				Quaternion rotation = this.transform.rotation* Quaternion.Euler (0,0,offsetAngle);
				GameObject go = (GameObject)Instantiate(handProjectile, currentPos, rotation);
				go.gameObject.GetComponent<GolemProjectileBehaviour> ().element = (GolemProjectileBehaviour.Element)((int)elementType);
			}
			yield return new WaitForSeconds (0.5f);
		}

		yield return new WaitForSeconds (1f);
		spriteRenderer.sprite = handBoss.idleSprite;

	}

	void ExplosionAttack ()
	{
		Vector3 target = Player.Instance.transform.position;

		GameObject go = (GameObject)Instantiate(handExplosion, target, Quaternion.identity);
		go.gameObject.GetComponent<HandExplosionScript> ().element = (HandExplosionScript.Element)((int)elementType);
	}

	void CheckHandDeath()
	{
		if (handHealth <= 0)
		{
			Destroy (this.gameObject);
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "PlayerBullet")
		{
			if (isActive)
			{
				string element = other.gameObject.GetComponent<Bullet> ().bulletElement;
				float rawDamage = other.gameObject.GetComponent<Bullet> ().totalDamage;
				StartCoroutine (TakeDamage (element,rawDamage));
			}
		}
	}

	IEnumerator TakeDamage (string element, float rawDamage)
	{
		float damageMultiplier = 0f;

		//Set Damage Multiplier
		if (element == elementType.ToString ())
		{
			damageMultiplier = 0f;
		}

		else if (element == weakElement)
		{
			damageMultiplier = 1f;
			spriteRenderer.color = Color.magenta;
		}

		else
		{
			damageMultiplier = 0.3f;
			spriteRenderer.color = Color.magenta;
		}

		float damageRecieved = rawDamage * damageMultiplier;

		handHealth -= damageRecieved;
		yield return new WaitForSeconds (0.1f);
		spriteRenderer.color = Color.white;
	}
		
}
