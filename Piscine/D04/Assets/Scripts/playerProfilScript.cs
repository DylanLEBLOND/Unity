using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerProfilScript : MonoBehaviour
{
	// Use this for initialization
	void Start ()
	{
		if (PlayerPrefs.HasKey ("user"))
		{
			Debug.Log ("Deja un User (<" + PlayerPrefs.GetString ("user") + ">");
			return;
		}

		PlayerPrefs.SetString ("user", "Player1");
		PlayerPrefs.SetInt ("Unlock1", 1);
		PlayerPrefs.SetInt ("Unlock2", 2);
		PlayerPrefs.SetInt ("Unlock3", 3);
		PlayerPrefs.SetInt ("Unlock4", 0);
		PlayerPrefs.SetInt ("Unlock5", 0);
		PlayerPrefs.SetInt ("Unlock6", 0);
		PlayerPrefs.SetInt ("Unlock7", 0);
		PlayerPrefs.SetInt ("Unlock8", 0);
		PlayerPrefs.SetInt ("Unlock9", 0);
		PlayerPrefs.SetInt ("Lifes", 42);
		PlayerPrefs.SetInt ("Rings", 9000);
		PlayerPrefs.SetInt ("Level1Score", 42);
		PlayerPrefs.SetInt ("Level2Score", 72);
		PlayerPrefs.SetInt ("Level3Score", 90);
		PlayerPrefs.SetInt ("Level4Score", 0);
		PlayerPrefs.SetInt ("Level5Score", 0);
		PlayerPrefs.SetInt ("Level6Score", 0);
		PlayerPrefs.SetInt ("Level4Score", 7);
		PlayerPrefs.SetInt ("Level5Score", 8);
		PlayerPrefs.SetInt ("Level6Score", 9);
	}

	public void saveUserPref()
	{
		PlayerPrefs.Save ();
	}

	public void deleteUserPref()
	{
		PlayerPrefs.DeleteAll ();
		PlayerPrefs.SetString ("user", "Player1");
		PlayerPrefs.SetInt ("Unlock1", 1);
		PlayerPrefs.SetInt ("Unlock2", 0);
		PlayerPrefs.SetInt ("Unlock3", 0);
		PlayerPrefs.SetInt ("Unlock4", 0);
		PlayerPrefs.SetInt ("Unlock5", 0);
		PlayerPrefs.SetInt ("Unlock6", 0);
		PlayerPrefs.SetInt ("Unlock7", 0);
		PlayerPrefs.SetInt ("Unlock8", 0);
		PlayerPrefs.SetInt ("Unlock9", 0);
		PlayerPrefs.SetInt ("Lifes", 0);
		PlayerPrefs.SetInt ("Rings", 0);
		PlayerPrefs.SetInt ("Level1Score", 0);
		PlayerPrefs.SetInt ("Level2Score", 0);
		PlayerPrefs.SetInt ("Level3Score", 0);
		PlayerPrefs.SetInt ("Level4Score", 0);
		PlayerPrefs.SetInt ("Level5Score", 0);
		PlayerPrefs.SetInt ("Level6Score", 0);
		PlayerPrefs.SetInt ("Level4Score", 0);
		PlayerPrefs.SetInt ("Level5Score", 0);
		PlayerPrefs.SetInt ("Level6Score", 0);
	}
}
