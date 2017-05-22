using System;
using System.Text;
using System.Collections.Generic;
using Tools;

namespace MinMax
{
	public class MinMaxClass
	{
		public static int nodeCount;

		public static Node MinMax (Node root, int depth, bool isGote, List<Node> movesPlayed, ref string gameWorkflow)
		{
			List <Node> children;
			int maxScore= int.MinValue;
			int selectedNode;
			bool foundNode;
			List<int> neverPlayedNode;
			int tic;
			int tac;

			Console.WriteLine ("Entree de l'algo MinMax Gote Turn ? {0}", isGote);
			gameWorkflow += "Entree de l'algo MinMax Gote Turn ? " + isGote.ToString () + "\n";

			nodeCount = 0;

			root.createChildren (isGote, true /* canDrop */, true /* sort */);
			if (root.getChildren ().Count == 0)
				return null;

			children = new List<Node> (root.getChildren ());
			root.clearChildren ();

			for (int i = 0; i < children.Count; i++)
			{
				Console.Write (children[i].getChildrenThreat ());
				gameWorkflow += children[i].getChildrenThreat().ToString ();
				if (i < children.Count - 1)
				{
					Console.Write (" | ");
					gameWorkflow += " | ";
				}
				else
				{
					Console.Write ("\n");
					gameWorkflow += "\n";
				}
			}

			selectedNode = 0;
			foundNode = false;
			neverPlayedNode = new List<int>();
			tic = Environment.TickCount;
			for (int i = 0; i < children.Count; i++)
			{
				nodeCount++;

				if (children [i].getChildrenThreat () == 999999999)		// Suicide Move (with the node sort this will be the last node so we can stop now)
					break;

				if (Node.ListContainsNode (movesPlayed, children[i]))
					continue;		// we will start an infinite loop

				if (! Node.ListContainsNode (movesPlayed, children [i]))
					neverPlayedNode.Add (i);

				if ((! children [i].stillAlive (!isGote) || children [i].getChildrenThreat() == -999999999))		// winning node
				{
					foundNode = true;
					selectedNode = i;
					break;
				}

				children[i].setScore (Min (children[i], depth - 1, isGote));

				if (children[i].getScore() > maxScore && children [i].getChildrenThreat() != 999999999)
				{
					maxScore = children[i].getScore();
					foundNode = true;
					selectedNode = i;
				}
			}
			tac = Environment.TickCount;

			if (!foundNode && neverPlayedNode.Count > 0)
				selectedNode = neverPlayedNode[new Random().Next (neverPlayedNode.Count)];

			Console.WriteLine ("Sortie de l'algo MinMax. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms (tic = " + tic.ToString() + " | tac = " + tac.ToString() + ")");
			gameWorkflow += "Sortie de l'algo MinMax. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms\n";

			Console.WriteLine ("Score du noeud selectionne : {0} | Threat = {1}", children[selectedNode].getScore(), children [selectedNode].getChildrenThreat());
			gameWorkflow += "Score du noeud selectionne : " + children[selectedNode].getScore().ToString() + " | Threat = " + children [selectedNode].getChildrenThreat().ToString() + "\n";

			return children [selectedNode];
		}

		private static int Min (Node node, int depth, bool isGote)
		{
			List <Node> children;
			int score = int.MaxValue;

			node.createChildren (!isGote, true /* canDrop */, false /* sort */);
			if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
				return node.evaluate (isGote);

			children = new List<Node> (node.getChildren());
			node.clearChildren ();

			nodeCount++;
			for (int i = 0; i < children.Count; i++)
				score = Math.Min (score, Max (children[i], depth - 1, isGote));

			return score;
		}

		private static int Max (Node node, int depth, bool isGote)
		{
			List <Node> children;
			int score = int.MinValue;

			node.createChildren (isGote, true /* canDrop */, false /* sort */);
			if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
				return node.evaluate (isGote);

			children = new List<Node> (node.getChildren());
			node.clearChildren ();

			nodeCount++;
			for (int i = 0; i < children.Count; i++)
				score = Math.Max (score, Min (children[i], depth - 1, isGote));

			return score;
		}
	}
}

