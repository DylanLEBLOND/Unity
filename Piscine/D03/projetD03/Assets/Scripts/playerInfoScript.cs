using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class playerInfoScript : MonoBehaviour
{
	public gameManager sourceInfo;

	public enum ePlayerInfo { HP, Energy };
	public ePlayerInfo info;

	private int	value;
	private Text textInfo;

	void Start ()
	{
		this.textInfo = this.GetComponent<Text> ();

		switch (this.info)
		{
			case ePlayerInfo.HP:
				this.value = sourceInfo.playerMaxHp;
				break;
			case ePlayerInfo.Energy:
				this.value = sourceInfo.playerStartEnergy;
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
			case ePlayerInfo.HP:
				this.value = sourceInfo.playerHp;
				break;
			case ePlayerInfo.Energy:
				this.value = sourceInfo.playerEnergy;
				break;
			default:
				this.textInfo.text = "N/A";
				return;
		}
		this.textInfo.text = this.value.ToString();
	}
}
