using UnityEngine;
using System.Collections.Generic;

/*
----------------------------------------------------
http://en.wikipedia.org/wiki/A*_search_algorithm
----------------------------------------------------
http://www.policyalmanac.org/games/aStarTutorial.htm
----------------------------------------------------

1) Add the starting square (or node) to the open list.

2) Repeat the following:

	a) Look for the lowest F cost square on the open list. 
		We refer to this as the current square.
	
	b) Switch it to the closed list.
	
	c) For each of the 8 squares adjacent to this current square …
		If it is not walkable or if it is on the closed list, ignore it. 
		Otherwise do the following.           
		If it isn’t on the open list, add it to the open list. 
		Make the current square the parent of this square. 
		Record the F, G, and H costs of the square. 
		If it is on the open list already, check to see if this path to that square is better, 
		using G cost as the measure. A lower G cost means that this is a better path. 
		If so, change the parent of the square to the current square, and recalculate 
		the G and F scores of the square. If you are keeping your open list sorted by F score, 
		you may need to resort the list to account for the change.
	
	d) Stop when you:
		Add the target square to the closed list, 
		in which case the path has been found (see note below), or
		Fail to find the target square, and the open list is empty. 
		In this case, there is no path.   

3) Save the path. Working backwards from the target square, 
	go from each square to its parent square until you reach the starting square. 
	That is your path. 
*/

public delegate void AStarPath(List<AStarNodeScript> path);

public class AStarPathScript
{
	public static event AStarPath OnPathFound;
	
	// GRID MAP of WalkAble (TRUE) & Not WalkAble (FALSE) Nodes
	public static List<List<bool>> gridMap;
	public static NodeIndex startIndex, endIndex; // Start & End AStarNodeScript's Index
	
	public enum HEURISTIC_METHODS
	{ 
		DIJKSTRA = 0, 
		CHEBYSHEV, 
		EUCLIDEAN, 
		EUCLIDEAN_SQUARED, 
		BALANCE_G_H, 
		MANHATTAN
	};
	public static HEURISTIC_METHODS heuristicType = HEURISTIC_METHODS.MANHATTAN;
	
	public static List<AStarNodeScript> pathNodes; // Path's Nodes
	
	private static List<AStarNodeScript> openList, closedList; // Open & Closed Lists
	
	private static AStarNodeScript currentNode; // CURRENT NODE
	
	private static List<NodeIndex> nodeIndexes; // Adjacents AStarNodeScript's Indexes
	private static List<NodeIndex> removeNodeIndexes; // Adjacents AStarNodeScript's Indexes to REMOVE
	
	private static AStarNodeScript node; // TEMP AStarNodeScript
	private static NodeIndex nodeIndex; // TEMP AStarNodeScript's Index
	
	private static int minF; // for searching MIN value

	public static bool isPathFound = false;
	
	private static void Init()
	{
		currentNode = null;

		openList = new List<AStarNodeScript>();
		closedList = new List<AStarNodeScript>();

		if (pathNodes != null)
		{
			pathNodes.Clear ();
			//Debug.Log ("aaaaaaaaaaaaaaaaa " + pathNodes.Count);
		}
		else
		{
			pathNodes = new List<AStarNodeScript>();
			//Debug.Log ("bbbbbbbbbbb " + pathNodes.Count);
		}
		
		nodeIndexes = new List<NodeIndex>();
		removeNodeIndexes = new List<NodeIndex>();
	}
	
	public static bool FindPath()
	{
		Init();

		if (!gridMap[startIndex.i][startIndex.j] ||
			!gridMap[endIndex.i][endIndex.j])
		{
			if (null != OnPathFound)
			{
				isPathFound = false;
				OnPathFound(pathNodes); // CallBack with EMPTY PATH of NODES
			}	
			return false;
		}
		
		node = new AStarNodeScript();
		node.nodeIndex = startIndex;
		
		openList.Add(node);
		
		do
		{
			FindLowestF();
			closedList.Add(currentNode);
			openList.Remove(currentNode);
			CheckAdjacents();
		}while(openList.Count > 0 && !ContainsNodeIndex(openList, endIndex));
		
		if (!ContainsNodeIndex(openList, endIndex))
		{
			if (null != OnPathFound)
			{
				isPathFound = false;
				OnPathFound(pathNodes); // CallBack with EMPTY PATH of NODES
			}
			return false;
		}
		
		node = GetNodeByIndex(openList, endIndex);
		do
		{
			pathNodes.Add(node);
			node = node.parent;
		}while (node != null);

		//! FIXED - In original code it does not return true if OnPathFound callback is not registered
		isPathFound = true;

		pathNodes.Reverse(); // REVERSE List of the Path of NODES
		if (null != OnPathFound)
		{
			OnPathFound(pathNodes); // CallBack with the Found Path of NODES
		}
		
		return true;
	}
	
	// Find The Lowest F's Price [F = G + H]
	private static void FindLowestF()
	{
		minF = int.MaxValue;
		foreach (AStarNodeScript n in openList)
		{
			if (n.F < minF)
			{
				minF = n.F;
				currentNode = n;
			}
		}
	}
	
