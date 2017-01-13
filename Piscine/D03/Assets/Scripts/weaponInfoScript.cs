using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class weaponInfoScript : MonoBehaviour
{
	public towerScript sourceInfo;

	public enum eTowerInfo { Damage, FireRate, Energy, Range };
	public eTowerInfo info;

	private float	value;
	private Text 	textInfo;

	void Start ()
	{
		this.textInfo = this.GetComponent<Text> ();

		switch (this.info)
		{
			case eTowerInfo.Damage:
				this.value = sourceInfo.damage;
				break;
			case eTowerInfo.FireRate:
				this.value = sourceInfo.fireRate;
				break;
			case eTowerInfo.Energy:
				this.value = sourceInfo.energy;
				break;
			case eTowerInfo.Range:
				this.value = sourceInfo.range;
				break;
			default:
				this.textInfo.text = "N/A";
				return;
		}
		this.textInfo.text = this.value.ToString();
	}

	void Update ()
	{
		switch (this.info)
		{
			case eTowerInfo.Damage:
				this.value = sourceInfo.damage;
				break;
			case eTowerInfo.FireRate:
				this.value = sourceInfo.fireRate;
				break;
			case eTowerInfo.Energy:
				this.value = sourceInfo.energy;
				break;
			case eTowerInfo.Range:
				this.value = sourceInfo.range;
				break;
			default:
				this.textInfo.text = "N/A";
				return;
		}
		this.textInfo.text = this.value.ToString();
	}
}
