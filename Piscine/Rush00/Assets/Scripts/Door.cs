using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour {
	public float angle;
	public Vector3 cooRotate;



	IEnumerator Wait(float duration)
	{
		yield return new WaitForSeconds(duration);   //Wait
		transform.rotation = Quaternion.AngleAxis (angle + 90f , cooRotate);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (coll.gameObject.tag == "IA" || coll.gameObject.tag == "Player") {
			transform.rotation = Quaternion.AngleAxis (angle, cooRotate);
			StartCoroutine (Wait (5f));
		}
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	}
}
