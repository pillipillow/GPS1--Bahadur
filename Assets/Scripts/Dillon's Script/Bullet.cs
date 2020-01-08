using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour 
{
	//! Sprites for the player bullets
	public Sprite turretBulletSprite;
	public Sprite secondaryFireSprite;
	public Sprite baseFireSprite;
	public Sprite baseWaterSprite;
	public Sprite baseEarthSprite;
	public Sprite baseAirSprite;
	private SpriteRenderer spriteRenderer;

	//! Damage for each element
	public float fireDamage;
	public float waterDamage;
	public float airDamage;
	public float airDamageSecond;
	public float earthDamage;
	public float earthDamageSecond;
	public float totalDamage;
	public int enemyBounceNumber;
	private float earthExplodeRadius = 0.5f;
	private bool airFirstHit = false;
	private int damageOfPlayer;

	//! Timers for the elements
	private float fireTimer = 0.3f;
	private int waterHits = 2;

	//! Speed of the projectiles + Element type
	public int projectileSpeed;
	[HideInInspector] public string bulletElement;

	//! If this bullet is from player or turret
	[HideInInspector] public bool isTurretBullet = false;

	Animator animator;

	void Start () 
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		SettingBulletDifference ();
		PlayerBulletStartProperties ();
		animator = GetComponent<Animator>();
	}

	void Update () 
	{
		BulletElementProperties ();
		Destroy (this.gameObject, 8f);
	}

	void SettingBulletDifference ()
	{
		if (isTurretBullet == false)
		{
			bulletElement = Player.Instance.element;
			damageOfPlayer = Player.Instance.playerDamage;
		}
		else if (isTurretBullet == true)
		{
			if (bulletElement == "Fire")
			{
				gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
				totalDamage = fireDamage;
			}
			if (bulletElement == "Air")
			{
				gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				totalDamage = airDamage;
			}
			if (bulletElement == "Earth")
			{
				gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
				totalDamage = earthDamage;
			}
			if (bulletElement == "Water")
			{
				gameObject.GetComponent<SpriteRenderer> ().color = Color.blue;
				totalDamage = waterDamage;
			}
		}
	}

	void PlayerBulletStartProperties ()
	{
		if (isTurretBullet == false)
		{
			//! Setting total damage for enemy to recieve
			if (bulletElement == "Fire") 
			{
				GetComponent<BoxCollider2D> ().enabled = false;
				spriteRenderer.sprite = baseFireSprite;
				totalDamage = fireDamage * damageOfPlayer;
			}
			else if (bulletElement == "Water") 
			{
				spriteRenderer.sprite = baseWaterSprite;
				totalDamage = waterDamage * damageOfPlayer;
			}
			else if (bulletElement == "Air") 
			{
				spriteRenderer.sprite = baseAirSprite;
				totalDamage = airDamage * damageOfPlayer;
				enemyBounceNumber = 3;
			}
			else if (bulletElement == "Earth") 
			{
				spriteRenderer.sprite = baseEarthSprite;
				totalDamage = earthDamage * damageOfPlayer;
			}
		}
	}

	void BulletElementProperties ()
	{
		if (isTurretBullet == false)
		{
			PlayerBulletUpdateProperties ();
		}
		else
		{
			transform.Translate (Vector3.up * Time.deltaTime * projectileSpeed);
		}
	}

	void PlayerBulletUpdateProperties ()
	{
		if (bulletElement != "Air")
		{
			transform.Translate (Vector3.right * Time.deltaTime * projectileSpeed);
		}

		//! So that fire will only damage enemies after a timeframe from initial cast
		if (bulletElement == "Fire") 
		{
			animator.Play("fireLava");
			if (fireTimer >= 0.2f)
			{
				fireTimer -= Time.deltaTime;
			}
			else if (fireTimer < 0.2f && fireTimer > 0)
			{
				fireTimer -= Time.deltaTime;
			}
			else if (fireTimer <= 0f)
			{
				fireTimer = 0.3f;
				GetComponent<BoxCollider2D> ().enabled = true;
			}
			Destroy (this.gameObject, 1.2f);
		}

		if (bulletElement == "Air")
		{
			if (airFirstHit == false)
			{
				transform.Translate (Vector3.right * Time.deltaTime * projectileSpeed);
			}

			if (enemyBounceNumber <= 0)
			{
				Destroy (this.gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (isTurretBullet == false)
		{
			if (other.gameObject.tag == "Wall")
			{
				if (bulletElement != "Fire")
				{
					Destroy (this.gameObject);
				}
			}

			if (other.gameObject.tag == "Jars")
			{
				Destroy (this.gameObject);
			}

			if (other.gameObject.tag == "Enemy")
			{
				//! Water bullets can pass through 3 enemies
				if (bulletElement == "Water") 
				{
					if (waterHits > 0)
					{
						waterHits --;
					}
					else
					{
						Destroy (this.gameObject);
					}
				}

				//! Air bullets will chain to enemies
				if (bulletElement == "Air") 
				{
					if (airFirstHit == false)
					{
						airFirstHit = true;
					}
					enemyBounceNumber --;
				}

				//! Earth bullets will explode on impact
				if (bulletElement == "Earth") 
				{
					GetComponent<CircleCollider2D> ().radius = earthExplodeRadius;
					projectileSpeed = 0;
					Destroy (this.gameObject, 0.1f);
				}
			}
		}
		else
		{
			if (other.gameObject.tag == "Enemy")
			{
				Destroy (this.gameObject);
			}

			if (other.gameObject.tag == "Wall")
			{
				Destroy (this.gameObject);
			}
		}
	}
}