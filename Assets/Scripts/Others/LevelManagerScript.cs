using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManagerScript : MonoBehaviour {

	//For pathfinding gridMap
	public GameObject[] wallList;
	public int[,] gridList;
	public float minX = 10000.0f;
	public float maxX = -10000.0f;
	public float minY = 10000.0f;
	public float maxY = -10000.0f;
	public float tileSize = 0.64f;
	public int mapRowCount = 0;
	public int mapColCount = 0;

	public GameObject pathMarkerPrefab;
	public List<AStarNodeScript> pathNodesList = new List<AStarNodeScript> ();

	private static LevelManagerScript mInstance;
	public static LevelManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("LevelManager");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("LevelManager");
					mInstance = obj.AddComponent<LevelManagerScript>();
					obj.tag = "LevelManager";
				}
				else
				{
					mInstance = tempObject.GetComponent<LevelManagerScript>();
				}
			}
			return mInstance;
		}
	}

	//get level size
	void Awake()
	{
		wallList = GameObject.FindGameObjectsWithTag ("Wall");

		for (int i = 0; i < wallList.Length; i++)
		{
			if (wallList[i].transform.position.x < minX)
			{
				minX = wallList[i].transform.position.x;
			}
			if (wallList[i].transform.position.x > maxX)
			{
				maxX = wallList[i].transform.position.x;
			}
			if (wallList[i].transform.position.y < minY)
			{
				minY = wallList[i].transform.position.y;
			}
			if (wallList[i].transform.position.y > maxY)
			{
				maxY = wallList[i].transform.position.y;
			}
		}

		mapRowCount = Mathf.CeilToInt((maxY - minY) / tileSize) + 1;
		mapColCount =  Mathf.CeilToInt((maxX - minX) / tileSize) + 1; 

		gridList = new int[mapRowCount, mapColCount];

		for(int i = 0; i < mapRowCount; i++)
		{
			for (int j = 0; j < mapColCount; j++)
			{
				gridList[i, j] = 0;
			}
		}

		for (int i = 0; i < wallList.Length; i++)
		{
			int colIndex = Mathf.RoundToInt((wallList [i].transform.position.x - minX) / tileSize);
			int rowIndex = Mathf.RoundToInt((wallList [i].transform.position.y - minY) / tileSize);
			//Debug.Log ("rowIndex : " + rowIndex + " colIndex : " + colIndex);
			gridList[mapRowCount - rowIndex - 1, colIndex] = 1;
		}

		string gridText = "";

		for(int i = 0; i < mapRowCount; i++)
		{
			for (int j = 0; j < mapColCount; j++)
			{
				gridText += gridList [i, j] + " ";
			}
			gridText += "\n";
		}

		//Debug.Log (gridText);

		LevelManagerScript.Instance.GenerateMap();
	}

	void Start()
	{
		OtherManagerScript.Instance.level++;
		if(OtherManagerScript.Instance.level<2)
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_GAMEPLAY);
		}

	}

	//draw gridmap
	public void GenerateMap()
	{
		if(AStarPathScript.gridMap != null)
		{
			AStarPathScript.gridMap.Clear();
		}
		else
		{
			AStarPathScript.gridMap = new List<List<bool>>();
		}

		for(int i = 0; i < mapRowCount; i++)
		{
			List<bool> boolList = new List<bool>();

			for(int j = 0; j < mapColCount; j++)
			{
				if(gridList[i,j] == 0)
				{
					boolList.Insert(j, true);
				}
				else
				{
					boolList.Insert(j, false);
				}
			}
			AStarPathScript.gridMap.Insert(i, boolList);
		}
	}

	//draw path to player
	public Vector3 GeneratePath(GameObject from, GameObject to)
	{
		//Debug.Log("GeneratePath : " + from.transform.position + " "+ to.transform.position);
		int startCol = Mathf.RoundToInt((from.transform.position.x - minX) / tileSize);
		int startRow = Mathf.RoundToInt((from.transform.position.y - minY) / tileSize);
		int targetCol = Mathf.RoundToInt((to.transform.position.x - minX) / tileSize);
		int targetRow = Mathf.RoundToInt((to.transform.position.y - minY) / tileSize);
		startRow = mapRowCount - startRow - 1;
		targetRow = mapRowCount - targetRow - 1;
		//Debug.Log ("startCol " + startCol + " startRow " + startRow);
		//Debug.Log ("targetCol " + targetCol + " targetRow " + targetRow);

		for(int i = 0; i < mapRowCount; i++)
		{
			for(int j = 0; j < mapColCount; j++)
			{
				if(i == startRow && j == startCol)
				{
					if(AStarPathScript.startIndex == null)
					{
						AStarPathScript.startIndex = new NodeIndex();
					}
					AStarPathScript.startIndex.i = i;
					AStarPathScript.startIndex.j = j;
				}
				else if(i == targetRow && j == targetCol)
				{
					if(AStarPathScript.endIndex == null)
					{
						AStarPathScript.endIndex = new NodeIndex();
					}
					AStarPathScript.endIndex.i = i;
					AStarPathScript.endIndex.j = j;
				}
			}
		}

		AStarPathScript.heuristicType = AStarPathScript.HEURISTIC_METHODS.MANHATTAN;
		AStarPathScript.FindPath();
		Vector3 nextPosition = from.transform.position;

		if (AStarPathScript.isPathFound)
		{
			nextPosition.x = AStarPathScript.pathNodes[1].nodeIndex.j * tileSize + minX;
			nextPosition.y = (mapRowCount - AStarPathScript.pathNodes[1].nodeIndex.i - 1) * tileSize + minY;
			//Debug.Log("NEXT NODE : " + AStarPathScript.pathNodes[0].nodeIndex.i + " "+ AStarPathScript.pathNodes[0].nodeIndex.j);
			pathNodesList.Clear ();

			/*for (int i = 0; i < AStarPathScript.pathNodes.Count; i++)
			{
				pathNodesList.Add (AStarPathScript.pathNodes [i]);
				Vector3 markerPos = Vector3.zero;
				markerPos.x = AStarPathScript.pathNodes[i].nodeIndex.j * tileSize + minX;
				markerPos.y = (mapRowCount - AStarPathScript.pathNodes[i].nodeIndex.i - 1) * tileSize + minY;
				Instantiate (pathMarkerPrefab, markerPos, Quaternion.identity);
				//Debug.Log(AStarPathScript.pathNodes[i].nodeIndex.j + " "+ AStarPathScript.pathNodes[i].nodeIndex.i);
			}*/
		}

		/*string gridText = "";
		for(int i = 0; i < mapRowCount; i++)
		{
			for (int j = 0; j < mapColCount; j++)
			{
				bool isDisplayed = false;
				for (int k = 0; k < AStarPathScript.pathNodes.Count; k++)
				{
					if (AStarPathScript.pathNodes [k].nodeIndex.j == j &&
					    AStarPathScript.pathNodes [k].nodeIndex.i == i)
					{
						gridText += "X";
						isDisplayed = true;
						break;
					}
				}
				if(!isDisplayed)
				{
					gridText += gridList [i, j] + " ";
				}
			}
			gridText += "\n";
		}

		Debug.Log (gridText);*/

		//Debug.Log ("nextPosition " + nextPosition.x + "  " + nextPosition.y);

		//pathNodesList.RemoveAt (0);
		return nextPosition;
	}

	public Vector3 GenerateNextNode()
	{
		Vector3 nextPosition = Vector3.zero;
		nextPosition.x = pathNodesList[0].nodeIndex.j * tileSize + minX;
		nextPosition.y = (mapRowCount - pathNodesList[0].nodeIndex.i - 1) * tileSize + minY;
		pathNodesList.RemoveAt (0);
		return nextPosition;
	}
}
