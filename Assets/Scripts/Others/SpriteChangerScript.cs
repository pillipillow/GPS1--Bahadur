using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteChangerScript : MonoBehaviour {

	private SpriteRenderer spriteRenderer;
	private Sprite change;
	public Sprite newSprite;
	// Use this for initialization
	void Start () {
		spriteRenderer = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("space")){
			spriteRenderer.sprite = newSprite; 
			print ("Hey");
		}
	}
}
