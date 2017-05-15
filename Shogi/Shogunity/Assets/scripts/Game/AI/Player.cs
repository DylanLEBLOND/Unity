using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShogiUtils;

public class Player : MonoBehaviour
{
	private string _name;
	private PlayerType _playerType 
	{
		get { return PlayerType.PLAYER; }
	}
	private int _difficulty;
	private GameColor _color;

	public Player (string name, int difficulty, ShogiUtils.GameColor color)
	{
		this._name = name;
		this._difficulty = difficulty;
		this._color = color;
	}

	public string getName ()
	{
		return this._name;
	}

	public PlayerType getPlayerType ()
	{
		return this._playerType;
	}

	public int getDifficulty ()
	{
		return this._difficulty;
	}

	public GameColor getColor ()
	{
		return this._color;
	}

	public void ItsMyTurn()
	{
		Debug.Log ("I'm Player and it's my turn");
	}

	public List<ShogiUtils.Coordinates> getPromotionZone()
	{
		List<ShogiUtils.Coordinates> coordinates = new List<Coordinates>();

		if (this._color == GameColor.SENTE)
		{
			for(int i=0; i<9; i++)
			{
				for(int j=6; j<9; j++)
					coordinates.Add(new Coordinates(i, j));
			}
		}
		else
		{
			for(int i=0; i<9; i++)
			{
				for(int j=0; j<3; j++)
					coordinates.Add(new Coordinates(i, j));
			}
		}
		return coordinates;
	}
}