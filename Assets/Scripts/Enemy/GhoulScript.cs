using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhoulScript : MonoBehaviour {

	public float ghoulHealth;
	public float timeToStartMoving;
	private bool canAct;
	public float inactiveTimer;

	public GameObject enemyDeathParticles;
	public GameObject projectile;
	public float fireAttackCooldown;
	public float waterAttackCooldown;
	public float earthAttackCooldown;
	public float airAttackCooldown;

	private float fireAttackTimer;
	private float waterAttackTimer;
	private float earthAttackTimer;
	private float airAttackTimer;

	private bool nextSkillReady;
	private int attackSequenceCount;

	private bool hit = false;
	private float damageRecieved;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () 
	{
		canAct = false;
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		spriteRenderer.color = Color.black;

		attackSequenceCount = 1;
		hit = false;

		fireAttackTimer = fireAttackCooldown;
		waterAttackTimer = waterAttackCooldown;
		earthAttackTimer = earthAttackCooldown;
		airAttackTimer = airAttackCooldown;

		nextSkillReady = true;
		StartCoroutine (AttackSequence ());
	}
		
	void Update () 
	{
		if (!canAct)
		{
			inactiveTimer += Time.deltaTime;

			if (spriteRenderer.color != Color.white)
			{
				byte colorRoC = System.Convert.ToByte(255 / timeToStartMoving);
				Color32 color = spriteRenderer.color;
				color.r += System.Convert.ToByte (colorRoC * Time.deltaTime);
				color.g += System.Convert.ToByte (colorRoC * Time.deltaTime);
				color.b += System.Convert.ToByte (colorRoC * Time.deltaTime);
				spriteRenderer.color = color;
			}

			if (inactiveTimer >= timeToStartMoving)
			{
				//spriteRenderer.color = Color.white;
				canAct = true;
			}
			return;
		}

		UpdateAttackTimer ();
		CheckGhoulDeath ();
	}

	IEnumerator FireAttack()
	{
		if (fireAttackTimer < fireAttackCooldown)
		{
			nextSkillReady = true;
			yield break;
		}
		nextSkillReady = false;

		fireAttackTimer = 0;

		Vector3 direction = Player.Instance.transform.position - this.transform.position;
		Vector3 attackPosition = this.transform.position + direction.normalized;
		int attackCount = Mathf.FloorToInt (direction.magnitude / direction.normalized.magnitude);
		RaycastHit2D[] hit = Physics2D.RaycastAll (this.transform.position,direction.normalized);
		Debug.Log ("Fire Attack Count: " + attackCount);


		if (attackCount > 0)
		{
			for (int i = 0; i < attackCount; i++)
			{
				for (int j = 0; j < hit.Length; j++)
				{
					if (hit [i])
					{
						//Debug.Log (hit [j].transform.tag);
						if (hit [j].transform.tag == "Wall")
						{
							break;
						}

						if (hit [j].transform.tag == "Player")
						{
							GameObject fire = (GameObject)Instantiate (projectile, attackPosition, Quaternion.identity);
							fire.gameObject.GetComponent<GhoulProjectileScript> ().ghoulProjectileElement = GhoulProjectileScript.GhoulProjectileElement.FIRE;
							attackPosition = fire.transform.position + direction.normalized;
						}
					}
				}
				yield return new WaitForSeconds (0.3f);

				if (i >= attackCount - 1)
				{
					nextSkillReady = true;
				}
			}
		}
		else
		{
			nextSkillReady = true;
		}
	}

	void AirAttack()
	{
		if (airAttackTimer < airAttackCooldown)
		{
			nextSkillReady = true;
			return;
		}
		nextSkillReady = false;

		airAttackTimer = 0;

		Vector3 spawnPos;
		Vector2 originPoint = gameObject.transform.position;

		do
		{
			float radius =this.GetComponent<Collider2D>().bounds.extents.y + 0.5f; 
			spawnPos = (Random.insideUnitCircle * radius) + originPoint;
		} while(checkIfOccupied (spawnPos, projectile));


		GameObject summon = (GameObject)Instantiate (projectile, spawnPos, Quaternion.identity);
		summon.gameObject.GetComponent<GhoulProjectileScript> ().ghoulProjectileElement = GhoulProjectileScript.GhoulProjectileElement.AIR;
		summon.transform.parent = this.transform;
		nextSkillReady = true;
	}

	IEnumerator EarthAttack()
	{
		if (earthAttackTimer < earthAttackCooldown)
		{
			nextSkillReady = true;
			yield break;
		}
		nextSkillReady = false;

		earthAttackTimer = 0;

		Vector3 teleportPos;
		Vector2 playerPos = Player.Instance.transform.position;

		do
		{
			teleportPos = (Random.insideUnitCircle * 2f + playerPos);

			do
			{
				int randomizedDirection = Random.Range (0,8);

				switch(randomizedDirection)
				{
					case (0):
						teleportPos = playerPos + new Vector2 (0,1); //north of player
						break;
					case (1):
						teleportPos = playerPos + new Vector2 (0,-1); //south of player
						break;
					case (2):
						teleportPos = playerPos + new Vector2 (1,0); //east of player
						break;
					case (3):
						teleportPos = playerPos + new Vector2 (-1,0); //west of player
						break;
					case (4):
						teleportPos = playerPos + new Vector2 (1,1); //northeast of player
						break;
					case (5):
						teleportPos = playerPos + new Vector2 (-1,1); //southwest of player
						break;
					case (6):
						teleportPos = playerPos + new Vector2 (1,-1); //northwest of player
						break;
					case (7):
						teleportPos = playerPos + new Vector2 (-1,-1); //sourtheast of player
						break;

					default:
						teleportPos = playerPos;
						break;
				}
			}while(checkIfOccupied(teleportPos, this.gameObject));

		}while(checkIfOccupied(teleportPos, this.gameObject));

		this.transform.position = teleportPos;

		yield return new WaitForSeconds (1f);

		Vector3 direction = Player.Instance.transform.position - this.transform.position;
		float angle = Mathf.Atan2 (direction.x, direction.y) * Mathf.Rad2Deg;
		print ("Angle: " + angle);

		for (int i = 0; i < 4; i++)
		{
			Vector3 rotation = new Vector3 (this.transform.position.x, this.transform.position.y, this.transform.position.z + (i * 90) + angle);
			GameObject rock = (GameObject)Instantiate (projectile, this.transform.position  , Quaternion.Euler (rotation));
			rock.gameObject.GetComponent<GhoulProjectileScript> ().ghoulProjectileElement = GhoulProjectileScript.GhoulProjectileElement.EARTH;

			if (i == 3)
			{
				nextSkillReady = true;
			}
		}
	}

	void WaterAttack()
	{
		if (waterAttackTimer < waterAttackCooldown)
		{
			nextSkillReady = true;
			return;
		}
		nextSkillReady = false;

		waterAttackTimer = 0;

		Vector3 direction = Player.Instance.transform.position - this.transform.position;
		direction.Normalize ();
		Vector3 firePoint =  this.transform.position + new Vector3 (direction.x * 0.64f, direction.y * 0.64f);
		Quaternion rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);

		GameObject water = (GameObject)Instantiate (projectile, firePoint, rotation);
		water.gameObject.GetComponent<GhoulProjectileScript> ().ghoulProjectileElement = GhoulProjectileScript.GhoulProjectileElement.WATER;

		nextSkillReady = true;
	}

	IEnumerator AttackSequence()
	{
		while (ghoulHealth > 0)
		{
			Debug.Log ("GhoulAttackSequence" + attackSequenceCount);
			if (canAct)
			{

				if (attackSequenceCount == 1)
				{
					yield return new WaitForSeconds (1f);
					StartCoroutine (EarthAttack());

					while (!nextSkillReady)
					{
						yield return null;
					}
					//StopCoroutine (EarthAttack());
					attackSequenceCount = 2;
				}
				else if (attackSequenceCount == 2)
				{
					yield return new WaitForSeconds (1f);
					StartCoroutine (FireAttack());

					while (!nextSkillReady)
					{
						yield return null;
					}
					//StopCoroutine (FireAttack());
					attackSequenceCount = 3;
				}
				else if (attackSequenceCount == 3)
				{
					yield return new WaitForSeconds (1f);
					AirAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 4;
					}
				}
				else if (attackSequenceCount == 4)
				{
					yield return new WaitForSeconds (1f);
					StartCoroutine (EarthAttack());

					while (!nextSkillReady)
					{
						yield return null;
					}
					//StopCoroutine (EarthAttack());
					attackSequenceCount = 5;
				}
				else if (attackSequenceCount == 5)
				{
					yield return new WaitForSeconds (1f);
					WaterAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 6;
					}
				}
				else if (attackSequenceCount == 6)
				{
					yield return new WaitForSeconds (1f);
					WaterAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 7;
					}
				}
				else if (attackSequenceCount == 7)
				{
					yield return new WaitForSeconds (1f);
					StartCoroutine (FireAttack ());

					while (!nextSkillReady)
					{
						yield return null;
					}
					//StopCoroutine (FireAttack());
					attackSequenceCount = 8;
				}
				else if (attackSequenceCount == 8)
				{
					yield return new WaitForSeconds (1f);
					AirAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 9;
					}
				}
				else if (attackSequenceCount == 9)
				{
					yield return new WaitForSeconds (1f);
					StartCoroutine (EarthAttack ());

					while (!nextSkillReady)
					{
						yield return null;
					}
					//StopCoroutine (EarthAttack ());
					attackSequenceCount = 10;
				}
				else if (attackSequenceCount == 10)
				{
					yield return new WaitForSeconds (1f);
					WaterAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 11;
					}
				}
				else if (attackSequenceCount == 11)
				{
					yield return new WaitForSeconds (1f);
					AirAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 12;
					}
				}
				else if (attackSequenceCount == 12)
				{
					yield return new WaitForSeconds (1f);
					StartCoroutine (EarthAttack());

					while (!nextSkillReady)
					{
						yield return null;
					}
					//StopCoroutine (EarthAttack());
					attackSequenceCount = 13;
				}
				else if (attackSequenceCount == 13)
				{
					yield return new WaitForSeconds (1f);
					WaterAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 14;
					}
				}
				else if (attackSequenceCount == 14)
				{
					yield return new WaitForSeconds (1f);
					StartCoroutine (FireAttack ());

					while (!nextSkillReady)
					{
						yield return null;
					}
					//StopCoroutine (FireAttack ());
					attackSequenceCount = 15;
				}
				else if (attackSequenceCount == 15)
				{
					yield return new WaitForSeconds (1f);
					AirAttack ();

					if (nextSkillReady == true)
					{
						attackSequenceCount = 16;
					}
				}
				else if (attackSequenceCount == 16)
				{
					yield return new WaitForSeconds (1f);

					if (nextSkillReady == true)
					{
						attackSequenceCount = 1;
					}
				}
			}

			yield return null;
		};
	}

	void UpdateAttackTimer()
	{
		fireAttackTimer += Time.deltaTime;
		waterAttackTimer += Time.deltaTime;
		earthAttackTimer += Time.deltaTime;
		airAttackTimer += Time.deltaTime;
	}

	bool checkIfOccupied(Vector3 targetPos, GameObject cols)
	{
		Collider2D[] col = Physics2D.OverlapBoxAll ((Vector2)targetPos, cols.GetComponent<Collider2D> ().bounds.size,
			                   cols.transform.localEulerAngles.z);

		for (int i = 0; i < col.Length; i++)
		{
			
			if (col [i].gameObject.tag == "Wall")
			{
				Debug.Log ("occupied");
				return true;
			}
			else if (col [i].gameObject.tag == "Enemy")
			{
				return true;
			}
		}
		return false;
	}


	IEnumerator OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "PlayerBullet")
		{
			hit = true;
			spriteRenderer.color = Color.magenta;
			damageRecieved = other.gameObject.GetComponent<Bullet>().totalDamage;

			ghoulHealth -= damageRecieved;

			yield return new WaitForSeconds (0.1f);

			hit = false;
			spriteRenderer.color = Color.white;
		}
	}

	void CheckGhoulDeath()
	{
		if (ghoulHealth <= 0)
		{
			GameObject particles = (GameObject)Instantiate (enemyDeathParticles, this.transform.position, Quaternion.identity);
			Destroy (this.gameObject);
		}
	}
		
}
