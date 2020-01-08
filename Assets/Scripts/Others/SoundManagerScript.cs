using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum AudioClipID
{
	BGM_MAIN_MENU = 0,
	BGM_GAMEPLAY = 1,
	BGM_DEATH = 2,
	//BGM_LOSE = 3,
	//BGM_WIN = 4,

	SFX_UPGRADE = 94,
	SFX_PROJECTILE_HIT = 95,
	SFX_ATTACK_AIR = 96,
	SFX_ATTACK_EARTH = 97,
	SFX_ATTACK_WATER = 98,
	SFX_ATTACK_FIRE = 99,
	SFX_ITEM_DROP = 100,
	SFX_INPUT_CLICK = 101,
	SFX_ACTIVATE_PANEL = 102,
	SFX_SNAP = 103,

	TOTAL = 9001
}

[System.Serializable]
public class AudioClipInfo
{
	public AudioClipID audioClipID;
	public AudioClip audioClip;
}


public class SoundManagerScript : MonoBehaviour 
{
	private static SoundManagerScript mInstance;
	
	public static SoundManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				if(GameObject.FindWithTag("SoundManager") != null)
				{
					mInstance = GameObject.FindWithTag("SoundManager").GetComponent<SoundManagerScript>();
				}
				else 
				{
					GameObject obj = new GameObject("_SoundManager");
					mInstance = obj.AddComponent<SoundManagerScript>();
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

	public float bgmVolume = 1.0f;
	public float sfxVolume = 1.0f;

	
	public List<AudioClipInfo> audioClipInfoList = new List<AudioClipInfo>();
	
	public AudioSource bgmAudioSource;
	public AudioSource sfxAudioSource;
	
	public List<AudioSource> loopingSFXAudioSourceList = new List<AudioSource>();
	
	// Use this for initialization
	void Awake () 
	{
		if(SoundManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}
	}

	// Use this for initialization
	void Start () 
	{
		AudioSource[] audioSourceList = this.GetComponentsInChildren<AudioSource>();
		
		if(audioSourceList[0].gameObject.name == "BGMAudioSource")
		{
			bgmAudioSource = audioSourceList[0];
			sfxAudioSource = audioSourceList[1];
		}
		else 
		{
			bgmAudioSource = audioSourceList[1];
			sfxAudioSource = audioSourceList[0];
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
	
	AudioClip FindAudioClip(AudioClipID audioClipID)
	{
		for(int i=0; i<audioClipInfoList.Count; i++)
		{
			if(audioClipInfoList[i].audioClipID == audioClipID)
			{
				return audioClipInfoList[i].audioClip;
			}
		}

		Debug.LogError("Cannot Find Audio Clip : " + audioClipID);

		return null;
	}
	
	//! BACKGROUND MUSIC (BGM)
	public void PlayBGM(AudioClipID audioClipID)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}

		bgmAudioSource.clip = clipToPlay;
		bgmAudioSource.volume = bgmVolume;
		bgmAudioSource.loop = true;
		bgmAudioSource.Play();
	}

	public void PlayBGMWithFadeIn(AudioClipID audioClipID, float fadeInDuration)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}

		bgmAudioSource.loop = true;

