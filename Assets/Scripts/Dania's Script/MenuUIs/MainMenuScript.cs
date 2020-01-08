using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuScript : MonoBehaviour {

	public GameObject creditsUI;

	// Use this for initialization
	void Start () 
	{
		if(OtherManagerScript.Instance.level > 0)
		{
			SoundManagerScript.Instance.PlayBGM(AudioClipID.BGM_MAIN_MENU);
		}
		OtherManagerScript.Instance.level = 0;
		CloseCredits();
		Destroy(Player.Instance.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StartLevel()
	{
		StartCoroutine(FadeManager.Instance.Fade(1));
		//FadeManager.Instance.FadeTo();
		/*Invoke("LoadTutorial",1.5f);*/
		LoadTutorial();

	}

	public void Quit()
	{
		Application.Quit();
	}

	public void Credits()
	{
		creditsUI.SetActive(true);
	}

	public void CloseCredits()
	{
		creditsUI.SetActive(false);
	}

	public void LoadTutorial()
	{
		SceneManagerScript.LoadTutorialScene();
	}
}
