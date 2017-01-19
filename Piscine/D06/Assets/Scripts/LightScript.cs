using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LightScript : MonoBehaviour
{
	public Canvas canvas;
	public bool isACamera = false;
	public bool underFog = true;

	private float speed;
	private float tic = 0.0f;
	private float tac = 0.0f;

	void Start ()
	{
		if (this.isACamera)
			this.speed = 0.05f;
		else
			this.speed = 0.025f;

		if (this.underFog)
			this.speed /= 3;
	}

	void OnTriggerStay(Collider coll)
	{
		if (coll.name == "RigidBodyFPSController" && tac - tic > 0.01f)
		{
			if (this.canvas.GetComponentInChildren<Slider> ().value + this.speed >= 1.0f)
			{
				this.canvas.GetComponent<MainScript> ().GameOver ();
				return;
			}
			this.canvas.GetComponentInChildren<Slider> ().value += this.speed;
			tic = Time.time;
		}
		tac = Time.time;

		this.canvas.GetComponent<MainScript>().isIn = true;
	}

	void OnTriggerExit (Collider coll)
	{
		if (coll.name == "RigidBodyFPSController")
			this.canvas.GetComponent<MainScript>().isIn = false;
	}
}
