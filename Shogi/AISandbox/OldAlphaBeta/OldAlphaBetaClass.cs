using System;
using System.Text;
using System.Collections.Generic;
using Tools;

namespace OldAlphaBeta
{
	public class OldAlphaBetaClass
	{
		public static int nodeCount;

		public static Node OldAlphaBeta (Node root, int depth, bool isGote, ref string gameWorkflow)
		{
			List <Node> children;
			int alpha = int.MinValue;
			int beta = int.MaxValue;
			int maxScore = -999999999;
			int selectedNode;
			int i;
			int tic;
			int tac;

			Console.WriteLine ("Entree de l'algo Old AlphaBeta Gote Turn ? {0}", isGote);
			gameWorkflow += "Entree de l'algo Old AlphaBeta Gote Turn ? " + isGote.ToString () + "\n";

			nodeCount = 0;

			root.createChildren (isGote, true /* canDrop */, false /* sort */);
			if (root.getChildren ().Count == 0)
				return null;

			children = new List<Node> (root.getChildren ());
			root.clearChildren ();

			selectedNode = 0;
			tic = Environment.TickCount;
			for (i = 0; i < children.Count; i++)
			{
				nodeCount++;

				children[i].setScore (AlphaBetaMin (children[i], depth - 1, alpha, beta, isGote));

				if (children [i].getScore () > maxScore)
				{
					selectedNode = i;
					maxScore = children [selectedNode].getScore ();
				}

				if (maxScore >= beta)
					break;

				if (maxScore > alpha)
					alpha = maxScore;
			}
			tac = Environment.TickCount;

			Console.WriteLine ("Sortie de l'algo OldAlphaBeta. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms");
			gameWorkflow += "Sortie de l'algo OldAlphaBeta. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms\n";

			Console.WriteLine ("Score du noeud selectionne : {0} | Threat = {1}", children[selectedNode].getScore(), children [selectedNode].getChildrenThreat());
			gameWorkflow += "Score du noeud selectionne : " + children[selectedNode].getScore().ToString() + " | Threat = " + children [selectedNode].getChildrenThreat().ToString() + "\n";

			return children [selectedNode];
		}

		private static int AlphaBetaMin (Node node, int depth, int alpha, int beta, bool isGote)
		{
			List <Node> children;
			int minScore = int.MaxValue;
			int score = int.MaxValue;

			node.createChildren (!isGote, true /* canDrop */, false /* sort */);
			if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
				return node.oldEvaluate (isGote);

			children = new List<Node> (node.getChildren());
			node.clearChildren ();

			nodeCount++;

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

		private static int AlphaBetaMax (Node node, int depth, int alpha, int beta, bool isGote)
		{
			List <Node> children;
			int maxScore = int.MinValue;
			int score = int.MinValue;

			node.createChildren (isGote, true /* canDrop */, false /* sort */);
			if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
				return node.oldEvaluate (isGote);

			children = new List<Node> (node.getChildren());
			node.clearChildren ();

			nodeCount++;

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
}

