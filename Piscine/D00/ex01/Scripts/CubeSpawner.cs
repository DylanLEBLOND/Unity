using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{

	public float		frequency;
	public GameObject	left;
	public GameObject	middle;
	public GameObject	right;

	private float tic;
	private float tac;

	void start ()
	{
		tic = 0;
		tac = frequency + 1;
	}

	// Update is called once per frame
	void Update ()
	{
		if (tac - tic > frequency)
		{
			switch (Random.Range (1, 4))
			{
				case 1:
					GameObject.Instantiate (left);
					break;
				case 2:
					GameObject.Instantiate (middle);
						break;
				case 3:
					GameObject.Instantiate (right);
					break;
			}
			tic = Time.time;
		}

		tac = Time.time;
	}
}
