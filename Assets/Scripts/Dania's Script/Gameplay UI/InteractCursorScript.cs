using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractCursorScript : MonoBehaviour {

	public Texture2D interactCursor;

	public CursorMode cursorMode = CursorMode.Auto;
	public Vector2 interactHotspot = Vector2.zero;

	// Use this for initialization
	void Start () 
	{
		//Cursor.lockState = CursorLockMode.Locked;
		Cursor.SetCursor(interactCursor, interactHotspot, cursorMode);
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
