using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemShootingScript : MonoBehaviour {

	public float shootRate;
	public int airFireSuccession;
	public float airProjectilesFireRate;
	public GameObject fireProjectile;
	public GameObject airProjectile;
	public GameObject earthProjectile;
	public GameObject waterProjectile;

	public float shootTimer;
	private int shotCount;
	private Vector3 currentPos;
	private Transform player;
	private string element;
	private GolemBehaviourScript golemBehaviour;
	private Vector2 direction;

	Animator animator;

	// Use this for initialization
	void Start () {

		player = GameObject.FindGameObjectWithTag ("Player").transform;
		currentPos = this.transform.position;
		golemBehaviour = gameObject.GetComponent<GolemBehaviourScript>();
		element = golemBehaviour.golemType.ToString ();
		shootTimer = (shootRate/2) + Random.Range (-1, 1);
		shootRate = shootRate + Random.Range (-1, 1);

		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!golemBehaviour.canAct)
		{
			return;
		}

		currentPos = this.GetComponent<Collider2D>().bounds.center;

		if (golemBehaviour.canAttack)
		{
			shootTimer += Time.deltaTime;

			if (shootTimer >= shootRate)
			{
				if (CheckIfInRange())
				{
					StartCoroutine (Shoot ());
					shootTimer = 0;
				}
			}
		}

	}

	IEnumerator Shoot()
	{
		if (element == "Fire")
		{
			animator.SetTrigger("waterGolemShoot");

			direction = player.position - currentPos;
			direction.Normalize ();

			Vector3 firePoint = currentPos + new Vector3 (direction.x * 0.64f, direction.y * 0.64f);

			yield return new WaitForSeconds (0.5f);

			Quaternion rotation1 = Quaternion.Euler (0, 0, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);
			Quaternion rotation2 = Quaternion.Euler (0, 0, (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg + 5));
			Quaternion rotation3 = Quaternion.Euler (0, 0, (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 5));
			Quaternion rotation4 = Quaternion.Euler (0, 0, (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg + 10));
			Quaternion rotation5 = Quaternion.Euler (0, 0, (Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg - 10));

			Instantiate (fireProjectile, firePoint, rotation1);
			Instantiate (fireProjectile, firePoint, rotation2);
			Instantiate (fireProjectile, firePoint, rotation3);
			Instantiate (fireProjectile, firePoint, rotation4);
			Instantiate (fireProjectile, firePoint, rotation5);

		}

		if (element == "Water")
		{
			animator.SetTrigger("waterGolemShoot");
			direction = player.position - currentPos;
			direction.Normalize ();

			Vector3 firePoint = currentPos + new Vector3 (direction.x * 0.64f, direction.y * 0.64f);

			Quaternion rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);

			yield return new WaitForSeconds (0.5f);

			Instantiate (waterProjectile, firePoint, rotation);
		}
			
		if (element == "Air")
		{
			animator.SetTrigger("waterGolemShoot");

			yield return new WaitForSeconds (0.5f);

			for (int i = 1; i <= airFireSuccession; i++)
			{
				direction = player.position - currentPos;
				direction.Normalize ();

				Vector3 firePoint = currentPos + new Vector3 (direction.x * 0.64f, direction.y * 0.64f);

				Quaternion rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);

				Instantiate (airProjectile, firePoint, rotation);

				yield return new WaitForSeconds (airProjectilesFireRate);
			}
		}

		if (element == "Earth")
		{
			animator.SetTrigger("waterGolemShoot");

			direction = player.position - currentPos;
			direction.Normalize ();

			Vector3 firePoint = currentPos + new Vector3 (direction.x * 0.64f, direction.y * 0.64f);

			yield return new WaitForSeconds (0.5f);

			Quaternion rotation = Quaternion.Euler (0, 0, Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg);

			Instantiate (earthProjectile, firePoint, rotation);
		}
	}

	bool CheckIfInRange()
	{
		Vector3 direction = Player.Instance.transform.position - this.transform.position;

		RaycastHit2D[] hit = Physics2D.RaycastAll (this.GetComponent<Collider2D>().bounds.center, direction.normalized, golemBehaviour.golemAttackRange);
		Debug.DrawRay (this.GetComponent<Collider2D>().bounds.center, direction.normalized, Color.green);

		for (int i = 0; i < hit.Length; i++)
		{
			if (hit [i].transform.tag == "Wall")
			{
				break;
			}

			else if (hit [i].transform.tag == "Player")
			{
				return true;
			}
		}

		return false;
	}
		
}