		StartCoroutine(FadeIn(bgmAudioSource, clipToPlay, fadeInDuration, bgmVolume));
	}

	public void PlayBGMWithFadeOutIn(AudioClipID audioClipID, float fadeOutDuration, float fadeInDuration)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}

		bgmAudioSource.volume = bgmVolume;
		bgmAudioSource.loop = true;

		if(fadeOutDuration > 0.0f && fadeInDuration > 0.0f)
		{
			StartCoroutine(FadeOutIn(bgmAudioSource, clipToPlay, fadeOutDuration, fadeInDuration, bgmVolume));
		}
	}
	
	public void PauseBGM()
	{
		if(bgmAudioSource.isPlaying)
		{
			bgmAudioSource.Pause();
		}
	}
	
	public void StopBGM()
	{
		if(bgmAudioSource.isPlaying)
		{
			bgmAudioSource.Stop();
		}
	}
	
	
	//! SOUND EFFECTS (SFX)
	public void PlaySFX(AudioClipID audioClipID)
	{
		sfxAudioSource.PlayOneShot(FindAudioClip(audioClipID), sfxVolume);
	}
	
	public void PlayLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}
		
		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToPlay)
			{
				if(loopingSFXAudioSourceList[i].isPlaying)
				{
					return;
				}

				loopingSFXAudioSourceList[i].volume = sfxVolume;
				loopingSFXAudioSourceList[i].Play();
				return;
			}
		}
		
		AudioSource newInstance = sfxAudioSource.gameObject.AddComponent<AudioSource>();
		newInstance.playOnAwake = false;
		newInstance.clip = clipToPlay;
		newInstance.volume = sfxVolume;
		newInstance.loop = true;
		newInstance.Play();
		loopingSFXAudioSourceList.Add(newInstance);
	}

	public void PlayLoopingSFXWithFadeIn(AudioClipID audioClipID, float fadeInDuration)
	{
		AudioClip clipToPlay = FindAudioClip(audioClipID);

		if(clipToPlay == null)
		{
			return;
		}

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToPlay)
			{
				if(loopingSFXAudioSourceList[i].isPlaying)
				{
					return;
				}

				loopingSFXAudioSourceList[i].volume = sfxVolume;
				StartCoroutine(FadeIn(loopingSFXAudioSourceList[i], clipToPlay, fadeInDuration, sfxVolume));
				return;
			}
		}

		AudioSource newInstance = sfxAudioSource.gameObject.AddComponent<AudioSource>();
		newInstance.playOnAwake = false;
		newInstance.loop = true;

		StartCoroutine(FadeIn(newInstance, clipToPlay, fadeInDuration, sfxVolume));
		loopingSFXAudioSourceList.Add(newInstance);
	}
	
	public void PauseLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToPause = FindAudioClip(audioClipID);

		if(clipToPause == null)
		{
			return;
		}
		
		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToPause)
			{
				loopingSFXAudioSourceList[i].Pause();
				return;
			}
		}
	}	
	
	public void StopLoopingSFX(AudioClipID audioClipID)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		if(clipToStop == null)
		{
			return;
		}
		
		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToStop)
			{
				loopingSFXAudioSourceList[i].Stop();
				return;
			}
		}
	}

	public void ChangePitchLoopingSFX(AudioClipID audioClipID, float value)
	{
		AudioClip clipToStop = FindAudioClip(audioClipID);

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			if(loopingSFXAudioSourceList[i].clip == clipToStop)
			{
				loopingSFXAudioSourceList[i].pitch = value;
				return;
			}
		}
	}
	
	public void SetBGMVolume(float value)
	{
		bgmVolume = value;
	}
	
	public void SetSFXVolume(float value)
	{
		sfxVolume = value;
	}

	public void FadeOutBGM(float fadeOutDuration)
	{
		StartCoroutine(FadeOut(bgmAudioSource, fadeOutDuration));
	}

	public void FadeOutSFX(float fadeOutDuration)
	{
		StartCoroutine(FadeOut(sfxAudioSource, fadeOutDuration));
	}

	public void FadeOutAllSounds(float fadeOutDuration)
	{
		List<AudioSource> allAudioSourceList = new List<AudioSource>();

		allAudioSourceList.Add(bgmAudioSource);
		allAudioSourceList.Add(sfxAudioSource);
		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			allAudioSourceList.Add(loopingSFXAudioSourceList[i]);
		}

		StartCoroutine(FadeOutAll(allAudioSourceList, fadeOutDuration));
	}

	public void FadeOutAllLoopingSFX(float fadeOutDuration)
	{
		List<AudioSource> allAudioSourceList = new List<AudioSource>();

		for(int i=0; i<loopingSFXAudioSourceList.Count; i++)
		{
			allAudioSourceList.Add(loopingSFXAudioSourceList[i]);
		}

		StartCoroutine(FadeOutAll(allAudioSourceList, fadeOutDuration));
	}

	//! coroutines

	IEnumerator FadeOut(AudioSource audioSource, float fadeOutDuration)
	{
		float fadeOutTimer = 0.0f;
		float fadeOutSpeed = audioSource.volume / fadeOutDuration * Time.deltaTime;;

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.deltaTime;
			audioSource.volume -= fadeOutSpeed;
			yield return null;
		}
		audioSource.volume = 0.0f;
		audioSource.Stop();
	}

	IEnumerator FadeOutIn(AudioSource audioSource, AudioClip audioClip, float fadeOutDuration, float fadeInDuration, float maxVolume)
	{
		float fadeOutTimer = 0.0f;
		float originalVolume = audioSource.volume;
		float fadeOutSpeed = originalVolume / fadeOutDuration * Time.deltaTime;

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.deltaTime;
			audioSource.volume -= fadeOutSpeed;
			yield return null;
		}
		StartCoroutine(FadeIn(audioSource, audioClip, fadeInDuration, maxVolume));
	}

	IEnumerator FadeIn(AudioSource audioSource, AudioClip audioClip, float fadeInDuration, float maxVolume)
	{
		audioSource.clip = audioClip;
		audioSource.volume = 0.0f;
		audioSource.Play();

		float fadeInTimer = 0.0f;
		float fadeInSpeed = maxVolume / fadeInDuration * Time.deltaTime;

		while(fadeInTimer < fadeInDuration)
		{
			fadeInTimer += Time.deltaTime;
			audioSource.volume += fadeInSpeed;
			yield return null;
		}
		audioSource.volume = maxVolume;
	}

	IEnumerator FadeOutAll(List<AudioSource> audioSourceList, float fadeOutDuration)
	{
		float fadeOutTimer = 0.0f;
		List<float> fadeOutSpeedList = new List<float>();

		for(int i=0; i<audioSourceList.Count; i++)
		{
			fadeOutSpeedList.Add(audioSourceList[i].volume / fadeOutDuration * Time.deltaTime);
		}

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.deltaTime;
			for(int i=0; i<audioSourceList.Count; i++)
			{
				audioSourceList[i].volume -= fadeOutSpeedList[i];
			}
			yield return null;
		}
		for(int i=0; i<audioSourceList.Count; i++)
		{
			audioSourceList[i].volume = 0.0f;
			audioSourceList[i].Stop();
		}
	}

	IEnumerator FadeOutInAll(List<AudioSource> audioSourceList, float fadeOutDuration, float fadeInDuration)
	{
		float fadeOutTimer = 0.0f;
		List<float> fadeOutSpeedList = new List<float>();
		List<float> maxVolumeList = new List<float>();

		for(int i=0; i<audioSourceList.Count; i++)
		{
			fadeOutSpeedList.Add(audioSourceList[i].volume / fadeOutDuration * Time.deltaTime);
			maxVolumeList.Add(audioSourceList[i].volume);
		}

		while(fadeOutTimer < fadeOutDuration)
		{
			fadeOutTimer += Time.deltaTime;
			for(int i=0; i<audioSourceList.Count; i++)
			{
				audioSourceList[i].volume -= fadeOutSpeedList[i];
			}
			yield return null;
		}
		StartCoroutine(FadeInAll(audioSourceList, fadeInDuration, maxVolumeList));
	}

	IEnumerator FadeInAll(List<AudioSource> audioSourceList, float fadeInDuration, List<float> maxVolumeList)
	{
		float fadeInTimer = 0.0f;
		List<float> fadeInSpeedList = new List<float>();

		for(int i=0; i<audioSourceList.Count; i++)
		{
			audioSourceList[i].volume = 0.0f;
			audioSourceList[i].Play();
			fadeInSpeedList.Add(maxVolumeList[i] / fadeInDuration * Time.deltaTime);
		}

		while(fadeInTimer < fadeInDuration)
		{
			fadeInTimer += Time.deltaTime;
			for(int i=0; i<audioSourceList.Count; i++)
			{				
				audioSourceList[i].volume += fadeInSpeedList[i];
			}
			yield return null;
		}
		for(int i=0; i<audioSourceList.Count; i++)
		{
			audioSourceList[i].volume = maxVolumeList[i];
		}
	}
}