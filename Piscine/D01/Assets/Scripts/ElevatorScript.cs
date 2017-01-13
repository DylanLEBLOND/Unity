using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElevatorScript : MonoBehaviour
{
	public Vector3	startPos;
	public float	yMin;
	public float	yMax;
	public bool		goesUp;

	void Start()
	{
		if (this.yMin > this.yMax)
		{
			this.yMin = 0;
			this.yMax = 0;
		}
		this.transform.position = this.startPos;
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.goesUp)
		{
			if (this.transform.position.y + 0.025f >= yMax)
			{
				this.transform.position = new Vector3 (this.transform.position.x, yMax, this.transform.position.z);
				this.goesUp = false;
			}
			else
				this.transform.position += new Vector3 (0, 0.025f, 0);
		}
		else
		{
			if (this.transform.position.y - 0.025f <= yMin)
			{
				this.transform.position = new Vector3 (this.transform.position.x, yMin, this.transform.position.z);
				this.goesUp = true;
			}
			else
				this.transform.position -= new Vector3 (0, 0.025f, 0);
		}
	}
}
