using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorSwitchScript : MonoBehaviour
{
	public Color		thomasColor = new Color (255, 255, 255, 255);
	public Color		johnColor = new Color (255, 255, 255, 255);
	public Color		claireColor = new Color (255, 255, 255, 255);

	public GameObject	elevator;

	private int			current;

	void Start()
	{
		this.current = 0;
	}

	public void reset ()
	{
		this.elevator.gameObject.layer = 14;
		this.elevator.GetComponent<SpriteRenderer> ().color = new Color (0, 0, 0, 255);
		this.current = 0;
	}

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (this.current != 1 && collider.tag == "Thomas")
		{
			this.elevator.gameObject.layer = 11;
			this.elevator.GetComponent<SpriteRenderer> ().color = this.thomasColor;
			this.current = 1;
		}
		if (this.current != 2 && collider.tag == "John")
		{
			this.elevator.gameObject.layer = 12;
			this.elevator.GetComponent<SpriteRenderer> ().color = this.johnColor;
			this.current = 2;
		}
		if (this.current != 3 && collider.tag == "Claire")
		{
			this.elevator.gameObject.layer = 13;
			this.elevator.GetComponent<SpriteRenderer> ().color = this.claireColor;
			this.current = 3;
		}
	}
}
