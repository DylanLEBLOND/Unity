using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScript : MonoBehaviour
{
	public List<DoorScript> doors = new List<DoorScript>();
	public bool isIn = false;

	private Vector3 StartPos;
	private Quaternion StartAngle;
	private float tic = 0.0f;
	private float tac = 0.0f;
	private float discretion = 0;
	private bool warningState = true;
	private float warningTic = 0.0f;
	private float warningTac = 0.0f;

	void Start ()
	{
		
		this.StartPos = this.transform.GetChild (0).transform.position;
		this.StartAngle = this.transform.GetChild (0).transform.rotation;
	}

	public void GameOver ()
	{
		StartCoroutine ("C_gameOver");
	}

	public void GameWin ()
	{
		StartCoroutine ("C_gameWin");
	}

	public IEnumerator C_gameOver()
	{
		this.transform.GetChild (3).gameObject.GetComponent<Text>().text = "Game Over";
		this.transform.GetChild (3).gameObject.GetComponent<Text>().color = new Color (0.75f, 0, 0, 1);
		this.transform.GetChild (3).gameObject.SetActive (true);
		this.transform.GetChild (4).gameObject.transform.GetChild(0).gameObject.SetActive (false);
		this.transform.GetChild (4).gameObject.transform.GetChild(1).gameObject.SetActive (false);
		this.transform.GetChild (4).gameObject.transform.GetChild(2).gameObject.SetActive (true);
		yield return new WaitForSeconds (5f);
		this.resetScene ();
	}

	public IEnumerator C_gameWin()
	{
		this.transform.GetChild (3).gameObject.GetComponent<Text>().text = "You Won";
		this.transform.GetChild (3).gameObject.GetComponent<Text>().color = new Color (0, 0.75f, 0, 1);
		this.transform.GetChild (3).gameObject.SetActive (true);
		this.transform.GetChild (4).gameObject.transform.GetChild(0).gameObject.SetActive (false);
		this.transform.GetChild (4).gameObject.transform.GetChild(1).gameObject.SetActive (false);
		this.transform.GetChild (4).gameObject.transform.GetChild(2).gameObject.SetActive (true);
		yield return new WaitForSeconds (5f);
		this.resetScene ();
	}


	private void resetScene ()
	{
		int i;

		for (i = 0; i < this.doors.Count; i++)
			this.doors [i].reset ();
		this.isIn = false;

		this.GetComponentInChildren<Slider> ().value = 0;
		this.transform.GetChild (2).gameObject.SetActive (false);
		this.transform.GetChild (3).gameObject.SetActive (false);
		this.transform.GetChild (4).gameObject.transform.GetChild(0).gameObject.SetActive (true);
		this.transform.GetChild (4).gameObject.transform.GetChild(1).gameObject.SetActive (false);
		this.transform.GetChild (4).gameObject.transform.GetChild(2).gameObject.SetActive (false);
		this.tic = 0.0f;
		this.tac = 0.0f;
		this.discretion = 0;
		this.warningState = true;
		this.warningTic = 0.0f;
		this.warningTac = 0.0f;

		this.transform.GetChild (0).transform.position = this.StartPos;
		this.transform.GetChild (0).transform.rotation = this.StartAngle;
	}

	void Update ()
	{
		if (0.75f <= this.discretion && this.discretion < 1.0f)
		{
			if (this.warningTac - this.warningTic > 0.1f)
			{
				this.transform.GetChild (2).gameObject.SetActive (warningState);
				this.warningState = !this.warningState;
				this.warningTic = Time.time;
			}
			this.warningTac = Time.time;

			if (this.transform.GetChild (4).gameObject.active)
			{
				this.transform.GetChild (4).gameObject.transform.GetChild(0).gameObject.SetActive (false);
				this.transform.GetChild (4).gameObject.transform.GetChild(1).gameObject.SetActive (true);
			}
		}

		if (this.tac - this.tic > 0.01f && this.discretion > 0.0f && ! this.isIn)
		{
			this.GetComponentInChildren<Slider> ().value -= 0.01f;
			if (this.GetComponentInChildren<Slider> ().value < 0)
				this.GetComponentInChildren<Slider> ().value = 0;
			this.tic = Time.time;
		}
		if (this.discretion < 0.75f && (this.transform.GetChild (2).gameObject.active || !this.transform.GetChild (4).gameObject.transform.GetChild(0).gameObject.active))
		{
			this.transform.GetChild (2).gameObject.SetActive (false);
			this.transform.GetChild (4).gameObject.transform.GetChild(0).gameObject.SetActive (true);
			this.transform.GetChild (4).gameObject.transform.GetChild(1).gameObject.SetActive (false);
		}
		this.tac = Time.time;

		this.discretion = this.GetComponentInChildren<Slider> ().value;
	}
}
