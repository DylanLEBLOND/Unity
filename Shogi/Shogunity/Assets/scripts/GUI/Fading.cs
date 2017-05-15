using UnityEngine;
using System.Collections;

/// <summary>
/// Effets de transition estomper.
/// </summary>

public class Fading : MonoBehaviour
{
	/// <summary>
	/// Texure de la transition.
	/// </summary>
	public Texture2D fadeOutTexture;

	/// <summary>
	/// Vitesse de transition.
	/// </summary>
	public float fadeSpeed = 0.8f;

	/// <summary>
	/// Profondeur où est dessinée la transition.
	/// </summary>
	private int drawDepth = -1000;

	/// <summary>
	/// Transparence de la transition.
	/// </summary>
	private float alpha = 1.0f;

	/// <summary>
	/// Sens d'évolution de la transparence de la transition.
	/// </summary>
	private int fadeDir = -1;

	/// <summary>
	/// Evenement sur interface graphique.
	/// </summary>
	void OnGUI ()
	{
		alpha += fadeDir * fadeSpeed * Time.deltaTime;
		alpha = Mathf.Clamp01(alpha);

		GUI.color = new Color(GUI.color.r, GUI.color.g, GUI.color.b, alpha);
		GUI.depth = drawDepth;
		GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), fadeOutTexture);
	}

	/// <summary>
	/// Initialisation de la transition.
	/// </summary>
	/// <returns>La vitesse de transtion.</returns>
	/// <param name="direction">Sens de la transition.</param>
	public float beginFade (int direction)
	{
		fadeDir = direction;
		return fadeSpeed;
	}

	/// <summary>
	/// Evenement la partie est chargée.
	/// </summary>
	void OnLevelWasLoaded ()
	{
//		alpha = 1;
		beginFade(-1);
	}

	/// <summary>
	/// Changement de scène.
	/// </summary>
	/// <param name="level">Nom de scène.</param>
	public void ChangeLevel(string level)
	{
		StartCoroutine(fadeOut(level));
	}

	/// <summary>
	/// Désestompage.
	/// </summary>
	/// <returns>Un enumerateur.</returns>
	/// <param name="level">Un nom de scène.</param>
	private IEnumerator fadeOut(string level)
	{
		float fadeTime = beginFade(1);
		yield return new WaitForSeconds(fadeTime);
		Application.LoadLevel (level);
	}
}