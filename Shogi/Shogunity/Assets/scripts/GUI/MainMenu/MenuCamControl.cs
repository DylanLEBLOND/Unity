using UnityEngine;
using System.Collections;

/// <summary>
/// Main Camera Controller
/// </summary>
public class MenuCamControl : MonoBehaviour
{
	/// <summary>
	/// Current Position of the camera
	/// </summary>
	public Transform currentMount;

	/// <summary>
	/// Movement Speed of the camera
	/// </summary>
	public float speed = 1.0f;

	void Start ()
	{
		transform.position = this.currentMount.position;
		transform.rotation = this.currentMount.rotation;
	}

	/// <summary>
	/// Set the position of the camera
	/// </summary>
	/// <param name="newMount">Nouveau point à atteindre.</param>
	public void setMount (Transform newMount)
	{
		currentMount = newMount;
	}

	void Update()
	{
		if (this.transform.position != this.currentMount.position)
		{
			transform.position = Vector3.Lerp(transform.position, currentMount.position, speed * Time.deltaTime);
			transform.rotation = Quaternion.Slerp(transform.rotation, currentMount.rotation, speed * Time.deltaTime);
		}
	}
}
