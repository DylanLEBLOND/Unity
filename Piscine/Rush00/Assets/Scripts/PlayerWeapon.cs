using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerWeapon : MonoBehaviour
{
    public Weapon DefaultWeapon;
	public Text textAmmo;
	private float time = 0.0f;
	private float pickupTime = 0.0f;

	public Weapon weapon
	{
		get { return GetComponentInChildren<Weapon> (); }
	    set
	    {
	        value = Instantiate(DefaultWeapon, transform.position, Quaternion.identity);
	        value.transform.parent = transform;
	        value.transform.localPosition = new Vector3(0.3f, 0, 0);
	    }
	}

    public void Start()
    {
        if (weapon == null)
            weapon = DefaultWeapon;
    }

	public void Update()
	{
		time += Time.deltaTime;
		if (Input.GetButton("Fire1") && weapon && weapon.CanFire())
		{
			weapon.Fire ();
            foreach (var o in Physics2D.OverlapCircleAll(transform.position, weapon.soundRadius))
            {
				if (o.tag == "IA")
					o.SendMessage("heroFound", this.GetComponentInParent<MoveHero> ());
            }
        }
		if (Input.GetButton("Fire2") && time - pickupTime > 0.5f)
		{
			if (weapon.tag == "DefaultWeapon")
			{
				foreach (var o in Physics2D.OverlapCircleAll (transform.position, 1.0f))
				{
					if (o.tag == "Weapon")
					{
                        DestroyImmediate(weapon.gameObject);
						o.transform.parent = transform;
					    o.transform.localEulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, -90);
                        o.GetComponent<Rigidbody2D> ().simulated = false;
                        o.transform.localPosition = new Vector3(0.3f, 0, 0);
					    pickupTime = time;
						return;
					}
				}
			}
			else
			{
				pickupTime = time;
				weapon.GetComponent<Rigidbody2D> ().velocity = -transform.up * 15;
				weapon.GetComponent<Rigidbody2D> ().simulated = true;
				weapon.transform.parent = null;
			    weapon = DefaultWeapon;
			}
		}
		textAmmo.GetComponent<Text> ().text = weapon.ammo.ToString ();
	}
}
