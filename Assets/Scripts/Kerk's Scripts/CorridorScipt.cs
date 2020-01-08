using UnityEngine;

public enum DIRECTION
{
	NORTH, EAST, SOUTH, WEST,
}

public class Corridor
{
	public int startXPos;         
	public int startYPos;         
	public int corridorLength;            
	public DIRECTION direction;   

	public int EndPositionX 
	{
		get
		{
			if (direction == DIRECTION.NORTH || direction == DIRECTION.SOUTH)
				return startXPos;
			if (direction == DIRECTION.EAST)
				return startXPos + corridorLength - 1;
			return startXPos - corridorLength + 1;
		}
	}


	public int EndPositionY 
	{
		get
		{
			if (direction == DIRECTION.EAST || direction == DIRECTION.WEST)
				return startYPos;
			if (direction == DIRECTION.NORTH)
				return startYPos + corridorLength - 1;
			return startYPos - corridorLength + 1;
		}
	}


	public void SetupCorridor (Room room, IntRange length, IntRange roomWidth, IntRange roomHeight, int columns, int rows, bool firstCorridor)
	{
		direction = (DIRECTION)Random.Range(0, 4);

		// Finding opposite direction
		DIRECTION oppositeDirection = (DIRECTION)(((int)room.enteringCorridor + 2) % 4);

		// Checking if double backs on itself
		if (!firstCorridor && direction == oppositeDirection)
		{
			int directionInt = (int)direction;
			directionInt++;
			directionInt = directionInt % 4;
			direction = (DIRECTION)directionInt;

		}
			
		corridorLength = length.Random;

		int maxLength = length.m_Max;

		switch (direction)
		{
		case DIRECTION.NORTH:
			startXPos = Random.Range (room.xPos, room.xPos + room.roomWidth - 1);

			startYPos = room.yPos + room.roomHeight;

			maxLength = rows - startYPos - roomHeight.m_Min;
			break;
		case DIRECTION.EAST:
			startXPos = room.xPos + room.roomWidth;
			startYPos = Random.Range(room.yPos, room.yPos + room.roomHeight - 1);
			maxLength = columns - startXPos - roomWidth.m_Min;
			break;
		case DIRECTION.SOUTH:
			startXPos = Random.Range (room.xPos, room.xPos + room.roomWidth);
			startYPos = room.yPos;
			maxLength = startYPos - roomHeight.m_Min;
			break;
		case DIRECTION.WEST:
			startXPos = room.xPos;
			startYPos = Random.Range (room.yPos, room.yPos + room.roomHeight);
			maxLength = startXPos - roomWidth.m_Min;
			break;
		}

		corridorLength = Mathf.Clamp (corridorLength, 1, maxLength);
	}
}