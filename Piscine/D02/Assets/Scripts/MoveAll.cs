using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAll : MonoBehaviour
{
	private List<Move> characters = new List<Move> ();

	public void addCharacter (Move newCharacter)
	{
		this.characters.Add (newCharacter);
	}

	public void moveSolo (Move character)
	{
		int i;

		for (i = 0; i < this.characters.Count; i++)
		{
			if (! this.characters [i].Equals (character))
				this.characters [i].setShouldWalk (false);
		}
	}

	// Update is called once per frame
	void Update ()
	{
		Vector3	destination;
		int		i;

		if (Input.GetMouseButtonDown (0))
		{
			destination = Camera.main.ScreenToWorldPoint (Input.mousePosition);

			for (i = 0; i < this.characters.Count; i++)
			{
				if (!this.characters [i].getShouldWalk ())
					continue;
				destination.z = this.characters[i].transform.position.z;
				this.characters [i].startMoving (destination);
			}
		}
		if (Input.GetMouseButtonDown (1))
		{
			for (i = 0; i < this.characters.Count; i++)
				this.characters [i].setShouldWalk (false);
		}
	}
}
