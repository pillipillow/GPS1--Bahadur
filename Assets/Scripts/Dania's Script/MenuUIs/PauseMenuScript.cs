using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour {

	public GameObject PlayerPrefab;
	public GameObject PauseUI;
	public GameObject QuitPanelUI;
	public GameObject SettingsUI;
	public static bool paused = false;
	public float switchTimer = 1.5f;

	private bool upgrade = false;

	// Use this for initialization
	void Start () 
	{
		PauseUI.SetActive(false);
		QuitPanelUI.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () 
	{
		upgrade = UpgradeMenuScript.upgraded;
		if(SettingsUI.activeSelf || QuitPanelUI.activeSelf || upgrade)
		{
			return;
		}
	
		if(Input.GetButtonDown("Pause"))
		{
			paused = !paused;
		}

		if(paused)
		{
			PauseUI.SetActive(true);
			Time.timeScale = 0f;
		}
		if(!paused)
		{
			PauseUI.SetActive(false);
			Time.timeScale = 1f;
		}
	}

	public void Resume()
	{
		paused = false;
	}

	public void Restart()
	{
		FadeManager.Instance.FadeTo();
		Invoke("RestartScene",1.5f);
		paused = false;
	}

	public void Quit()
	{
		QuitPanelUI.SetActive(true);
	}

	public void Yes()
	{
		FadeManager.Instance.FadeTo();
		Invoke("MainMenu",switchTimer);
		QuitPanelUI.SetActive(false);
		paused = false;
	}

	public void No ()
	{
		QuitPanelUI.SetActive(false);
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
