using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewDoorControllerScript : MonoBehaviour {

	public bool doorsOpen;
	public bool isVertical;
	public bool isTutorial;
	public GameObject leftDoor;
	public GameObject rightDoor;

	public Sprite verticalLeftDoorOpen;
	public Sprite verticalRightDoorOpen;
	public Sprite sideLeftDoorOpen;
	public Sprite sideRightDoorOpen;
	 
	public Sprite verticalLeftDoorClose;
	public Sprite verticalRightDoorClose;
	public Sprite sideLeftDoorClose;
	public Sprite sideRightDoorClose;

	private SpriteRenderer leftDoorSpriteRenderer;
	private SpriteRenderer rightDoorSpriteRenderer;
	private RoomManagerScript roomManager;

	// Use this for initialization
	void Start () 
	{
		roomManager = gameObject.GetComponentInParent<RoomManagerScript> ();
		leftDoorSpriteRenderer = leftDoor.GetComponent<SpriteRenderer> ();
		rightDoorSpriteRenderer = rightDoor.GetComponent<SpriteRenderer> ();
		doorsOpen = false;
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (roomManager.doorLocked)
		{
			doorsOpen = false;
		}

		if (doorsOpen)
		{
			if (isVertical)
			{
				leftDoorSpriteRenderer.sprite = verticalLeftDoorOpen;
				rightDoorSpriteRenderer.sprite = verticalRightDoorOpen;
			}
			else
			{
				leftDoorSpriteRenderer.sprite = sideLeftDoorOpen;
				rightDoorSpriteRenderer.sprite = sideRightDoorOpen;

				if (isTutorial)
				{
					rightDoor.transform.localScale = new Vector3 (1.5f, 1f, 1f);
					leftDoor.transform.localScale = new Vector3 (1.5f, 1.5f, 1f);
				}
				else
				{
					leftDoor.transform.localScale = new Vector3 (1f, 1.72f, 1f);
				}
			}
			leftDoor.GetComponentInChildren<BoxCollider2D> ().enabled = false;
			rightDoor.GetComponentInChildren<BoxCollider2D> ().enabled = false;
		}
		else
		{
			if (isVertical)
			{
				leftDoorSpriteRenderer.sprite = verticalLeftDoorClose;
				rightDoorSpriteRenderer.sprite = verticalRightDoorClose;
			}
			else
			{
				leftDoorSpriteRenderer.sprite = sideLeftDoorClose;
				rightDoorSpriteRenderer.sprite = sideRightDoorClose;

				if (isTutorial)
				{
					rightDoor.transform.localScale = new Vector3 (1f, 1.22f , 1f);
					leftDoor.transform.localScale = new Vector3 (1f, 1.5f, 1f);
				}
				else
				{
					leftDoor.transform.localScale = new Vector3 (1f, 1f, 1f);
				}
			}
			leftDoor.GetComponentInChildren<BoxCollider2D> ().enabled = true;
			rightDoor.GetComponentInChildren<BoxCollider2D> ().enabled = true;
		}

	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			if (roomManager.doorLocked)
			{
				return;
			}

			doorsOpen = true;

		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.gameObject.tag == "Player")
		{
			doorsOpen = false;
		}
	}
}
