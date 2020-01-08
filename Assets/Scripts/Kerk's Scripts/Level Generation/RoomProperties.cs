using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomProperties : MonoBehaviour 
{

	[System.Flags]
	public enum CORRIDOR_DIRECTION
	{
		NORTH = 1,
		EAST = 2,
		SOUTH = 4,
		WEST = 8
	}

	public enum ROOM_TYPE
	{
		STARTROOM = 1,
		MOBSMALL = 2,
		MOBMEDIUM = 4,
		MOBBIG = 8,
		MINIBOSS = 16,
		BOSS = 32
	}

	[EnumButtons]
	public CORRIDOR_DIRECTION direction;
	public ROOM_TYPE roomType;
	public float roomHeight;
	public float roomWidth;
	public string apuapu;
	bool allCorridorsConnected;
	GameObject vertexHolders; 
	GameObject topLeft; 
	GameObject topRight;
	GameObject bottomLeft;
	private BoxCollider2D boundingBox;



	public float GetRoomHeight()
	{
		boundingBox = this.GetComponent<BoxCollider2D>();
		roomHeight = boundingBox.bounds.size.y;
		return roomHeight;
	}


	public float GetRoomWidth()
	{
		boundingBox = this.GetComponent<BoxCollider2D>();
		roomWidth = boundingBox.bounds.size.x;
		return roomWidth;
	}



	void Awake () {

		Debug.Log("Wassup i am " + this);
		apuapu = "walao";
		allCorridorsConnected = false;
		roomHeight = GetRoomHeight();
		roomWidth = GetRoomWidth();
		//Debug.Log(roomHeight);
		//Debug.Log(roomWidth);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
