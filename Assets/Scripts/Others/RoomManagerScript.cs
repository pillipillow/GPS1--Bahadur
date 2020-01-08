using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomManagerScript : MonoBehaviour {

	public enum RoomType {ROOM_NORMAL = 0, ROOM_COLLAPSED, ROOM_MINIBOSS, ROOM_BOSS};
	public RoomType roomType;

	public bool doorLocked;
	public bool autoLockDoors;
	public bool unlockOnTriggerExit;
	public bool isTutorial;
	//private int spawnOptionCount;

	private List<GameObject> spawnerList = new List<GameObject> ();
	public bool triggered;

	public GameObject spawnPoint;
	public GameObject boss;
	public GameObject djinn;
	public GameObject portal;

	[HideInInspector] public bool bossDefeated;
	[HideInInspector] public bool upgradeDone;
	//[HideInInspector] public int spawnOption;

	private int spawnSelect;

	void Start ()
	{
		doorLocked = false;

		if (roomType == RoomType.ROOM_NORMAL)
		{
			PrepareSpawnerList();
		}

		if (!isTutorial)
		{
			spawnSelect = Random.Range (0, 3);
		}
	}

	void Update()
	{
		if (!triggered)
		{
			return;
		}

		if (roomType == RoomType.ROOM_BOSS)
		{
			if (boss == null)
			{
				bossDefeated = true;
			}

			if (bossDefeated)
			{
				if (isTutorial)
				{
					if (!upgradeDone)
					{
						SummonDjinn ();
						upgradeDone = true;
					}

					if (upgradeDone)
					{
						if (!djinn.activeSelf)
						{
							portal.SetActive (true);
							doorLocked = false;
						}
					}
				}
				else
				{
					portal.SetActive (true);
					doorLocked = false;
				}
			}
		}
		else if (roomType == RoomType.ROOM_NORMAL)
		{
			EnableSpawners ();

			if (spawnerList.Count > 0)
			{
				for (int i = 0; i < spawnerList.Count; i++)
				{
					if (spawnerList [i].gameObject.GetComponentInChildren<GolemSpawner> ().cleared == false)
					{
						return;
					}
				}

				doorLocked = false;
			}
		}

		else if (roomType == RoomType.ROOM_MINIBOSS)
		{
			if (boss == null)
			{
				bossDefeated = true;
			}

			if (bossDefeated)
			{
				if (!upgradeDone)
				{
					SummonDjinn ();
					upgradeDone = true;
				}

				doorLocked = false;
			}
		}

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (spawnPoint != null)
			{
				Player.Instance.checkPoint = spawnPoint.transform.position;
			}

			if (!triggered)
			{
				triggered = true;

				if (autoLockDoors)
				{
					if (roomType != RoomType.ROOM_MINIBOSS)
					{
						doorLocked = true;
					}
				}

				if (roomType == RoomType.ROOM_BOSS)
				{
					if (!isTutorial)
					{
						boss.SetActive (true);
					}
				}
			}
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (roomType == RoomType.ROOM_MINIBOSS)
			{
				if (boss)
				{
					if (!boss.GetComponent<HandBehaviourScript> ().isActive)
					{
						triggered = false;
					}
				}
			}

			if (unlockOnTriggerExit)
			{
				doorLocked = false;
			}
		}
	}
		
	void PrepareSpawnerList()
	{
		foreach (Transform child in transform)
		{
			if (child != null)
			{
				if (child.CompareTag("GolemSpawner"))
				{
					spawnerList.Add(child.gameObject);
				}
			}
		}
	}

	void EnableSpawners()
	{
		for (int i = 0; i < spawnerList.Count; i++)
		{
			if (spawnerList[i].gameObject.GetComponentInChildren<GolemSpawner>().cleared == false)
			{
				spawnerList [i].gameObject.GetComponent<GolemSpawner> ().spawnOption = spawnSelect;
				spawnerList [i].SetActive (true);
				return;
			}
		}
	}

	void SummonDjinn()
	{
		GameObject player = Player.Instance.gameObject;
		Vector3 teleportPos;

		do
		{
			teleportPos = (Random.insideUnitCircle * 1) + (Vector2)player.transform.position;
		} while (CheckIfOccupied (teleportPos, player));

		djinn.transform.position = teleportPos;
		djinn.SetActive (true);
	}

	bool CheckIfOccupied(Vector3 targetPos, GameObject cols)
	{
		Collider2D[] col = Physics2D.OverlapBoxAll ((Vector2)targetPos, cols.GetComponent<Collider2D>().bounds.size,
			cols.transform.localEulerAngles.z);

		for (int i = 0; i < col.Length; i++)
		{
			if (col [i].gameObject.tag == "Wall")
			{
				return true;
			}

			if (col [i].gameObject.tag == "Portal")
			{
				if (col [i].gameObject.transform.position == targetPos)
				{
					return true;
				}
			}
		}
		return false;
	}
}
