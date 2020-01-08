using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BounceAreaScript : MonoBehaviour 
{
	[HideInInspector] public List<GameObject> enemyStorage = new List<GameObject> ();
	public float projectileSpeed;

	private Transform targetedEnemy;
	private bool firstEnemy = true;
	private bool startFindEnemy = true;

	void Update ()
	{
		if (firstEnemy == false)
		{
			if (startFindEnemy == true)
			{
				ScanForTarget();
				startFindEnemy = false;
			}

			if (targetedEnemy == null)
			{
				Destroy (this.gameObject);
			}
			else if (targetedEnemy != null)
			{
				Vector3 direction = targetedEnemy.transform.position - transform.position;
				direction.Normalize();
				Debug.DrawLine(transform.position, transform.position + direction, Color.white);
				transform.LookAt(new Vector3(transform.position.x, transform.position.y, 1.0f), new Vector3(-direction.y, direction.x, 0.0f));
				transform.Translate (Vector3.right * Time.deltaTime * projectileSpeed);
			}

			if (targetedEnemy == this.transform)
			{
				Destroy (this.gameObject);
			}
		}
	}

	void OnTriggerEnter2D (Collider2D enemy) 
	{
		if (enemy.gameObject.tag == "Enemy")
		{
			if (!enemyStorage.Contains (enemy.gameObject))
			{
				transform.position = enemy.gameObject.transform.position;
				enemyStorage.Add (enemy.gameObject);

				if (firstEnemy == true)
				{
					firstEnemy = false;
				}

				if (firstEnemy == false)
				{
					startFindEnemy = true;
				}
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
		Transform closestObj = null;

		foreach (GameObject obj in taggedGameObjects)
		{
			Vector3 objectPos = obj.transform.position;
			float distanceSqr = (objectPos - transform.position).sqrMagnitude;
			distance = Vector3.Distance (this.transform.position, objectPos);

			if (distanceSqr < nearestDistanceSqr && objectPos != this.transform.position && distance > 5f)
			{
				nearestObj = null;
			}

			if (distanceSqr < nearestDistanceSqr && objectPos != this.transform.position && distance <= 5f)
			{
				if (!enemyStorage.Contains (obj.gameObject) && obj.gameObject.GetComponent<BoxCollider2D> ().enabled != false)
				{
					nearestObj = obj.transform;
					nearestDistanceSqr = distanceSqr;
				}
				else
				{
					nearestObj = null;
				}
			}

			if (nearestObj != null)
			{
				closestObj = nearestObj;
			}
			else if (nearestObj == closestObj)
			{
				closestObj = null;
			}
		}
		return closestObj;
	}
}
