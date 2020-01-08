using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Level02Script : MonoBehaviour {

	public Text text1;
	public Text text2;
	public Text text3;
	public bool canPress = false;

	IEnumerator Start()
	{
		text1.canvasRenderer.SetAlpha(0.0f);
		text2.canvasRenderer.SetAlpha(0.0f);
		text3.canvasRenderer.SetAlpha(0.0f);

		FadeIn(text1);
		yield return new WaitForSeconds(2.5f);
		FadeIn(text2);
		yield return new WaitForSeconds(2.5f);
		FadeIn(text3);
		canPress = true;
		yield return new WaitForSeconds(2.5f);
	}

	void Update()
	{
		if(canPress)
		{
			if(Input.GetKeyDown(KeyCode.Space))
			{
				FadeManager.Instance.FadeTo();
				Invoke("MainMenu",1.5f);
			}
		}
	}

	void FadeIn(Text text)
	{
		text.CrossFadeAlpha(1.0f,1.5f,false);

	}

	void FadeOut(Text text)
	{
		text.CrossFadeAlpha(0.0f,3f,false);
	}

	public void MainMenu()
	{
		SceneManagerScript.LoadMainMenuScene();
	}
}
