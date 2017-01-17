using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class clignotementScript : MonoBehaviour
{
	private float	tic;
	private float	tac;
	private bool	currentState;

	void Start ()
	{
		this.tic = 0;
		this.tac = 0;
		this.currentState = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Return))
		{
			Application.LoadLevel ("DataSelect");
		}

		if (this.tac - this.tic > 0.5)
		{
			this.GetComponent<Text> ().enabled = this.currentState;
			this.currentState = !this.currentState;
			this.tic = Time.time;
		}

		this.tac = Time.time;
	}
}
