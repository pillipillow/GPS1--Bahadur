using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DungeonManagerNew : MonoBehaviour 
{
	public enum CORRIDOR_DIRECTION
	{
		NORTH,
		EAST,
		SOUTH,
		WEST,
		TOTAL
	}

	[Serializable]
	public class Count
	{
		public int minimum;
		public int maximum;

		public Count (int min, int max)
		{
			minimum = min;
			maximum = max;
		}
	}


	[HideInInspector]
	public int level = 1;

	private int numMobRoomsToSpawn = 2;
	private Transform levelLayout;
	private int roomsSpawned = 0;
	public List<GameObject> startRooms;
	public List<GameObject> mobRooms;
	public List<GameObject> miniBossRoom;
	public List<GameObject> bossRoom;
	private RoomProperties prevRoomProperties;
	private GameObject prevRoom;
	public Vector3 nextRoomOffset;
	private float heightOffset;
	private float widthOffset;

	public GameObject instantiateLevel;

	private static DungeonManagerNew mInstance;
	public static DungeonManagerNew Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("DungeonManager");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("_DungeonManager");
					mInstance = obj.AddComponent<DungeonManagerNew>();
					obj.tag = "DungeonManager";
				}
				else
				{
					mInstance = tempObject.GetComponent<DungeonManagerNew>();
				}
			}
			return mInstance;
		}
	}

	private CORRIDOR_DIRECTION RandomDirection()
	{
		return (CORRIDOR_DIRECTION)(UnityEngine.Random.Range(0, Enum.GetNames(typeof(CORRIDOR_DIRECTION)).Length - 1));
	}

	private CORRIDOR_DIRECTION RandomDirectionBasedOnPreviousRoom(CORRIDOR_DIRECTION previousRoomDirection)
	{
		if(previousRoomDirection == CORRIDOR_DIRECTION.NORTH)
		{
			previousRoomDirection = CORRIDOR_DIRECTION.SOUTH;
		}
		else if(previousRoomDirection == CORRIDOR_DIRECTION.EAST)
		{
			previousRoomDirection = CORRIDOR_DIRECTION.WEST;
		}
		else if(previousRoomDirection == CORRIDOR_DIRECTION.SOUTH)
		{
			previousRoomDirection = CORRIDOR_DIRECTION.NORTH;
		}
		else if(previousRoomDirection == CORRIDOR_DIRECTION.WEST)
		{
			previousRoomDirection = CORRIDOR_DIRECTION.EAST;
		}

		CORRIDOR_DIRECTION nextRoomDirection;
		do
		{
			nextRoomDirection = RandomDirection();
		}
		while(nextRoomDirection == previousRoomDirection);

		return nextRoomDirection;
	}

	void CalculateNextRoomOffsetPosition(CORRIDOR_DIRECTION previousRoomDirection, int roomWidth, int roomHeight)
	{
		if(previousRoomDirection == CORRIDOR_DIRECTION.NORTH)
		{
			nextRoomOffset += new Vector3 (0f, roomWidth * 0.64f, 0f); 
		}
		else if(previousRoomDirection == CORRIDOR_DIRECTION.EAST)
		{
			nextRoomOffset += new Vector3 (roomWidth * 0.64f, 0f, 0f);
		}
		else if(previousRoomDirection == CORRIDOR_DIRECTION.SOUTH)
		{
			nextRoomOffset += new Vector3 (0f, -roomHeight * 0.64f, 0f);
		}
		else if(previousRoomDirection == CORRIDOR_DIRECTION.WEST)
		{
			nextRoomOffset += new Vector3 (-roomWidth * 0.64f, 0f, 0f);
		}
		Debug.Log ("previousRoomDirection : " + previousRoomDirection + " nextRoomOffset : " + nextRoomOffset);
	}


	void SpawnStartRoom()
	{
		GameObject toInstantiate;
		int randomStartRoomIndex = Random.Range(0, startRooms.Count-1);
		toInstantiate = Instantiate(startRooms[randomStartRoomIndex], new Vector3 (0,0,0), Quaternion.identity);
		prevRoomProperties = toInstantiate.GetComponent<RoomProperties>();
		widthOffset = prevRoomProperties.roomHeight;
		heightOffset = prevRoomProperties.roomWidth;
		Debug.Log (prevRoomProperties);
		Debug.Log (heightOffset);
		Debug.Log (widthOffset);
		roomsSpawned++;
	}

	void SpawnMobRooms()
	{
		GameObject toInstantiate;
		GameObject prevRoom;
		float test;

		toInstantiate = Instantiate(mobRooms[0], new Vector3 (widthOffset, heightOffset , 0), Quaternion.identity);
		test = toInstantiate.GetComponent<RoomProperties>().roomHeight / 2;
		toInstantiate.transform.position = 	new Vector3 (widthOffset, (heightOffset/2) - test , 0);
		Debug.Log("I am a" + test);
		prevRoom = mobRooms[0];
		prevRoomProperties = toInstantiate.GetComponent<RoomProperties>();
		prevRoomProperties.direction = RoomProperties.CORRIDOR_DIRECTION.EAST;
		Debug.Log(prevRoomProperties.direction);
		int numMobRoomsSpawned = 0;

		//do
		//{
			if (prevRoomProperties.direction == RoomProperties.CORRIDOR_DIRECTION.NORTH)
			{
				int index = -1;

				for (int i=0; i<mobRooms.Count; i++)
				{
					if (mobRooms[i] != mobRooms[0])
					{
						if ((mobRooms[i].GetComponent<RoomProperties>().direction & RoomProperties.CORRIDOR_DIRECTION.SOUTH) == RoomProperties.CORRIDOR_DIRECTION.SOUTH)
						{
							Debug.Log("IM IN");
							index = i;
							break;
						}
					}
				}

				if (index != -1)
				{
;
					toInstantiate = Instantiate(mobRooms[index], new Vector3 (0, -heightOffset ,0), Quaternion.identity);
					numMobRoomsSpawned++;
				}
			}

			else if (prevRoomProperties.direction == RoomProperties.CORRIDOR_DIRECTION.EAST)
			{
				int index = -1;

				for (int i=0; i<mobRooms.Count; i++)
				{
					if (mobRooms[i] != mobRooms[0])
					{
						if ((mobRooms[i].GetComponent<RoomProperties>().direction & RoomProperties.CORRIDOR_DIRECTION.WEST) == RoomProperties.CORRIDOR_DIRECTION.WEST)
						{
							index = i;
							break;
						}
					}
				}

				if (index != -1)
				{
					toInstantiate = Instantiate(mobRooms[index], new Vector3 (widthOffset, 0 ,0), Quaternion.identity);
					numMobRoomsSpawned++;
				}
				
			}

			else if (prevRoomProperties.direction == RoomProperties.CORRIDOR_DIRECTION.SOUTH)
			{
				int index = -1;

				for (int i=0; i<mobRooms.Count; i++)
				{
					if (mobRooms[i] != mobRooms[0])
					{
						if ((mobRooms[i].GetComponent<RoomProperties>().direction & RoomProperties.CORRIDOR_DIRECTION.NORTH) == RoomProperties.CORRIDOR_DIRECTION.NORTH)
						{
							index = i;
							break;
						}
					}
				}

				if (index != -1)
				{
					toInstantiate = Instantiate(mobRooms[index], new Vector3 (0, heightOffset ,0), Quaternion.identity);
					numMobRoomsSpawned++;
				}

			}

			else if (prevRoomProperties.direction == RoomProperties.CORRIDOR_DIRECTION.WEST)
			{
				int index = -1;

				for (int i=0; i<mobRooms.Count; i++)
				{
					if (mobRooms[i] != mobRooms[0])
					{
						if ((mobRooms[i].GetComponent<RoomProperties>().direction & RoomProperties.CORRIDOR_DIRECTION.EAST) == RoomProperties.CORRIDOR_DIRECTION.EAST)
						{
							index = i;
							break;
						}
					}

				}

				if (index != -1)
				{
					toInstantiate = Instantiate(mobRooms[index], new Vector3 (-widthOffset, 0 ,0), Quaternion.identity);
					numMobRoomsSpawned++;
				}

			}


		//}while (numMobRoomsSpawned != numMobRoomsToSpawn);

	}

	void SpawnMiniBossRoom()
	{
		int randomMiniBossRoomIndex = Random.Range(0, miniBossRoom.Count-1);
		Instantiate(miniBossRoom[randomMiniBossRoomIndex], new Vector3 (0,0,0), Quaternion.identity);
		roomsSpawned++;
	}

	void SpawnBossRoom()
	{
		// Check Direction

	}	

	void SpawnLevel()
	{
		Debug.Log("HEYEY");
		Instantiate(instantiateLevel, new Vector3 (0,0,0), Quaternion.identity);

	}

	public void LevelSetup()
	{
		levelLayout = new GameObject ("Level " + level).transform;

		//SpawnStartRoom();
		//SpawnMobRooms();
		//SpawnMiniBossRoom();
		//SpawnBossRoom();
	}

	void Start () 
	{
		SpawnLevel();
	}
	
	void Update () 
	{
		
	}
}
