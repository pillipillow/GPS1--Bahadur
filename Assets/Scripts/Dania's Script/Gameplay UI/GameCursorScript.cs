using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCursorScript : MonoBehaviour {

	public Texture2D crosshairCursor;
	public Texture2D interactCursor;

	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 hotSpot = Vector2.zero;
	public Vector2 interactHotspot = Vector2.zero;

	private bool pause = false;
	private bool upgrade = false;

	// Use this for initialization
	void Start () 
	{
		//Cursor.lockState = CursorLockMode.Locked;
		hotSpot.x = 35;
		hotSpot.y = 50;
		Cursor.SetCursor(crosshairCursor, hotSpot, cursorMode);
	}

	void Update()
	{
		pause = PauseMenuScript.paused;
		upgrade = UpgradeMenuScript.upgraded;
		if(pause || upgrade)
		{
			Cursor.SetCursor(interactCursor, interactHotspot, cursorMode);
		}
		else if(!pause || upgrade)
		{
			Cursor.SetCursor(crosshairCursor, hotSpot, cursorMode);
		}
	}


	void OnMouseEnter()
	{
		Cursor.SetCursor(interactCursor, interactHotspot, cursorMode);
	}


	void OnMouseExit()
	{
		Cursor.SetCursor(null, Vector2.zero, cursorMode);
	}
}
