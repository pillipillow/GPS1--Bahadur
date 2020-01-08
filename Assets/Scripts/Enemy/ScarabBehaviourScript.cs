//Ng E-Tjing 0113695
//Bahadur
//
//Note:this script is now obselete

/*
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScarabBehaviourScript : MonoBehaviour {

	public enum ScarabDirection {UP = 0, DOWN, LEFT, RIGHT};
	public ScarabDirection scarabDirection;
	public float scarabHp;
	public float scarabMoveSpeed;

	private SpriteRenderer spriteRenderer;
	Vector3 nextPosition = Vector3.zero;

	// Use this for initialization
	void Start () 
	{
		spriteRenderer = gameObject.GetComponent<SpriteRenderer> ();

		nextPosition = LevelManagerScript.Instance.GeneratePath(this.gameObject, Player.Instance.gameObject);
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 prevPos = this.transform.position;
		Vector3 distance = nextPosition - this.transform.position;

		this.transform.Translate (distance.normalized * (scarabMoveSpeed * Time.deltaTime));
		//Debug.Log ("distance : " + distance);

		if (distance.magnitude < 0.01f * scarabMoveSpeed / 0.7f)
		{
			nextPosition = LevelManagerScript.Instance.GeneratePath(this.gameObject, Player.Instance.gameObject);
		}

		if (this.transform.position.x < prevPos.x)
		{
			if (scarabDirection != ScarabDirection.LEFT)
			{
				spriteRenderer.flipX = false;
				this.transform.rotation = Quaternion.Euler (this.transform.position.x, this.transform.position.y, 0);
				scarabDirection = ScarabDirection.LEFT;
			}
		}
		else if (this.transform.position.x > prevPos.x)
		{
			if (scarabDirection != ScarabDirection.RIGHT)
			{
				spriteRenderer.flipX = true;
				this.transform.rotation = Quaternion.Euler (this.transform.position.x, this.transform.position.y, 0);
				scarabDirection = ScarabDirection.RIGHT;
			}
		}
		else if (this.transform.position.y > prevPos.y)
		{
			if (scarabDirection != ScarabDirection.DOWN)
			{
				spriteRenderer.flipX = false;
				this.transform.rotation = Quaternion.Euler (this.transform.position.x, this.transform.position.y, 90);
				scarabDirection = ScarabDirection.DOWN;
			}
		}
		else if (this.transform.position.y < prevPos.y)
		{
			if (scarabDirection != ScarabDirection.UP)
			{
				spriteRenderer.flipX = false;
				this.transform.rotation = Quaternion.Euler (this.transform.position.x, this.transform.position.y, 270);
				scarabDirection = ScarabDirection.UP;
			}
		}
	}
}
*/