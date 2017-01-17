using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IAMove : MonoBehaviour
{
	public int				HP;
	public Vector3			startPoint;
	public Vector3			endPoint;
	public float			speed;
	public float			visualRange;
	public float			blindSpotRange;
	public float			agroDuration;
	public enum eWeapon { Gun, Bat };
	public eWeapon 			WeaponType;
	[Range (1.0f , 20.0f)]
	public float			fireRate = 1.0f;

	private bool			roundTrip;
	private bool			agro;
	private float			agroStart;
	private MoveHero		Hero;
	private List<Vector3>	heroOldPos;
	private List<Vector3>	oldPos;
	private float			tic;
	private float			tac;

	void Start ()
	{
		this.roundTrip = false;
		this.agro = false;
		this.agroStart = 0;
		this.Hero = null;
		this.heroOldPos = new List<Vector3> ();
		this.oldPos = new List<Vector3> ();
		this.tic = 0;
		this.tac = 0;

		this.changeDirection (this.startPoint, this.endPoint);
	}

	public void ApplyDamage (int damage)
	{
		if (damage < 0)
		{
			this.HP = 0;
			GameObject.Destroy (this.gameObject);
		}

		this.HP -= damage;
		if (this.HP <= 0)
		{
			this.HP = 0;
			GameObject.Destroy (this.gameObject);
		}
	}

	public void heroFound (MoveHero hero)
	{
		this.agro = true;
		this.agroStart = Time.time;
		this.Hero = hero;
		this.oldPos.Add (this.transform.position);
		this.heroOldPos.Add (this.Hero.gameObject.transform.position);
		this.transform.position = Vector3.MoveTowards (this.transform.position, this.heroOldPos[this.heroOldPos.Count - 1], this.speed * 2 * Time.deltaTime);
	}

	void Update ()
	{
		if (this.agro)
		{
			if (this.Hero == null || this.agroDuration < Time.time - this.agroStart)
			{
				this.agro = false;
				this.agroStart = 0;
				this.heroOldPos.Clear();
				return;
			}
			if (this.tac - this.tic > 1 / this.fireRate)
			{
				if (this.transform.childCount != 0 &&  this.transform.GetChild (0).name == "weapon")
					this.transform.GetChild (0).gameObject.GetComponent<WeaponIA> ().Fire ();
				else
					this.GetComponent<WeaponIA> ().Fire ();
				this.tic = Time.time;
			}
			this.tac = Time.time;

			this.heroIsAtRange (this.heroOldPos [this.heroOldPos.Count - 1], this.oldPos [this.oldPos.Count - 1]);
			if (this.oldPos[this.oldPos.Count - 1] != this.transform.position)
				this.oldPos.Add (this.transform.position);

			this.changeDirection (this.transform.position, this.Hero.gameObject.transform.position);
			if (Vector3.Distance (this.transform.position, this.Hero.transform.position) > 1)
			{
				this.heroOldPos.Add (this.Hero.gameObject.transform.position);
				this.transform.position = Vector3.MoveTowards (this.transform.position, this.heroOldPos [0], this.speed * 2 * Time.deltaTime);
				this.heroOldPos.RemoveAt (0);
			}
		}
		else
		{
			if (this.oldPos.Count > 0)
			{
				this.returnAtOrigin ();
				return;
			}
			if (! this.roundTrip)
				this.moveIA (this.endPoint, this.startPoint);
			else
				this.moveIA (this.startPoint, this.endPoint);
		}
	}

	private bool heroIsAtRange (Vector3 blindPoint, Vector3 rangePoint)
	{
		RaycastHit2D hitRange = Physics2D.CircleCast (this.transform.position, 1.5f, rangePoint);
		RaycastHit2D hitBlindSpot = Physics2D.CircleCast (this.transform.position, 1.5f, blindPoint);

		if (hitRange && hitRange.collider.gameObject.tag.Equals ("Player"))
		{
			if (Vector3.Distance (this.transform.position, hitRange.collider.transform.position) < this.visualRange)
			{
				if (!this.agro)
				{
					this.agro = true;
					this.Hero = hitRange.collider.gameObject.GetComponent<MoveHero> ();
				}
				this.agroStart = Time.time;
				return true;
			}
		}
		if (hitBlindSpot && hitBlindSpot.collider.gameObject.tag == "Player")
		{
			if (Vector3.Distance (this.transform.position, hitBlindSpot.collider.transform.position) < this.blindSpotRange)
			{
				if (!this.agro)
				{
					this.agro = true;
					this.Hero = hitBlindSpot.collider.gameObject.GetComponent<MoveHero> ();
				}
				this.agroStart = Time.time;
				return true;
			}
		}

		return false;
	}

	private void changeDirection (Vector3 fromPoint, Vector3 toPoint)
	{
		Vector3	vector;
		float	angle;

		vector = toPoint - fromPoint;
		if (vector != Vector3.zero)
		{
			angle = Mathf.Atan2 (vector.y, vector.x) * Mathf.Rad2Deg;
			this.transform.rotation = Quaternion.AngleAxis (angle + 90, Vector3.forward);
		}
	}

	private void moveIA (Vector3 dest, Vector3 orig)
	{
		if (this.transform.position != dest)
		{
			if (this.heroIsAtRange (dest, orig))
			{
				this.oldPos.Add (this.transform.position);

				this.heroOldPos.Add (this.Hero.gameObject.transform.position);
				this.transform.position = Vector3.MoveTowards (this.transform.position, this.heroOldPos[this.heroOldPos.Count - 1], this.speed * 2 * Time.deltaTime);
				return;
			}

			this.transform.position = Vector3.MoveTowards (this.transform.position, dest, this.speed * Time.deltaTime);
			if (this.transform.position == dest)
			{
				this.changeDirection (dest, orig);
				this.roundTrip = ! this.roundTrip;
			}
		}
	}

	private void returnAtOrigin ()
	{
		if (this.heroIsAtRange (this.oldPos[this.oldPos.Count - 1], this.transform.position))
		{
			this.oldPos.Add (this.transform.position);

			this.heroOldPos.Add (this.Hero.gameObject.transform.position);
			this.transform.position = Vector3.MoveTowards (this.transform.position, this.heroOldPos[this.heroOldPos.Count - 1], this.speed * 2 * Time.deltaTime);
			return;
		}

		this.transform.position = Vector3.MoveTowards (this.transform.position, this.oldPos[this.oldPos.Count - 1], this.speed * Time.deltaTime);
		this.changeDirection (this.transform.position, this.oldPos[this.oldPos.Count - 1]);
		this.oldPos.RemoveAt (this.oldPos.Count - 1);
		if (this.oldPos.Count == 0)
		{
			if (this.roundTrip)
				this.changeDirection (this.transform.position, this.startPoint);
			else
				this.changeDirection (this.transform.position, this.endPoint);
		}
	}
}
