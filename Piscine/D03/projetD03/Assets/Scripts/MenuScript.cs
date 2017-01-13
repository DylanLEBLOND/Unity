using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScript : MonoBehaviour
{
	public void StageOne (string SceneStageOne)
	{
		Application.LoadLevel (SceneStageOne);
	}

	public void ExitGame ()
	{
		Application.Quit ();
	}
}
