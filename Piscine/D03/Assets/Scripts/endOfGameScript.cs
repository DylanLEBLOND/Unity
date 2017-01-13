using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class endOfGameScript : MonoBehaviour
{
	public gameManager	manager;

	private bool end;

	public void		showPanel (bool state)
	{
		Transform butTransform;
		int i;


		Debug.Log ("state = " + state);
		for (i = 0; i < this.transform.childCount; i++)
		{
			butTransform = this.transform.GetChild (i);
			butTransform.gameObject.SetActive (state);
		}
		this.GetComponent <Image> ().enabled = state;
		Debug.Log ("show panel ? " + this.GetComponent <Image> ().enabled);
	}

	public void		showGameOver ()
	{
		this.transform.GetChild (0).GetComponent<Text> ().text = "Game Over";
		this.transform.GetChild (0).gameObject.SetActive (true);
		this.transform.GetChild (1).gameObject.SetActive (true);
		this.GetComponent <Image> ().enabled = true;
		this.end = true;
	}
		
	public void		showScore ()
	{
		char	letter;
		string	bonus = "";

		switch (this.manager.playerMaxHp / this.manager.playerHp)
		{
			case 1:
				letter = 'S';
				break;
			case 2:
				letter = 'A';
				break;
			case 3:
				letter = 'B';
				break;
			case 4:
				letter = 'C';
				break;
			case 5:
				letter = 'D';
				break;
			case 6:
				letter = 'E';
				break;
			default:
				letter = 'F';
				break;
		}

		if (this.manager.playerMaxHp == this.manager.playerHp)
		{
			bonus = "S";
			if (this.manager.playerEnergy >= this.manager.playerStartEnergy)
			{
				bonus = "SS";
				if (this.manager.playerEnergy >= 2 * this.manager.playerStartEnergy)
				{
					letter = 'Z';
					bonus = "";
				}
			}
		}

		this.transform.GetChild (0).GetComponent<Text> ().text = "Score = " + letter.ToString() + bonus;
		this.transform.GetChild (0).gameObject.SetActive (true);
		this.transform.GetChild (2).gameObject.SetActive (true);
		this.GetComponent <Image> ().enabled = true;
		this.end = true;
	}

	void Start ()
	{
		this.end = false;
		this.showPanel (false);
	}

	void Update ()
	{
		if (!this.end && this.manager.playerHp <= 0)
			this.showGameOver ();
		if (!this.end && this.manager.lastWave)
			this.showScore();
	}
}
