using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandExplosionScript : MonoBehaviour {

	public enum Element
	{
		FIRE = 0,
		WATER,
		EARTH,
		AIR,

		TOTAL
	}

	public Element element;

	public Sprite chargeUp;
	public Sprite FireExplosion;
	public Sprite WaterExplosion;
	public Sprite AirExplosion;
	public Sprite EarthExplosion;

	public float chargeUpTime;

	private SpriteRenderer spriteRenderer;
	private float chargeUpTimer;

	// Use this for initialization
	void Start () {
		chargeUpTimer = 0;
		gameObject.GetComponent<Collider2D>().enabled = false;

		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		spriteRenderer.sprite = chargeUp;
	}
	
	// Update is called once per frame
	void Update () 
	{
		chargeUpTimer += Time.deltaTime;

		if (chargeUpTimer >= chargeUpTime)
		{
			switch (element)
			{
				case(Element.AIR):
					spriteRenderer.sprite = AirExplosion;
					break;
				case(Element.WATER):
					spriteRenderer.sprite = WaterExplosion;
					break;
				case(Element.EARTH):
					spriteRenderer.sprite = EarthExplosion;
					break;
				case(Element.FIRE):
					spriteRenderer.sprite = FireExplosion;
					break;
			}

			gameObject.GetComponent<Collider2D>().enabled = true;
			Destroy (this.gameObject, 0.5f);
		}
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			Destroy (this.gameObject, 0.5f);
		}
	}
}
