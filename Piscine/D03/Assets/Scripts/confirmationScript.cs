using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class confirmationScript : MonoBehaviour
{
	public pauseScript panelPause;

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

	public void		showConfirmation ()
	{
		this.panelPause.showPanel (false);
		this.showPanel (true);
	}

	public void		answerNo ()
	{
		this.panelPause.stopPause ();
		this.showPanel (false);
	}

	public void		answerYes (string SceneStageZero)
	{
		Application.LoadLevel (SceneStageZero);
	}

	void Start ()
	{
		this.showPanel (false);
	}
}
