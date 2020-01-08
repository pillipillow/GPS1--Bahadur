using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneManagerScript : MonoBehaviour {

	public static void LoadScene(string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}

	public static void LoadCurrentScene()
	{
		string currentScene = SceneManager.GetActiveScene().name;
		LoadScene(currentScene);

		if (Player.Instance.level == 2)
		{
			Player.Instance.playerDamage = Player.Instance.playerDamageStore;
			Player.Instance.playerSpeed = Player.Instance.playerSpeedStore;
			Player.Instance.playerHPMax = Player.Instance.playerHPMaxStore;
			Player.Instance.critChance = Player.Instance.playerCritChanceStore;
			Player.Instance.critDamage = Player.Instance.playerCritDamageStore;

			Player.Instance.playerCantShoot = false;
			Player.Instance.playerHP = Player.Instance.playerHPMax;
			Player.Instance.playerIsDead = false;
			Player.Instance.playerDeathDuration = 4f;
			Player.Instance.transform.position = new Vector2 (2.5f, 3f);
		}
	}

	public static void LoadMainMenuScene()
	{
		Destroy (Player.Instance.gameObject);
		LoadScene("MainMenu");
	}

	/*public static void LoadGameOverScene()
	{
		LoadScene("GameOver");
	}*/

	public static void LoadTutorialScene()
	{
		LoadScene("TutorialScene");	
	}

	public static void LoadLevel01()
	{
		LoadScene("Level01");
	}

	public static void LoadLevel02()
	{
		LoadScene("Level02");
	}

}
