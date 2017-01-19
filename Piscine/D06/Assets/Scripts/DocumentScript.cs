using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DocumentScript : MonoBehaviour
{
	public Canvas canvas;

	public void GameWin ()
	{
		this.canvas.SendMessage ("GameWin");
	}
}
