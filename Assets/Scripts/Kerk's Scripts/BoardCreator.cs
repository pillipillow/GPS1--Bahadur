using System.Collections;
using UnityEngine;

public class BoardCreator : MonoBehaviour
{
	public enum TILE_TYPE
	{
		WALL, FLOOR,
	}


	public int columns = 100;
	public int rows = 100;
	public IntRange numRooms = new IntRange (5, 5);
	public IntRange roomWidth = new IntRange (5, 5);
	public IntRange roomHeight = new IntRange (5, 5);   
	public IntRange corridorLength = new IntRange (5, 5);
	public GameObject[] floorTiles;
	public GameObject[] wallTiles;
	public GameObject player;

	private TILE_TYPE[][] tiles;
	private Room[] rooms;
	private Corridor[] corridors;
	private GameObject boardHolder;

	private void Start ()
	{
		boardHolder = new GameObject("BoardHolder");

		SetupTilesArray ();

		CreateRoomsAndCorridors ();

		SetTilesValuesForRooms ();
		SetTilesValuesForCorridors ();

		InstantiateTiles ();
	}


	void SetupTilesArray ()
	{
		tiles = new TILE_TYPE[columns][];

		for (int i = 0; i < tiles.Length; i++)
		{
			tiles[i] = new TILE_TYPE[rows];
		}
	}


	void CreateRoomsAndCorridors ()
	{
		rooms = new Room[numRooms.Random];

		corridors = new Corridor[rooms.Length - 1];

		rooms[0] = new Room ();
		corridors[0] = new Corridor ();

		rooms[0].SetupRoom(roomWidth, roomHeight, columns, rows);

		corridors[0].SetupCorridor(rooms[0], corridorLength, roomWidth, roomHeight, columns, rows, true);

		for (int i = 1; i < rooms.Length; i++)
		{
			rooms[i] = new Room ();

			rooms[i].SetupRoom (roomWidth, roomHeight, columns, rows, corridors[i - 1]);

			if (i < corridors.Length)
			{
				corridors[i] = new Corridor ();

				corridors[i].SetupCorridor(rooms[i], corridorLength, roomWidth, roomHeight, columns, rows, false);
			}

			if (i == rooms.Length *.5f)
			{
				Vector3 playerPos = new Vector3 (rooms[i].xPos, rooms[i].yPos, 0);
				Instantiate(player, playerPos, Quaternion.identity);
			}
		}

	}


	void SetTilesValuesForRooms ()
	{
		for (int i = 0; i < rooms.Length; i++)
		{
			Room currentRoom = rooms[i];

			for (int j = 0; j < currentRoom.roomWidth; j++)
			{
				int xCoord = currentRoom.xPos + j;

				for (int k = 0; k < currentRoom.roomHeight; k++)
				{
					int yCoord = currentRoom.yPos + k;

					tiles[xCoord][yCoord] = TILE_TYPE.FLOOR;
				}
			}
		}
	}


	void SetTilesValuesForCorridors ()
	{
		// Go through every corridor...
		for (int i = 0; i < corridors.Length; i++)
		{
			Corridor currentCorridor = corridors[i];

			// and go through it's length.
			for (int j = 0; j < currentCorridor.corridorLength; j++)
			{
				// Start the coordinates at the start of the corridor.
				int xCoord = currentCorridor.startXPos;
				int yCoord = currentCorridor.startYPos;

				// Depending on the direction, add or subtract from the appropriate
				// coordinate based on how far through the length the loop is.
				switch (currentCorridor.direction)
				{
				case DIRECTION.NORTH:
					yCoord += j;
					break;
				case DIRECTION.EAST:
					xCoord += j;
					break;
				case DIRECTION.SOUTH:
					yCoord -= j;
					break;
				case DIRECTION.WEST:
					xCoord -= j;
					break;
				}

				tiles[xCoord][yCoord] = TILE_TYPE.FLOOR;
			}
		}
	}


	void InstantiateTiles ()
	{
		for (int i = 0; i < tiles.Length; i++)
		{
			for (int j = 0; j < tiles[i].Length; j++)
			{

				if (tiles [i] [j] == TILE_TYPE.FLOOR) 
				{
					InstantiateFromArray (floorTiles, i, j);
				}

				else if (tiles[i][j] == TILE_TYPE.WALL)
				{
					InstantiateFromArray (wallTiles, i, j);
				}
			}
		}
	}





	void InstantiateFromArray (GameObject[] prefabs, float xCoord, float yCoord)
	{
		int randomIndex = Random.Range(0, prefabs.Length);

		Vector3 position = new Vector3(xCoord, yCoord, 0f);

		GameObject tileInstance = Instantiate(prefabs[randomIndex], position, Quaternion.identity) as GameObject;

		tileInstance.transform.parent = boardHolder.transform;
	}
}