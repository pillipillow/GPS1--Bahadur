//
//Door Controller Script
//Can be used by doors of any orientation
//Opens when player touches the collider, would not open when the room is locked
//Treaxen Ng E-Tjing 0113695
//

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorControllerScript : MonoBehaviour {

	public bool doorsOpen;
	public GameObject positiveDoor;
	public GameObject negativeDoor;

	private Quaternion positiveOrientation;
	private Quaternion negativeOrientation;
	private RoomManagerScript roomManager;


	void Start()
	{
		roomManager = gameObject.GetComponentInParent<RoomManagerScript> ();
		positiveOrientation = positiveDoor.transform.rotation;
		negativeOrientation = negativeDoor.transform.rotation;
		doorsOpen = false;
	}

	void Update () 
	{
		if (roomManager.doorLocked)
		{
			doorsOpen = false;
		}

		if (doorsOpen)
		{
			Vector3 posRot = positiveOrientation.eulerAngles;
			posRot = new Vector3 (posRot.x, posRot.y, posRot.z + 90);

			Vector3 negRot = negativeOrientation.eulerAngles;
			negRot = new Vector3 (negRot.x, negRot.y, negRot.z - 90);

			positiveDoor.transform.rotation = Quaternion.Euler (posRot);
			negativeDoor.transform.rotation = Quaternion.Euler (negRot);

			this.GetComponent <BoxCollider2D> ().enabled = false;
		}
		else
		{
			positiveDoor.transform.rotation = positiveOrientation;
			negativeDoor.transform.rotation = negativeOrientation;

			this.GetComponent <BoxCollider2D> ().enabled = true;
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
		
}
