using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
	[Range (1, 100)]
	public int STR = 10;
	[Range (1, 100)]
	public int AGI = 10;
	[Range (1, 100)]
	public int CON = 10;
	[Range (1, 100)]
	public int Armor = 10;

	public int HP;
	private int maxHP
	{ 
		get { return CON * 5; }
	}

	private int _minDamage { get { return STR / 2; } }
	public int minDamage { get { return _minDamage; } }
	private int _maxDamage { get { return _minDamage + 4; } }
	public int maxDamage { get { return _maxDamage; } }
	[Range (1, 100)]
	public int Level = 1;
	[Range (0, 10000)]
	public int XP = 15;
	[Range (0, 10000)]
	public int money = 100;

	private GameObject	player;

	private bool attackState = false;
	private bool canAttack = false;
	private bool attackOnProgress = false;
	private bool deadOnProgress = false;
	private float tic = 0;
	private float tac = 0;

	void Start()
	{
		this.HP = this.maxHP;
	}

//	public void tookDamage (GameObject src, int amount)
	public void tookDamage (GameObject src, int baseDamage)
	{
		int degatTook = baseDamage * (1 - this.Armor / 200);

		if (this.HP - degatTook <= 0)
		{
			this.HP = 0;
			src.gameObject.GetComponent<Maya> ().XP += this.XP;
			src.gameObject.GetComponent<Maya> ().money += this.money;
			StartCoroutine ("Dead");
		}
		else
			this.HP -= degatTook;
	}

	public IEnumerator Dead()
	{
		while (this.attackOnProgress)
			yield return new WaitForSeconds (0.1f);

		this.deadOnProgress = true;
		this.GetComponent<NavMeshAgent> ().enabled = false;
		this.GetComponent<CapsuleCollider> ().enabled = false;

		this.GetComponent<Rigidbody> ().detectCollisions = false;


		this.GetComponent<Animator> ().SetBool ("Alive", false);

		yield return new WaitForSeconds (3);

		while (this.transform.position.y > -5)
		{
			this.transform.position -= new Vector3 (0, 0.01f, 0);
			yield return new WaitForSeconds (0.01f);
		}

		GameObject.Destroy (this.gameObject);
	}

	public IEnumerator Attack()
	{
		if (!this.deadOnProgress)
		{
			this.attackOnProgress = true;

			this.GetComponent<Animator> ().SetBool ("Attack", true);
			this.GetComponent<NavMeshAgent> ().Stop ();

			yield return new WaitForSeconds (0.75f);

			this.GetComponent<NavMeshAgent> ().Resume ();
			this.GetComponent<Animator> ().SetBool ("Attack", false);

			this.attackOnProgress = false;
		}
	}

	void OnCollisionStay (Collision coll)
	{
		if (this.attackState && this.canAttack && coll.gameObject.tag == "Player" && this.tac - this.tic > 1f)
		{
			if (coll.gameObject.GetComponent<Maya> ().HP > 0)
			{
				StartCoroutine ("Attack");
				coll.gameObject.SendMessage ("tookDamage", 1);
				this.tic = Time.time;
			}
		}
		this.tac = Time.time;
	}

	void Update ()
	{
		if (this.HP > 0)
		{
			bool found = false;

			foreach (Collider col in Physics.OverlapSphere (this.transform.position, 5))
			{
				if (col.gameObject.tag == "Player")
				{
					this.player = col.gameObject;
					this.attackState = true;
					found = true;
				}
			}

			if (!found)
			{
				if (this.GetComponent<Animator> ().GetBool ("Run"))
					this.GetComponent<Animator> ().SetBool ("Run", false);
				if (this.attackState)
					this.attackState = false;
				if (this.canAttack)
					this.canAttack = false;
			}
				

			if (this.attackState)
			{
				this.GetComponent<NavMeshAgent> ().destination = this.player.transform.position;

				if (Vector3.Distance (this.transform.position, this.GetComponent<NavMeshAgent> ().destination) < 1f)
				{
					this.GetComponent<NavMeshAgent> ().destination = this.transform.position;
					this.GetComponent<Animator> ().SetBool ("Run", false);

					if (this.canAttack == false)
						this.canAttack = true;
				}
				else
				{
					if (!this.GetComponent<Animator> ().GetBool ("Run"))
						this.GetComponent<Animator> ().SetBool ("Run", true);
				}
			}
			else
				this.GetComponent<Animator> ().SetBool ("Run", false);
		}
	}
}
