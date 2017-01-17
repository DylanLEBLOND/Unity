using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuPboutons : MonoBehaviour {
	public void appExit()
	{
		Application.Quit();
	}
	public void appStart () {
		int scene = 1;
		SceneManager.LoadScene(scene, LoadSceneMode.Single);
	}
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
