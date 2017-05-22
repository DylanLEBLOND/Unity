using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ShogiUtils;
using Tools;

public class _GameManager : MonoBehaviour
{
	public PlayerHandler player1;
	public int player1Score = 0;
	public int player1MovesCount = 0;
	public PlayerHandler player2;
	public int player2Score = 0;
	public int player2MovesCount = 0;
	public Node currentState;
	[Range(1, 2)]
	public int currentPlayerIndex = 1;
	public TokenAnimator selectedToken = null;
	public static _GameManager instance = null;

	public Texture[] textures;

	public GameObject loadHUD;
	public GameObject timeHUD;
	public GameObject scoreHUD;
	public GameObject currentPlayerHUD;
	public GameObject selectedTokenHUD;
	public GameObject promotionMenu;
	public GameObject pauseMenu;
	public GameObject saveMenu;
	public GameObject endOfGameMenu;
	public bool WaitingPromotion = false;

	private bool _gameInitialized = false;
	public bool gameInitialized { get { return _gameInitialized; } }

	private int _turn = 1;
	private float _timer = 0;
	private Text _goteScoreHUD;
	private Light _goteLightHUD;
	private Text _senteScoreHUD;
	private Light _senteLightHUD;
	private float _tmpTimeScale = 1;
	private bool _gameInProgress = false;
	private bool _undoRequired;
	private bool _promote = false;
	private List<Node> _movesPlayed = new List<Node> ();
	private List<Node> _undoMoves = new List<Node> ();

	void Awake ()
	{
		if (instance == null)
			instance = this;
		else
			Destroy (this);
	}
#if false
	public static int [,] defaultState = { {      0       ,       0       ,       0       ,      0      ,      0      , Token.KING  ,  Token.GPAWN  ,        0      , Token.GPAWN  },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       , Token.SBISHOP ,      0       },
										   {      0       ,       0       ,       0       ,      0      , Token.GROOK ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      , Token.JEWEL ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       } };
