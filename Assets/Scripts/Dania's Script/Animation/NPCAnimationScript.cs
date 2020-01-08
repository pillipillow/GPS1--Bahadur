using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCAnimationScript : MonoBehaviour {

	Transform player;
	Transform npcPos;
	SpriteRenderer npc;

	// Use this for initialization
	void Start () 
	{
		player = Player.Instance.transform;
		npc = this.GetComponent<SpriteRenderer>();
		npcPos = this.transform;	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!player)
		{
			return;
		}

		if(npcPos.position.x < player.position.x)
		{
			npc.flipX = false;
		}
		if(npcPos.position.x > player.position.x)
		{
			npc.flipX = true;
		}
	}
}
