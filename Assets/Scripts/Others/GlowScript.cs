using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlowScript : MonoBehaviour {


	ShootElement shootScript;
	string currentElement;
	string prevElement;
	ParticleSystem emitter;
	ParticleSystem tempParticle;
	Gradient grad;

	Color chosenColor;
	private string playerElement;

	// Use this for initialization
	void Start () 
	{
		chosenColor = Color.red;
		//GameObject shootElement = GameObject.Find("Shoot Element");
		//grad = new Gradient();
		//shootScript = shootElement.GetComponent<ShootElement>();

	}

	void ChangeColor()
	{
		//currentElement = shootScript.shootElement;
		ParticleSystem.Particle[] particles = new ParticleSystem.Particle[GetComponent<ParticleSystem>().particleCount];
		var particleCount = GetComponent<ParticleSystem>().GetParticles(particles);
		int i = 0;

		while (i < particleCount)
		{

			particles[i].startColor = chosenColor;
				
			i++;
			GetComponent<ParticleSystem>().SetParticles(particles, particleCount);
		}

	}
	// Update is called once per frame
	void Update () 
	{
		ChangeColor();
		playerElement = Player.Instance.element;

		if (playerElement == "Fire")
		{
			chosenColor = Color.red;
		}

		else if (playerElement == "Air")
		{
			chosenColor = Color.white;
		}

		else if (playerElement == "Earth")
		{
			chosenColor = Color.green;
		}

		else if (playerElement == "Water")
		{
			chosenColor = new Color(0.3f, 0.7f, 1f);
		}
	

		/*
		if (currentElement == "Water")
		{
			Debug.Log(currentElement);
			var color = GetComponent<ParticleSystem>().colorOverLifetime.color;
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.blue, 0.1f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
			color = new ParticleSystem.MinMaxGradient(grad);

			Debug.Log(color);
		}

		else if (currentElement == "Fire")
		{
			var color = GetComponent<ParticleSystem>().colorOverLifetime.color;
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.red, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
			color = new ParticleSystem.MinMaxGradient(grad);

		}

		else if (currentElement == "Air")
		{

			var color = GetComponent<ParticleSystem>().colorOverLifetime.color;
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.white, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
			color = new ParticleSystem.MinMaxGradient(grad);
		}

		else if (currentElement == "Earth")
		{

			var color = GetComponent<ParticleSystem>().colorOverLifetime.color;
			grad.SetKeys( new GradientColorKey[] { new GradientColorKey(Color.green, 0.0f), new GradientColorKey(Color.white, 1.0f) }, new GradientAlphaKey[] { new GradientAlphaKey(1.0f, 0.0f), new GradientAlphaKey(0.0f, 1.0f) } );
			color = new ParticleSystem.MinMaxGradient(grad);
		}*/
	}
}
