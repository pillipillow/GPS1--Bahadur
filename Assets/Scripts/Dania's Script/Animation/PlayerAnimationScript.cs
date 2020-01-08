using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationScript : MonoBehaviour {

	Animator animator;
	private bool pause = false; //*PLEASE UNCOMMENT THIS ONCE PAUSE SCRIPT UI HAS BEEN PLACED*
	private bool upgrade = false;
	private bool talking = false;
	private string element;

	// Use this for initialization
	void Start () 
	{
		animator = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		pause = PauseMenuScript.paused;  //*PLEASE UNCOMMENT THIS ONCE PAUSE SCRIPT UI HAS BEEN PLACED*
		upgrade = UpgradeMenuScript.upgraded;
		talking = Player.Instance.talking;
		element = Player.Instance.element;
		if (pause == true || upgrade == true)  
		{
			//print("The game is pausing");
			return;
		}

		if (Player.Instance.playerHP <= 0)
		{
			animator.Play("playerDeath");

			if (Player.Instance.playerInDeath == false)
			{
				Player.Instance.playerHP = Player.Instance.playerHPMax;
				animator.Play("playerIdle");
			}
		}

		if(talking == true)
		{
			animator.Play("playerIdle");
			animator.SetBool("playerWalk",false);
			animator.SetBool("shootFire",false);
			animator.SetBool("shootElements",false);
		}

		if(talking == false)
		{
			if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.A)|| Input.GetKey(KeyCode.D))
			{
				animator.SetBool("playerWalk",true);
			}
			else
			{
				animator.SetBool("playerWalk",false);
			}

			if(Input.GetMouseButtonDown(0))
			{
				if(element == "Fire")
				{
					animator.SetBool("shootFire",true);
				}
				else
				{
					animator.SetBool("shootElements",true);
				}
			}
			else if(Input.GetMouseButtonUp(0))
			{
				animator.SetBool("shootFire",false);
				animator.SetBool("shootElements",false);
			}
		}

		//Flipping & Look at mouse
		Vector3 mousePos = Input.mousePosition;
		mousePos.z = -10; 
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint (mousePos);

		if(talking == false)
		{
			if (cursorPosition.x < this.transform.position.x) 
			{
				transform.localScale = new Vector3(-1,1,1);
			} 
			if(cursorPosition.x > this.transform.position.x) 
			{
				transform.localScale = new Vector3(1,1,1);
			}
		}
	}
}
