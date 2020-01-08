using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueScript : MonoBehaviour {

	public GameObject panelBox;
	public Text theText;
	public TextAsset textFile;

	public string[] textLines;
	public int startLine;
	public int currentLine;
	public int endAtLine;

	public bool isActive;
	public bool destroyWhenActivated; //for script once only
	public bool requireButtonPress;
	private bool waitForPress;
	public bool stopPlayerMovement;

	// Use this for initialization
	void Start () 
	{
		if(textFile !=null)
		{
			textLines = (textFile.text.Split('\n'));
		}

		if(endAtLine == 0)
		{
			endAtLine = textLines.Length - 1;
		}

		if(isActive)
		{
			EnableTextBox();
		}
		else
		{
			DisableTextBox();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!isActive)
		{
			return;
		}
	
		theText.text = textLines[currentLine];

		if(waitForPress == true) 
		{
			if(Input.GetKeyDown(KeyCode.F))
			{
				EnableTextBox();
				print("Button is pressed");
			}
		}

		if(currentLine>endAtLine) //Currentline above the endline
		{
			DisableTextBox();
			currentLine = startLine; //reset to the start line
		}
		else if(Input.GetKeyDown(KeyCode.Space)) //current line cyclces through the number line
		{
			currentLine++;
		}
	}

	public void EnableTextBox()
	{
		panelBox.SetActive(true);
		isActive = true;
		if(stopPlayerMovement)
		{
			Player.Instance.talking = true;
		}
	}

	public void DisableTextBox()
	{
		panelBox.SetActive(false);
		isActive = false;
		Player.Instance.talking = false;
		currentLine = startLine;
	}

	void OnTriggerEnter2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			if(requireButtonPress)
			{
				return;
			}

			EnableTextBox();
			print("Player Enters");
		}
	}

	void OnTriggerStay2D(Collider2D other)
	{
		if(other.name == "Player")
		{
			if(requireButtonPress)
			{
				waitForPress =  true;
				isActive = true;
				print("Player interact");
				return;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other)
	{
		if (other.name == "Player") 
		{
			waitForPress = false;
			DisableTextBox();

			if(destroyWhenActivated) //for one time event once only Ie. NPC Yelling
			{
				Destroy(gameObject);
			}
		}
		print("Player Exit");
	}

}



