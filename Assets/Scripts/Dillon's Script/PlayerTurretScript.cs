using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTurretScript : MonoBehaviour 
{
	[HideInInspector] public string turretElement;
	public GameObject TurretBullet;
	public float turretLifeSpan;
	public float turretHealth;
	public float turretShootRate;

	private Transform targetedEnemy;
	private float turretShootRateStorage;
	private bool startFindEnemy;

	void Start () 
	{
		turretElement = Player.Instance.element;
		startFindEnemy = true;
		turretShootRateStorage = turretShootRate;

		if (turretElement == "Fire")
		{
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		}
		if (turretElement == "Air")
		{
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
		}
		if (turretElement == "Earth")
		{
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
		}
		if (turretElement == "Water")
		{
			this.gameObject.GetComponent<SpriteRenderer> ().color = Color.blue;
		}
	}

	void Update () 
	{
		TurretLifeSpan ();
		TurretHealthManager ();
		ShootAtTarget ();
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "EnemyProjectile")
		{
			Destroy (other.gameObject);
			turretHealth --;
		}
	}

	void TurretHealthManager ()
	{
		if (turretHealth == 0)
		{
			Destroy (this.gameObject);
		}
	}

	void TurretLifeSpan ()
	{
		if (turretLifeSpan > 0)
		{
			turretLifeSpan -= Time.deltaTime;
		}
		if (turretLifeSpan <= 0) 
		{
			Destroy (this.gameObject);
		}
	}

	void ShootAtTarget ()
	{
		if (startFindEnemy == false)
		{
			if (turretShootRate > 0)
			{
				turretShootRate -= Time.deltaTime;
			}
			if (turretShootRate <= 0)
			{
				startFindEnemy = true;
			}
		}
		else if (startFindEnemy == true)
		{
			ScanForTarget();

			if (targetedEnemy != null)
			{
				Vector3 direction = targetedEnemy.transform.position - transform.position;
				direction.Normalize();
				transform.LookAt(new Vector3(transform.position.x, transform.position.y, 1.0f), new Vector3(direction.x, direction.y, 0.0f));
				GameObject go = (GameObject)Instantiate (TurretBullet, transform.position, transform.rotation);

				go.GetComponent<Bullet>().bulletElement = turretElement;
				go.GetComponent<Bullet>().isTurretBullet = true;
				go.transform.parent = this.transform;

				startFindEnemy = false;
				turretShootRate = turretShootRateStorage;
			}
		}
	}

	void ScanForTarget ()
	{
		targetedEnemy = GetNearestTaggedObject ();
	}

	Transform GetNearestTaggedObject ()
	{
		float nearestDistanceSqr = Mathf.Infinity;
		float distance;
		GameObject[] taggedGameObjects;
		taggedGameObjects = GameObject.FindGameObjectsWithTag ("Enemy");
		Transform nearestObj = null;

		foreach (GameObject obj in taggedGameObjects)
		{
			Vector3 objectPos = obj.transform.position;
			float distanceSqr = (objectPos - transform.position).sqrMagnitude;
			distance = Vector3.Distance (this.transform.position, objectPos);

			if (distanceSqr < nearestDistanceSqr && distance > 5f)
			{
				nearestObj = null;
			}

			if (distanceSqr < nearestDistanceSqr && distance <= 5f)
			{
				if (obj.gameObject.GetComponent<BoxCollider2D> ().enabled != false)
				{
					nearestObj = obj.transform;
					nearestDistanceSqr = distanceSqr;
				}
				else
				{
					nearestObj = null;
				}
			}
		}
		return nearestObj;
	}
}
