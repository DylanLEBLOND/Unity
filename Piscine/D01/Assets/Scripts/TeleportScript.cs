using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportScript : MonoBehaviour
{
	public GameObject TeleportOut;

	void OnTriggerEnter2D (Collider2D collider)
	{
		if (this.tag == "Untagged")
			collider.transform.position = this.TeleportOut.transform.position;
		if (this.tag.Contains (collider.tag) && this.TeleportOut != null)
			collider.transform.position = this.TeleportOut.transform.position;
	}
}
