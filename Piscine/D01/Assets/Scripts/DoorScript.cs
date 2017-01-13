using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
	public Color		thomasColor = new Color (255, 255, 255, 255);
	public Color		johnColor = new Color (255, 255, 255, 255);
	public Color		claireColor = new Color (255, 255, 255, 255);
	public Color		basicDoorColor  = new Color (255, 255, 255, 255);

	[Range(-1, 3)]
	public int			doorSwitch = 0;
	public GameObject[] doors;

	private bool	isPressed;
	private Color	defaultColor;
	private Color	openColor;
	private int		openLayer;

	void Start ()
	{
		this.isPressed = false;
		switch (this.doorSwitch)
		{
			case 0:
				this.defaultColor = this.basicDoorColor;
				break;
			case 1:
				this.defaultColor = this.thomasColor;
				break;
			case 2:
				this.defaultColor = this.johnColor;
				break;
			case 3:
				this.defaultColor = this.claireColor;
				break;
			default:
				this.defaultColor = new Color (255, 255, 255, 255);
				break;
		}
		this.GetComponent<SpriteRenderer> ().color = this.defaultColor;
		this.openLayer = 14;
		this.openColor = new Color (0, 0, 0, 255);
	}

	public void reset ()
	{
		int i;

		for (i = 0; i < this.doors.Length; i++)
		{
			switch (this.doors [i].tag)
			{
				case "Door":
					this.doors [i].gameObject.layer = 0;
					this.doors [i].gameObject.GetComponent<SpriteRenderer> ().color = this.basicDoorColor;
					break;
				case "ThomasDoor":
					this.doors [i].gameObject.layer = 0;
					this.doors [i].gameObject.GetComponent<SpriteRenderer> ().color = this.thomasColor;
					break;
				case "JohnDoor":
					this.doors [i].gameObject.layer = 0;
					this.doors [i].gameObject.GetComponent<SpriteRenderer> ().color = this.johnColor;
					break;
				case "ClaireDoor":
					this.doors [i].gameObject.layer = 0;
					this.doors [i].gameObject.GetComponent<SpriteRenderer> ().color = this.claireColor;
					break;
				default:
					break;
			}
		}
		this.GetComponent<SpriteRenderer> ().color = this.defaultColor;
		this.isPressed = false;
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		Color col;
		string tag;
		int i;

		if (this.isPressed)
			return;

		if (collider.tag == "Thomas" || collider.tag == "John" || collider.tag == "Claire")
		{
			if ((this.doorSwitch == 1 && collider.tag != "Thomas") ||
				(this.doorSwitch == 2 && collider.tag != "John") ||
				(this.doorSwitch == 3 && collider.tag != "Claire"))
				return ;

			if (this.doorSwitch == 0)
			{
				for (i = 0; i < this.doors.Length; i++)
				{
					if (this.doors [i].tag == "Door")
					{
						this.doors [i].gameObject.layer = this.openLayer;
						this.doors [i].GetComponent<SpriteRenderer> ().color -= this.openColor;
					}
				}
				this.GetComponent<SpriteRenderer>().color = openColor;
				this.isPressed = true;
			}

			switch (collider.tag)
			{
				case "Thomas":
					col = this.thomasColor;
					tag = "ThomasDoor";
					break;
				case "John":
					col = this.johnColor;
					tag = "JohnDoor";
					break;
				case "Claire":
					col = this.claireColor;
					tag = "ClaireDoor";
					break;
				default:
					col = new Color (255, 255, 255, 255);
					tag = "null";
					break;
			}

			for (i = 0; i < this.doors.Length; i++)
			{
				if (this.doors [i].tag == tag)
				{
					this.doors [i].gameObject.layer = this.openLayer;
					this.doors [i].GetComponent<SpriteRenderer> ().color -= this.openColor;
				}
			}
			this.GetComponent<SpriteRenderer>().color = col;
			this.isPressed = true;
		}
	}
}
