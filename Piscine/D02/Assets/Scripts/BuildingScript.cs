using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingScript : MonoBehaviour
{
	public int			maxHp;
	public bool			isTownHall;
	public GameObject	resident;
	public Vector3		spawnPos;

	private float	tic;
	private float	tac;

	void Start ()
	{
		this.tic = 0;
		this.tac = 0;
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (this.isTownHall && this.tac - this.tic >= 10)
		{
			GameObject.Instantiate (this.resident, this.spawnPos, Quaternion.identity);
			tic = Time.time;
		}
		tac = Time.time;
	}
}
