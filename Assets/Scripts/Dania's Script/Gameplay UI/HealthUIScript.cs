using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIScript : MonoBehaviour {

	private int maxHeartAmount = 10; //the maximumm health container can appear
	public float startHearts = 3; //the initial health amount of hearts
	public float currentHealth;
	private float maxHealth;
	private int healthPerHeart = 2; //pieces of the heart (ie. half heart = 1, full heart = 2)

	public Image[] healthImages;
	public Sprite[] healthSprites;

	bool addHearts;

	void Start () 
	{
		startHearts =  Player.Instance.playerHP/2; //ie. 6pt = 3hearts * 2pts
		maxHealth = maxHeartAmount * healthPerHeart; // ie. 20pts = 10 hearts * 2 pts
		checkHealthAmount();
	}

	void checkHealthAmount()
	{
		for(int i = 0; i < maxHeartAmount; i++)
		{
			if(startHearts <= i)
			{
				healthImages[i].enabled = false;
			}
			else
			{
				healthImages[i].enabled = true;
			}
		}
		UpdateHearts();
	}

	void Update()
	{
		currentHealth = Player.Instance.playerHP;
		TakeDamage(currentHealth);
		UpdateHearts();
		checkHealthAmount();
	}

	void UpdateHearts()
	{
		bool empty = false;
		int i = 0;

		foreach(Image image in healthImages)
		{
			if(empty)
			{
				image.sprite = healthSprites [0];
			}
			else
			{
				i++;
				if(currentHealth>= i * healthPerHeart)
				{
					image.sprite = healthSprites[healthSprites.Length - 1];
				}
				else
				{
					int currentHearthHealth = (int)(healthPerHeart - (healthPerHeart*i - currentHealth));
					int healthPerImage =  healthPerHeart/(healthSprites.Length - 1);
					int imageIndex = currentHearthHealth / healthPerImage;
					image.sprite = healthSprites [imageIndex];
					empty = true;
				}
			}
		}
	}

	public void TakeDamage(float updateHealth)
	{
		//currentHealth += amount;
		updateHealth = Mathf.Clamp(currentHealth,0,startHearts*healthPerHeart);
		UpdateHearts();
	}

	public void AddHeartContainer()
	{
		if(Player.Instance.playerHPMax == maxHealth)
		{
			return;
		}
		startHearts++;
		Player.Instance.playerHP = Mathf.Clamp(currentHealth,0,startHearts*maxHeartAmount);

		Player.Instance.playerHP = startHearts * healthPerHeart; 
		Player.Instance.playerHPMax = startHearts * healthPerHeart;

		checkHealthAmount();
	}
}
