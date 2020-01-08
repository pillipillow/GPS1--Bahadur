using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverMenuScript : MonoBehaviour {

	public GameObject gameOverUI;

	GameObject GameplayUI;
	bool paused = false; 

	void Start()
	{
		gameOverUI.SetActive(false);
		GameplayUI = GameObject.Find("GameplayUI");
	}

	void Update()
	{
		if (Player.Instance.playerIsDead == true)
		{
			paused = true;
			gameOverUI.SetActive(true);
			GameplayUI.GetComponent<GameCursorScript>().enabled = false;
		}

		if(paused)
		{
			Time.timeScale = 0f;
		}
		if(!paused)
		{
			Time.timeScale = 1f;
		}
	}

	public void Restart()
	{
		FadeManager.Instance.FadeTo();
		Invoke("RestartScene",1.5f);
		paused = false;
	}

	public void Quit()
	{
		FadeManager.Instance.FadeTo();
		Invoke("MainMenu",1.5f);
		paused = false;
	}

	public void MainMenu()
	{
		SceneManagerScript.LoadMainMenuScene();
	}

	public void RestartScene()
	{
		SceneManagerScript.LoadCurrentScene();
	}
}
