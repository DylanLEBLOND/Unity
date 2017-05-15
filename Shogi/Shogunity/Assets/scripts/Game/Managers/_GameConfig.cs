using UnityEngine;
using System.Collections;
using ShogiUtils;
using ShogiData;

/// <summary>
/// The Game Configuration
/// </summary>
public class _GameConfig : MonoBehaviour
{
	/// <summary>
	/// Reference to the instance of the GameConfig object
	/// </summary>
	public static _GameConfig instance = null;

	/// <summary>
	///	The game identifier
	/// </summary>
	public int gameId;

	/// <summary>
	/// The game mode
	/// </summary>
	public GameMode gameMode = GameMode.UNDEFINED;

	/// <summary>
	/// The name of the first player.
	/// </summary>
	public string player1Name = "UNKNOWN";

	/// <summary>
	/// The type of the first player.
	/// </summary>
	public PlayerType player1Type = PlayerType.UNDEFINED;

	/// <summary>
	///  The difficulty of the first player; Available only on AIvsAI
	/// </summary>
	public int player1Difficulty = -1;

	/// <summary>
	/// The name of the second player.
	/// </summary>
	public string player2Name = "UNKNOWN";

	/// <summary>
	/// The type of the second player.
	/// </summary>
	public PlayerType player2Type = PlayerType.UNDEFINED;

	/// <summary>
	///  The difficulty of the second player; Available only on PLAYERVsAI and AIvsAI
	/// </summary>
	public int player2Difficulty = -1;

	/// <summary>
	/// Specifies whether or not it's a loaded game
	/// </summary>
	public bool loadedGame = false;

	/// <summary>
	/// Pieces texture Identifier
	/// </summary>
	public int TextureID = 1;

	void Awake ()
	{
		if (instance == null)
		{
			instance = this;
			DontDestroyOnLoad (gameObject);
		}
		else
			Destroy (this);
	}

	void Start ()
	{
		ScoreSave.createDirectoryIfNotExists();
	}
}
