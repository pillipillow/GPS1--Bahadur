using UnityEngine;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

public class DungeonManagerBackup : MonoBehaviour
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


	public IntRange numOfRooms = new IntRange (5, 10);
	public IntRange roomWidth = new IntRange (5, 10);
	public IntRange roomHeight = new IntRange (5, 10);
	private IntRange noOfCorridors = new IntRange (1, 4);

	public Count wallCount = new Count (5, 9);
	public GameObject door;
	private Room[] rooms;
	//	private CORRIDOR_DIRECTION corridorDirection; 
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject[] enemies;
	public GameObject[] outerWallTiles;

	private Transform boardHolder;
	private List <Vector3> gridPositions = new List <Vector3> ();


	Vector3 nextRoomOffset = Vector3.zero;
	public Vector3 spawnPoint;
	// To clear each positions on the grid and then to add a vector 3 to each position on the grid
	/*	void InitialiseList ()
	{
		gridPositions.Clear ();

		for(int x = 1; x < roomWidth-1; x++)
		{
			for(int y = 1; y < roomHeight-1; y++)
			{
				gridPositions.Add (new Vector3(x, y, 0f));
			}
		}
	}*/

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

	//Instantiates the outer walls and floor
	void BoardSetup ()
	{

		boardHolder = new GameObject ("Board").transform;

		int tempNumberOfRooms = numOfRooms.Random;
		CORRIDOR_DIRECTION previousRoomDirection = CORRIDOR_DIRECTION.TOTAL;

		do
		{
			int widthOfRoom = roomWidth.Random;
			int heightOfRoom = roomHeight.Random;
			CORRIDOR_DIRECTION[] corridorDirection = new CORRIDOR_DIRECTION[4];
			int corridorsToSpawn = 1;//noOfCorridors.Random;
			Debug.Log (corridorsToSpawn);

			Transform roomHolder = new GameObject ("Room"+tempNumberOfRooms).transform;
			roomHolder.transform.SetParent (boardHolder);

			//nextRoomOffset += new Vector3 (-widthOfRoom/2 * 0.64f, -heightOfRoom/2 * 0.64f, 0f); 

			for (int i = 0; i < corridorsToSpawn; i++)
			{
				if(previousRoomDirection == CORRIDOR_DIRECTION.TOTAL)
				{
					corridorDirection [i] = RandomDirection();
					Debug.Log (corridorDirection [i]);
				}
				else
				{
					corridorDirection [i] = RandomDirectionBasedOnPreviousRoom(previousRoomDirection);
					Debug.Log (corridorDirection [i]);
				}
			}

			for (int x = -1; x < widthOfRoom + 1; x++)
			{

				for (int y = -1; y < heightOfRoom + 1; y++)
				{
					GameObject toInstantiate = floorTiles [Random.Range (0, floorTiles.Length)];

					//Outer walls
					if (x == -1 || x == widthOfRoom || y == -1 || y == heightOfRoom)
						toInstantiate = outerWallTiles [Random.Range (0, outerWallTiles.Length)];

					GameObject instance = Instantiate (toInstantiate, new Vector3 (nextRoomOffset.x + x * 0.64f, nextRoomOffset.y + y * 0.64f, 0f), Quaternion.identity) as GameObject;

					//Setting the hierarchy for neatness
					instance.transform.SetParent (roomHolder);
				}
			}

			if(previousRoomDirection == CORRIDOR_DIRECTION.TOTAL)
			{	
				spawnPoint = new Vector3 (widthOfRoom / 2 * 0.64f, heightOfRoom / 2 * 0.64f, 0f);
			}

			//! HAXXXXX
			previousRoomDirection = corridorDirection[0];

			CalculateNextRoomOffsetPosition(previousRoomDirection, roomWidth.Max, roomHeight.Max);

			tempNumberOfRooms--;
		} while(tempNumberOfRooms > 0);
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

	//Function to get a random position. Not used for now, will use later on when spawning mosnters or walls
	/*Vector3 RandomPosition ()
	{
		int randomIndex = Random.Range (0, gridPositions.Count);

		Vector3 randomPosition = gridPositions[randomIndex];

		gridPositions.RemoveAt (randomIndex);

		return randomPosition;
	}


	//Instantiates an array of objects
	void LayoutObjectAtRandom (GameObject[] tileArray, int minimum, int maximum)
	{
		int objectCount = Random.Range (minimum, maximum+1);

		for(int i = 0; i < objectCount; i++)
		{
			Vector3 randomPosition = RandomPosition();

			GameObject tileChoice = tileArray[Random.Range (0, tileArray.Length)];

			Instantiate(tileChoice, randomPosition, Quaternion.identity);
		}
	}*/


	//Initializes the level
	public void SetupScene (int level)
	{
		BoardSetup ();
		//InitialiseList ();

		//LayoutObjectAtRandom (wallTiles, wallCount.minimum, wallCount.maximum);

		//As difficulty increases, more enemies
		//int enemyCount = (int)Mathf.Log(level, 2f);

		//LayoutObjectAtRandom (enemies, enemyCount, enemyCount);

		//Instantiate (door, new Vector3 (columns/2, rows/2 - 2, 0f), Quaternion.identity);
	}
}
