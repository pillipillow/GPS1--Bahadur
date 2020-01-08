using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulProjectileScript : MonoBehaviour {

	public enum GhoulProjectileElement {FIRE, WATER, AIR, EARTH};
	public GhoulProjectileElement ghoulProjectileElement;

	public Sprite fireSprite;
	public Sprite waterSprite;
	public Sprite airSprite;
	public Sprite earthSprite;
	public Sprite secondaryFireSprite;

	public GameObject secondaryAirProjectile;

	public float earthProjectileSpeed;
	public float waterProjectileSpeed;
	public float airProjectilesFireRate;
	public float fireAliveLimit;
	public float waterAliveLimit;
	public float earthAliveLimit;
	public float airAliveLimit;

	public float aliveTime;
	private float turretAttackTimer;
	private bool triggered;
	private Vector3 newPos;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () 
	{
		spriteRenderer = this.GetComponent<SpriteRenderer> ();

		if (ghoulProjectileElement == GhoulProjectileElement.FIRE)
		{
			spriteRenderer.sprite = fireSprite;
			spriteRenderer.color = Color.magenta;
		}

		else if (ghoulProjectileElement == GhoulProjectileElement.WATER)
		{
			spriteRenderer.sprite = waterSprite;
		}

		else if (ghoulProjectileElement == GhoulProjectileElement.AIR)
		{
			spriteRenderer.sprite = airSprite;
		}

		else if (ghoulProjectileElement == GhoulProjectileElement.EARTH)
		{
			spriteRenderer.sprite = earthSprite;
		}

		gameObject.GetComponent<BoxCollider2D> ().size = spriteRenderer.sprite.bounds.size;
	}
	
	// Update is called once per frame
	void Update () 
	{
		aliveTime += Time.deltaTime;

		if (ghoulProjectileElement == GhoulProjectileElement.FIRE)
		{
			FireBehaviour ();
		}

		else if (ghoulProjectileElement == GhoulProjectileElement.WATER)
		{
			WaterBehaviour ();
		}

		else if (ghoulProjectileElement == GhoulProjectileElement.AIR)
		{
			AirBehaviour ();
		}

		else if (ghoulProjectileElement == GhoulProjectileElement.EARTH)
		{
			EarthBehaviour ();
		}
	}

	void FireBehaviour()
	{
		if (aliveTime < fireAliveLimit / 2)
		{
			GetComponent<BoxCollider2D> ().enabled = false;
		}
		else if (aliveTime >= fireAliveLimit / 2)
		{
			spriteRenderer.sprite = secondaryFireSprite;
			GetComponent<BoxCollider2D> ().enabled = true;
		}

		if (aliveTime >= fireAliveLimit)
		{
			Destroy (this.gameObject);
		}

	}

	void WaterBehaviour()
	{
		transform.Translate (Vector3.right * Time.deltaTime * earthProjectileSpeed);

		if (aliveTime >= waterAliveLimit)
		{
			Destroy (gameObject);
		}
	}

	void EarthBehaviour()
	{
		transform.Translate (Vector3.right * Time.deltaTime * earthProjectileSpeed);

		if (aliveTime >= earthAliveLimit)
		{
			Destroy (gameObject);
		}
	}

	//turrets fire towards player
	void AirBehaviour()
	{
		turretAttackTimer += Time.deltaTime;

		Vector3 direction = Player.Instance.transform.position - this.transform.position;
		direction.Normalize ();

		Vector3 firePoint = this.transform.position + new Vector3 (direction.x * 0.64f, direction.y * 0.64f);

		Quaternion rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);

		if (turretAttackTimer >= airProjectilesFireRate)
		{
			turretAttackTimer = 0;
			Instantiate (secondaryAirProjectile, firePoint, rotation);
		}

		if (aliveTime >= airAliveLimit)
		{
			Destroy (gameObject);
		}
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Wall")
		{
			Destroy (this.gameObject);
		}

		else if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "PlayerTurret")
		{
			if (Player.Instance.playerInvul == false && Player.Instance.playerDodge == false && ghoulProjectileElement != GhoulProjectileElement.FIRE)
			{
				Destroy (this.gameObject);
			}

		}
	}
}
