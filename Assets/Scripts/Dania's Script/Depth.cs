using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Depth : MonoBehaviour {

	public int rangePerUnitY = 1;
	SpriteRenderer spriteRenderer;
	public int targetOffset = 0; 

	// Use this for initialization
	void Start () 
	{
		spriteRenderer = this.GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{	//sorting order change based on y position
		spriteRenderer.sortingOrder = -(int)(transform.position.y * rangePerUnitY) + targetOffset; 
	}
}
