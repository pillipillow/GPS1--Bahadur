using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackPlayerScript : MonoBehaviour 
{
	public LayerMask hittingWalls;

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (Player.Instance.playerKnockbackEffect == false)
			{
				float knockbackDistance = 1.5f;
				Vector3 direction = other.transform.position - transform.position;
				direction.Normalize();
				transform.LookAt (new Vector3 (transform.position.x, transform.position.y, 1.0f), new Vector3 (direction.x, direction.y, 0.0f));
				Vector3 knockbackPosition = transform.position + (transform.up * knockbackDistance);
				RaycastHit2D hit = Physics2D.Raycast (other.transform.position, knockbackPosition - other.transform.position, knockbackDistance, hittingWalls);

				if (hit)
				{
					other.transform.position = Vector3.Lerp (transform.position, hit.point, 10f);
				}
				else
				{
					other.transform.position = Vector3.Lerp (transform.position, knockbackPosition, 10f);
				}

				this.transform.rotation = other.transform.rotation;
				Player.Instance.playerKnockbackEffect = true;
			}
		}
	}
}
