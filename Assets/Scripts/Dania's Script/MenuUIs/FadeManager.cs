using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FadeManager : MonoBehaviour {

	private static FadeManager mInstance;
	public static FadeManager Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("FadeManager");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("FadeManager");
					mInstance = obj.AddComponent<FadeManager>();
					obj.tag = "FadeManager";
				}
				else
				{
					mInstance = tempObject.GetComponent<FadeManager>();
				}
			}
			return mInstance;
		}
	}

	public Image fadeImage;
	public float crossfadeSpeed = 1.5f;
	public float fadeBetween = 3f;

	public int fadeDir = -1; //in = -1, out = 1

	void Start()
	{
		fadeImage.enabled = true;
		//fadeImage.canvasRenderer.SetAlpha(1.0f);
		StartCoroutine(Fade(1));
	}

	public void FadeTo()
	{
		StartCoroutine(Fade(-1));
	}
	
	public IEnumerator Fade(int fadeDir)
	{
		if(fadeDir == -1)
		{
			fadeImage.canvasRenderer.SetAlpha(0.0f);
			fadeImage.CrossFadeAlpha(1.0f,crossfadeSpeed,false);
			yield return new WaitForSeconds(fadeBetween);
			print("Fade in");
		}
		else if(fadeDir == 1)
		{
			fadeImage.canvasRenderer.SetAlpha(1.0f);
			fadeImage.CrossFadeAlpha(0.0f,crossfadeSpeed,false);
			yield return new WaitForSeconds(fadeBetween);
			print("Fade Out");
		}
	}


	/*void OnEnable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable()
	{
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
	{
		Fade(-1);
	}*/
}
