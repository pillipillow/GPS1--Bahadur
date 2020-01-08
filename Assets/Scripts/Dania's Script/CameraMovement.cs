using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour {

	public float cameraX = 0;
	public float cameraY = 0;
	public float smoothTime = 0.3F;
	public float Distance = 3;

    private Vector3 velocity = Vector3.zero;
	private Transform player;

	void Start ()
	{
		player = Player.Instance.transform;
	}

    void Update ()
	{
		if (!player)
		{
			return;
		}
		Vector3 playerPosition = player.position + new Vector3 (cameraX, cameraY, -10);
		transform.position = Vector3.SmoothDamp (transform.position, playerPosition, ref velocity, smoothTime);
	}
}
