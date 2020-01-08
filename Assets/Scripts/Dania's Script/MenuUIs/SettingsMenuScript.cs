using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuScript : MonoBehaviour {

	public GameObject SettingsUI;

	public Toggle fullScreen;

	public Slider[] volumeSliders;
	GameObject bgmAudioSource;
	GameObject sfxAudioSource;
	AudioSource bgmSource;
	AudioSource sfxSource;

	public GameObject brightnessUI;
	Image brightness;
	public Slider brightnessSlider;
	Color alpha;

	// Use this for initialization
	void Start () 
	{
		bgmAudioSource = GameObject.Find("BGMAudioSource");
		bgmSource = bgmAudioSource.GetComponent<AudioSource>();

		sfxAudioSource = GameObject.Find("SFXAudioSource");
		sfxSource = sfxAudioSource.GetComponent<AudioSource>();

		volumeSliders [0].value = SoundManagerScript.Instance.bgmVolume;
		volumeSliders [1].value = SoundManagerScript.Instance.sfxVolume;

		brightness = brightnessUI.GetComponent<Image>();

		brightnessSlider.direction = Slider.Direction.RightToLeft;
		brightnessSlider.minValue = 0;
		brightnessSlider.maxValue = 0.5f;
		alpha = brightness.color;

		brightnessSlider.value = OtherManagerScript.Instance.brightVal;

		Back();
	}
	
	// Update is called once per frame
	void Update () 
	{
		/*if(SettingsUI.activeSelf)
		{
			if(Input.GetButtonDown("Pause"))
			{
				Back();
			}
		}*/	
	}

	public void SetFullScreen()
	{
		Screen.fullScreen = !Screen.fullScreen;
	}

	public void SetVolumeBGM()
	{
		bgmSource.volume = volumeSliders[0].value;
		SoundManagerScript.Instance.SetBGMVolume(bgmSource.volume);
	}

	public void SetVolumeSFX()
	{
		sfxSource.volume = volumeSliders[1].value;
		SoundManagerScript.Instance.SetSFXVolume(sfxSource.volume);
	}

	public void SetBrightnessValue()
	{
		alpha.a = brightnessSlider.value;
		brightness.color = alpha;
		OtherManagerScript.Instance.SetBrightness(alpha.a);
	}

	public void Settings()
	{
		SettingsUI.SetActive(true);
	}

	public void Back()
	{
		SettingsUI.SetActive(false);
	}
}
