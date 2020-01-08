using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrontPositionScript : MonoBehaviour 
{
	public float rotateRadius;

	private Transform shootElement;

	void Start () 
	{
		shootElement = transform.parent.transform;
	}

	void Update () 
	{
		Vector3 shootElementToMouseDir = Camera.main.ScreenToWorldPoint (Input.mousePosition) - shootElement.position;
		shootElementToMouseDir.z = 0;
		transform.position = shootElement.position + (rotateRadius * shootElementToMouseDir.normalized);
	}
}
