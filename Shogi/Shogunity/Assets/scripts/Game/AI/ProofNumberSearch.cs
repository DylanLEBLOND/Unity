using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tools;

public class ProofNumberSearch : MonoBehaviour, IPlayer
{
	private string proofNumberSearchPath;
	private string rootPath;
	private string movesPlayedPath;
	private string resultPath;
	private Process pnsProcess;
	private bool _moveReady = false;
	private Node _bestMove = null;

	void Awake ()
	{
		this.proofNumberSearchPath = Application.streamingAssetsPath + "/ProofNumberSearchLauncher.exe";
		this.rootPath = Application.streamingAssetsPath + "/Root" + new System.Random (Environment.TickCount).Next() + ".xml";
		this.movesPlayedPath = Application.streamingAssetsPath + "/MovesPlayed" + new System.Random (Environment.TickCount).Next() + ".xml";
		this.resultPath = Application.streamingAssetsPath + "/Result" + new System.Random (Environment.TickCount).Next() + ".xml";
	}

	public void searchMove (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		SerializeNode.Serialize (root, rootPath);
		SerializeNode.SerializeList (movesPlayed, movesPlayedPath);

		ProcessStartInfo processInfo = new ProcessStartInfo (this.proofNumberSearchPath);
		processInfo.Arguments = this.rootPath + " " + depth.ToString () + " " + isGote.ToString () + " " + this.movesPlayedPath + " " + this.resultPath;
		processInfo.CreateNoWindow = true;
		processInfo.UseShellExecute = false;

		this._bestMove = null;
		this._moveReady = false;
		this.pnsProcess = Process.Start (processInfo);

		StartCoroutine ("WaitingForResponse");
	}

	private IEnumerator WaitingForResponse ()
	{
		while (! this.pnsProcess.HasExited)
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
