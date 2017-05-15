using System;
using System.Collections;
using System.Collections.Generic;
using Tools;

public class AlphaBeta : IPlayer
{
	public AlphaBeta () { }

	protected override void RunAlgorithm (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		/* find a way to enable / disable the aspiration heuristic */
//		this._bestMove = runAlphaBeta (root, depth, int.MinValue, int.MaxValue, isGote, movesPlayed);
		this._bestMove = AspirationSearch (root, depth, isGote, movesPlayed);
	}

	private Node AspirationSearch (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		int totalNodeCount = 0;
		int alpha = int.MinValue;
		int beta = int.MaxValue;
		Node bestNode = null;
		int bestValue;
		int depthSearch;
		int tic;
		int tac;

		tic = Environment.TickCount;
		for (depthSearch = 1; depthSearch <= depth; depthSearch++) // iterative deepening
		{
			bestNode = this.runAlphaBeta (root, depthSearch, alpha, beta, isGote, movesPlayed);
			bestValue = bestNode.getScore ();
			totalNodeCount += nodeCount;

			if (alpha < bestValue && bestValue < beta)                  //Ajustment of the aspiration window
			{                                                           //if best node's value is within the window
				alpha = bestValue - 100;                                   //re ajustement of the bounds values
				beta = bestValue + 100;                                    //for the next search of alphaBeta
			}
			else	//if outside
			{
				alpha = int.MinValue;		//reset alpha and beta values
				beta = int.MaxValue;
				depthSearch--;		// re-search at the same deep
			}
		}
		tac = Environment.TickCount;

		this.duration = tac - tic;
		this.nodeScore = bestNode.getScore ();
		this.nodeThreat = bestNode.getChildrenThreat ();

		return bestNode;
	}

	private Node runAlphaBeta (Node root, int depth, int alpha, int beta, bool isGote, List<Node> movesPlayed)
	{
		List <Node> children;
		int maxScore = -999999999;
		int selectedNode;
		bool foundNode;
		List<int> neverPlayedNode;
		int i;
		int tic;
		int tac;

		this.nodeCount = 0;

		root.createChildren (isGote, true /* canDrop */, true /* sort */);
		if (root.getChildren ().Count == 0)
			return null;

		children = new List<Node> (root.getChildren ());
		root.clearChildren ();

		selectedNode = 0;
		foundNode = false;
		neverPlayedNode = new List<int>();

		tic = Environment.TickCount;
		for (i = 0; i < children.Count; i++)
		{
			this.nodeCount++;

			if (! Node.ListContainsNode (movesPlayed, children [i]) && children [i].getChildrenThreat() != 999999999)
				neverPlayedNode.Add (i);

			if ((! children [i].stillAlive (!isGote) || children [i].getChildrenThreat() == -999999999) && ! Node.ListContainsNode (movesPlayed, children[i]))		// terminal node
			{
				selectedNode = i;
				foundNode = true;
				break;
			}

			children[i].setScore (AlphaBetaMin (children[i], depth - 1, alpha, beta, isGote));

			if (children [i].getScore () > maxScore && children [i].getChildrenThreat() != 999999999 && ! Node.ListContainsNode (movesPlayed, children[i]))
			{
				selectedNode = i;
				maxScore = children [selectedNode].getScore ();
				foundNode = true;
			}

			if (maxScore >= beta)
			{
				foundNode = true;
				break;
			}

			if (maxScore > alpha)
				alpha = maxScore;
		}
		tac = Environment.TickCount;

		if (!foundNode && neverPlayedNode.Count > 0)
			selectedNode = neverPlayedNode[new System.Random().Next (neverPlayedNode.Count)];

		this.duration = tac - tic;
		this.nodeScore = children [selectedNode].getScore ();
		this.nodeThreat = children [selectedNode].getChildrenThreat ();

		return children [selectedNode];
	}

	private int AlphaBetaMin (Node node, int depth, int alpha, int beta, bool isGote)
	{
		List <Node> children;
		int minScore = int.MaxValue;
		int score = int.MaxValue;

		node.createChildren (!isGote, true /* canDrop */, false /* sort */);
		if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
			return node.evaluate (isGote);

		children = new List<Node> (node.getChildren());
		node.clearChildren ();

		this.nodeCount++;

		for (int i = 0; i < children.Count; i++)
		{
			score = AlphaBetaMax (children[i], depth - 1, alpha, beta, isGote);

			if (score < minScore)
				minScore = score;

			if (minScore <= alpha)
				return minScore;		// alpha cut off

			if (minScore < beta)
				beta = minScore;
		}
		return minScore;
	}

	private int AlphaBetaMax (Node node, int depth, int alpha, int beta, bool isGote)
	{
		List <Node> children;
		int maxScore = int.MinValue;
		int score = int.MinValue;

		node.createChildren (isGote, true /* canDrop */, false /* sort */);
		if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
			return node.evaluate (isGote);

		children = new List<Node> (node.getChildren());
		node.clearChildren ();

		this.nodeCount++;

		for (int i = 0; i < children.Count; i++)
		{
			score = AlphaBetaMin (children[i], depth - 1, alpha, beta, isGote);

			if (score > maxScore)
				maxScore = score;

			if (maxScore >= beta)
				return maxScore;		// beta cut off

			if (maxScore > alpha)
				alpha = maxScore;
		}
		return maxScore;
	}
}
