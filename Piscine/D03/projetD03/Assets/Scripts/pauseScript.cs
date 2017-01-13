using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class pauseScript : MonoBehaviour
{
	public gameManager	manager;

	private bool		isPaused;

	public void		showPanel (bool state)
	{
		Transform butTransform;
		int i;


		Debug.Log ("state = " + state);
		for (i = 0; i < this.transform.childCount; i++)
		{
			butTransform = this.transform.GetChild (i);
			butTransform.gameObject.SetActive (state);
		}
		this.GetComponent <Image> ().enabled = state;
		Debug.Log ("show panel ? " + this.GetComponent <Image> ().enabled);
	}

	public void		startPause ()
	{
		this.manager.pause (true);
		this.showPanel (true);
		this.isPaused = true;
	}
		
	public void		stopPause ()
	{
		this.manager.pause (false);
		this.showPanel (false);
		this.isPaused = false;
	}

	void Start ()
	{
		this.stopPause();
	}

	void Update ()
	{
		if (!this.isPaused && Input.GetKeyDown (KeyCode.Escape))
			this.startPause ();
	}
}
