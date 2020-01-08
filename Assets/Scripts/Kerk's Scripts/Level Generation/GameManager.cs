using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameManager : MonoBehaviour
{
	//Singleton
	public static GameManager instance = null;
	private DungeonManagerNew boardScript;
	public GameObject player;

	void Awake()
	{
		if (instance == null) 
		{	
			instance = this;
		}

		else if (instance != this) 
		{
			Destroy (gameObject);    
		}
		DontDestroyOnLoad(gameObject);

		boardScript = GetComponent<DungeonManagerNew>();

	}

	void InitializeGame()
	{
		DestroyMap ();
		boardScript.LevelSetup();

	}

	void DestroyMap()
	{
		/*GameObject[] tiles = GameObject.FindGameObjectsWithTag ("Tiles");

		foreach (GameObject destroyThis in tiles)
		{
			GameObject.Destroy(destroyThis);
		}*/
	}

	void Update()
	{

		if (Input.GetKeyDown ("space")) 
		{
			Destroy(GameObject.Find("Board"));
			InitializeGame ();
		}
	}
}