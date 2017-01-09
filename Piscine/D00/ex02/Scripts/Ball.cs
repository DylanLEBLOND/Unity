using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
	private int 		score;
	private float		dir;
	private int 		speed;

	// Use this for initialization
	void 			Start ()
	{
		this.score = -15;
		this.dir = 1.0f;
		this.speed = 0;
	}

	public void		setSpeed(int speed)
	{
		this.speed = speed;
	}

	public int		getSpeed()
	{
		return this.speed;
	}

	// Update is called once per frame
	void 			Update ()
	{
		if (this.speed > 0)
		{
			if (this.transform.position.y >= 8.0f)
				dir = -1.0f;
			if (this.transform.position.y <= -8.0f)
				dir = 1.0f;

			this.transform.position += new Vector3 (0, this.dir * this.speed * 0.01f, 0);


			if (5.0f <= this.transform.position.y && this.transform.position.y <= 6.5f && this.speed < 5)
				GameObject.Destroy(this.gameObject);

			this.speed--;

			if (this.speed == 0)
			{
				this.score += 5;
				this.dir = 1.0f;
				if (this.score <= 0)
					Debug.Log ("score:" + this.score);
			}
		}
	}
}
