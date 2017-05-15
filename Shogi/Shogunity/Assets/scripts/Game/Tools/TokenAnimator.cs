using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class TokenAnimator : MonoBehaviour
{
	public Token.TokenType tokenType;
	public IntVector2 coord;
	public bool goteIsTheOwner = true;
	public bool selected = false;
	public bool promotedToken = false;
	public bool isCaptured = false;

	public void OnMouseDown ()
	{
		Debug.Log ("Token " + this.name + " Clicked");

		if ((_GameManager.instance.player1.playerType == ShogiUtils.PlayerType.PLAYER &&
			this.goteIsTheOwner && _GameManager.instance.currentPlayerIndex == 1) ||
			(_GameManager.instance.player2.playerType == ShogiUtils.PlayerType.PLAYER &&
			!this.goteIsTheOwner && _GameManager.instance.currentPlayerIndex == 2))
		{
			Debug.Log ("OKKK Current Player = " + _GameManager.instance.currentPlayerIndex + " goteIsTheOwner = " + this.goteIsTheOwner);
//			_GameBoard.instance.showLegalMove (this);
			_GameManager.instance.selectedToken = this;
			_GameBoard.instance.unloadEnemyTokensColliders (goteIsTheOwner);
			_GameBoard.instance.loadBoxesColliders ();
			this.selected = true;
		}
		else
			Debug.Log ("Nope its not mine");
	}
}
