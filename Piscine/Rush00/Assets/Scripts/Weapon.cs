using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public Bullet ammoType;
	public int ammo = -1;
	public int fireRate;
    public float soundRadius;
    public float MeleeRange;
    public bool IsLethal = true;
    public bool IsLethalThrown = false;

	private float time = 0.0f;
	private float lastFireTime = 0.0f;

	public bool CanFire(bool ignoreAmmo = false)
	{
		if (ammo == 0)
			return false;
		return time - lastFireTime > (1.0f / fireRate);
	}

	public void Fire(bool ignoreAmmo = false)
	{
        GetComponentInParent<AudioSource>().Play();
	    if (MeleeRange == 0)
	    {
			Debug.Log ("Je suis la " + this.tag + " | " + this.gameObject.layer);
            var bullet = Instantiate(ammoType, transform.position, Quaternion.identity);
			bullet.transform.parent = transform;
	        bullet.GetComponent<Bullet>().IsLethal = IsLethal;
            bullet.GetComponent<Rigidbody2D>().velocity = -transform.right * -25;

            lastFireTime = time;
            if (!ignoreAmmo && ammo > 0)
                ammo--;
	        return;
	    }
        foreach (var o in Physics2D.OverlapCircleAll(transform.position, MeleeRange))
        {
			if (o.gameObject.tag == "IA")
			{
				if (IsLethal)
					o.SendMessage ("ApplyDamage", -1);
				else
					o.SendMessage ("ApplyDamage", 2);
			}
        }
	}

    public void Update ()
	{
		time += Time.deltaTime;
	}

    public void OnCollisionEnter2D(Collision2D coll)
    {
        if (coll.gameObject.tag == "IA" && GetComponent<Rigidbody2D>().velocity.magnitude > 1.0f)
        {
			if (IsLethalThrown)
            	coll.gameObject.SendMessage("ApplyDamage", -1);
			else
				coll.gameObject.SendMessage("ApplyDamage", 1);
        }
    }
}
