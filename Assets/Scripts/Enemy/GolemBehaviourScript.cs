using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemBehaviourScript : MonoBehaviour {

	//limiters
	public bool canAct;
	public bool canAttack;

	//sprites
	public Sprite fireGolemSprite;
	public Sprite waterGolemSprite;
	public Sprite airGolemSprite;
	public Sprite earthGolemSprite;

	//to set the element of golem
	public enum GolemType 
	{
		Fire = 0,
		Water,
		Air,
		Earth,

		TOTAL
	};

	public GolemType golemType;

	//golem basic info
	public float golemHealth;
	public float golemDamage;
	public float golemMovementSpeed;
	public float golemAttackRange;
	public float golemMinimumProximity;

	//particle effects
	public GameObject enemyDeathParticles;

	//sprite handling
	private SpriteRenderer spriteRenderer;

	private Transform player;
	private Vector3 currentPos;
	private string weakElement;
	private bool hit = false;

	Animator animator;
	public RuntimeAnimatorController fireGolem;
	public RuntimeAnimatorController airGolem;
	public RuntimeAnimatorController earthGolem;

	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		currentPos = this.transform.position;

		//set golem sprite
		spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
		animator = GetComponent<Animator>();

		if (golemType == GolemType.Fire)
		{
			animator.runtimeAnimatorController = fireGolem as RuntimeAnimatorController;
		}
		else if (golemType == GolemType.Water)
		{
			spriteRenderer.sprite = waterGolemSprite;
		}
		else if (golemType == GolemType.Air)
		{
			animator.runtimeAnimatorController = airGolem as RuntimeAnimatorController;
		}
		else if (golemType == GolemType.Earth)
		{
			animator.runtimeAnimatorController = earthGolem as RuntimeAnimatorController;
		}

		this.gameObject.GetComponent<SpriteRenderer> ().color = Color.gray;

		//set the weak element of golem
		if (golemType == GolemType.Fire)
		{
			weakElement = "Water";
		} 

		else if (golemType == GolemType.Water)
		{
			weakElement = "Earth";
		} 

		else if (golemType == GolemType.Air)
		{
			weakElement = "Fire";
		}

		else if (golemType == GolemType.Earth)
		{
			weakElement = "Air";
		}

	}

	void Update () 
	{
		if (!canAct)
		{
			gameObject.GetComponent<Collider2D> ().enabled = false;
			return;
		}
			
		gameObject.GetComponent<Collider2D>().enabled = true;
		//MoveToPlayer ();
		CheckGolemDeath ();

		if (!hit)
		{
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "PlayerBullet")
		{
			StartCoroutine ("TakeDamage", other.gameObject);
		}
	}

	void CheckGolemDeath()
	{
		if (golemHealth <= 0)
		{
			GameObject particles = (GameObject)Instantiate (enemyDeathParticles, this.transform.position, Quaternion.identity);
			particles.GetComponent<EnemyDeathParticleScript> ().SetColor ((int)golemType);
			Destroy (this.gameObject);
		}
	}

	IEnumerator TakeDamage (GameObject source)
	{
		string element = source.gameObject.GetComponent<Bullet> ().bulletElement;
		float rawDamage = source.gameObject.GetComponent<Bullet> ().totalDamage;
		float damageMultiplier = 0f;
		float damageRecieved = 0f;

		//Set Damage Multiplier
		if (element == golemType.ToString ())
		{
			damageMultiplier = 0f;
		}

		else if (element == weakElement)
		{
			damageMultiplier = 1f;
			hit = true;
			spriteRenderer.color = Color.magenta;
		}

		else
		{
			hit = true;
			damageMultiplier = 0.3f;
			spriteRenderer.color = Color.magenta;
		}

		damageRecieved = rawDamage * damageMultiplier;
		golemHealth -= damageRecieved;

		yield return new WaitForSeconds (0.1f);
		hit = false;
		spriteRenderer.color = Color.white;
	}
}
