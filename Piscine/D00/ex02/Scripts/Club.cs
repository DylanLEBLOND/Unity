using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Club : MonoBehaviour
{
	public Ball			ball;

	private int			speed;
	private Vector3		posOrigin;
	private bool		pressed;

	void Start ()
	{
		speed = 0;
		posOrigin = this.transform.localPosition;
		pressed = false;
	}

	// Update is called once per frame
	void Update ()
	{
		if (ball != null)
		{
			if (Input.GetKey (KeyCode.Space))
			{
				this.transform.localPosition -= new Vector3 (0, 0.1f, 0);
				this.speed++;
				this.pressed = true;
			}
			if (!Input.GetKey (KeyCode.Space) && this.speed > 0)
			{
				this.transform.localPosition = this.posOrigin;
				this.ball.setSpeed (this.speed);
				this.speed--;
			}
			if (ball == null)
			{
				this.transform.position = new Vector3 (-0.5f, -6.5f, 0);
			}
			if (this.pressed && this.speed == 0 && ball.getSpeed () == 0)
			{
				this.transform.position = this.ball.transform.position + new Vector3 (-0.5f, 0.5f, 0);
				this.speed = 0;
				this.pressed = false;
			}
		}
	}
}
