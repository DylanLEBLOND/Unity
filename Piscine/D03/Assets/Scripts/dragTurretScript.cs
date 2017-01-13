using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class dragTurretScript : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	public gameManager			manager;
	public towerScript			turretInfo;
	public static GameObject	turret;

	private bool				canMove;
	private Vector3				startPos;
	private Color				defaultColor;

	void Start()
	{
		this.canMove = false;
		this.startPos = this.transform.position;
		this.defaultColor = this.GetComponent<Image> ().color;
	}

	void Update()
	{
		if (this.manager.playerEnergy < this.turretInfo.energy)
		{
			if (this.canMove)
			{
				Debug.Log ("player energy = " + this.manager.playerEnergy + " | turret energy needed = " + this.turretInfo.energy);
				this.canMove = false;
				this.GetComponent <Image> ().color = Color.red;
			}
		}
		else
		{
			if (!this.canMove)
			{
				Debug.Log ("player energy = " + this.manager.playerEnergy + " | turret energy needed = " + this.turretInfo.energy);
				this.canMove = true;
				this.GetComponent <Image> ().color = this.defaultColor;
			}
		}
	}

	public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log ("JE SUIS LA");

		turret = this.gameObject;

		if (this.canMove)
		{
			this.startPos = this.transform.position;
			this.GetComponentInParent<CanvasGroup> ().blocksRaycasts = false;
		}
	}

	public void OnDrag (PointerEventData eventData)
	{
		Vector3 cameraPos;

		if (turret != null && this.canMove)
		{
			cameraPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			Debug.Log ("JE SUIS LA 2");
			this.transform.position = new Vector3 (cameraPos.x, cameraPos.y, this.transform.position.z);
		}
	}

	public void OnEndDrag (PointerEventData eventData)
	{

		if (turret != null)
			turret = null;

		if (this.canMove)
		{
			Vector3 cameraPos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (cameraPos, Vector2.zero);
			if (hit && hit.collider.gameObject.tag == "empty")
			{
				Debug.Log ("JE SUIS LA 3 | HIT = " + hit.collider.gameObject.name);
				GameObject.Instantiate (this.turretInfo, new Vector3 (hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, this.transform.position.z), Quaternion.identity);
				hit.collider.gameObject.GetComponent<Collider2D>().enabled = false;
				this.manager.playerEnergy -= this.turretInfo.energy;
			}
			this.transform.position = this.startPos;
			this.GetComponentInParent<CanvasGroup> ().blocksRaycasts = true;
		}
	}
}
