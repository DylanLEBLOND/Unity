using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using ShogiUtils;

/// <summary>
/// Main Menu Manager
/// </summary>
public class MainMenuButtons : MonoBehaviour
{
	/// <summary>
	/// Player 1 Name Input
	/// </summary>
	public InputField player1NameInput;

	/// <summary>
	/// Player 2 Name Input
	/// </summary>
	public InputField player2NameInput;

	/// <summary>
	/// Set the Player 1 name
	/// </summary>
	public void SetPlayer1Name ()
	{
		_GameConfig.instance.player1Name = (string.IsNullOrEmpty(player1NameInput.text)) ? "Player 1" : player1NameInput.text;
	}

	/// <summary>
	/// Set the Player 2 name
	/// </summary>
	public void SetPlayer2Name()
	{
		_GameConfig.instance.player2Name = (string.IsNullOrEmpty(player2NameInput.text)) ? "Player 2" : player2NameInput.text;
	}

	/// <summary>
	/// Start the game with two Player players
	/// </summary>
	public void StartPlayerVsPlayerMode ()
	{
		_GameConfig.instance.gameMode = GameMode.PLAYER_VS_PLAYER;

		this.SetPlayer1Name();
		_GameConfig.instance.player1Type = PlayerType.PLAYER;
		_GameConfig.instance.player1Difficulty = -1;

		this.SetPlayer2Name();
		_GameConfig.instance.player2Type = PlayerType.PLAYER;
		_GameConfig.instance.player2Difficulty = -1;

		this.StartGame();
	}

	/// <summary>
	///  Set a Game Mode that required a AI
	/// </summary>
	/// <param name="stringGameMode">the game mode</param>
	public void setAIGameMode (string stringGameMode)
	{
		GameMode gameMode;

		switch (stringGameMode)
		{
			case "PlayerVs.AI":
				gameMode = GameMode.PLAYER_VS_AI;
				break;
			case "AIVs.AI":
				gameMode = GameMode.AI_VS_AI;
				break;
			default:    // Should Never Occur
				_GameConfig.instance.gameMode = GameMode.UNDEFINED;
				return;
		}
		_GameConfig.instance.gameMode = gameMode;
	}

	/// <summary>
	/// Set the first AI parameters
	/// </summary>
	/// <param name="AI1Name">Name of the first AI</param>
	public void SetAI1 (string AI1Name)
	{
		PlayerType player1Type;

		switch (AI1Name)
		{
			case "Gladiator":
				player1Type = PlayerType.GLADIATOR;
				break;
			case "Predator":
				player1Type = PlayerType.PREDATOR;
				break;
			case "Terminator":
				player1Type = PlayerType.TERMINATOR;
				break;
			default:    // Should Never Occur
				_GameConfig.instance.player1Name = "UNKNOWN";
				_GameConfig.instance.player1Type = PlayerType.UNDEFINED;
				return;
		}

		_GameConfig.instance.player1Name = AI1Name;
		_GameConfig.instance.player1Type = player1Type;
	}

	public void SetSecretAI1 ()
	{
		string stringDepthAI;
		int depthAI;

		_GameConfig.instance.player1Name = "Secret";
		_GameConfig.instance.player1Type = PlayerType.SECRET;

		stringDepthAI = GameObject.Find("InputDepth1").GetComponent<InputField>().text;
		if (! int.TryParse (stringDepthAI, out depthAI))
			depthAI = 3;

		_GameConfig.instance.player1Difficulty = depthAI > 0 ? depthAI : 3;
	}

	/// <summary>
	/// Set the first AI difficulty
	/// </summary>
	/// <param name="difficulty">difficulty of the first AI</param>
	public void SetAI1Difficulty(int difficulty)
	{
		if (difficulty < 2 || 4 < difficulty)
			difficulty = 2;

		_GameConfig.instance.player1Difficulty = difficulty;
	}

