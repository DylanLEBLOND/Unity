using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
	public MoveAll		moveALL;

	public Animator		walkAnimator;
	public AudioClip	walkAudio;

	private bool		shouldWalk;
	private Vector3		destination;

	void Start ()
	{
		this.walkAnimator.enabled = false;
		this.shouldWalk = false;
		this.destination = this.transform.position;

		this.moveALL.addCharacter (this);
	}

	void OnMouseDown ()
	{
		if (Input.GetMouseButtonDown (0))
		{
			if (Input.GetKey (KeyCode.LeftControl) || Input.GetKey (KeyCode.RightControl))
				this.shouldWalk = true;
			else
			{
				this.shouldWalk = true;
				this.moveALL.moveSolo (this);
			}
		}
	}

	public bool getShouldWalk ()
	{
		return this.shouldWalk;
	}

	public void setShouldWalk (bool should)
	{
		this.shouldWalk = should;
	}

	public void startMoving (Vector3 destination)
	{
		Vector3	direction;
		float	angle;

		if (!this.shouldWalk)
			return;

		this.destination = destination;

		direction = destination - this.transform.position;
		if (direction != Vector3.zero)
		{
			angle = Mathf.Atan2 (direction.y, direction.x) * Mathf.Rad2Deg;
			this.transform.rotation = Quaternion.AngleAxis (angle + 90, Vector3.forward);
		}
		this.walkAnimator.enabled = true;
		AudioSource.PlayClipAtPoint (this.walkAudio, this.transform.position);
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.destination != this.transform.position)
			this.transform.position = Vector3.MoveTowards (this.transform.position, this.destination, 2f * Time.deltaTime);
		else
		{
			this.walkAnimator.enabled = false;
		}
	}
}
