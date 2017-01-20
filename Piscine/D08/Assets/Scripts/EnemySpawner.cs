using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
	public List<GameObject> EnemyList;

	private float tic = 0;
	private float tac = 10;

	void Update ()
	{
		if (this.transform.childCount < 5 && this.tac - this.tic >= 10)
		{
			GameObject.Instantiate (this.EnemyList[Random.Range (0, this.EnemyList.Count)], this.transform.position, Quaternion.identity, this.transform);
			this.tic = Time.time;
		}
		this.tac = Time.time;
	}
}
