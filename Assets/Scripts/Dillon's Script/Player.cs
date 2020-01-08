using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour 
{
	//! Singleton for player
	private static Player mInstance;
	public static Player Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag ("Player");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("_Player");
					mInstance = obj.AddComponent<Player>();
					obj.tag = "Player";
				}
				else 
				{
					mInstance = tempObject.GetComponent<Player>();
				}
			}
			return mInstance;
		}
	}
	public static bool CheckInstanceExist()
	{
		return mInstance;
	}

	//! Player save point
	[HideInInspector] public Vector3 playerPreviousPoint;
	[HideInInspector] public Vector3 checkPoint;
	public bool isTutorialMode;

	//! Player stats
	[HideInInspector] public string element = "Fire";
	[HideInInspector] public bool isGod;
	public bool playerIsDead = false;
	public float playerFallPit = 0.5f;
	public float playerDeathDuration = 4f;
	public float playerHP;
	public float playerHPMax;
	public float playerSpeed;
	public float critChance;
	public float critDamage;
	public float stunDuration;
	public float slowDuration;
	public float tormentDamage;
	public float tormentDuration;
	public int playerDamage;
	private float playerBaseSpeed;

	//! Player status updates
	[HideInInspector] public bool playerDodge = false;
	[HideInInspector] public bool playerInvul = false;
	public LayerMask hittingWalls;
	public GameObject teleportPosition;
	public float playerInvulDuration;
	public float playerDodgeCooldown;
	public float playerCantMoveDuration;
	public bool playerCantShoot;
	public bool playerInDeath;
	private bool tookPitDamage;
	private SpriteRenderer playerSprite;
	public bool playerCantMove = false;
	private bool playerTookDamage = false;
	private bool playerDodged = false;
	private float currentColor;
	private float playerCantMoveDurationCount;
	private float playerDodgeTimer;
	private float playerInvulCounter;

	//! Player debuffs from enemies
	public bool playerKnockbackEffect = false;
	private float playerMoveSpeed;

	//! Upgrades for the player
	public float critChanceUpgrade;
	public float critDamageUpgrade;
	public float stunDurationUpgrade;
	public float slowDurationUpgrade;
	public float tormentDamageUpgrade;

	//! For player skills
	[HideInInspector] public bool getTurretSkill;
	public GameObject PlayerTurret;
	public float spawnTurretCooldown;
	private float spawnTurretCooldownStorage;
	private bool spawnTurret;

	//! Player change scenes
	public int level = 1;
	private bool SwitchScene = false;

	//! For pausing the game
	[HideInInspector] public bool talking = false;
	private bool pause = false; 
	private bool upgrade = false;

	//! For storage when restart
	public int playerDamageStore;
	public float playerSpeedStore;
	public float playerHPMaxStore;
	public float playerCritChanceStore;
	public float playerCritDamageStore;

	void Start () 
	{
		playerCantShoot = false;
		isTutorialMode = true;
		getTurretSkill = false;
		spawnTurret = true;
		spawnTurretCooldownStorage = spawnTurretCooldown;
		playerCantMoveDuration = playerCantMoveDuration + playerFallPit;

		playerBaseSpeed = playerSpeed;
		playerSprite = GetComponent <SpriteRenderer> ();
		playerMoveSpeed = playerSpeed;
		checkPoint = this.transform.position;
	}

	//! FixedUpdate required cuz it has Rigidbody2D
	void FixedUpdate () 
	{
		pause = PauseMenuScript.paused;  
		upgrade = UpgradeMenuScript.upgraded;
		if (pause == true || upgrade == true || talking == true) 
		{
			print("The game is pausing");
			return;
		}

		PlayerMovement ();
		PlayerInDodge ();
		PlayerKnockedBack ();
		SkillSpawnTurret ();
		PlayerCantMove ();
		PlayerInvul ();
		ActivateGodMode ();
		PlayerDeath ();
		PlayerSwitchScene ();
	}

	void Update () 
	{
		ChangeElements();
	}

	void OnTriggerEnter2D (Collider2D other) 
	{
		if (other.gameObject.tag == "EnemyProjectile") 
		{
			if (playerTookDamage == false && playerInvul == false && playerDodge == false) 
			{
				//! For achievementImmortal failed
				if (PlayerAchievementScript.Instance.achievementImmortal == true)
				{
					PlayerAchievementScript.Instance.achievementImmortal = false;
				}
				playerTookDamage = true;
			}
		}

		if (other.gameObject.tag == "Pitfall")
		{
			if (tookPitDamage == false)
			{
				tookPitDamage = true;
				if (playerHP <= 1f && playerHP > 0f)
				{
					playerHP = 0f;
				}
				else if (playerHP > 1f)
				{
					StartCoroutine (PlayerRespawnAndDeath());
				}
			}
		}

		if (other.gameObject.tag == "Portal")
		{
			SwitchScene = true;
		}
	}

	void OnTriggerExit2D (Collider2D other) 
	{
		if (other.gameObject.tag == "Portal")
		{
			SwitchScene = false;
		}
	}

	IEnumerator PlayerRespawnAndDeath ()
	{
		float x = Input.GetAxis ("Horizontal");
		float y = Input.GetAxis ("Vertical");
		float previousDistance = 0.4f;

		//! For achievementImmortal failed
		if (PlayerAchievementScript.Instance.achievementImmortal == true)
		{
			PlayerAchievementScript.Instance.achievementImmortal = false;
		}

		playerCantMove = true;

		if (isTutorialMode != true)
		{
			playerHP -= 1;
		}

		yield return new WaitForSeconds (playerFallPit);
		Vector3 pos = this.transform.position - new Vector3 (x,y).normalized * previousDistance;
		this.transform.position = pos;
		tookPitDamage = false;
	}

	void PlayerKnockedBack ()
	{
		if (playerKnockbackEffect == true)
		{
			playerKnockbackEffect = false;
		}
	}

	void PlayerCantMove ()
	{
		if (playerCantMove == true)
		{
			if (playerHP > 0)
			{
				if (playerCantMoveDurationCount < playerCantMoveDuration)
				{
					playerCantMoveDurationCount += Time.deltaTime;
				}
				else if (playerCantMoveDurationCount >= playerCantMoveDuration)
				{
					playerCantMoveDurationCount = 0;
					playerCantMove = false;
				}

				if (playerCantMoveDurationCount > playerFallPit)
				{
					currentColor = playerCantMoveDurationCount / 2;
					playerSprite.color = new Color (1f, currentColor, currentColor, 1f);
				}
			}
		}
	}
		
	void PlayerSwitchScene ()
	{
		if (SwitchScene == true)
		{
			if (Input.GetKeyDown (KeyCode.F)) 
			{
				if (level == 1)
				{
					Debug.Log ("changing to level 1");
					SwitchScene = false;
					DontDestroyOnLoad (this.gameObject);
					playerDamageStore = playerDamage;
					playerSpeedStore = playerSpeed;
					playerHPMaxStore = playerHPMax;
					playerCritChanceStore = critChance;
					playerCritDamageStore = critDamage;

					FadeManager.Instance.FadeTo ();
					Invoke ("LoadLevel1",1.5f);
					level = 2;
				}

				else if(level == 2)
				{
					Debug.Log ("changing to level 2");
					SwitchScene = false;
					FadeManager.Instance.FadeTo ();
					Invoke ("LoadLevel2",1.5f);
					level = 3;
				}
			}
		}
	}

	public void LoadLevel1 ()
	{
		SceneManagerScript.LoadLevel01 ();
		isTutorialMode = false;
		transform.position = new Vector2 (2.5f, 3f);
	}

	public void LoadLevel2 ()
	{
		SceneManagerScript.LoadLevel02 ();
	}

	void PlayerDeath () 
	{
		if (playerHP <= 0) 
		{
			StopCoroutine (PlayerRespawnAndDeath());

			if (isTutorialMode)
			{
				if (playerDeathDuration >= 3.9f)
				{
					playerCantMove = true;
					playerCantShoot = true;
					this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
					playerInDeath = true;
					playerDeathDuration -= Time.deltaTime;
				}
				else if (playerDeathDuration < 3.9f && playerDeathDuration > 0f)
				{
					playerDeathDuration -= Time.deltaTime;
				}
				else if (playerDeathDuration <= 0f)
				{
					this.transform.position = checkPoint;
					this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
					playerInDeath = false;
					playerCantMove = false;
					playerCantShoot = false;
					tookPitDamage = false;
					playerDeathDuration = 4f;
				}
			}
			else
			{
				if (playerDeathDuration >= 3.9f)
				{
					playerCantMove = true;
					playerCantShoot = true;
					this.gameObject.GetComponent<BoxCollider2D> ().enabled = false;
					playerInDeath = true;
					playerDeathDuration -= Time.deltaTime;
				}
				else if (playerDeathDuration < 3.9f && playerDeathDuration > 0f)
				{
					playerDeathDuration -= Time.deltaTime;
				}
				else if (playerDeathDuration <= 0f)
				{
					this.transform.position = checkPoint;
					this.gameObject.GetComponent<BoxCollider2D> ().enabled = true;
					playerInDeath = false;
					playerIsDead = true;
					tookPitDamage = false;
				}
			}
		}
	}

	void SkillSpawnTurret ()
	{
		if (getTurretSkill == true)
		{
			if (spawnTurret == false)
			{
				if (spawnTurretCooldown > 0)
				{
					spawnTurretCooldown -= Time.deltaTime;
				}
				if (spawnTurretCooldown <= 0)
				{
					spawnTurret = true;
				}
			}
			else if (Input.GetKey (KeyCode.Q) && spawnTurret == true)
			{
				Instantiate (PlayerTurret, transform.position, transform.rotation);
				spawnTurret = false;
				spawnTurretCooldown = spawnTurretCooldownStorage;
			}
		}
	}

	void ChangeElements () 
	{
		//! Using numbers pad to change elements
		if (Input.GetKeyDown (KeyCode.Alpha1)) 
		{
			if (element == "Fire") {} 
			else 
			{
				element = "Fire";
			}
		}
		else if (Input.GetKeyDown (KeyCode.Alpha2)) 
		{
			if (element == "Air") {}
			else 
			{
				element = "Air";
			}
		}
		else if (Input.GetKeyDown (KeyCode.Alpha3)) 
		{
			if (element == "Earth") {}
			else 
			{
				element = "Earth";
			}
		}
		else if (Input.GetKeyDown (KeyCode.Alpha4)) 
		{
			if (element == "Water") {}
			else 
			{
				element = "Water";
			}
		}

		//! Using mouse scroll to change elements
		if (Input.GetAxis ("Mouse ScrollWheel") > 0f) 
		{
			if (element == "Fire") 
			{
				element = "Water";
			}
			else if (element == "Water") 
			{
				element = "Earth";
			}
			else if (element == "Earth") 
			{
				element = "Air";
			}
			else if (element == "Air") 
			{
				element = "Fire";
			}
		}
		else if (Input.GetAxis ("Mouse ScrollWheel") < 0f) 
		{
			if (element == "Air") 
			{
				element = "Earth";
			}
			else if (element == "Earth") 
			{
				element = "Water";
			}
			else if (element == "Water") 
			{
				element = "Fire";
			}
			else if (element == "Fire") 
			{
				element = "Air";
			}
		}
	}

	void ActivateGodMode ()
	{
		if (Input.GetKey (KeyCode.L))
		{
			if (isGod)
			{
				this.gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
				isGod = false;
			}
			else
			{
				this.gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
				isGod = true;
			}
		}
	}

	void PlayerMovement () 
	{
		if (playerCantMove == false)
		{
			//! For player movement + dodge (Since player can only dodge while moving)
			if (Input.GetKey (KeyCode.W)) 
			{
				transform.Translate (Vector3.up * Time.deltaTime * playerSpeed);
				PlayerDodge ();
			}
			else if (Input.GetKey (KeyCode.S)) 
			{
				transform.Translate (Vector3.down * Time.deltaTime * playerSpeed);
				PlayerDodge ();
			}
			if (Input.GetKey (KeyCode.A)) 
			{
				transform.Translate (Vector3.left * Time.deltaTime * playerSpeed);
				PlayerDodge ();
			}
			else if (Input.GetKey (KeyCode.D)) 
			{
				transform.Translate (Vector3.right * Time.deltaTime * playerSpeed);
				PlayerDodge ();
			}
		}
	}

	void PlayerInDodge ()
	{
		//! For player dodge duration to decrease its duration, it needs to be constantly updated
		if (playerDodge == true)
		{
			float x = Input.GetAxis ("Horizontal");
			float y = Input.GetAxis ("Vertical");
			float distance = 1f * playerSpeed / playerBaseSpeed;

			RaycastHit2D hit = Physics2D.Raycast (this.transform.position, new Vector3 (x,y), distance, hittingWalls);
			Instantiate (teleportPosition, this.transform.position, Quaternion.identity);

			if (hit)
			{
				this.transform.position = hit.point;
				Instantiate (teleportPosition, this.transform.position, Quaternion.identity);
			}
			else
			{
				Vector3 pos = this.transform.position + new Vector3 (x,y).normalized * distance;
				Instantiate (teleportPosition, pos, Quaternion.identity);
				this.transform.position = pos;
			}

			playerDodge = false;
			playerDodged = true;
			playerDodgeCooldown = 0f;
		}

		//! To set cooldown for the player dodge so dodge isn't permanent
		if (playerDodged == true) 
		{
			if (playerDodgeCooldown >= 0f && playerDodgeCooldown < 0.5f) 
			{
				playerDodgeCooldown += Time.deltaTime;
			} 
			else if (playerDodgeCooldown >= 0.5f) 
			{
				playerDodgeCooldown = 0.5f;
				playerDodged = false;
			}
		}
	}

	void PlayerDodge () 
	{
		if (Input.GetMouseButton (1)) 
		{
			if (playerDodge == false && playerDodged == false) 
			{
				if (PlayerAchievementScript.Instance.achievementBlinkMaster < 15)
				{
					PlayerAchievementScript.Instance.achievementBlinkMaster ++;
				}
				SoundManagerScript.Instance.PlaySFX (AudioClipID.SFX_SNAP);
				playerDodge = true;
			}
		}
	}

	void PlayerInvul () 
	{
		if (playerInvul == false) 
		{
			if (playerTookDamage == true) 
			{
				if (isGod)
				{
					return;
				}

				playerHP = playerHP - 1;
				playerInvul = true;
				playerTookDamage = false;
				playerInvulCounter = playerInvulDuration;
			}
		} 
		
		if (playerInvul == true) 
		{
			if (playerInvulCounter > playerInvulDuration * 0.75f)
			{
				playerSprite.color = new Color (1f, 0f, 0f, 1f);
			}
			else if (playerInvulCounter > playerInvulDuration * 0.5f)
			{
				playerSprite.color = new Color (1f, 1f, 1f, 1f);
			}
			else if (playerInvulCounter > playerInvulDuration * 0.25f)
			{
				playerSprite.color = new Color (1f, 1f, 1f, 1f);
			}
			else if (playerInvulCounter > 0f)
			{
				playerSprite.color = new Color (1f, 0f, 0f, 1f);
			}
			else 
			{
				playerSprite.color = new Color (1f, 1f, 1f, 1f);
				playerInvul = false;
			}
			playerInvulCounter -= Time.deltaTime;
		}
	}

	public void SlowPlayer (float slowValue, bool isSlowing)
	{
		if (isSlowing)
		{
			playerSpeed -= slowValue;
		}
		else
		{
			playerSpeed += slowValue;
		}
	}
}