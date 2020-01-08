using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GolemWayFinderScript : MonoBehaviour {

	/*public float minDistance;
	public LayerMask raycastLayer;
	public float raycastLength;

	private Vector3 finalVector;
	private Vector3 prevPos;
	private Vector3 dir;
	private Transform player;
	private bool avoiding;*/

	private GolemBehaviourScript golemBehaviour;
	Vector3 nextPosition = Vector3.zero;
	Animator animator;

	void Start () 
	{
		golemBehaviour = gameObject.GetComponentInParent<GolemBehaviourScript>();
		animator = GetComponent<Animator>();
		//player = Player.Instance.transform;
		nextPosition = LevelManagerScript.Instance.GeneratePath(this.gameObject, Player.Instance.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (!golemBehaviour.canAct)
		{
			return;
		}
		else
		{
			if (!CheckIfInRange ())
			{
				Vector3 distance = nextPosition - this.GetComponent<Collider2D>().bounds.center;

				this.transform.Translate (distance.normalized * (golemBehaviour.golemMovementSpeed * Time.deltaTime));
				//Debug.Log ("distance : " + distance);

				if (distance.magnitude < 0.01f * golemBehaviour.golemMovementSpeed / 0.7f)
				{
					//animator.Play ("waterGolemWalk");
					nextPosition = /*LevelManagerScript.Instance.GenerateNextNode();*/ LevelManagerScript.Instance.GeneratePath (this.gameObject, Player.Instance.gameObject);
				}
			}
			else
			{
				return;
			}
		
			/*prevPos = golemBehaviour.transform.position;

			finalVector = SeekPlayer ();
			finalVector += AvoidObstacles ();

			golemBehaviour.transform.position += finalVector;
			//print ("final vector: " + finalVector);
			dir = golemBehaviour.transform.position - prevPos;
			//print ("dir: " + dir.x + " " + dir.y);*/
		}

	}

	bool CheckIfInRange()
	{
		Vector3 direction = Player.Instance.transform.position - this.transform.position;

		RaycastHit2D[] hit = Physics2D.RaycastAll (this.GetComponent<Collider2D>().bounds.center, direction.normalized, golemBehaviour.golemMinimumProximity);
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
	/*
	Vector3 AvoidObstacles()
	{
		Vector3 avoidingForce = new Vector3 (0, 0, 0);
		//Vector3 dir = player.position - this.transform.position;

		RaycastHit2D hit;

		hit = Physics2D.Raycast (this.transform.position, dir.normalized, raycastLength, raycastLayer);
		Debug.DrawRay (this.transform.position, dir.normalized * raycastLength, Color.green);

		if (hit)
		{
			if (hit.transform)
			{
				if (hit.transform.tag == "Wall")
				{
					//avoiding = true;
					Vector3 distance = (Vector2)this.transform.position - hit.point;

					if (Mathf.Abs (distance.x) > Mathf.Abs (distance.y))
					{
						avoidingForce.x = 5.0f;
						//finalVector.y = 0.0f;
					}
					else
					{
						//finalVector.x = 0.0f;
						avoidingForce.y = 5.0f;
					}
					//avoidingForce.x = this.transform.position.x - hit.point.x;
					//avoidingForce.y = this.transform.position.y - hit.point.y;

					//print ("hit aaaaaaa : " + avoidingForce + " " + avoidingForce.normalized + " " + (golemBehaviour.golemMovementSpeed * Time.deltaTime));
					avoidingForce = avoidingForce.normalized * (golemBehaviour.golemMovementSpeed * Time.deltaTime);
					//Debug.Log ("hit : " + avoidingForce.x + " " + avoidingForce.y);
				}
				else
				{
					//avoiding = false;
					avoidingForce = Vector3.zero;
				}
			}
		}
			
		return avoidingForce;
	}

	Vector3 SeekPlayer()
	{
		

		//print ("seeking");
		Vector3 direction = player.position - this.transform.position;
		Vector3 steeringForce = direction.normalized * (golemBehaviour.golemMovementSpeed * Time.deltaTime);

		return steeringForce;
	}*/
}
