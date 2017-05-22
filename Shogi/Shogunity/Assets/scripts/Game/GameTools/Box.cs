using UnityEngine;
using System.Collections;
using ShogiUtils;
using System;
using Tools;

public class Box : MonoBehaviour
{
	public GameObject surface;
	public Transform center;
	public IntVector2 coord;
	public bool busy;
	public TokenAnimator token;

	void OnMouseDown ()
	{
		Debug.Log ("Case " + this.name + " Clicked [Coord (" + coord.x + ";" + coord.y + ")]");
		if (_GameManager.instance.selectedToken == null)
			return;
		else
		{
			if (!_GameBoard.instance.checkIfMoveIsPossible (_GameManager.instance.selectedToken, this.coord))		// remove the focus
			{
				Debug.Log ("invalid Movement");
				_GameBoard.instance.reloadAllTokensColliders ();
				_GameBoard.instance.unloadBoxesColliders ();
				_GameBoard.instance.hideLegalMoves ();
				_GameManager.instance.selectedToken.selected = false;
				_GameManager.instance.selectedToken = null;
				return;
			}
			Debug.Log ("valid Movement");
			_GameBoard.instance.applyMove (this, _GameManager.instance.selectedToken);
			_GameBoard.instance.reloadAllTokensColliders ();
			_GameBoard.instance.unloadBoxesColliders ();
			_GameBoard.instance.hideLegalMoves ();
			_GameManager.instance.currentState = _GameBoard.instance.getCurrentState ();
			_GameManager.instance.nextTurn ();
		}
	}
}
