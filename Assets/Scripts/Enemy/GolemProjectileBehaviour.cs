using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemProjectileBehaviour : MonoBehaviour {

	public enum Element
	{
		FIRE = 0,
		WATER,
		EARTH,
		AIR,

		TOTAL
	}

	public Element element;

	public float projectileSpeed;
	public float aliveLimit;
	public bool canMove;
	public bool isSecondary;
	public bool isBossProjectile;
	public GameObject miniEarthProjectile;
	public GameObject waterTrail;

	private float aliveTime;
	private bool triggered;
	private Vector3 newPos;
	private SpriteRenderer spriterenderer;

	void Start()
	{
		spriterenderer = gameObject.GetComponent<SpriteRenderer> ();

		if (isBossProjectile)
		{
			switch (element)
			{
				case (Element.AIR):
					spriterenderer.color = Color.white;
					break;
				case (Element.FIRE):
					spriterenderer.color = Color.red;
					break;
				case (Element.WATER):
					spriterenderer.color = Color.blue;
					break;
				case (Element.EARTH):
					spriterenderer.color = Color.yellow;
					break;
					
			}
		}	
	}

	void FixedUpdate () 
	{
		if (canMove)
			transform.Translate (Vector3.right * Time.deltaTime * projectileSpeed);
	}

	void Update()
	{
		aliveTime += Time.fixedDeltaTime;

		//special behaviours
		if (element == Element.EARTH)
		{
			EarthBehaviour ();
		}

		if (element == Element.WATER)
		{
			WaterBehaviour ();
		}

	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Wall")
		{
			Destroy (this.gameObject);
		}

		if (collider.gameObject.tag == "Player" || collider.gameObject.tag == "PlayerTurret")
		{
			if (element == Element.WATER && isSecondary)
			{
				triggered = true;
				Player.Instance.SlowPlayer (1f, true);
				return;
			}

			if (Player.Instance.playerInvul == false && Player.Instance.playerDodge == false)
			{
				Destroy (this.gameObject);
			}

		}
	}

	void OnTriggerExit2D(Collider2D collider)
	{
		if (collider.gameObject.tag == "Player")
		{
			if (element == Element.WATER && isSecondary)
			{
				triggered = false;
				Player.Instance.SlowPlayer (1f, false);
				return;
			}
		}
	}
		
	void EarthBehaviour()
	{
		if (isSecondary || isBossProjectile)
		{
			return;
		}

		if (aliveTime >= aliveLimit)
		{
			aliveTime = 0;

			for (int i = 0; i < 4; i++)
			{
				Vector3 rotation = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z + (i * 90));

				GameObject rock = (GameObject)Instantiate (miniEarthProjectile, this.transform.position  , Quaternion.Euler (rotation));
			}

			Destroy (this.gameObject);
		}
	}

	void WaterBehaviour()
	{
		if (!isSecondary && !isBossProjectile)
		{
			if (aliveTime >= 0.2f)
			{
				aliveTime = 0;
				GameObject trail = (GameObject)Instantiate (waterTrail, this.transform.position  , this.transform.rotation);
				Destroy (trail, 2);
			}
		}
	}

	void OnDestroy()
	{
		if (element == Element.WATER && isSecondary && triggered)
		{
			Player.Instance.playerSpeed += 1f;
		}
	}

}
