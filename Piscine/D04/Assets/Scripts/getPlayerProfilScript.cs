using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class getPlayerProfilScript : MonoBehaviour
{
	private int				currentLevel;
	private Vector3[]		levelPos;

	private void setUserStats (Transform PanelUser)
	{
		PanelUser.transform.GetChild (0).gameObject.transform.GetChild(0).gameObject.GetComponent<Text> ().text = PlayerPrefs.GetInt ("Lifes").ToString();
		PanelUser.transform.GetChild (1).gameObject.transform.GetChild(0).gameObject.GetComponent<Text> ().text = PlayerPrefs.GetInt ("Rings").ToString();
	}

	private void printLevelInfo (int level)
	{
		Transform Level = this.transform.GetChild (2).gameObject.transform.GetChild (level - 1);

		this.transform.GetChild (1).gameObject.transform.position = Level.gameObject.transform.position;
		this.transform.GetChild (3).gameObject.GetComponent<Text> ().text = Level.gameObject.name;
		this.transform.GetChild (4).gameObject.GetComponent<Text> ().text = "High Score: " + PlayerPrefs.GetInt("Level" + level.ToString() + "Score").ToString();
	}

	private void setLevelInfo (GameObject Levels)
	{
		int i;
		Transform level;

		for (i = 0; i < Levels.transform.childCount; i++)
		{
			level = Levels.transform.GetChild(i);
			if (PlayerPrefs.GetInt ("Unlock" + (i + 1).ToString()) == 0)
			{
				level.gameObject.GetComponent<Image> ().color -= new Color (0, 0, 0, 0.5f);
				level.gameObject.GetComponent<Button> ().enabled = false;
			}
		}
	}

	public void loadLevel (string levelName)
	{
		Application.LoadLevel (levelName);
	}

	void Start ()
	{
		this.currentLevel = 1;

		Debug.Log ("Begin");
		if (PlayerPrefs.GetString ("user") == "Player1")
		{
			Debug.Log ("Je suis la");
			this.setUserStats (this.transform.GetChild (0));
			this.setLevelInfo (this.transform.GetChild(2).gameObject);
			this.printLevelInfo (1);
		}
		Debug.Log ("Je sors (user name: " + PlayerPrefs.GetString ("user"));
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.UpArrow) &&
		    (this.currentLevel != 1 && this.currentLevel != 4 && this.currentLevel != 7))
		{
			this.currentLevel -= 1;
			this.printLevelInfo (this.currentLevel);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow) &&
			(this.currentLevel != 3 && this.currentLevel != 6 && this.currentLevel != 9))
		{
			this.currentLevel += 1;
			this.printLevelInfo (this.currentLevel);
		}
		if (Input.GetKeyDown (KeyCode.LeftArrow) &&
			(this.currentLevel != 1 && this.currentLevel != 2 && this.currentLevel != 3))
		{
			this.currentLevel -= 3;
			this.printLevelInfo (this.currentLevel);
		}
		if (Input.GetKeyDown (KeyCode.RightArrow) &&
			(this.currentLevel != 7 && this.currentLevel != 8 && this.currentLevel != 9))
		{
			this.currentLevel += 3;
			this.printLevelInfo (this.currentLevel);
		}
	}
}
