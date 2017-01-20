using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Maya : MonoBehaviour
{
	[Range (1, 100)]
	public int STR = 10;
	[Range (1, 100)]
	public int AGI = 10;
	[Range (1, 100)]
	public int CON = 10;
	[Range (1, 100)]
	public int Armor = 10;
	private int _HP { get { return CON * 5; } set { _HP = value; } }
	public int HP {	get { return _HP; } }
	public int minDamage { get { return STR / 2; } }
	private int maxDamage { get { return minDamage + 4; } }
	[Range (1, 100)]
	public int Level = 1;
	[Range (0, 99999)]
	public int XP = 0;
	[Range (0, 99999)]
	public int money = 0;

	public bool Immortal = false;
	private int xpNextLevel { get { return (Level - 1) * 100 + Level * 100; } }

	private GameObject enemy;
	private bool attackState = false;
	private bool canAttack = false;

	private Animator		anim;
	private NavMeshAgent	nav;
	private Vector3			destination;

	private float			tic = 0;
	private float			tac = 0;

	void Start ()
	{
		this.anim = this.GetComponent<Animator> ();
		this.anim.SetBool ("Alive", true);
		this.anim.SetBool ("Run", false);
		this.anim.SetBool ("Attack", false);

		this.nav = this.GetComponent<NavMeshAgent> ();

		this.destination = this.transform.position;
	}

	public void tookDamage (int baseDamage)
	{
		if (this.Immortal)
			return;

		this._HP -= baseDamage;
		if (this._HP <= 0)
			StartCoroutine ("Dead");
	}

	public IEnumerator Dead()
	{
		this.anim.SetBool ("Alive", false);

		yield return new WaitForSeconds (3);

		this.gameObject.SetActive (false);
//		GameObject.Destroy (this.gameObject);
	}

	public IEnumerator Attack()
	{
		this.anim.SetBool ("Attack", true);
		this.nav.Stop();

		yield return new WaitForSeconds (0.75f);

		this.nav.Resume();
		this.anim.SetBool ("Attack", false);
	}


	void OnCollisionStay (Collision coll)
	{
		if (this.attackState && this.canAttack && coll.gameObject.tag == "Enemy" && this.tac - this.tic > 1f)
		{
			if (coll.gameObject.GetComponent<Enemy> ().HP > 0)
			{
				StartCoroutine ("Attack");
				if (75 + this.AGI - coll.gameObject.GetComponent<Enemy> ().AGI > 0)
					coll.gameObject.GetComponent<Enemy> ().tookDamage (this.gameObject, Random.Range (this.minDamage, this.maxDamage));
				this.tic = Time.time;
			}
		}
		this.tac = Time.time;
	}

	// Update is called once per frame
	void Update ()
	{
		if (this.XP >= this.xpNextLevel)
		{
			this.Level++;
		}

		if (Input.GetMouseButtonDown (0))
		{
			RaycastHit hit;

			if (Physics.Raycast (Camera.main.ScreenPointToRay (Input.mousePosition), out hit, 100))
			{
				if (hit.collider.gameObject.tag == "Enemy")
				{
					this.enemy = hit.collider.gameObject;
					this.attackState = true;
				}
				else
					this.attackState = false;

				if (!this.anim.GetBool ("Run"))
					this.anim.SetBool ("Run", true);
				this.destination = hit.point;
				this.nav.destination = this.destination;
			}
		}

		if (this.transform.position == this.nav.destination && this.anim.GetBool("Run"))
			this.anim.SetBool ("Run", false);

		if (Input.GetKeyDown (KeyCode.Space))
		{
			this.attackState = false;
			this.canAttack = false;

			this.anim.SetBool ("Run", false);
			this.anim.SetBool ("Attack", false);

			this.nav.destination = this.transform.position;
		}

		if (this.attackState)
		{
			this.nav.destination = this.enemy.transform.position;

			if (Vector3.Distance (this.transform.position, this.nav.destination) < 1f)
			{
				this.nav.destination = this.transform.position;

				if (this.canAttack == false)
					this.canAttack = true;

				foreach (Collider col in Physics.OverlapSphere (this.transform.position, 1))
				{
					if (col.gameObject.tag == "Enemy")
						return;
				}
				this.attackState = false;
				this.canAttack = false;
			}
		}
	}
}
