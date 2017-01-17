using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MoveHero : MonoBehaviour
{
	public GameObject	hero;
	public GameObject	canvasMenu;
	public int			HP;
	public float		speed = 0.1f;

	void Start ()
	{
		this.canvasMenu.SetActive (false);
	}

	void MouseLooking()
	{
		Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10);
		Vector3 lookPos = Camera.main.ScreenToWorldPoint(mousePos);
		lookPos = lookPos - transform.position;
		float angle = Mathf.Atan2(lookPos.y, lookPos.x) * Mathf.Rad2Deg;
		hero.transform.rotation = Quaternion.AngleAxis(angle + 90f, Vector3.forward);
	}

	public void ApplyDamage (int damage)
	{
		if (damage < 0)
		{
			this.HP = 0;
			this.canvasMenu.SetActive (true);
			this.canvasMenu.transform.GetChild(0).transform.GetChild (2).GetComponent <Text>().text = "Game Over";
			GameObject.Destroy (this.gameObject);
		}

		this.HP -= damage;
		if (this.HP <= 0)
		{
			this.HP = 0;
			this.canvasMenu.SetActive (true);
			this.canvasMenu.transform.GetChild(0).transform.GetChild (2).GetComponent <Text>().text = "Game Over";
			GameObject.Destroy (this.gameObject);
		}
	}

	void Update ()
	{
		Vector3 dp = new Vector3();

        dp.x += speed * Input.GetAxis("Horizontal");
        dp.y += speed * Input.GetAxis("Vertical");

		MouseLooking();
		hero.transform.position += dp;

		foreach (var o in Physics2D.OverlapCircleAll(transform.position, 100))
		{
			if (o.gameObject.tag == "IA")
				return;
		}
		this.canvasMenu.SetActive (true);
		this.canvasMenu.transform.GetChild(0).transform.GetChild (2).GetComponent <Text>().text = "Stage Clear";
		GameObject.Destroy (this.gameObject);
	}
}
