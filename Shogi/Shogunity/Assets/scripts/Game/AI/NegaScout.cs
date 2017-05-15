using System;
using System.Collections;
using System.Collections.Generic;
using Tools;

public class NegaScout : IPlayer
{
	public NegaScout () { }

	protected override void RunAlgorithm (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		this._bestMove = runNegaScout (root, depth, isGote, movesPlayed);
	}

	private Node runNegaScout (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		List <Node> children;
		int alpha = int.MinValue;
		int beta = int.MaxValue;
		int maxScore = int.MinValue;
		int selectedNode;
		bool foundNode;
		List<int> neverPlayedNode;
		int tic;
		int tac;

		this.nodeCount = 0;

		root.createChildren (isGote, true /* canDrop */, true /* sort */);
		if (root.getChildren ().Count == 0)
			return null;

		children = new List<Node> (root.getChildren ());
		root.clearChildren ();

		for (int i = 0; i < children.Count; i++)
		{
			this.childInfo += children[i].getChildrenThreat().ToString ();
			if (i < children.Count - 1)
				this.childInfo += " | ";
			else
				this.childInfo += "\n";
		}

		selectedNode = 0;
		foundNode = false;
		neverPlayedNode = new List<int>();

		tic = Environment.TickCount;
		for (int i = 0; i < children.Count; i++)
		{
			this.nodeCount++;

			if (children [i].getChildrenThreat () == 999999999)		// Suicide Move
				break;

			if (! Node.ListContainsNode (movesPlayed, children [i]))
				neverPlayedNode.Add (i);

			if ((! children [i].stillAlive (!isGote) || children [i].getChildrenThreat() == -999999999) && ! Node.ListContainsNode (movesPlayed, children[i]))		// terminal node
			{
				foundNode = true;
				selectedNode = i;
				break;
			}

			if (i == 0)
			{
				children[i].setScore (NegaScoutMin (children [i], alpha, beta, depth - 1, isGote));
				if (children [i].getScore () > maxScore && children [i].getChildrenThreat() != 999999999 && ! Node.ListContainsNode (movesPlayed, children[i]))
				{
					maxScore = children [i].getScore ();
					foundNode = true;
					selectedNode = i;
				}
				if (maxScore >= beta)
					break;
			}
			else
			{
				children[i].setScore (NegaScoutMin (children [i], maxScore, maxScore + 1, depth - 1, isGote));
				if (children [i].getScore () > maxScore)
				{
					if (children [i].getScore () >= beta && children [i].getChildrenThreat () != 999999999 && !Node.ListContainsNode (movesPlayed, children [i]))
					{
						maxScore = children [i].getScore ();
						foundNode = true;
						selectedNode = i;
					}
					else
					{
						maxScore = NegaScoutMin (children [i], children [i].getScore (), beta, depth - 1, isGote);
						foundNode = true;
						selectedNode = i;
					}
				}
				if (maxScore >= beta)
					break;
			}
		}
		tac = Environment.TickCount;

		if (!foundNode && neverPlayedNode.Count > 0)
			selectedNode = neverPlayedNode[new Random().Next (neverPlayedNode.Count)];

		this.duration = tac - tic;
		this.nodeScore = children [selectedNode].getScore ();
		this.nodeThreat = children [selectedNode].getChildrenThreat ();

		return children [selectedNode];
	}

	private int NegaScoutMin (Node node, int alpha, int beta, int depth, bool isGote)
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
		minScore = Math.Min (minScore, NegaScoutMax (children [0], alpha, beta, depth - 1, isGote));
		if (minScore <= alpha)
			return (minScore);

		for (int i = 1; i < children.Count; i++)
		{
			score = NegaScoutMax (children [i], minScore, minScore + 1, depth - 1, isGote);
			if (score <= minScore)
			{
				if (score <= alpha)
					minScore = score;
				else
					minScore = NegaScoutMax (children [i], alpha, score, depth - 1, isGote);
			}
			if (minScore <= alpha)
				return minScore;
		}

		return minScore;
	}

	private int NegaScoutMax (Node node, int alpha, int beta, int depth, bool isGote)
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
		maxScore = Math.Max (maxScore, NegaScoutMin (children [0], alpha, beta, depth - 1, isGote));
		if (maxScore >= beta)
			return (maxScore);

		for (int i = 1; i < children.Count; i++)
		{
			score = NegaScoutMin (children [i], maxScore, maxScore + 1, depth - 1, isGote);
			if (score > maxScore)
			{
				if (score >= beta)
					maxScore = score;
				else
					maxScore = NegaScoutMin (children [i], score, beta, depth - 1, isGote);
			}
			if (maxScore >= beta)
				return maxScore;
		}

		return maxScore;
	}
}
