using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Spawn
{
	public enum SpawnElement {Fire,Water,Air,Earth,Neutral};
	public SpawnElement[] spawnElement;
}

public class GolemSpawner : MonoBehaviour {

	public Spawn[] spawn;
	public GameObject golem;
	public GameObject scarab;
	public float spawnRange;
	public float spawnDelay;
	public bool cleared;

	private SpriteRenderer spriteRenderer;
	private Color tempColor;
	public bool spawned;
	private float spawnTimer;
	private Spawn[] enemy;

	[HideInInspector]
	public int spawnOption;

	void Start()
	{
		/*spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
		tempColor = spriteRenderer.color;
		tempColor.a = 255f;
		spriteRenderer.color = tempColor;
		*/
	}

	void Update()
	{
		//Debug.Log (spriteRenderer.color.a);
		spawnTimer += Time.deltaTime;

		if (spawnTimer >= spawnDelay)
		{
			if (!spawned)
			{
				spawned = true;

				for (int i = 0; i < spawn[spawnOption].spawnElement.Length; i++)
				{
					SpawnGolem (spawn[spawnOption].spawnElement[i]);
				}
			}

		}

		if (spawnTimer >= spawnDelay + 0.5f)
		{
			if (spawned)
			{
				spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();
				tempColor = spriteRenderer.color;
				tempColor.a = 0f;
				spriteRenderer.color = tempColor;
			}
		}

		if (spawned)
		{
			if (this.transform.childCount <= 0)
			{
				cleared = true;
			}
		}
	}

	void SpawnGolem(Spawn.SpawnElement element)
	{
		Vector3 spawnPos;
		Vector2 originPoint = gameObject.transform.position;

		do
		{
			spawnPos = (Random.insideUnitCircle * spawnRange) + originPoint;
		} while(checkIfOccupied (spawnPos,golem));


		if (element == Spawn.SpawnElement.Neutral)
		{
			GameObject summonedScarab = (GameObject)Instantiate (scarab, spawnPos, Quaternion.identity);
		}

		GameObject summonedgolem = (GameObject)Instantiate (golem, spawnPos, Quaternion.identity);

		if (element == Spawn.SpawnElement.Air)
		{
			summonedgolem.GetComponent<GolemBehaviourScript> ().golemType = GolemBehaviourScript.GolemType.Air;
		}
		else if (element == Spawn.SpawnElement.Earth)
		{
			summonedgolem.GetComponent<GolemBehaviourScript> ().golemType = GolemBehaviourScript.GolemType.Earth;
		}
		else if (element == Spawn.SpawnElement.Water)
		{
			summonedgolem.GetComponent<GolemBehaviourScript> ().golemType = GolemBehaviourScript.GolemType.Water;
		}
		else if (element == Spawn.SpawnElement.Fire)
		{
			summonedgolem.GetComponent<GolemBehaviourScript> ().golemType = GolemBehaviourScript.GolemType.Fire;
		}

		summonedgolem.transform.parent = this.transform;
		summonedgolem.GetComponent<GolemBehaviourScript> ().canAct = true;
	}

	/*
	bool checkIfOccupied(Vector3 targetPos)
	{
		List<GameObject> obstacleList = new List<GameObject>();

		foreach (GameObject other in GameObject.FindGameObjectsWithTag ("Enemy"))
		{
			obstacleList.Add (other);
		}

		foreach (GameObject other in GameObject.FindGameObjectsWithTag ("Wall"))
		{
			obstacleList.Add (other);
		}

		foreach (GameObject other in obstacleList)
		{
			if (other.transform.position == targetPos)
			{
				print ("stacked");
				return true;
			}
		}

		return false;
	}
	*/

	bool checkIfOccupied(Vector3 targetPos, GameObject cols)
	{
		Collider2D[] col = Physics2D.OverlapBoxAll ((Vector2)targetPos, cols.GetComponent<Collider2D>().bounds.size,
			cols.transform.localEulerAngles.z);

		for (int i = 0; i < col.Length; i++)
		{
			if (col [i].gameObject.tag == "Wall")
			{
				Debug.Log ("occupied");
				return true;
			}

			if (col [i].gameObject.tag == "Enemy")
			{
				if (col [i].gameObject.transform.position == targetPos)
				{
					Debug.Log ("occupied");
					return true;
				}
			}
		}
		return false;
	}

}
