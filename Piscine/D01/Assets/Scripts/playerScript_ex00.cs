using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript_ex00 : MonoBehaviour
{
	private bool	isActif;

	void Start ()
	{
		Physics2D.gravity = new Vector2 (0, -5);
		this.isActif = false;
	}

	public void reset (Vector3 pos)
	{
		this.transform.position = pos;
		this.isActif = false;
	}

	public void setIsActif (bool isActif)
	{
		this.isActif = isActif;
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.isActif)
		{
			Rigidbody2D current = this.GetComponent<Rigidbody2D>();

			if (Input.GetKey (KeyCode.RightArrow))
			{
				this.transform.position += new Vector3 (0.1f, 0, 0);
			}
			if (Input.GetKey (KeyCode.LeftArrow))
			{
				this.transform.position -= new Vector3 (0.1f, 0, 0);
			}
			if (Input.GetKeyDown (KeyCode.Space) && current.velocity == Vector2.zero)
			{
				current.AddForce(Vector3.up * 250);
			}
		}
	}
}
