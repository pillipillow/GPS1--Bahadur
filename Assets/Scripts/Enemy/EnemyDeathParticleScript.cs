using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDeathParticleScript : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		Destroy (this.gameObject, 2f);
	}
	
	public void SetColor (int index)
	{
		/*ParticleSystem.Particle[] particles = new ParticleSystem.Particle[GetComponent<ParticleSystem> ().particleCount];
		var particleCount = GetComponent<ParticleSystem> ().GetParticles (particles);
		int i = 0;
		Debug.Log ("Particle array : " + particles.Length);
		Debug.Log ("Particle count : " + particleCount);
		while (i < particleCount)
		{
			Debug.Log ("Color Index : " + index);
			if (index == 0)
			{
				particles [i].startColor = Color.red;
			}
			else if (index == 1)
			{
				particles [i].startColor = Color.blue;
			}
			else if (index == 2)
			{
				particles [i].startColor = Color.white;
			}
			else if (index == 3)
			{
				particles [i].startColor = Color.yellow;
			}

			i++;
		};
		*/

		ParticleSystem ps = GetComponent<ParticleSystem> ();
		ParticleSystem.MainModule psm = ps.main;

		if (index == 0)
		{
			psm.startColor = new ParticleSystem.MinMaxGradient(Color.red);		
		}
		else if (index == 1)
		{
			psm.startColor = new ParticleSystem.MinMaxGradient(Color.blue);
		}
		else if (index == 2)
		{
			psm.startColor = new ParticleSystem.MinMaxGradient(Color.white);
		}
		else if (index == 3)
		{
			psm.startColor = new ParticleSystem.MinMaxGradient(Color.yellow);
		}


	}
}
