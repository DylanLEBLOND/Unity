using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraScript_ex00 : MonoBehaviour
{
	public playerScript_ex00	thomas;
	public Vector3				thomasInitPos;

	public playerScript_ex00	john;
	public Vector3				johnInitPos;

	public playerScript_ex00	claire;
	public Vector3				claireInitPos;

	public int					nextScene;

	private int					current;

	void Start()
	{
		if (this.claire == null || this.john == null || this.thomas == null)
		{
			Debug.LogError ("obj param == null");
			return;
		}

		this.thomas.transform.position = this.thomasInitPos;
		this.john.transform.position = this.johnInitPos;
		this.claire.transform.position = this.claireInitPos;

		this.transform.position = new Vector3 (this.thomas.transform.position.x - this.transform.lossyScale.x / 2, (this.thomas.transform.position.y + 2) - this.transform.lossyScale.y / 2, this.transform.position.z);
		this.thomas.setIsActif (true);
		this.current = 1;
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.claire == null || this.john == null || this.thomas == null)
		{
			Debug.LogError ("obj param == null");
			return;
		}

		if (Input.GetKeyDown (KeyCode.Escape))
		{
			this.thomas.reset (this.thomasInitPos);
			this.john.reset (this.johnInitPos);
			this.claire.reset (this.claireInitPos);

			this.thomas.setIsActif (true);
			this.current = 1;
		}

		if (Input.GetKeyDown (KeyCode.N))
		{
			Application.LoadLevel (this.nextScene);
		}
			
		if (this.current != 1 && Input.GetKeyDown (KeyCode.Keypad1) || Input.GetKeyDown (KeyCode.Alpha1))
		{
			this.thomas.setIsActif (true);
			this.john.setIsActif (false);
			this.claire.setIsActif (false);
			this.current = 1;
		}
		else if (this.current != 2 && Input.GetKeyDown (KeyCode.Keypad2) || Input.GetKeyDown (KeyCode.Alpha2))
		{
			this.thomas.setIsActif (false);
			this.john.setIsActif (true);
			this.claire.setIsActif (false);
			this.current = 2;
		}
		else if (this.current != 3 && Input.GetKeyDown (KeyCode.Keypad3) || Input.GetKeyDown (KeyCode.Alpha3))
		{
			this.thomas.setIsActif (false);
			this.john.setIsActif (false);
			this.claire.setIsActif (true);
			this.current = 3;
		}

		switch (this.current)
		{
			case 1:
				this.transform.position = new Vector3 (this.thomas.transform.position.x - this.transform.lossyScale.x / 2, (this.thomas.transform.position.y + 2) - this.transform.lossyScale.y / 2, this.transform.position.z);
				break;
			case 2:
				this.transform.position = new Vector3 (this.john.transform.position.x - this.transform.lossyScale.x / 2, (this.john.transform.position.y + 2) - this.transform.lossyScale.y / 2, this.transform.position.z);
				break;
			case 3:
				this.transform.position = new Vector3 (this.claire.transform.position.x - this.transform.lossyScale.x / 2, (this.claire.transform.position.y + 2) - this.transform.lossyScale.y / 2, this.transform.position.z);
				break;
			default:
				break;
		}
	}
}
