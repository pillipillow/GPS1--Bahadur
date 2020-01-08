using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniCamera : MonoBehaviour {

	public float cameraX = 0;
	public float cameraY = 0;
	public float smoothTime = 0.3F;

    private Vector3 velocity = Vector3.zero;
	private Transform player;

	void Start ()
	{
		player = Player.Instance.transform;
	}

    void Update ()
	{
		Vector3 targetPosition = player.position + new Vector3 (cameraX, cameraY, -10);
		transform.position = Vector3.SmoothDamp (transform.position, targetPosition, ref velocity, smoothTime);
	}

	//OLD VERSION
	/*Transform target; 
	void Start () 
	{
		target = GameObject.Find("Player").transform;  //target set to player's position/rotation/scale
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.position = target.position + new Vector3 (cameraX,cameraY,-10); //camera transform position (move) equal to player's position but also adjusted -10 on Z axis
	}*/
}
