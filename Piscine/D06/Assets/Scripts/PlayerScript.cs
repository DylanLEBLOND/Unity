using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
	private bool gotTheKey = false;

	public void reset ()
	{
		this.gotTheKey = false;
	}

	void OnTriggerEnter (Collider coll)
	{
		if (coll.gameObject.tag == "Key" && ! this.gotTheKey)
		{
			coll.gameObject.transform.GetChild (0).GetComponent<AudioSource> ().Play();
			coll.gameObject.transform.GetChild (1).gameObject.SetActive (false);
			this.gotTheKey = true;;
		}
	}

	void OnCollisionEnter (Collision coll)
	{
		if (coll.gameObject.tag == "DoorController")
		{
			if (coll.gameObject.GetComponent<DoorScript> ().openTheDoor (this.gotTheKey))
				this.gotTheKey = false;
		}

		if (coll.gameObject.tag == "Documents")
			coll.gameObject.SendMessage ("GameWin");
	}
}
