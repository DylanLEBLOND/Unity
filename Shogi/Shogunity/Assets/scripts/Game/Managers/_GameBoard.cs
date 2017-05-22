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
		int [][] tmpState = new int[9][];
		for (int i = 0; i < 9; i++)
			tmpState [i] = new int[9];

		this._currentState.getState (ref tmpState);

		for (int i = 0; i < 9; i++)
		{
			for (int j = 0; j < 9; j++)
			{
				if (tmpState [i][j] != 0)
				{
					tmpBox = this.boxes [i * 9 + j];
					tokenType = Token.getTokenType (tmpState [i][j]);
					goteIsTheOwner = tmpState [i][j] > 0 ? true : false;
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

	public void hideLegalMoves ()
	{
		for (int i = 0; i < this.boxes.Count; i++)
		{
			this.boxes [i].gameObject.gameObject.transform.rotation = Quaternion.Euler (90, 0, 0);
			this.boxes [i].gameObject.GetComponent <Renderer> ().material.color = new Color (0, 0, 0, 0);
		}
	}

	private bool columnBusy (int x, bool goteIsTheOwner)
	{
		for (int y = 0; y < 9; y++)
			if (this.boxes [y * 9 + x].busy && this.boxes [y * 9 + x].token.tokenType == Token.TokenType.PAWN && this.boxes [y * 9 + x].token.goteIsTheOwner == goteIsTheOwner)
				return true;
		return false;
	}

	private void showDropMoves (Token.TokenType tokenType, bool goteIsTheOwner)
	{
		int index;

		for (int y = 0; y < 9; y++)
		{
			for (int x = 0; x < 9; x++)
			{
				index = y * 9 + x;
				if (this.boxes [index].busy || (tokenType == Token.TokenType.PAWN && this.columnBusy (x, goteIsTheOwner)))
					continue;
				else if (((goteIsTheOwner && y >= 8) || (!goteIsTheOwner && y <= 0)) &&
						(tokenType == Token.TokenType.PAWN || tokenType == Token.TokenType.LANCE))
				{
					this.boxes [index].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				}
				else if (((goteIsTheOwner && y >= 7) || (!goteIsTheOwner && y <= 1)) && tokenType == Token.TokenType.KNIGHT)
				{
					this.boxes [index].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				}
				else
				{
					this.boxes [index].gameObject.GetComponent <Renderer> ().material.color = Color.green;
					this.boxes [index].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
				}
			}
		}
	}

	private void showPawnMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		if ((goteIsTheOwner && coord.y >= 8) || (!goteIsTheOwner && coord.y <= 0))
			return;

		int yDir = goteIsTheOwner ? 1 : -1;
		int nextIndex = (coord.y + yDir) * 9 + coord.x;

		if (this.boxes[nextIndex].busy && this.boxes[nextIndex].token.goteIsTheOwner == goteIsTheOwner)
			this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
		else
			this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

		this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
	}

	private void showLanceMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		if ((goteIsTheOwner && coord.y >= 8) || (!goteIsTheOwner && coord.y <= 0))
			return;

		int yDir = goteIsTheOwner ? 1 : -1;
		int nextIndex = (coord.y + yDir) * 9 + coord.x;
		bool obstacleFound = false;

		if (this.boxes[nextIndex].busy)
			obstacleFound = true;

		if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
			this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
		else
			this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

		this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);

		if ((goteIsTheOwner && coord.y >= 7) || (!goteIsTheOwner && coord.y <= 1))
			return;

		int nextNextIndex = (coord.y + 2 * yDir) * 9 + coord.x;

		if (obstacleFound || (this.boxes[nextNextIndex].busy && this.boxes[nextNextIndex].token.goteIsTheOwner == goteIsTheOwner))
			this.boxes [nextNextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
		else
			this.boxes [nextNextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

		this.boxes [nextNextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
	}

	public void showKnightMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		if ((goteIsTheOwner && coord.y >= 7) || (!goteIsTheOwner && coord.y <= 1))
			return;

		int yDir = goteIsTheOwner ? 2 : -2;

		if (coord.x > 0)
		{
			int nextIndexLeft = (coord.y + yDir) * 9 + (coord.x - 1);

			if (this.boxes [nextIndexLeft].busy && this.boxes [nextIndexLeft].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		if (coord.x < 8)
		{
			int nextIndexRight = (coord.y + yDir) * 9 + (coord.x + 1);

			if (this.boxes [nextIndexRight].busy && this.boxes [nextIndexRight].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}
	}

	public void showSilverMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		int nextIndexRight;
		int nextIndexLeft;

		if (coord.y > 0)
		{
			if (coord.x > 0)
			{
				nextIndexLeft = (coord.y - 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexLeft].busy && this.boxes [nextIndexLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			if (! goteIsTheOwner)
			{
				int nextIndexCenter = (coord.y - 1) * 9 + coord.x;

				if (this.boxes [nextIndexCenter].busy && this.boxes [nextIndexCenter].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexCenter].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexCenter].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexCenter].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			if (coord.x < 8)
			{
				nextIndexRight = (coord.y - 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexRight].busy && this.boxes [nextIndexRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}

		if (coord.y < 8)
		{
			if (coord.x > 0)
			{
				nextIndexLeft = (coord.y + 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexLeft].busy && this.boxes [nextIndexLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			if (goteIsTheOwner)
			{
				int nextIndexCenter = (coord.y + 1) * 9 + coord.x;

				if (this.boxes [nextIndexCenter].busy && this.boxes [nextIndexCenter].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexCenter].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexCenter].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexCenter].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			if (coord.x < 8)
			{
				nextIndexRight = (coord.y + 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexRight].busy && this.boxes [nextIndexRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}
	}

	public void showGoldMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		int nextIndexLeft;
		int nextIndexRight;
		int nextIndexBot;
		int nextIndexTop;

		if (coord.x > 0)
		{
			nextIndexLeft = coord.y * 9 + (coord.x - 1);

			if (this.boxes [nextIndexLeft].busy && this.boxes [nextIndexLeft].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		if (coord.x < 8)
		{
			nextIndexRight = coord.y * 9 + (coord.x + 1);

			if (this.boxes [nextIndexRight].busy && this.boxes [nextIndexRight].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		if (coord.y > 0)
		{
			if (!goteIsTheOwner && coord.x > 0)
			{
				int nextIndexDiagLeft = (coord.y - 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexDiagLeft].busy && this.boxes [nextIndexDiagLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexDiagLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexDiagLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexDiagLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			nextIndexBot = (coord.y - 1) * 9 + coord.x;

			if (this.boxes [nextIndexBot].busy && this.boxes [nextIndexBot].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexBot].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexBot].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexBot].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);

			if (!goteIsTheOwner && coord.x < 8)
			{
				int nextIndexDiagRight = (coord.y - 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexDiagRight].busy && this.boxes [nextIndexDiagRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexDiagRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexDiagRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexDiagRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}

		if (coord.y < 8)
		{
			if (goteIsTheOwner && coord.x > 0)
			{
				int nextIndexDiagLeft = (coord.y + 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexDiagLeft].busy && this.boxes [nextIndexDiagLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexDiagLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexDiagLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexDiagLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			nextIndexTop = (coord.y + 1) * 9 + coord.x;

			if (this.boxes [nextIndexTop].busy && this.boxes [nextIndexTop].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexTop].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexTop].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexTop].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);

			if (goteIsTheOwner && coord.x < 8)
			{
				int nextIndexDiagRight = (coord.y + 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexDiagRight].busy && this.boxes [nextIndexDiagRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexDiagRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexDiagRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexDiagRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}
	}

	public void showBishopMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		int nextX, nextY, nextIndex;
		bool obstacleFound;

		/* Top Right */

		obstacleFound = false;
		nextY = coord.y + 1;
		for (nextX = coord.x + 1; nextY < 9 && nextX < 9; nextY++, nextX++)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Top Left */

		obstacleFound = false;
		nextY = coord.y + 1;
		for (nextX = coord.x - 1; nextY < 9 && nextX >= 0; nextY++, nextX--)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Bottom Right */

		obstacleFound = false;
		nextY = coord.y - 1;
		for (nextX = coord.x + 1; nextY >= 0 && nextX < 9; nextY--, nextX++)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Bottom Left */

		obstacleFound = false;
		nextY = coord.y - 1;
		for (nextX = coord.x - 1; nextY >= 0 && nextX >= 0; nextY--, nextX--)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}
	}

	public void showPromotedBishopMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		int nextX, nextY, nextIndex;
		int nextIndexLeft;
		int nextIndexRight;
		int nextIndexBot;
		int nextIndexTop;
		bool obstacleFound;

		/* Top Right */

		obstacleFound = false;
		nextY = coord.y + 1;
		for (nextX = coord.x + 1; nextY < 9 && nextX < 9; nextY++, nextX++)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Top Left */

		obstacleFound = false;
		nextY = coord.y + 1;
		for (nextX = coord.x - 1; nextY < 9 && nextX >= 0; nextY++, nextX--)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Bottom Right */

		obstacleFound = false;
		nextY = coord.y - 1;
		for (nextX = coord.x + 1; nextY >= 0 && nextX < 9; nextY--, nextX++)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Bottom Left */

		obstacleFound = false;
		nextY = coord.y - 1;
		for (nextX = coord.x - 1; nextY >= 0 && nextX >= 0; nextY--, nextX--)
		{
			nextIndex = nextY * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Top */

		if (coord.y < 8)
		{
			nextIndexTop = (coord.y + 1) * 9 + coord.x;

			if (this.boxes [nextIndexTop].busy && this.boxes [nextIndexTop].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexTop].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexTop].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexTop].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Bot */

		if (coord.y > 0)
		{
			nextIndexBot = (coord.y - 1) * 9 + coord.x;

			if (this.boxes [nextIndexBot].busy && this.boxes [nextIndexBot].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexBot].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexBot].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexBot].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Left */

		if (coord.x > 0)
		{
			nextIndexLeft = coord.y * 9 + (coord.x - 1);

			if (this.boxes [nextIndexLeft].busy && this.boxes [nextIndexLeft].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Right */

		if (coord.x < 8)
		{
			nextIndexRight = coord.y * 9 + (coord.x + 1);

			if (this.boxes [nextIndexRight].busy && this.boxes [nextIndexRight].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}
	}

	public void showRookMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		int nextX, nextY, nextIndex;
		bool obstacleFound;

		/* Top */

		obstacleFound = false;
		for (nextY = coord.y + 1; nextY < 9; nextY++)
		{
			nextIndex = nextY * 9 + coord.x;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Bot */

		obstacleFound = false;
		for (nextY = coord.y - 1; nextY >= 0; nextY--)
		{
			nextIndex = nextY * 9 + coord.x;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Right */

		obstacleFound = false;
		for (nextX = coord.x + 1; nextX < 9; nextX++)
		{
			nextIndex = coord.y * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Left */

		obstacleFound = false;
		for (nextX = coord.x - 1; nextX >= 0; nextX--)
		{
			nextIndex = coord.y * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}
	}

	public void showPromotedRookMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		int nextX, nextY, nextIndex;
		int nextIndexTopLeft;
		int nextIndexTopRight;
		int nextIndexBotLeft;
		int nextIndexBotRight;
		bool obstacleFound;

		/* Top */

		obstacleFound = false;
		for (nextY = coord.y + 1; nextY < 9; nextY++)
		{
			nextIndex = nextY * 9 + coord.x;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Bot */

		obstacleFound = false;
		for (nextY = coord.y - 1; nextY >= 0; nextY--)
		{
			nextIndex = nextY * 9 + coord.x;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Right */

		obstacleFound = false;
		for (nextX = coord.x + 1; nextX < 9; nextX++)
		{
			nextIndex = coord.y * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Left */

		obstacleFound = false;
		for (nextX = coord.x - 1; nextX >= 0; nextX--)
		{
			nextIndex = coord.y * 9 + nextX;

			if (! obstacleFound)
			{
				if (this.boxes [nextIndex].busy && this.boxes [nextIndex].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				if (this.boxes [nextIndex].busy)
					obstacleFound = true;
			}
			else if (obstacleFound)
				this.boxes [nextIndex].gameObject.GetComponent <Renderer> ().material.color = Color.red;

			this.boxes [nextIndex].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Top Diagonal */

		if (coord.y < 8)
		{
			/* Top Left */

			if (coord.x > 0)
			{
				nextIndexTopLeft = (coord.y + 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexTopLeft].busy && this.boxes [nextIndexTopLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexTopLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexTopLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexTopLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			/* Top Right */

			if (coord.x < 8)
			{
				nextIndexTopRight = (coord.y + 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexTopRight].busy && this.boxes [nextIndexTopRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexTopRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexTopRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexTopRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}

		/* Bot Diagonal */

		if (coord.y > 0)
		{
			/* Bot Left */

			if (coord.x > 0)
			{
				nextIndexBotLeft = (coord.y - 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexBotLeft].busy && this.boxes [nextIndexBotLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexBotLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexBotLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexBotLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			/* Bot Right */

			if (coord.x < 8)
			{
				nextIndexBotRight = (coord.y - 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexBotRight].busy && this.boxes [nextIndexBotRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexBotRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexBotRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexBotRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}
	}

	public void showKingMoves (IntVector2 coord, bool goteIsTheOwner)
	{
		int nextIndexLeft;
		int nextIndexRight;
		int nextIndexBot;
		int nextIndexTop;
		int nextIndexTopLeft;
		int nextIndexTopRight;
		int nextIndexBotLeft;
		int nextIndexBotRight;

		/* Top */

		if (coord.y < 8)
		{
			/* Top Left */

			if (coord.x > 0)
			{
				nextIndexTopLeft = (coord.y + 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexTopLeft].busy && this.boxes [nextIndexTopLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexTopLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexTopLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexTopLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			/* Top Middle */

			nextIndexTop = (coord.y + 1) * 9 + coord.x;

			if (this.boxes [nextIndexTop].busy && this.boxes [nextIndexTop].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexTop].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexTop].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexTop].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);

			/* Top Right */

			if (coord.x < 8)
			{
				nextIndexTopRight = (coord.y + 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexTopRight].busy && this.boxes [nextIndexTopRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexTopRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexTopRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexTopRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}

		/* Bot */

		if (coord.y > 0)
		{
			/* Bot Left */

			if (coord.x > 0)
			{
				nextIndexBotLeft = (coord.y - 1) * 9 + (coord.x - 1);

				if (this.boxes [nextIndexBotLeft].busy && this.boxes [nextIndexBotLeft].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexBotLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexBotLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexBotLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}

			/* Bot Middle */

			nextIndexBot = (coord.y - 1) * 9 + coord.x;

			if (this.boxes [nextIndexBot].busy && this.boxes [nextIndexBot].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexBot].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexBot].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexBot].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);


			/* Bot Right */

			if (coord.x < 8)
			{
				nextIndexBotRight = (coord.y - 1) * 9 + (coord.x + 1);

				if (this.boxes [nextIndexBotRight].busy && this.boxes [nextIndexBotRight].token.goteIsTheOwner == goteIsTheOwner)
					this.boxes [nextIndexBotRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
				else
					this.boxes [nextIndexBotRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

				this.boxes [nextIndexBotRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
			}
		}

		/* Left */

		if (coord.x > 0)
		{
			nextIndexLeft = coord.y * 9 + (coord.x - 1);

			if (this.boxes [nextIndexLeft].busy && this.boxes [nextIndexLeft].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexLeft].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexLeft].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}

		/* Right */

		if (coord.x < 8)
		{
			nextIndexRight = coord.y * 9 + (coord.x + 1);

			if (this.boxes [nextIndexRight].busy && this.boxes [nextIndexRight].token.goteIsTheOwner == goteIsTheOwner)
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.red;
			else
				this.boxes [nextIndexRight].gameObject.GetComponent <Renderer> ().material.color = Color.green;

			this.boxes [nextIndexRight].gameObject.gameObject.transform.rotation = Quaternion.Euler (-90, 0, 0);
		}
	}

	public void showLegalMoves (TokenAnimator token)
	{
		if (token.isCaptured)
		{
			showDropMoves (token.tokenType, token.goteIsTheOwner);
			return;
		}

		switch (token.tokenType)
		{
			case Token.TokenType.PAWN:
				showPawnMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.LANCE:
				showLanceMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.KNIGHT:
				showKnightMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.SILVER:
				showSilverMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.PROMOTED_PAWN:
			case Token.TokenType.PROMOTED_LANCE:
			case Token.TokenType.PROMOTED_KNIGHT:
			case Token.TokenType.PROMOTED_SILVER:
			case Token.TokenType.GOLD:
				showGoldMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.BISHOP:
				showBishopMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.PROMOTED_BISHOP:
				showPromotedBishopMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.ROOK:
				showRookMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.PROMOTED_ROOK:
				showPromotedRookMoves (token.coord, token.goteIsTheOwner);
				break;
			case Token.TokenType.KING:
				showKingMoves (token.coord, token.goteIsTheOwner);
				break;
			default:
				return;
		}
	}

	private bool canMoveLikeAPawn (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece, bool goteIsTheOwner)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];

		this._currentState.getState (ref curState);

		if (dropPiece && ((goteIsTheOwner && expectedCoord.y < 8) || (!goteIsTheOwner && expectedCoord.y > 0)) && curState [expectedCoord.y] [expectedCoord.x] == 0)
			return true;		// you cant drop the pawn in the last line

		bool alreadyTaken = false;
		for (int i = 0; i < 9; i++)
			if (i != currentCoord.y && curState[i][expectedCoord.x] == (goteIsTheOwner ? Token.GPAWN : Token.SPAWN))
				alreadyTaken = true;

		if (alreadyTaken)
		{
			Debug.Log ("Lol");
			return false;		// you can't drop on a column where an ally pawn is
		}

		if (currentCoord.x != expectedCoord.x || Math.Abs (currentCoord.y - expectedCoord.y) != 1)
			return false;		// A pawn can't change his column and a pawn can only jump 1 case

		if ((_GameManager.instance.currentPlayerIndex == 1 && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && expectedCoord.y - currentCoord.y > 0))
			return false;		// A pawn can only move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
	}

	private bool canMoveLikeALance (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece, bool goteIsTheOwner)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && ((goteIsTheOwner && expectedCoord.y < 8) || (!goteIsTheOwner && expectedCoord.y > 0)) && curState [expectedCoord.y][expectedCoord.x] == 0)
			return true;		// you cant drop the lance in the last line

		if (currentCoord.x != expectedCoord.x || Math.Abs (currentCoord.y - expectedCoord.y) < 1 || Math.Abs (currentCoord.y - expectedCoord.y) > 2)
			return false;		// A lance can't change his column and a lance can only jump 1 or 2 case

		int yDir = _GameManager.instance.currentPlayerIndex == 1 ? 1 : -1;
		if (Math.Abs (currentCoord.y - expectedCoord.y) == 2 && curState [currentCoord.y + yDir] [currentCoord.x] != 0)
			return false;		// A lance can't jump an obstacle

		if ((_GameManager.instance.currentPlayerIndex == 1 && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && expectedCoord.y - currentCoord.y > 0))
			return false;		// A lance can only move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAKnight (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece, bool goteIsTheOwner)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && ((goteIsTheOwner && expectedCoord.y < 7) || (!goteIsTheOwner && expectedCoord.y > 1)) && curState [expectedCoord.y][expectedCoord.x] == 0)
			return true;		// you cant drop the knight in the last line

		if (Math.Abs (currentCoord.x - expectedCoord.x) != 1 || Math.Abs (currentCoord.y - expectedCoord.y) != 2)
			return false;

		if ((_GameManager.instance.currentPlayerIndex == 1 && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && expectedCoord.y - currentCoord.y > 0))
			return false;		// A knight can only move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
	}

	private bool canMoveLikeASilver (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y][expectedCoord.x] == 0)
			return true;
		
		if (Math.Abs (currentCoord.x - expectedCoord.x) > 1 || Math.Abs (currentCoord.y - expectedCoord.y) != 1)
			return false;

		if ((_GameManager.instance.currentPlayerIndex == 1 && currentCoord.x == expectedCoord.x && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && currentCoord.x == expectedCoord.x && expectedCoord.y - currentCoord.y > 0))
			return false;		// A silver can stay on the same column only if he move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAGold (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y][expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) > 1 || Math.Abs (currentCoord.y - expectedCoord.y) > 1)
			return false;

		if ((_GameManager.instance.currentPlayerIndex == 1 && currentCoord.x != expectedCoord.x && expectedCoord.y - currentCoord.y < 0) ||
			(_GameManager.instance.currentPlayerIndex == 2 && currentCoord.x != expectedCoord.x && expectedCoord.y - currentCoord.y > 0))
			return false;		// A gold can move in diagonal only if he move forward

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
	}

	private bool canMoveLikeABishop (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y][expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) != Math.Abs (currentCoord.y - expectedCoord.y))
			return false;		// its not a diagonale

		if (currentCoord.x == expectedCoord.x && currentCoord.y == expectedCoord.y)
			return false;		// you are not moving

		int xDir, yDir;

		xDir = currentCoord.x < expectedCoord.x ? 1 : -1;
		yDir = currentCoord.y < expectedCoord.y ? 1 : -1;

		int x = currentCoord.x + xDir;
		for (int y = currentCoord.y + yDir; y != expectedCoord.y && x != expectedCoord.x; y += yDir, x += xDir)
			if (curState [y] [x] != 0)
				return false;

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAPromotedBishop (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y][expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) <= 1 && Math.Abs (currentCoord.y - expectedCoord.y) <= 1)
		{
			if (Math.Abs (currentCoord.x - expectedCoord.x) == 0 && Math.Abs (currentCoord.y - expectedCoord.y) == 0)
				return false;
			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
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

			int x = currentCoord.x + xDir;
			for (int y = currentCoord.y + yDir; y != expectedCoord.y && x != expectedCoord.x; y += yDir, x += xDir)
				if (curState [y][x] != 0)
					return false;

			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
		}
	}

	private bool canMoveLikeARook (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y][expectedCoord.x] == 0)
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
			if (currentCoord.x == expectedCoord.x ? curState [point][expectedCoord.x] != 0 : curState [expectedCoord.y][point] != 0)
				return false;

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
	}

	private bool canMoveLikeAPromotedRook (IntVector2 currentCoord, IntVector2 expectedCoord, bool dropPiece)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (dropPiece && curState [expectedCoord.y][expectedCoord.x] == 0)
			return true;

		if (Math.Abs (currentCoord.x - expectedCoord.x) <= 1 && Math.Abs (currentCoord.y - expectedCoord.y) <= 1)
		{
			if (Math.Abs (currentCoord.x - expectedCoord.x) == 0 && Math.Abs (currentCoord.y - expectedCoord.y) == 0)
				return false;
			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
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
				if (currentCoord.x == expectedCoord.x ? curState [point][expectedCoord.x] != 0 : curState [expectedCoord.y][point] != 0)
					return false;

			return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
		}
	}

	private bool canMoveLikeAKing (IntVector2 currentCoord, IntVector2 expectedCoord)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (Math.Abs (currentCoord.x - expectedCoord.x) > 1 || Math.Abs (currentCoord.y - expectedCoord.y) > 1)
			return false;

		if (Math.Abs (currentCoord.x - expectedCoord.x) == 0 && Math.Abs (currentCoord.y - expectedCoord.y) == 0)
			return false;

		return (_GameManager.instance.currentPlayerIndex == 1 ? curState [expectedCoord.y][expectedCoord.x] <= 0 : curState [expectedCoord.y][expectedCoord.x] >= 0);
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
			yield return new WaitForSeconds (0.1f);

		if (_GameManager.instance.Promote ())
		{
			this._tmpToken.tokenType = Token.getPromotedTokenType (this._tmpToken.tokenType);
			this._tmpToken.promotedToken = true;
			this._tmpToken.gameObject.transform.rotation = Quaternion.Euler (90, 0, this._tmpToken.goteIsTheOwner ? 0 : 180);
		}
		Debug.Log ("promote = " + _GameManager.instance.Promote ());
	}

	public void applyMove (Box box, TokenAnimator token)
	{
		int [][] curState = new int[9][];
		for (int i = 0; i < 9; i++)
			curState [i] = new int[9];
		this._currentState.getState (ref curState);

		if (curState [box.coord.y][box.coord.x] != 0)
		{
			List <Token.TokenType> capturedTokens;

			if (curState [box.coord.y][box.coord.x] < 0)
			{
				capturedTokens = this._currentState.getGoteCapturedTokens ();
				capturedTokens.Add (box.token.tokenType);
				this._currentState.setGoteCapturedTokens (capturedTokens);
				this.player1Bench.transform.GetChild (1).GetComponent <CaptureBench> ().AddToken (box.token);
				_GameManager.instance.player1Score -= curState [box.coord.y][box.coord.x];
			}
			else
			{
				capturedTokens = this._currentState.getSenteCapturedTokens ();
				capturedTokens.Add (box.token.tokenType);
				this._currentState.setSenteCapturedTokens (capturedTokens);
				this.player2Bench.transform.GetChild (1).GetComponent <CaptureBench> ().AddToken (box.token);
				_GameManager.instance.player2Score += curState [box.coord.y][box.coord.x];
			}
		}

		if (! token.isCaptured)
		{
			Box oldBox = this.boxes [token.coord.y * 9 + token.coord.x];

			if (! token.promotedToken && ((token.goteIsTheOwner && box.coord.y >= 6) || (!token.goteIsTheOwner && box.coord.y <= 2)) &&
				Token.isAPromotableToken (token.tokenType))		// promotion zone
			{
				if ((token.tokenType == Token.TokenType.PAWN || token.tokenType == Token.TokenType.LANCE) &&
					((token.goteIsTheOwner && box.coord.y == 8) || (!token.goteIsTheOwner && box.coord.y == 0)))
				{
					token.tokenType = Token.getPromotedTokenType (token.tokenType);
					token.promotedToken = true;
					token.gameObject.transform.rotation = Quaternion.Euler (90, 0, token.goteIsTheOwner ? 0 : 180);
				}
				else if (token.tokenType == Token.TokenType.KNIGHT &&
						((token.goteIsTheOwner && box.coord.y >= 7) || (!token.goteIsTheOwner && box.coord.y <= 1)))
				{
					token.tokenType = Token.getPromotedTokenType (token.tokenType);
					token.promotedToken = true;
					token.gameObject.transform.rotation = Quaternion.Euler (90, 0, token.goteIsTheOwner ? 0 : 180);
				}
				else
				{
					_GameManager.instance.AskForPromotion ();

					this._tmpToken = token;
					StartCoroutine ("Promote");
				}
			}

			oldBox.token = null;
			oldBox.busy = false;
			curState [token.coord.y][token.coord.x] = 0;
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

		curState [box.coord.y][box.coord.x] = Token.getTokenValue (token.tokenType, token.goteIsTheOwner);
		this._currentState.setState (ref curState);
	}
}
