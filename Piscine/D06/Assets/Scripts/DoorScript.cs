using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	private bool doorOpen = false;

	void Start ()
	{
		this.transform.GetChild(0).gameObject.SetActive (true);
	}

	public bool openTheDoor (bool gotTheKey)
	{
		if (gotTheKey && !this.doorOpen)
		{
			this.transform.GetChild (3).gameObject.GetComponent<AudioSource> ().Play ();
			this.transform.GetChild (0).gameObject.SetActive (false);
			this.doorOpen = true;
		}
		else
			this.transform.GetChild (2).gameObject.GetComponent<AudioSource> ().Play ();

		return this.doorOpen;
	}

	public void closeTheDoor ()
	{
		this.transform.GetChild(0).gameObject.SetActive (true);
	}

	public void reset ()
	{
		this.doorOpen = false;
		this.transform.GetChild(0).gameObject.SetActive (true);
		this.transform.GetChild(0).gameObject.transform.GetChild(0).gameObject.transform.GetChild(1).gameObject.SetActive (true);
	}
}
