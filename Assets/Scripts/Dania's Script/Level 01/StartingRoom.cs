using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingRoom : MonoBehaviour {

	public GameObject floorPad;
	SteppedScript floor;

	public GameObject roomManager;

	// Use this for initialization
	void Start () 
	{
		floor = floorPad.GetComponent<SteppedScript>();	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(floor.step)
		{
			roomManager.GetComponent<RoomManagerScript>().doorLocked = false;
		}
	}
}