	private static void CheckAdjacents()
	{
		nodeIndexes.Clear();
		removeNodeIndexes.Clear();
		
		if (null == currentNode)
		{
			return;
		}
		
		nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i - 1; // Up Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j;		// Current Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);
		
		/*nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i - 1; // Up Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j + 1;	// Right Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);*/
		
		nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i; 	// Current Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j + 1;	// Right Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);
		
		/*nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i + 1; // Down Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j + 1;	// Right Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);*/
		
		nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i + 1; // Down Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j;		// Current Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);
		
		/*nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i + 1; // Down Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j - 1;	// Left Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);*/
		
		nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i; 	// Current Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j - 1;	// Left Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);
		
		/*nodeIndex = new NodeIndex();
		nodeIndex.i = currentNode.nodeIndex.i - 1; // Up Row Adjacent AStarNodeScript
		nodeIndex.j = currentNode.nodeIndex.j - 1;	// Left Column Adjacent AStarNodeScript
		nodeIndexes.Add(nodeIndex);*/
		
		foreach (NodeIndex n in nodeIndexes)
		{
			if (n.i < 0 || n.j < 0 || 
				n.i >= gridMap.Count || n.j >= gridMap[0].Count ||
				false == gridMap[n.i][n.j] ||
				ContainsNodeIndex(openList, n) || ContainsNodeIndex(closedList, n))
			{
				removeNodeIndexes.Add(n);
			}
		}
		
		foreach (NodeIndex n in removeNodeIndexes)
		{
			nodeIndexes.Remove(n);
		}
		
		foreach (NodeIndex indx in nodeIndexes)
		{
			node = GetNodeByIndex(openList, indx);
			if (null == node)
			{
				// ADD New AStarNodeScript in Open List
				node = new AStarNodeScript();
				node.nodeIndex.i = indx.i;
				node.nodeIndex.j = indx.j;
				openList.Add(node);
				
				if (node.nodeIndex.i != currentNode.nodeIndex.i && 
					node.nodeIndex.j != currentNode.nodeIndex.j)
				{
					node.G = 14 + currentNode.G;
				} 
				else
				{
					node.G = 10 + currentNode.G;
				}
				
				node.H = CalcHeuristicPrice(node);
				node.F = node.G + node.H;
			}
			else
			{
				if (node.nodeIndex.i != currentNode.nodeIndex.i && 
					node.nodeIndex.j != currentNode.nodeIndex.j)
				{
					if (node.G > 14 + currentNode.G)
					{
						node.G = 14 + currentNode.G;
						node.F = node.G + node.H;
						node.parent = currentNode;
					}
				} 
				else
				{
					if (node.G > 10 + currentNode.G)
					{
						node.G = 10 + currentNode.G;
						node.F = node.G + node.H;
						node.parent = currentNode;
					}
				}
			}
			
			// ADD Parent AStarNodeScript
			node.parent = currentNode;
		}
	}
	
	/*
		1) http://www.policyalmanac.org/games/heuristics.htm
		2) http://theory.stanford.edu/~amitp/GameProgramming/Heuristics.html
	*/
	public static int CalcHeuristicPrice(AStarNodeScript n)
	{
		switch (heuristicType)
		{
			case HEURISTIC_METHODS.DIJKSTRA:
				return HeuristicDijkstra(n);
			
			case HEURISTIC_METHODS.CHEBYSHEV:
				return HeuristicChebyshev(n);
			
			case HEURISTIC_METHODS.EUCLIDEAN:
				return HeuristicEuclidean(n);
			
			case HEURISTIC_METHODS.EUCLIDEAN_SQUARED:
				return HeuristicEuclideanSquared(n);
			
			case HEURISTIC_METHODS.BALANCE_G_H:
				return HeuristicBalanceGH(n);
			
			case HEURISTIC_METHODS.MANHATTAN:
				return HeuristicManhattan(n);
			
			default: // MANHATTAN's Algorithm
				return HeuristicManhattan(n);
		}
	}
	
	public static int HeuristicDijkstra(AStarNodeScript n)
	{
		return 0;
	}
	
	public static int HeuristicChebyshev(AStarNodeScript n)
	{
		return 10 * Mathf.Max(Mathf.Abs(endIndex.i - n.nodeIndex.i), Mathf.Abs(endIndex.j - n.nodeIndex.j));
	}
	
	public static int HeuristicEuclidean(AStarNodeScript n)
	{
		return 10 * (int)Mathf.Sqrt(Mathf.Pow(endIndex.i - n.nodeIndex.i, 2) + Mathf.Pow(endIndex.j - n.nodeIndex.j, 2));
	}
	
	public static int HeuristicEuclideanSquared(AStarNodeScript n)
	{
		return 10 * (int)(Mathf.Pow(endIndex.i - n.nodeIndex.i, 2) + Mathf.Pow(endIndex.j - n.nodeIndex.j, 2));
	}
	
	public static int HeuristicBalanceGH(AStarNodeScript n)
	{
		int xDistance = Mathf.Abs(endIndex.i - n.nodeIndex.i);
		int yDistance = Mathf.Abs(endIndex.j - n.nodeIndex.j);
		if (xDistance > yDistance)
		{
			return 14 * yDistance + 10 * (xDistance - yDistance);
		}
		else
		{
			return 14 * xDistance + 10 * (yDistance - xDistance);
		}
	}
	
	public static int HeuristicManhattan(AStarNodeScript n)
	{
		return 10 * (Mathf.Abs(endIndex.i - n.nodeIndex.i) + Mathf.Abs(endIndex.j - n.nodeIndex.j));
	}
	
	public static bool ContainsNodeIndex(List<AStarNodeScript> list, NodeIndex indx)
	{
		if (null == list || null == indx) return false;
		
		foreach (AStarNodeScript n in list)
		{
			if (n.nodeIndex.i == indx.i && n.nodeIndex.j == indx.j)
			{
				return true;
			}
		}
		
		return false;
	}
	
	public static AStarNodeScript GetNodeByIndex(List<AStarNodeScript> list, NodeIndex indx)
	{
		if (null == list || null == indx) return null;
		
		foreach (AStarNodeScript n in list)
		{
			if (n.nodeIndex.i == indx.i && n.nodeIndex.j == indx.j)
			{
				return n;
			}
		}
		
		return null;
	}
}