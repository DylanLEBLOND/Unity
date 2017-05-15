using System.Collections;
using System.Collections.Generic;
using System.Threading;
using ShogiUtils;
using Tools;

public class IPlayer
{
	public int nodeCount;
	public int duration;
	public int nodeScore;
	public int nodeThreat;
	protected Node _bestMove = null;


	public string childInfo = "";

	private object _handle = new object ();
	private System.Threading.Thread _thread = null;
	private bool _moveReady = false;
	public bool moveReady
	{
		get
		{
			bool tmp;
			lock (_handle) { tmp = _moveReady; }
			return tmp;
		}
		set { lock (_handle) { _moveReady = value; } }
	}

	public virtual void Start (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		this._thread = new System.Threading.Thread (() => Run (root, depth, isGote, movesPlayed));
		this._thread.Start ();
	}

	public virtual void Abort()
	{
		this._thread.Abort();
	}

	protected virtual void RunAlgorithm (Node root, int depth, bool isGote, List<Node> movesPlayed) { }

	protected virtual void OnFinished () { }

	public virtual bool Update()
	{
		if (this.moveReady)
		{
			OnFinished();
			return true;
		}
		return false;
	}

	private void Run (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		RunAlgorithm (root, depth, isGote, movesPlayed);
		this.moveReady = true;
	}

	public Node getBestMove ()
	{
		return this._bestMove;
	}
}
