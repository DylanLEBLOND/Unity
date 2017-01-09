using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
	private float		frequency;
	private float		tic;
	private float		tac;

	// Use this for initialization
	void Start ()
	{
		frequency = Random.Range (0.001f, 0.01f);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.transform.localPosition.y < -4.0)
		{
			GameObject.Destroy (this.gameObject);
			return;
		}

		if (this.gameObject.name.Equals ("cubeA prefab(Clone)") && Input.GetKeyDown (KeyCode.A))
		{
			if (Mathf.Abs (-4.0f - this.transform.localPosition.y) < 1.0f)
			{
				GameObject.Destroy (this.gameObject);
				return;
			}
			else
			{
				Debug.Log ("Precision: " + Mathf.Abs (-4.0f - this.transform.localPosition.y));
			}
		}
		if (this.gameObject.name.Equals ("cubeS prefab(Clone)") && Input.GetKeyDown (KeyCode.S))
		{
			if (Mathf.Abs (-4.0f - this.transform.localPosition.y) < 1.0f)
			{
				GameObject.Destroy (this.gameObject);
				return;
			}
			else
			{
				Debug.Log ("Precision: " + Mathf.Abs (-4.0f - this.transform.localPosition.y));
			}
		}
		if (this.gameObject.name.Equals ("cubeD prefab(Clone)") && Input.GetKeyDown (KeyCode.D))
		{
			if (Mathf.Abs (-4.0f - this.transform.localPosition.y) < 1.0f)
			{
				GameObject.Destroy (this.gameObject);
				return;
			}
			else
			{
				Debug.Log ("Precision: " + Mathf.Abs (-4.0f - this.transform.localPosition.y));
			}
		}

		if (tac - tic > frequency)
		{
			if (this.transform.localPosition.y - 0.05f < -4.0f)
				this.transform.localPosition = new Vector3 (this.transform.localPosition.x, -4.0f, this.transform.localPosition.y);
			else
				this.transform.localPosition -= new Vector3 (0, 0.05f, 0);
			tic = Time.time;
		}

		tac = Time.time;
	}
}