#endif
//#if false
	public static int [,] defaultState = { { Token.GLANCE , Token.GKNIGHT , Token.GSILVER , Token.GGOLD , Token.KING  , Token.GGOLD , Token.GSILVER , Token.GKNIGHT , Token.GLANCE },
										   {      0       , Token.GBISHOP ,       0       ,      0      ,      0      ,      0      ,       0       ,  Token.GROOK  ,      0       },
										   { Token.GPAWN  ,  Token.GPAWN  ,  Token.GPAWN  , Token.GPAWN , Token.GPAWN , Token.GPAWN ,  Token.GPAWN  ,  Token.GPAWN  , Token.GPAWN  },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										   {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										   { Token.SPAWN  ,  Token.SPAWN  ,  Token.SPAWN  , Token.SPAWN , Token.SPAWN , Token.SPAWN ,  Token.SPAWN  ,  Token.SPAWN  , Token.SPAWN  },
										   {      0       ,  Token.SROOK  ,       0       ,      0      ,      0      ,      0      ,       0       , Token.SBISHOP ,      0       },
										   { Token.SLANCE , Token.SKNIGHT , Token.SSILVER , Token.SGOLD , Token.JEWEL , Token.SGOLD , Token.SSILVER , Token.SKNIGHT , Token.SLANCE } };
//#endif

	void Start ()
	{
		if (this.player1 == null)
			this.player1 = this.transform.GetChild(0).gameObject.GetComponent<PlayerHandler>();
		if (this.player2 == null)
			this.player2 = this.transform.GetChild(1).gameObject.GetComponent<PlayerHandler>();

		this.player1.init (_GameConfig.instance.player1Name, GameColor.GOTE, _GameConfig.instance.player1Type, _GameConfig.instance.player1Difficulty);
		this.player2.init (_GameConfig.instance.player2Name, GameColor.SENTE, _GameConfig.instance.player2Type, _GameConfig.instance.player2Difficulty);

		this.scoreHUD.transform.GetChild (0).transform.GetChild (0).GetComponent <Text> ().text = _GameConfig.instance.player1Name;
		this._goteScoreHUD = this.scoreHUD.transform.GetChild (0).transform.GetChild (1).GetComponent <Text> ();
		this._goteScoreHUD.text = this.player1Score.ToString ();
		this._goteLightHUD = this.scoreHUD.transform.GetChild (0).transform.GetChild (2).transform.GetChild (0).GetComponent <Light> ();
		this._goteLightHUD.color = this.currentPlayerIndex == 1 ? Color.white : Color.black;

		this.scoreHUD.transform.GetChild (1).transform.GetChild (0).GetComponent <Text> ().text = _GameConfig.instance.player2Name;
		this._senteScoreHUD = this.scoreHUD.transform.GetChild (1).transform.GetChild (1).GetComponent <Text> ();
		this._senteScoreHUD.text = this.player2Score.ToString ();
		this._senteLightHUD = this.scoreHUD.transform.GetChild (1).transform.GetChild (2).transform.GetChild (0).GetComponent <Light> ();
		this._senteLightHUD.color = this.currentPlayerIndex == 1 ? Color.black : Color.white;

		this.currentPlayerHUD.GetComponent <Text>().text = (this.currentPlayerIndex == 1 ? this.player1.playerName : this.player2.playerName).ToString() + " Turn";

		this.currentState = new Node ();

		int [][] jaggedTestState = new int [9][];
		for (int i = 0; i < 9; i++)
			jaggedTestState [i] = new int[9];

		for (int i = 0; i < 9; i++)
			for (int j = 0; j < 9; j++)
				jaggedTestState [i][j] = defaultState[i,j];

		this.currentState.setState (ref jaggedTestState);

		_GameBoard.instance.textureToUse = this.textures [_GameConfig.instance.TextureID];
		_GameBoard.instance.setCurrentState (this.currentState);
		_GameBoard.instance.initState ();

		this._movesPlayed.Add (new Node (this.currentState));

		this._timer = 0;
		this._gameInitialized = true;
		StartCoroutine ("WaitingAllReady");
	}

	private IEnumerator WaitingAllReady ()
	{
		while (!_GameBoard.instance.gameBoardIsInitialized ||
				this.player1.playerCurrentState != PlayerHandler.playerState.Waiting ||
		 		this.player1.playerCurrentState != PlayerHandler.playerState.Waiting)
			yield return new WaitForSeconds (1f);

		this.loadHUD.SetActive (false);
		this._gameInProgress = true;
	}

	private IEnumerator UndoCo ()
	{
		yield break;		// Remove this if you want to activate the undo effect

		Node oldMove;
		Node oldNode;
		int [][] oldState = new int[9][];
		for (int i = 0; i < 9; i++)
			oldState [i] = new int[9]; 

		Token.TokenInfo oldInfo;
		Token.TokenInfo newInfo = new Token.TokenInfo ();

		this._undoRequired = true;

		while ((this.player1.playerCurrentState != PlayerHandler.playerState.Waiting) &&
				(this.player2.playerCurrentState != PlayerHandler.playerState.Waiting))
			yield return null;

		if (this._movesPlayed.Count == 0)
		{
			this._undoRequired = false;
			yield break;
		}

		oldNode = new Node (this._movesPlayed [this._movesPlayed.Count - 1]);
		this._movesPlayed [this._movesPlayed.Count - 2].getState (ref oldState);
		oldNode.setState (ref oldState);
		this.currentState = new Node (oldNode);

		this._undoMoves.Add (new Node (this._movesPlayed [this._movesPlayed.Count - 1]));

		oldMove = this._movesPlayed [this._movesPlayed.Count - 1];
		oldInfo = oldMove.getTokenInfo ();

		newInfo.Clear ();
		newInfo.tokenType = oldInfo.tokenType;
		newInfo.oldX = oldInfo.newX;
		newInfo.oldY = oldInfo.newY;
		newInfo.newX = oldInfo.oldX;
		newInfo.newY = oldInfo.oldY;
		newInfo.newValue = oldInfo.oldValue;
		newInfo.capturedSomething = oldInfo.capturedSomething;
		newInfo.capturedType = oldInfo.capturedType;
		newInfo.capturedValue = oldInfo.capturedValue;

		if (oldInfo.capturedSomething)
		{
			if (this.currentPlayerIndex == 1)		// sente was playing during the last turn
				this.player2Score -= oldInfo.capturedValue;
			else
				this.player1Score -= oldInfo.capturedValue;
		}

		oldMove.setTokenInfo (newInfo);

		_GameBoard.instance.moveToken (oldMove, true /* isUndo */);

		this._movesPlayed.RemoveAt (this._movesPlayed.Count - 1);

		this.currentPlayerIndex = this.currentPlayerIndex == 1 ? 2 : 1;
		this._turn--;
		this._undoRequired = false;
	}

	public void Undo ()
	{
		StartCoroutine ("UndoCo");
	}

	public void Redo () { }

	public void AskForPromotion ()
	{
		this.WaitingPromotion = true;
		if (Time.timeScale != 0)
			this._tmpTimeScale = Time.timeScale;
		Time.timeScale = 0;
		this._gameInProgress = false;
		this.promotionMenu.SetActive (true);
	}

	public void Promotion (bool ok)
	{
		this._promote = ok;
		Debug.Log ("this._promote = " + this._promote + " | ok = " + ok);

		this.promotionMenu.SetActive (false);
		this._gameInProgress = true;
		Time.timeScale = this._tmpTimeScale;
		this.WaitingPromotion = false;
	}

	public bool Promote ()
	{
		return this._promote;
	}

	public void Pause (bool paused)
	{
		if (paused == true)
		{
			if (Time.timeScale != 0)
				this._tmpTimeScale = Time.timeScale;
			Time.timeScale = 0;
			this._gameInProgress = false;
			this.pauseMenu.SetActive (true);
		}
		else
		{
			this.pauseMenu.SetActive (false);
			this._gameInProgress = true;
			Time.timeScale = this._tmpTimeScale;
		}
	}

	public void Restart ()
	{
		this.player1Score = 0;
		this.player1MovesCount = 0;
		this.player2Score = 0;
		this.player2MovesCount = 0;
		this.currentPlayerIndex = 1;

		this.player1.reInit ();
		this.player2.reInit ();

		this._movesPlayed.Clear ();
		this._undoMoves.Clear ();

		this.currentState = new Node ();
		int [][] jaggedTestState = new int [9][];
		for (int i = 0; i < 9; i++)
			jaggedTestState [i] = new int[9];

		for (int i = 0; i < 9; i++)
			for (int j = 0; j < 9; j++)
				jaggedTestState [i][j] = defaultState[i,j];
		this.currentState.setState (ref jaggedTestState);

		_GameBoard.instance.setCurrentState (this.currentState);
		_GameBoard.instance.reInit ();

		this._timer = 0;
		this._turn = 0;
		this._gameInitialized = true;

		this.loadHUD.SetActive (true);
		this.pauseMenu.SetActive (false);
		this.endOfGameMenu.SetActive (false);
		Time.timeScale = this._tmpTimeScale;

		StartCoroutine ("WaitingAllReady");
	}

	public void SaveGame ()
	{
		this.saveMenu.SetActive (true);
		this.pauseMenu.SetActive (false);
	}

	public void Quit (bool saveGame)
	{
		if (saveGame)
		{
		}

		GameObject.DestroyImmediate (_GameConfig.instance);
		Time.timeScale = this._tmpTimeScale;
		Application.LoadLevel ("MainMenu");
	}

	public void EndOfGame ()
	{
		bool player1won = this.currentState.stillAlive (true);
		Time.timeScale = 0;

		this.endOfGameMenu.SetActive (true);

		this.endOfGameMenu.transform.GetChild (1).GetComponent <Text> ().text = (player1won ? this.player1.playerName : this.player2.playerName).ToString() + " Won !";

		this.endOfGameMenu.transform.GetChild (2).GetComponent <Text> ().text += (player1won ? this.player1Score : this.player2Score).ToString();

		this._gameInProgress = false;
	}

	private void updateScore (Node currentState)
	{
		Token.TokenInfo info;

		info = currentState.getTokenInfo ();

		if (info.capturedSomething)
		{
			if (info.capturedValue < 0)
				this.player1Score -= info.capturedValue;
			else
				this.player2Score += info.capturedValue;
		}
	}

	private void updateHUD ()
	{
		this._timer += Time.deltaTime;

		if (this._timer > 3600)
			this.timeHUD.transform.GetChild(1).GetComponent <Text> ().text = (Mathf.Floor(this._timer / 3600).ToString("00") + ":" + Mathf.Floor((this._timer % 3600) / 60).ToString("00") + ":" + Mathf.Floor((this._timer % 3600) % 60).ToString("00"));
		else if (this._timer > 60)
			this.timeHUD.transform.GetChild(1).GetComponent <Text> ().text = ("00:" + Mathf.Floor(this._timer / 60).ToString("00") + ":" + Mathf.Floor(this._timer % 60).ToString("00"));
		else
			this.timeHUD.transform.GetChild(1).GetComponent <Text> ().text = ("00:00:" + Mathf.Floor(this._timer % 60).ToString("00"));

		this._goteScoreHUD.text = this.player1Score.ToString ();
		this._senteScoreHUD.text = this.player2Score.ToString ();

		if (this.selectedToken != null)
			this.selectedTokenHUD.GetComponent <Text> ().text = this.selectedToken.tokenType.ToString () + " selected";
		else
			this.selectedTokenHUD.GetComponent <Text> ().text = "No token selected";
	}

	private void playMove (bool player1)
	{
		if (this._undoRequired)
		{
			this.player1.cancelMove ();
			return;
		}

		this.currentState = new Node (player1 ? this.player1.getMove () : this.player2.getMove ());

		this._movesPlayed.Add (new Node (this.currentState));
		_GameBoard.instance.moveToken (this.currentState, false /* isUndo */);

		this.updateScore (this.currentState);

		if (this._undoMoves.Count != 0)
			this._undoMoves.Clear ();

		if (!this.currentState.stillAlive (true /* gote */) || !this.currentState.stillAlive (false /* sente */))
			return;

		this.currentPlayerIndex = this.currentPlayerIndex == 1 ? 2 : 1;

		this._goteLightHUD.color = this.currentPlayerIndex == 1 ? Color.white : Color.black;
		this._senteLightHUD.color = this.currentPlayerIndex == 1 ? Color.black : Color.white;
		this.currentPlayerHUD.GetComponent <Text>().text = (this.currentPlayerIndex == 1 ? this.player1.playerName : this.player2.playerName).ToString() + " Turn";

		this._turn++;
	}

	public void nextTurn ()
	{
		this.updateScore (this.currentState);

		if (this._undoMoves.Count != 0)
			this._undoMoves.Clear ();

		if (!this.currentState.stillAlive (true /* gote */) || !this.currentState.stillAlive (false /* sente */))
			return;

		this.selectedToken = null;

		this.currentPlayerIndex = this.currentPlayerIndex == 1 ? 2 : 1;

		this._goteLightHUD.color = this.currentPlayerIndex == 1 ? Color.white : Color.black;
		this._senteLightHUD.color = this.currentPlayerIndex == 1 ? Color.black : Color.white;
		this.currentPlayerHUD.GetComponent <Text>().text = (this.currentPlayerIndex == 1 ? this.player1.playerName : this.player2.playerName).ToString() + " Turn";

		this._turn++;
	}

	void Update ()
	{
		this.updateHUD ();

		if (!this._gameInProgress || this._undoRequired)
			return;

		if (!this.currentState.stillAlive (true /* gote */) || !this.currentState.stillAlive (false /* sente */))
		{
			this.EndOfGame ();
			return;
		}

		if ((this.currentPlayerIndex == 1 && this.player1.playerType == PlayerType.PLAYER) ||
			(this.currentPlayerIndex == 2 && this.player2.playerType == PlayerType.PLAYER))
			return;		// nothing to do

		if (this.currentPlayerIndex == 1)
		{
			switch (this.player1.playerCurrentState)
			{
				case PlayerHandler.playerState.Waiting:
					Debug.Log ("============ Turn " + this._turn + " Gote Playing ============");
					this.player1.startPlay (this.currentState, this._movesPlayed);
					break;
				case PlayerHandler.playerState.Playing:
					if (this._undoRequired)
						this.player1.cancelMove ();
					return;
				case PlayerHandler.playerState.Played:
					this.playMove (true);
					break;
				default:	// should never occur
					Debug.Log ("CRITICAL ERROR: INVALID PLAYER 1 STATE");
					return;
			}
		}
		else
		{
			switch (this.player2.playerCurrentState)
			{
				case PlayerHandler.playerState.Waiting:
					Debug.Log ("============ Turn " + this._turn + " Sente Playing ============");
					this.player2.startPlay (this.currentState, this._movesPlayed);
					break;
				case PlayerHandler.playerState.Playing:
					if (this._undoRequired)
						this.player2.cancelMove ();
					return;
				case PlayerHandler.playerState.Played:
					this.playMove (false);
					break;
				default:	// should never occur
					Debug.Log ("CRITICAL ERROR: INVALID PLAYER 2 STATE");
					return;
			}
		}
	}
}