	/// <summary>
	/// Personalize the first AI difficulty rate
	/// </summary>
	public void SetAI1PersoDifficulty()
	{
		string stringDepthAI;
		int depthAI;

		stringDepthAI = GameObject.Find("InputDepth1").GetComponent<InputField>().text;
		if (! int.TryParse (stringDepthAI, out depthAI))
			depthAI = 3;

		_GameConfig.instance.player1Difficulty = depthAI > 0 ? depthAI : 3;
		Debug.Log("DIFFICULTE " + _GameConfig.instance.player1Difficulty);
	}

	/// <summary>
	/// Set the second AI parameters
	/// </summary>
	/// <param name="AI2Name">Name of the first AI</param>
	public void SetAI2 (string AI2Name)
	{
		PlayerType player2Type;

		switch (AI2Name)
		{
			case "Gladiator":
				player2Type = PlayerType.GLADIATOR;
				break;
			case "Predator":
				player2Type = PlayerType.PREDATOR;
				break;
			case "Terminator":
				player2Type = PlayerType.TERMINATOR;
				break;
			default:    // Should Never Occur
				_GameConfig.instance.player2Name = "UNKNOWN";
				_GameConfig.instance.player2Type = PlayerType.UNDEFINED;
				return;
		}

		_GameConfig.instance.player2Name = AI2Name;
		_GameConfig.instance.player2Type = player2Type;
	}

	/// <summary>
	/// Set the second AI difficulty
	/// </summary>
	/// <param name="difficulty">difficulty of the second AI</param>
	public void SetAI2Difficulty (int difficulty)
	{
		if (difficulty < 2 || 4 < difficulty)
			difficulty = 2;

		_GameConfig.instance.player2Difficulty = difficulty;

		this.StartGame();
	}

	public void SetSecretAI2 ()
	{
		string stringDepthAI2;
		int depthAI2;

		_GameConfig.instance.player2Name = "Secret";
		_GameConfig.instance.player2Type = PlayerType.SECRET;

		stringDepthAI2 = GameObject.Find("InputDepth2").GetComponent<InputField>().text;
		if (! int.TryParse (stringDepthAI2, out depthAI2))
			depthAI2 = 3;

		_GameConfig.instance.player1Difficulty = depthAI2 > 0 ? depthAI2 : 3;

		this.StartGame();
	}

	/// <summary>
	/// Personalize the second AI difficulty rate
	/// </summary>
	public void SetAI2PersoDifficulty()
	{
		string stringDepthAI2;
		int depthAI2;

		stringDepthAI2 = GameObject.Find("InputDepth2").GetComponent<InputField>().text;
		if (! int.TryParse (stringDepthAI2, out depthAI2))
			depthAI2 = 3;

		_GameConfig.instance.player2Difficulty = depthAI2 > 0 ? depthAI2 : 3;
		Debug.Log("DIFFICULTE " + _GameConfig.instance.player2Difficulty);

		this.StartGame();
	}

	/// <summary>
	/// Start the game with the current parameters
	/// </summary>
	public void StartGame()
	{
		if ((_GameConfig.instance.gameMode == GameMode.UNDEFINED) ||
			(_GameConfig.instance.player1Type == PlayerType.UNDEFINED) ||
			(_GameConfig.instance.player2Type == PlayerType.UNDEFINED))
			return;

		_GameConfig.instance.loadedGame = false;
		_GameConfig.instance.GetComponent<Fading>().ChangeLevel("Game");
	}

	public void SetJapaneseTexture ()
	{
		_GameConfig.instance.TextureID = 0;
	}
		
	public void SetAltJapTexture ()
	{
		_GameConfig.instance.TextureID = 1;
	}

	public void SetEnglishTexture ()
	{
		_GameConfig.instance.TextureID = 2;
	}

	/// <summary>
	/// Exit the game
	/// </summary>
	public void ExitGame()
	{
		Application.Quit();
	}
}
