using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
	public GameObject mainCharacter;

	private Vector3 offset;

	void Start ()
	{
		this.offset = this.transform.position - this.mainCharacter.transform.position;
	}
	
	// Update is called once per frame
	void Update ()
	{
		this.transform.position = this.mainCharacter.transform.position + this.offset;
	}
}
