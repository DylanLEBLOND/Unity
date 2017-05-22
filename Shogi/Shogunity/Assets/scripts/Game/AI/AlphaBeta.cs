using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class AlphaBeta : MonoBehaviour, IPlayer
{
	private string alphaBetaPath;
	private string rootPath;
	private string movesPlayedPath;
	private string resultPath;
	private Process alphaProcess;
	private bool _moveReady = false;
	private Node _bestMove = null;

	void Awake ()
	{
		this.alphaBetaPath = Application.streamingAssetsPath + "/AlphaBetaLauncher.exe";
		this.rootPath = Application.streamingAssetsPath + "/Root" + new System.Random (Environment.TickCount).Next() + ".xml";
		this.movesPlayedPath = Application.streamingAssetsPath + "/MovesPlayed" + new System.Random (Environment.TickCount).Next() + ".xml";
		this.resultPath = Application.streamingAssetsPath + "/Result" + new System.Random (Environment.TickCount).Next() + ".xml";

		UnityEngine.Debug.Log ("AlphaBeta Path = " + this.alphaBetaPath);
	}

	public void searchMove (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		SerializeNode.Serialize (root, rootPath);
		SerializeNode.SerializeList (movesPlayed, movesPlayedPath);

		ProcessStartInfo processInfo = new ProcessStartInfo (this.alphaBetaPath);
		processInfo.Arguments = this.rootPath + " " + depth.ToString () + " " + isGote.ToString () + " " + this.movesPlayedPath + " " + this.resultPath;
		processInfo.CreateNoWindow = true;
		processInfo.UseShellExecute = false;

		this._bestMove = null;
		this._moveReady = false;
		this.alphaProcess = Process.Start (processInfo);

		StartCoroutine ("WaitingForResponse");
	}

	private IEnumerator WaitingForResponse ()
	{
		while (! this.alphaProcess.HasExited)
			yield return new WaitForSeconds (0.1f);

		this._bestMove = SerializeNode.Deserialize (this.resultPath);
		this._moveReady = true;
	}

	public bool moveReady ()
	{
		return this._moveReady;
	}

	public Node getBestMove ()
	{
		this._moveReady = false;
		return this._bestMove;
	}
}
