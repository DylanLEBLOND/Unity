using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class CaptureBench : MonoBehaviour
{
	public bool goteCaptureBench;

	public GameObject CBpawn1;
	public GameObject CBpawn2;
	public GameObject CBpawn3;
	public GameObject CBpawn4;
	public GameObject CBpawn5;
	public GameObject CBpawn6;
	public GameObject CBlance1;
	public GameObject CBlance2;
	public GameObject CBknight1;
	public GameObject CBknight2;
	public GameObject CBsilver1;
	public GameObject CBsilver2;
	public GameObject CBgold1;
	public GameObject CBgold2;
	public GameObject CBbishop1;
	public GameObject CBbishop2;
	public GameObject CBrook1;
	public GameObject CBrook2;

	public void AddToken (TokenAnimator token)
	{
		token.transform.parent = this.transform;
		switch (token.tokenType)
		{
			case Token.TokenType.PAWN:
				if (this.CBpawn1.transform.GetChild (0).transform.childCount <= 10)
				{
					token.transform.parent = this.CBpawn1.transform.GetChild (0).transform;
					token.transform.position = this.CBpawn1.transform.GetChild (0).transform.position;
				}
				else
				{
					token.transform.parent = this.CBpawn2.transform.GetChild (0).transform;
					token.transform.position = this.CBpawn2.transform.GetChild (0).transform.position;
				}		
				break;
			case Token.TokenType.LANCE:
				if (this.CBlance1.transform.GetChild (0).transform.childCount == 1)
				{
					token.transform.parent = this.CBlance1.transform.GetChild (0).transform;
					token.transform.position = this.CBlance1.transform.GetChild (0).transform.position;
				}
				else
				{
					token.transform.parent = this.CBlance2.transform.GetChild (0).transform;
					token.transform.position = this.CBlance2.transform.GetChild (0).transform.position;
				}		
				break;
			case Token.TokenType.KNIGHT:
				if (this.CBknight1.transform.GetChild (0).transform.childCount == 1)
				{
					token.transform.parent = this.CBknight1.transform.GetChild (0).transform;
					token.transform.position = this.CBknight1.transform.GetChild (0).transform.position;
				}
				else
				{
					token.transform.parent = this.CBknight2.transform.GetChild (0).transform;
					token.transform.position = this.CBknight2.transform.GetChild (0).transform.position;
				}		
				break;
			case Token.TokenType.SILVER:
				if (this.CBsilver1.transform.GetChild (0).transform.childCount == 1)
				{
					token.transform.parent = this.CBsilver1.transform.GetChild (0).transform;
					token.transform.position = this.CBsilver1.transform.GetChild (0).transform.position;
				}
				else
				{
					token.transform.parent = this.CBsilver2.transform.GetChild (0).transform;
					token.transform.position = this.CBsilver2.transform.GetChild (0).transform.position;
				}		
				break;
			case Token.TokenType.GOLD:
				if (this.CBgold1.transform.GetChild (0).transform.childCount == 1)
				{
					token.transform.parent = this.CBgold1.transform.GetChild (0).transform;
					token.transform.position = this.CBgold1.transform.GetChild (0).transform.position;
				}
				else
				{
					token.transform.parent = this.CBgold2.transform.GetChild (0).transform;
					token.transform.position = this.CBgold2.transform.GetChild (0).transform.position;
				}		
				break;
			case Token.TokenType.BISHOP:
				if (this.CBbishop1.transform.GetChild (0).transform.childCount == 1)
				{
					token.transform.parent = this.CBbishop1.transform.GetChild (0).transform;
					token.transform.position = this.CBbishop1.transform.GetChild (0).transform.position;
				}
				else
				{
					token.transform.parent = this.CBbishop2.transform.GetChild (0).transform;
					token.transform.position = this.CBbishop2.transform.GetChild (0).transform.position;
				}		
				break;
			case Token.TokenType.ROOK:
				if (this.CBrook1.transform.GetChild (0).transform.childCount == 1)
				{
					token.transform.parent = this.CBrook1.transform.GetChild (0).transform;
					token.transform.position = this.CBrook1.transform.GetChild (0).transform.position;
				}
				else
				{
					token.transform.parent = this.CBrook2.transform.GetChild (0).transform;
					token.transform.position = this.CBrook2.transform.GetChild (0).transform.position;
				}		
				break;
			default:
				break;
		}
		token.goteIsTheOwner = goteCaptureBench;
		token.coord = new IntVector2 (-1, -1);
		token.isCaptured = true;

		if (goteCaptureBench)
			token.transform.rotation = Quaternion.Euler (token.promotedToken ? 90 : 270, 0, token.promotedToken ? 0 : 180);
		else
			token.transform.rotation = Quaternion.Euler (token.promotedToken ? 90 : 270, 0, token.promotedToken ? 180 : 0);
	
		token.promotedToken = false;
		token.selected = false;
	}

	public TokenAnimator GetToken (Token.TokenType tokenType)
	{
		Transform parent = this.goteCaptureBench ? _GameBoard.instance.transform.GetChild (0).transform.GetChild(0).transform : _GameBoard.instance.transform.GetChild (1).transform.GetChild(0).transform;
		TokenAnimator token = null;

		switch (tokenType)
		{
			case Token.TokenType.PAWN:
				if (this.CBpawn2.transform.GetChild (0).transform.childCount != 0)
					token = this.CBpawn2.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				else if (this.CBpawn1.transform.GetChild (0).transform.childCount != 0)
					token = this.CBpawn1.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				break;
			case Token.TokenType.LANCE:
				if (this.CBlance2.transform.GetChild (0).transform.childCount != 0)
					token = this.CBlance2.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				else if (this.CBlance1.transform.GetChild (0).transform.childCount != 0)
					token = this.CBlance1.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				break;
			case Token.TokenType.KNIGHT:
				if (this.CBknight2.transform.GetChild (0).transform.childCount != 0)
					token = this.CBknight2.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				else if (this.CBknight1.transform.GetChild (0).transform.childCount != 0)
					token = this.CBknight1.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				break;
			case Token.TokenType.SILVER:
				if (this.CBsilver2.transform.GetChild (0).transform.childCount != 0)
					token = this.CBsilver2.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				else if (this.CBsilver1.transform.GetChild (0).transform.childCount != 0)
					token = this.CBsilver1.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				break;
			case Token.TokenType.GOLD:
				if (this.CBgold2.transform.GetChild (0).transform.childCount != 0)
					token = this.CBgold2.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				else if (this.CBgold1.transform.GetChild (0).transform.childCount != 0)
					token = this.CBgold1.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				break;
			case Token.TokenType.BISHOP:
				if (this.CBbishop2.transform.GetChild (0).transform.childCount != 0)
					token = this.CBbishop2.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				else if (this.CBbishop1.transform.GetChild (0).transform.childCount != 0)
					token = this.CBbishop1.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				break;
			case Token.TokenType.ROOK:
				Debug.Log ("Bon");
				if (this.CBrook2.transform.GetChild (0).transform.childCount != 0)
				{
					Debug.Log ("Pas Bon");
					token = this.CBrook2.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				}
				else if (this.CBrook1.transform.GetChild (0).transform.childCount != 0)
				{
					Debug.Log ("WTFF");
					token = this.CBrook1.transform.GetChild (0).transform.GetChild (0).transform.GetComponent <TokenAnimator> ();
				}
				break;
			default:
				token = null;
				break;
		}

		if (token == null)
		{
			Debug.Log ("NULL LOLOLOL");
			return null;
		}
		
		token.transform.parent = parent;
		token.isCaptured = false;

		return token;
	}
}
