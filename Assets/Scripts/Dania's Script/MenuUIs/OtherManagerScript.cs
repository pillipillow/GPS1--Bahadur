using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OtherManagerScript : MonoBehaviour {

	//Was suppose to be the GameManager but Kerk needs it but now he doesn't so I don't bother changing it
	private static OtherManagerScript mInstance;
	public static OtherManagerScript Instance
	{
		get
		{
			if(mInstance == null)
			{
				GameObject tempObject = GameObject.FindWithTag("OtherManager");

				if(tempObject == null)
				{
					GameObject obj = new GameObject("OtherManager");
					mInstance = obj.AddComponent<OtherManagerScript>();
					obj.tag = "OtherManager";
				}
				else
				{
					mInstance = tempObject.GetComponent<OtherManagerScript>();
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

	[HideInInspector]public float brightVal = 0;
	public int level = 0;

	// Use this for initialization
	void Awake () 
	{
		if(OtherManagerScript.CheckInstanceExist())
		{
			Destroy(this.gameObject);
		}	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SetBrightness(float value)
	{
		brightVal = value;
	}
}
