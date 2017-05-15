using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShogiUtils;
using System;
using Tools;

public class _GameBoard : MonoBehaviour
{
	public GameObject box;
	public GameObject originBox;
	public GameObject rightmostBox;
	public GameObject topmostBox;
	public GameObject pawnPrefabs;
	public GameObject lancePrefabs;
	public GameObject knightPrefabs;
	public GameObject silverPrefabs;
	public GameObject goldPrefabs;
	public GameObject bishopPrefabs;
	public GameObject rookPrefabs;
	public GameObject jewelPrefabs;
	public GameObject kingPrefabs;
	public List<Box> boxes = new List<Box>();
	public GameObject player1Bench;
	public GameObject player2Bench;

	public Texture textureToUse;

	public static _GameBoard instance = null;

	private Node _currentState;
	private bool _gameBoardCreated = false;
	private bool _gameBoardInitialized = false;
	public bool gameBoardIsInitialized { get { return _gameBoardInitialized; } }
	private List<GameObject> _allTokens = new List<GameObject> ();
	private bool _reinit = false;
	[Range(1, 2)]
	private int _currentView = 1;
	private TokenAnimator _tmpToken;

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (this);
	}
		
	void Start ()
	{
		GameObject tmpBox;
		Vector3 tmpBoxPosition;
		float xSep = Vector3.Distance(originBox.transform.position, rightmostBox.transform.position) / 8;
		float zSep = Vector3.Distance(originBox.transform.position, topmostBox.transform.position) / 8;

		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				tmpBoxPosition = new Vector3 (originBox.transform.position.x + (float) (j * xSep), originBox.transform.position.y, originBox.transform.position.z + (float) (i * zSep));
				tmpBox = Instantiate (box, tmpBoxPosition, Quaternion.identity, this.transform.GetChild(2).transform);
				tmpBox.name = "box_" + (i) + "_" + (j);
				tmpBox.GetComponent <Box> ().coord = new IntVector2 (j, i);
				tmpBox.GetComponent <Box> ().GetComponent <BoxCollider>().size = Vector3.zero;
				this.boxes.Add (tmpBox.GetComponent <Box> ());
			}
		}

		this._gameBoardCreated = true;
	}

	public void setCurrentState (Node currentState)
	{
		this._currentState = new Node (currentState);
	}

	public Node getCurrentState ()
	{
		return this._currentState;
	}

	public void reInit ()
	{
		for (int i = 0; i < this._allTokens.Count; i++)
			GameObject.DestroyImmediate (this._allTokens [i]);
		this._allTokens.Clear ();

		for (int i = 0; i < this.boxes.Count; i++)
		{
			if (this.boxes [i].busy)
			{
				this.boxes [i].token = null;
				this.boxes [i].busy = false;
			}
		}

		this._reinit = true;
		StartCoroutine ("coInitState");
		this._reinit = false;
	}

	public void initState ()
	{
		StartCoroutine ("coInitState");
	}

	private IEnumerator coInitState ()
	{
		while (! this._gameBoardCreated && ! this._reinit)
			yield return new WaitForSeconds (0.1f);

		GameObject tmpGameObject;
		Box tmpBox;
		bool goteIsTheOwner;
		Token.TokenType tokenType;
		Transform parent;
		int [,] tmpState = new int[9,9];

		this._currentState.getState (ref tmpState);

		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				if (tmpState [i, j] != 0)
				{
					tmpBox = this.boxes [i * 9 + j];
					tokenType = Token.getTokenType (tmpState [i, j]);
					goteIsTheOwner = tmpState [i, j] > 0 ? true : false;
					parent = goteIsTheOwner ? this.transform.GetChild (0).transform.GetChild(0).transform : this.transform.GetChild (1).transform.GetChild(0).transform;

					switch (tokenType)
					{
						case Token.TokenType.PAWN:
							tmpGameObject = Instantiate (pawnPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							break;
						case Token.TokenType.PROMOTED_PAWN:
							tmpGameObject = Instantiate (pawnPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							tmpBox.token.promotedToken = true;
							break;
						case Token.TokenType.LANCE:
							tmpGameObject = Instantiate (lancePrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							break;
						case Token.TokenType.PROMOTED_LANCE:
							tmpGameObject = Instantiate (lancePrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							tmpBox.token.promotedToken = true;
							break;
						case Token.TokenType.KNIGHT:
							tmpGameObject = Instantiate (knightPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							break;
						case Token.TokenType.PROMOTED_KNIGHT:
							tmpGameObject = Instantiate (knightPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							tmpBox.token.promotedToken = true;
							break;
						case Token.TokenType.SILVER:
							tmpGameObject = Instantiate (silverPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							break;
						case Token.TokenType.PROMOTED_SILVER:
							tmpGameObject = Instantiate (silverPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							tmpBox.token.promotedToken = true;
							break;
						case Token.TokenType.GOLD:
							tmpGameObject = Instantiate (goldPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							break;
						case Token.TokenType.BISHOP:
							tmpGameObject = Instantiate (bishopPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							break;
						case Token.TokenType.PROMOTED_BISHOP:
							tmpGameObject = Instantiate (bishopPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							tmpBox.token.promotedToken = true;
							break;
						case Token.TokenType.ROOK:
							tmpGameObject = Instantiate (rookPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							break;
						case Token.TokenType.PROMOTED_ROOK:
							tmpGameObject = Instantiate (rookPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
							tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							tmpBox.token.promotedToken = true;
							break;
						case Token.TokenType.KING:
							if (goteIsTheOwner)
							{
								tmpGameObject = Instantiate (kingPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
								tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							}
							else
							{
								tmpGameObject = Instantiate (jewelPrefabs, tmpBox.transform.position, Quaternion.identity, parent);
								tmpBox.token = tmpGameObject.GetComponent <TokenAnimator> ();
							}
							break;
						default:
							yield break;
					}

					if (goteIsTheOwner)
						tmpBox.token.gameObject.transform.rotation = Quaternion.Euler (tmpBox.token.promotedToken ? 90 : 270, 0, tmpBox.token.promotedToken ? 0 : 180);
					else
						tmpBox.token.gameObject.transform.rotation = Quaternion.Euler (tmpBox.token.promotedToken ? 90 : 270, 0, tmpBox.token.promotedToken ? 180 : 0);
					
					tmpBox.token.gameObject.transform.GetChild(0).GetComponent <Renderer>().materials[1].SetTexture("_MainTex", textureToUse);
					tmpBox.token.coord = tmpBox.coord;
					tmpBox.token.goteIsTheOwner = goteIsTheOwner;
					tmpBox.token.selected = false;
					tmpBox.token.isCaptured = false;

					tmpBox.busy = true;

					_allTokens.Add (tmpBox.token.gameObject);
				}
			}
		}
		this._gameBoardInitialized = true;
	}

	public void moveToken (Node move, bool isUndo)
	{
		Box newBox;
		Token.TokenInfo moveInfo = move.getTokenInfo ();

		isUndo = isUndo;

		if (moveInfo.newY < 0 || moveInfo.newX < 0)
		{
			Debug.Log ("Invalid New Position");
			return;
		}

		newBox = this.boxes [moveInfo.newY * 9 + moveInfo.newX];

		if (! moveInfo.droppedPiece)
		{
			Box oldBox = this.boxes [moveInfo.oldY * 9 + moveInfo.oldX];

			if (moveInfo.capturedSomething)
			{
				if (moveInfo.capturedValue < 0)
					this.player1Bench.transform.GetChild (1).GetComponent <CaptureBench> ().AddToken (newBox.token);
				else
					this.player2Bench.transform.GetChild (1).GetComponent <CaptureBench> ().AddToken (newBox.token);
			}

			newBox.token = oldBox.token;
			newBox.token.transform.position = newBox.transform.position;
			oldBox.token = null;
			oldBox.busy = false;
		}
		else
		{
			if (moveInfo.newValue > 0)
				newBox.token = this.player1Bench.transform.GetChild (1).GetComponent <CaptureBench> ().GetToken (moveInfo.tokenType);
			else
				newBox.token = this.player2Bench.transform.GetChild (1).GetComponent <CaptureBench> ().GetToken (moveInfo.tokenType);

			newBox.token.gameObject.transform.position = newBox.transform.position;
		}
			
		if (moveInfo.promoted)
		{
			if (moveInfo.newValue > 0)		// Gote Piece
				newBox.token.gameObject.gameObject.transform.rotation = Quaternion.Euler (90, 0, 0);
			else
				newBox.token.gameObject.gameObject.transform.rotation = Quaternion.Euler (90, 0, 180);
		}

		newBox.token.coord = newBox.coord;
		newBox.busy = true;
		this._currentState = new Node (move);
	}

	public void unloadEnemyTokensColliders (bool isGote)
	{
		for (int i = 0; i < this._allTokens.Count; i++)
			if (this._allTokens [i].GetComponent <TokenAnimator>().goteIsTheOwner != isGote)
				this._allTokens [i].GetComponent <BoxCollider>().size = Vector3.zero;
	}

	public void reloadAllTokensColliders ()
	{
		for (int i = 0; i < this._allTokens.Count; i++)
			this._allTokens [i].GetComponent <BoxCollider>().size = new Vector3 (3.15f, 3.6f, 1);
	}

	public void loadBoxesColliders ()
	{
		for (int i = 0; i < this.boxes.Count; i++)
			this.boxes [i].GetComponent <BoxCollider>().size = new Vector3 (20, 8, 3.5f);
	}

	public void unloadBoxesColliders ()
	{
		for (int i = 0; i < this.boxes.Count; i++)
			this.boxes [i].GetComponent <BoxCollider>().size = Vector3.zero;
	}

	private bool canMoveLikeAPawn (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece, bool goteIsTheOwner)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && ((goteIsTheOwner && expectedCoord.y < 8) || (!goteIsTheOwner && expectedCoord.y > 0)) && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;		// you cant drop the lance in the last line

		if (currentCoord.x != expectedCoord.x || Math.Abs (currentCoord.y - expectedCoord.y) != 1)
			return false;		// A pawn can't change his column and a pawn can only jump 1 case

		if ((_GameManager.instance.currentPlayerIndex == 1 && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && expectedCoord.y - currentCoord.y > 0))
			return false;		// A pawn can only move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	private bool canMoveLikeALance (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece, bool goteIsTheOwner)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && ((goteIsTheOwner && expectedCoord.y < 8) || (!goteIsTheOwner && expectedCoord.y > 0)) && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;		// you cant drop the lance in the last line

		if (currentCoord.x != expectedCoord.x || Math.Abs (currentCoord.y - expectedCoord.y) < 1 || Math.Abs (currentCoord.y - expectedCoord.y) > 2)
			return false;		// A lance can't change his column and a pawn can only jump 1 or 2 case

		if ((_GameManager.instance.currentPlayerIndex == 1 && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && expectedCoord.y - currentCoord.y > 0))
			return false;		// A lance can only move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAKnight (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece, bool goteIsTheOwner)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && ((goteIsTheOwner && expectedCoord.y < 7) || (!goteIsTheOwner && expectedCoord.y > 1)) && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;		// you cant drop the knight in the last line

		if (Math.Abs (currentCoord.x - expectedCoord.x) != 1 || Math.Abs (currentCoord.y - expectedCoord.y) != 2)
			return false;

		if ((_GameManager.instance.currentPlayerIndex == 1 && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && expectedCoord.y - currentCoord.y > 0))
			return false;		// A knight can only move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	private bool canMoveLikeASilver (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;
		
		if (Math.Abs (currentCoord.x - expectedCoord.x) > 1 || Math.Abs (currentCoord.y - expectedCoord.y) != 1)
			return false;

		if ((_GameManager.instance.currentPlayerIndex == 1 && currentCoord.x == expectedCoord.x && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && currentCoord.x == expectedCoord.x && expectedCoord.y - currentCoord.y > 0))
			return false;		// A silver can stay on the same column only if he move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAGold (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) > 1 || Math.Abs (currentCoord.y - expectedCoord.y) > 1)
			return false;

		if ((_GameManager.instance.currentPlayerIndex == 1 && currentCoord.x != expectedCoord.x && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && currentCoord.x != expectedCoord.x && expectedCoord.y - currentCoord.y > 0))
			return false;		// A gold can move in diagonal only if he move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	private bool canMoveLikeABishop (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) != Math.Abs (currentCoord.y - expectedCoord.y))
			return false;		// its not a diagonale

		if (currentCoord.x == expectedCoord.x && currentCoord.y == expectedCoord.y)
			return false;		// you are not moving

		int xDir, yDir;

		xDir = currentCoord.x < expectedCoord.x ? 1 : -1;
		yDir = currentCoord.y < expectedCoord.y ? 1 : -1;

		for (int y = currentCoord.y + yDir; y != expectedCoord.y; y += yDir)
			for (int x = currentCoord.x + xDir; x != expectedCoord.x; x += xDir)
				if (curState [y, x] != 0)
				{
					Debug.Log ("WTF 3");
					return false;
				}

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAPromotedBishop (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) <= 1 && Math.Abs (currentCoord.y - expectedCoord.y) <= 1)
		{
			if (Math.Abs (currentCoord.x - expectedCoord.x) == 0 && Math.Abs (currentCoord.y - expectedCoord.y) == 0)
				return false;
			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
		}
		else
		{
			if (Math.Abs (currentCoord.x - expectedCoord.x) != Math.Abs (currentCoord.y - expectedCoord.y))
				return false;		// its not a diagonale

			if (currentCoord.x == expectedCoord.x && currentCoord.y == expectedCoord.y)
				return false;		// you are not moving

			int xDir, yDir;

			xDir = currentCoord.x < expectedCoord.x ? 1 : -1;
			yDir = currentCoord.y < expectedCoord.y ? 1 : -1;

			for (int y = currentCoord.y + yDir; y != expectedCoord.y; y += yDir)
				for (int x = currentCoord.x + xDir; x != expectedCoord.x; x += xDir)
					if (curState[y, x] != 0)
						return false;

			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
		}
	}

	private bool canMoveLikeARook (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) != 0 && Math.Abs (currentCoord.y - expectedCoord.y) != 0)
			return false;		// its not a line

		if (currentCoord.x == expectedCoord.x && currentCoord.y == expectedCoord.y)
			return false;		// you are not moving

		int lineBegin, lineEnd, lineDir;

		lineBegin = currentCoord.x == expectedCoord.x ? currentCoord.y : currentCoord.x;
		lineEnd = currentCoord.x == expectedCoord.x ? expectedCoord.y : expectedCoord.x;
		lineDir = lineBegin < lineEnd ? 1 : -1;

		for (int point = lineBegin + lineDir; point < lineEnd; point += lineDir)
			if (currentCoord.x == expectedCoord.x ? curState [point, expectedCoord.x] != 0 : curState [expectedCoord.y, point] != 0)
				return false;

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAPromotedRook (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y, expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) <= 1 && Math.Abs (currentCoord.y - expectedCoord.y) <= 1)
		{
			if (Math.Abs (currentCoord.x - expectedCoord.x) == 0 && Math.Abs (currentCoord.y - expectedCoord.y) == 0)
				return false;
			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
		}
		else
		{
			if (Math.Abs (currentCoord.x - expectedCoord.x) != 0 && Math.Abs (currentCoord.y - expectedCoord.y) != 0)
				return false;		// its not a line

			if (currentCoord.x == expectedCoord.x && currentCoord.y == expectedCoord.y)
				return false;		// you are not moving

			int lineBegin, lineEnd, lineDir;

			lineBegin = currentCoord.x == expectedCoord.x ? currentCoord.y : currentCoord.x;
			lineEnd = currentCoord.x == expectedCoord.x ? expectedCoord.y : expectedCoord.x;
			lineDir = lineBegin < lineEnd ? 1 : -1;

			for (int point = lineBegin + lineDir; point < lineEnd; point += lineDir)
				if (currentCoord.x == expectedCoord.x ? curState [point, expectedCoord.x] != 0 : curState [expectedCoord.y, point] != 0)
					return false;

			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
		}
	}

	private bool canMoveLikeAKing (IntVector2 currentCoord, IntVector2 expectedCoord)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (Math.Abs (currentCoord.x - expectedCoord.x) > 1 || Math.Abs (currentCoord.y - expectedCoord.y) > 1)
			return false;

		if (Math.Abs (currentCoord.x - expectedCoord.x) == 0 && Math.Abs (currentCoord.y - expectedCoord.y) == 0)
			return false;

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y, expectedCoord.x] <= 0 : curState [expectedCoord.y, expectedCoord.x] >= 0);
	}

	public bool checkIfMoveIsPossible (TokenAnimator token, IntVector2 pos)
	{
		switch (token.tokenType)
		{
			case Token.TokenType.PAWN:
				return canMoveLikeAPawn (token.coord, pos, token.isCaptured, token.goteIsTheOwner);
			case Token.TokenType.LANCE:
				return canMoveLikeALance (token.coord, pos, token.isCaptured, token.goteIsTheOwner);
			case Token.TokenType.KNIGHT:
				return canMoveLikeAKnight (token.coord, pos, token.isCaptured, token.goteIsTheOwner);
			case Token.TokenType.SILVER:
				return canMoveLikeASilver (token.coord, pos, token.isCaptured);
			case Token.TokenType.PROMOTED_PAWN:
			case Token.TokenType.PROMOTED_LANCE:
			case Token.TokenType.PROMOTED_KNIGHT:
			case Token.TokenType.PROMOTED_SILVER:
			case Token.TokenType.GOLD:
				return canMoveLikeAGold (token.coord, pos, token.isCaptured);
			case Token.TokenType.BISHOP:
				return canMoveLikeABishop (token.coord, pos, token.isCaptured);
			case Token.TokenType.PROMOTED_BISHOP:
				return canMoveLikeAPromotedBishop (token.coord, pos, token.isCaptured);
			case Token.TokenType.ROOK:
				return canMoveLikeARook (token.coord, pos, token.isCaptured);
			case Token.TokenType.PROMOTED_ROOK:
				return canMoveLikeAPromotedRook (token.coord, pos, token.isCaptured);
			case Token.TokenType.KING:
				return canMoveLikeAKing (token.coord, pos);
			default:
				return false;
		}
	}

	private IEnumerator Promote ()
	{
		while (_GameManager.instance.WaitingPromotion)
			yield return null;

		if (_GameManager.instance.Promote ())
		{
			this._tmpToken.tokenType = Token.getPromotedTokenType (this._tmpToken.tokenType);
			this._tmpToken.promotedToken = true;
			this._tmpToken.gameObject.transform.rotation = Quaternion.Euler (90, 0, this._tmpToken.goteIsTheOwner ? 0 : 180);
		}
	}

	public void applyMove (Box box, TokenAnimator token)
	{
		int[,] curState = new int[9,9];
		this._currentState.getState (ref curState);

		if (curState [box.coord.y, box.coord.x] != 0)
		{
			List <Token.TokenType> capturedTokens;

			if (curState [box.coord.y, box.coord.x] < 0)
			{
				capturedTokens = this._currentState.getGoteCapturedTokens ();
				capturedTokens.Add (box.token.tokenType);
				this._currentState.setGoteCapturedTokens (capturedTokens);
				this.player1Bench.transform.GetChild (1).GetComponent <CaptureBench> ().AddToken (box.token);
				_GameManager.instance.player1Score -= curState [box.coord.y, box.coord.x];
			}
			else
			{
				capturedTokens = this._currentState.getSenteCapturedTokens ();
				capturedTokens.Add (box.token.tokenType);
				this._currentState.setSenteCapturedTokens (capturedTokens);
				this.player2Bench.transform.GetChild (1).GetComponent <CaptureBench> ().AddToken (box.token);
				_GameManager.instance.player2Score += curState [box.coord.y, box.coord.x];
			}
		}

		if (! token.isCaptured)
		{
			Box oldBox = this.boxes [token.coord.y * 9 + token.coord.x];

			if (! token.promotedToken && ((token.goteIsTheOwner && box.coord.y >= 6) || (!token.goteIsTheOwner && box.coord.y <= 2)) &&
				Token.isAPromotableToken (token.tokenType))		// promotion zone
			{
				_GameManager.instance.AskForPromotion ();

				this._tmpToken = token;
				StartCoroutine ("Promote");
			}

			oldBox.token = null;
			oldBox.busy = false;
			curState [token.coord.y, token.coord.x] = 0;
		}
		else
		{
			List <Token.TokenType> capturedTokens;

			if (token.goteIsTheOwner)
			{
				capturedTokens = this._currentState.getGoteCapturedTokens ();
				capturedTokens.Remove (token.tokenType);
				this._currentState.setGoteCapturedTokens (capturedTokens);
				this.player1Bench.transform.GetChild (1).GetComponent <CaptureBench> ().GetToken (token.tokenType);
			}
			else
			{
				capturedTokens = this._currentState.getSenteCapturedTokens ();
				capturedTokens.Remove (token.tokenType);
				this._currentState.setSenteCapturedTokens (capturedTokens);
				this.player2Bench.transform.GetChild (1).GetComponent <CaptureBench> ().GetToken (token.tokenType);
			}
		}

		box.token = token;
		box.token.coord = box.coord;
		box.token.transform.position = box.transform.position;
		box.busy = true;

		curState [box.coord.y, box.coord.x] = Token.getTokenValue (token.tokenType, token.goteIsTheOwner);
		this._currentState.setState (ref curState);
	}
}
