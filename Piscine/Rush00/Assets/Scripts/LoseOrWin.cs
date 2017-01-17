using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class LoseOrWin : MonoBehaviour {

	public void Again()
	{
		int scene = SceneManager.GetActiveScene().buildIndex;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	public void Quit()
	{
		int scene = 0;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
}
