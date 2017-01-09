using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Balloon : MonoBehaviour
{
	public GameObject	balloon;

	private float 		tic;
	private float		tac;
	private float		last;
	private int			breath;

	// Use this for initialization
	void Start ()
	{
		tic = 0;
		tac = 0;
		last = 0;
		breath = 5;
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3 scale = balloon.transform.localScale;

		if (Input.GetKeyDown (KeyCode.Space) && breath > 0)
		{
			if (scale.x < 7.5 && scale.y < 7.5 && scale.z < 7.5)
				balloon.transform.localScale += new Vector3 (0.1f, 0.1f, 0.1f);
			else
			{
				GameObject.Destroy (balloon);
				Debug.Log ("Ballon life time: " + Mathf.RoundToInt (Time.time));
			}

			breath--;
			tic = Time.time;
		}
		else if (tac - tic > 1 && tac - last > 0.05)
		{
			if (scale.x > 0.2 && scale.y > 0.2 && scale.z > 0.2)
			{
				balloon.transform.localScale += new Vector3 (-0.1f, -0.1f, -0.1f);
			}
			last = Time.time;
		}
		else if (tac - tic > 1)
		{
			breath++;
		}

		tac = Time.time - Time.deltaTime;
	}
}
