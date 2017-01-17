using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public bool IsLethal = true;

	public void OnTriggerEnter2D (Collider2D coll)
	{
		if (coll.gameObject.tag == "Wall" || coll.gameObject.tag == "Mur" || coll.gameObject.tag == "Door")
			Destroy (gameObject);

		if (this.IsLethal)
		{
			if (this.transform.parent.gameObject.tag == "Weapon")
			{
				if (this.transform.parent.gameObject.transform.parent.gameObject.tag == coll.gameObject.tag)
					return;
			}
			if (this.transform.parent.gameObject.tag ==  coll.gameObject.tag)
				return;
			if (coll.gameObject.tag == "IA" || coll.gameObject.tag == "Player")
				coll.gameObject.SendMessage ("ApplyDamage", -1);
		}
	}
}
