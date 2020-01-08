using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAchievementScript : MonoBehaviour 
{
	private static PlayerAchievementScript mInstance;
	public static PlayerAchievementScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				if(GameObject.FindWithTag("AchievementsManager") != null)
				{
					mInstance = GameObject.FindWithTag("AchievementsManager").GetComponent<PlayerAchievementScript>();
				}
				else 
				{
					GameObject obj = new GameObject("_AchievementsManager");
					mInstance = obj.AddComponent<PlayerAchievementScript>();
				}
				DontDestroyOnLoad(mInstance.gameObject);
			}
			return mInstance;
		}
	}
	public static bool CheckInstanceExist()
	{
		return mInstance;
	}

	//! For player achievements
	/* Need Remember to reset when entering next level for some achievements */
	public int achievementPyromaniac;
	public int achievementStormBringer;
	public int achievementLandBreaker;
	public int achievementTidalLord;
	public int achievementBlinkMaster;
	public int achievementExplorer;
	public bool achievementImmortal;
	public bool achievementWhoNeedsHealing;
	public bool achievementTheDeadShallRest;
	public bool achievementFallenBeast;

	void Start ()
	{
		achievementPyromaniac = 0;
		achievementStormBringer = 0;
		achievementLandBreaker = 0;
		achievementTidalLord = 0;
		achievementBlinkMaster = 0;
		achievementExplorer = 0;
		achievementImmortal = true;
		achievementWhoNeedsHealing = true;
		achievementTheDeadShallRest = false;
		achievementFallenBeast = false;
	}
}
