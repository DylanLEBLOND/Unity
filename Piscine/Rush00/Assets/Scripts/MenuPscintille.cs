using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPscintille : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	IEnumerator Wait()
	{
		yield return new WaitForSeconds(1f);   //Wait
		GetComponent<Text> ().color = Color.blue;
		yield return new WaitForSeconds(1f);
		GetComponent<Text> ().color = Color.green;
		yield return new WaitForSeconds(1f);
		GetComponent<Text> ().color = Color.magenta;
		yield return new WaitForSeconds(1f);
		GetComponent<Text> ().color = Color.black;
	}

	// Update is called once per frame
	void Update () {
		StartCoroutine (Wait());
	}
}
