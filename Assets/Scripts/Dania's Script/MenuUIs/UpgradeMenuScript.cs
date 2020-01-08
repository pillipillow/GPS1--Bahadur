using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpgradeMenuScript : MonoBehaviour {

	private static UpgradeMenuScript mInstance;
	public static UpgradeMenuScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag ("NPCUpgrade");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("_NPCUpgrade");
					mInstance = obj.AddComponent<UpgradeMenuScript>();
					obj.tag = "NPCUpgrade";
				}
				else 
				{
					mInstance = tempObject.GetComponent<UpgradeMenuScript>();
				}
			}
			return mInstance;
		}
	}
	public static bool CheckInstanceExist()
	{
		return mInstance;
	}


	public GameObject UpgradeUI;
	public GameObject[] UpgradeButtons;
	GameObject HeartUI;
	HealthUIScript heartScript;
	public static bool upgraded = false; //This use static instead of the singleton because the singletonn was implemented late
	public int uiNumber;

	private bool waitForPress;

	// Use this for initialization
	void Start () 
	{
		DisableUI();
		gameObject.GetComponent<Collider2D>().enabled = true;
		HeartUI = GameObject.Find("HealthUI");
		heartScript = HeartUI.GetComponent<HealthUIScript>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(waitForPress == true) 
		{
			if(Input.GetKeyDown(KeyCode.F))
			{
				EnabledUI();
				print("Button is pressed");
			}
		}

	}

	void EnabledUI()
	{
		UpgradeUI.SetActive(true);
		uiNumber = Random.Range(0,3);
		if(uiNumber == 0)
		{
			UpgradeButtons[0].SetActive(true);
			UpgradeButtons[1].SetActive(true);
			UpgradeButtons[2].SetActive(true);
			UpgradeButtons[3].SetActive(false);
			UpgradeButtons[4].SetActive(false);
		}
		else if(uiNumber == 1)
		{
			UpgradeButtons[0].SetActive(false);
			UpgradeButtons[1].SetActive(true);
			UpgradeButtons[2].SetActive(false);
			UpgradeButtons[3].SetActive(true);
			UpgradeButtons[4].SetActive(true);
		}
		else if(uiNumber == 2)
		{
			UpgradeButtons[0].SetActive(false);
			UpgradeButtons[1].SetActive(true);
			UpgradeButtons[2].SetActive(true);
			UpgradeButtons[3].SetActive(false);
			UpgradeButtons[4].SetActive(true);
		}
		upgraded = true;
		Time.timeScale = 0;
		print("Upgrade screen");
	}

	void DisableUI()
	{
		UpgradeUI.SetActive(false);
		upgraded = false;
		//Time.timeScale = 1;
	}

	public void DisableUpgrade()
	{
		DisableUI();
		waitForPress = false;
		//gameObject.GetComponent<Collider2D>().enabled = false;
		//Destroy(gameObject);
		this.gameObject.SetActive(false);
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			waitForPress =  true;
		}
	}

	void OnTriggerExit2D (Collider2D other)
	{
		if (other.name == "Player") 
		{
			waitForPress = false;
		}

		DisableUI();
	}

	public void HPButton()
	{
		if(!HeartUI)
		{
			return;
		}
		heartScript.AddHeartContainer();
		DisableUpgrade();
	}

	public void DMGButton()
	{
		Player.Instance.playerDamage++;
		DisableUpgrade();
	}

	public void SPDButton()
	{
		Player.Instance.playerSpeed += 0.1f;
		DisableUpgrade();
	}

	public void CRITCHANCE()
	{
		Player.Instance.critChance = Player.Instance.critChance + 3;
		DisableUpgrade();
	}

	public void CRITMULTIPLIER()
	{
		Player.Instance.critDamage = Player.Instance.critDamage + 2;
		DisableUpgrade();
	}

}
