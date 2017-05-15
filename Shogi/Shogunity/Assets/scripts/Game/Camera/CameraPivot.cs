using UnityEngine;
using System.Collections;

/// <summary>
/// Singleton de rotation de camera.
/// </summary>

public class CameraPivot : MonoBehaviour
{
	/// <summary>
	/// Points centres de rotation de la camera.
	/// </summary>
	public Transform pivotPoint1, pivotPoint2;

	/// <summary>
	/// Point de rotation actuel.
	/// </summary>
	Transform currentPivotPoint;

	/// <summary>
	/// Vitesse de rotation.
	/// </summary>
	public float speed = 5;

	/// <summary>
	/// Etat d'activitée.
	/// </summary>
	private bool isActive = false;

	void Awake ()
	{
//		Debug.Log ("READY... ACTION!!");
		if (_GameConfig.instance.gameMode == ShogiUtils.GameMode.PLAYER_VS_PLAYER)
			isActive = true;
		else
			transform.position = pivotPoint1.transform.position;
	}

	void Update ()
	{
		if (isActive)
		{
			if (_GameManager.instance.currentPlayerIndex == 1)
				currentPivotPoint = pivotPoint1;
			else
				currentPivotPoint = pivotPoint2;

			// translation
			transform.position = Vector3.Lerp (transform.position, currentPivotPoint.position, speed * Time.deltaTime);

			// rotation
			//transform.rotation = currentPivotPoint.rotation;
			transform.rotation = Quaternion.Slerp (transform.rotation, currentPivotPoint.rotation, speed * Time.deltaTime * 0.66f);
		}
	}
}
