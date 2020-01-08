using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootElement : MonoBehaviour 
{
	//! The rate of fire for the elements
	public float fireRate;
	public float waterRate;
	public float airRate;
	public float earthRate;
	private float attackTimer = 0f;

	//! For raycast
	public LayerMask hittingFloor;

	//! The type of element the player shot and its transform
	[HideInInspector] public string shootElement;
	public Transform BulletTrailPrefabFire;
	public Transform BulletTrailPrefabWater;
	public Transform BulletTrailPrefabAir;
	public Transform BulletTrailPrefabEarth;

	//! The sounds for the player shooting for each element
	public AudioClip[] shootClip;
	private AudioSource sound;

	//! The point of fire the bullets will be instantiated at
	private Transform firePoint;

	//! For pausing
	private bool pause = false; 
	private bool upgrade = false;
	private bool talking = false;

	void Start () 
	{
		firePoint = transform.Find ("Shoot Position");
		sound = GetComponent<AudioSource>();
	}

	void Update () 
	{
		pause = PauseMenuScript.paused;  
		upgrade = UpgradeMenuScript.upgraded;
		talking = Player.Instance.talking;

		if (pause == true || upgrade == true || talking == true) 
		{
			return;
		}

		//! Where the shoot rate happens
		if (attackTimer == 0f) 
		{
			if (Input.GetMouseButton (0) && Player.Instance.playerCantShoot == false) 
			{
				CheckElement ();
				Shoot ();
			}
		} 
		else 
		{
			if (attackTimer <= 0f) 
			{
				attackTimer = 0f;
			} 
			else 
			{
				attackTimer -= Time.deltaTime;
			}
		}
	}

	void Shoot () 
	{
		if (shootElement == "Water" || shootElement == "Air" || shootElement == "Earth") 
		{
			Effect ();
		} 
		else if (shootElement == "Fire") 
		{
			Vector2 mousePosition = new Vector2 (Camera.main.ScreenToWorldPoint (Input.mousePosition).x, Camera.main.ScreenToWorldPoint (Input.mousePosition).y);
			RaycastHit2D hit = Physics2D.Raycast (mousePosition, Vector3.down, 0.0001f, hittingFloor);

			if (hit)
			{
				Instantiate (BulletTrailPrefabFire, mousePosition, BulletTrailPrefabFire.rotation);
				SoundManagerScript.Instance.PlaySFX (AudioClipID.SFX_ATTACK_FIRE);
			}
		}
	}

	void Effect () 
	{
		if (shootElement == "Water") 
		{
			Instantiate (BulletTrailPrefabWater, firePoint.position, firePoint.rotation);
			SoundManagerScript.Instance.PlaySFX (AudioClipID.SFX_ATTACK_WATER);
		}
		else if (shootElement == "Air") 
		{
			Instantiate (BulletTrailPrefabAir, firePoint.position, firePoint.rotation);
			SoundManagerScript.Instance.PlaySFX (AudioClipID.SFX_ATTACK_AIR);
		}
		else if (shootElement == "Earth") 
		{
			Instantiate (BulletTrailPrefabEarth, firePoint.position, firePoint.rotation);
			SoundManagerScript.Instance.PlaySFX (AudioClipID.SFX_ATTACK_EARTH);
		}
	}

	void CheckElement () 
	{
		shootElement = Player.Instance.element;

		if (shootElement == "Fire") 
		{
			attackTimer = fireRate;
		}
		else if (shootElement == "Water") 
		{
			attackTimer = waterRate;
		}
		else if (shootElement == "Air") 
		{
			attackTimer = airRate;
		}
		else if (shootElement == "Earth") 
		{
			attackTimer = earthRate;
		}
	}
}
