using System;
using System.Collections;
using System.Collections.Generic;
using Tools;

public class MinMax : IPlayer
{
	public MinMax () { }

	protected override void RunAlgorithm (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		this._bestMove = runMinMax (root, depth, isGote, movesPlayed);
	}

	private Node runMinMax (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		List <Node> children;
		int maxScore= int.MinValue;
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

		selectedNode = 0;
		foundNode = false;
		neverPlayedNode = new List<int>();

		tic = Environment.TickCount;
		for (int i = 0; i < children.Count; i++)
		{
			this.nodeCount++;

			if (! Node.ListContainsNode (movesPlayed, children [i]))
				neverPlayedNode.Add (i);

			if ((! children [i].stillAlive (!isGote) || children [i].getChildrenThreat() == -999999999) && ! Node.ListContainsNode (movesPlayed, children[i]))		// terminal node
			{
				foundNode = true;
				selectedNode = i;
				break;
			}

			children[i].setScore (Min (children[i], depth - 1, isGote));

			if (children[i].getScore() > maxScore && children [i].getChildrenThreat() != 999999999 && ! Node.ListContainsNode (movesPlayed, children[i]))
			{
				maxScore = children[i].getScore();
				foundNode = true;
				selectedNode = i;
			}
		}
		tac = Environment.TickCount;

		if (!foundNode && neverPlayedNode.Count > 0)
			selectedNode = neverPlayedNode[new System.Random().Next (neverPlayedNode.Count)];

		this.duration = tac - tic;
		this.nodeScore = children [selectedNode].getScore ();
		this.nodeThreat = children [selectedNode].getChildrenThreat ();

		return children [selectedNode];
	}

	private int Min (Node node, int depth, bool isGote)
	{
		List <Node> children;
		int score = int.MaxValue;

		node.createChildren (!isGote, true /* canDrop */, false /* sort */);
		if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
			return node.evaluate (isGote);

		children = new List<Node> (node.getChildren());
		node.clearChildren ();

		this.nodeCount++;
		for (int i = 0; i < children.Count; i++)
			score = Math.Min (score, Max (children[i], depth - 1, isGote));

		return score;
	}

	private int Max (Node node, int depth, bool isGote)
	{
		List <Node> children;
		int score = int.MinValue;

		node.createChildren (isGote, true /* canDrop */, false /* sort */);
		if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
			return node.evaluate (isGote);

		children = new List<Node> (node.getChildren());
		node.clearChildren ();

		this.nodeCount++;
		for (int i = 0; i < children.Count; i++)
			score = Math.Max (score, Min (children[i], depth - 1, isGote));

		return score;
	}
}
