using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using ShogiUtils;
using Tools;

public class PlayerHandler : MonoBehaviour
{
	public enum playerState
	{
		Waiting, ShouldPlay, Playing, Played, Cancelling, Unknown
	}

	private string _playerName = "UNKNOWN";
	public string playerName { get { return this._playerName; } }

	private GameColor _playerColor;
	public GameColor playerColor { get { return this._playerColor; } }

	private PlayerType _playerType = PlayerType.UNDEFINED;
	public PlayerType playerType { get { return this._playerType; } }

	private int _playerDifficulty = -1;
	public int playerDifficulty { get { return this._playerDifficulty; } }

	private playerState _playerCurrentState = playerState.Unknown;
	public playerState playerCurrentState	{ get { return this._playerCurrentState; } }

	public GameObject pivot;

	private Node _currentGameState = null;
	private List<Node> _movesPlayed = new List<Node> ();
	private IPlayer _player;

	public void init (string playerName, GameColor playerColor, PlayerType playerType, int playerDifficulty)
	{
		this._playerName = playerName;
		this._playerColor = playerColor;
		this._playerType = playerType;
		this._playerDifficulty = playerDifficulty;

		switch (playerType)
		{
			case PlayerType.SECRET:
				this._player = this.gameObject.AddComponent <MinMax>();
				break;
			case PlayerType.GLADIATOR:
				this._player = this.gameObject.AddComponent <AlphaBeta>();
				break;
			case PlayerType.PREDATOR:
				this._player = this.gameObject.AddComponent <NegaScout>();
				break;
			case PlayerType.TERMINATOR:
				this._player = this.gameObject.AddComponent <ProofNumberSearch>();
				break;
			default:
				break;
		}

		this._playerCurrentState = playerState.Waiting;
	}

	public void reInit ()
	{
		this._currentGameState = null;
		this._movesPlayed.Clear ();
		this._playerCurrentState = playerState.Waiting;
	}

	public void startPlay (Node currentGameState, List<Node> movesPlayed)
	{
		if (this.playerCurrentState != playerState.Waiting)
			return;

		this._currentGameState = currentGameState;
		this._movesPlayed.Clear ();
		this._movesPlayed.AddRange (movesPlayed);
		this._playerCurrentState = playerState.ShouldPlay;
	}

	private void searchMove ()
	{
		if (this.playerCurrentState != playerState.ShouldPlay)
			return;

		this._player.searchMove (this._currentGameState, this._playerDifficulty, this.playerColor == GameColor.GOTE ? true : false, this._movesPlayed);
		this._playerCurrentState = playerState.Playing;
		StartCoroutine ("checkMove");
	}

	private IEnumerator checkMove ()
	{
		while (! this._player.moveReady())
		{
			if (this._playerCurrentState != playerState.Playing)
				yield break;
			yield return new WaitForSeconds (0.1f);
		}

		this._currentGameState = null;
		this._movesPlayed.Clear ();
		this._playerCurrentState = playerState.Played;
	}

	public void cancelMove ()
	{
		this._playerCurrentState = playerState.Cancelling;

		//TODO

		this._playerCurrentState = playerState.Waiting;
	}

	public Node getMove ()
	{
		if (this.playerCurrentState == playerState.Played)
		{
			this._playerCurrentState = playerState.Waiting;
			return new Node (this._player.getBestMove ());
		}

		return null;
	}

	void Update ()
	{
		if (this._playerCurrentState == playerState.ShouldPlay)
			this.searchMove ();
	}
}
