using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerScript_ex01 : MonoBehaviour
{
	private bool	isActif;
	private bool	isOut;

	void Start ()
	{
		Physics2D.gravity = new Vector2 (0, -5);
		this.isActif = false;
		this.isOut = false;
	}

	public void reset (Vector3 pos)
	{
		this.transform.position = pos;
		this.isActif = false;
		this.isOut = false;
	}

	public void setIsActif (bool isActif)
	{
		this.isActif = isActif;
	}
		
	void OnTriggerEnter2D (Collider2D collider)
	{
		if (collider.tag == this.tag)
			this.isOut = true;
	}

	void OnTriggerExit2D (Collider2D collider)
	{
		if (collider.tag == this.tag)
			this.isOut = false;
	}

	public bool getIsOut()
	{
		return this.isOut;
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
