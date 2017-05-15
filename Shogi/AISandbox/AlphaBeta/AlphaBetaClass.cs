using System;
using System.Text;
using System.Collections.Generic;
using Tools;

namespace AlphaBeta
{
	public class AlphaBetaClass
	{
		public static int nodeCount;

		public static Node AspirationSearch(Node root, int depth, bool isGote, List<Node> movesPlayed, int index, ref string gameWorkflow)
		{
			int totalNodeCount = 0;
			int alpha = int.MinValue;
			int beta = int.MaxValue;
			Node bestNode = null;
			int bestValue;
			int depthSearch;
			int tic;
			int tac;

			Console.WriteLine ("Entree de l'algo AlphaBeta avec Aspiration Gote Turn ? {0}", isGote);
			gameWorkflow += "Entree de l'algo AlphaBeta avec Aspiration Gote Turn ? " + isGote.ToString () + "\n";

			tic = Environment.TickCount;
			for (depthSearch = 1; depthSearch <= depth; depthSearch++) // iterative deepening
			{
				bestNode = AlphaBeta (root, depthSearch, alpha, beta, isGote, movesPlayed, index, ref gameWorkflow);
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

			Console.WriteLine ("Sortie de l'algo AlphaBeta avec Aspiration. Nodes Checked = " + totalNodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms");
			gameWorkflow += "Sortie de l'algo AlphaBeta avec Aspiration. Nodes Checked = " + totalNodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms\n";

			Console.WriteLine ("Score du noeud selectionne : {0} | Threat = {1}", bestNode.getScore(), bestNode.getChildrenThreat());
			gameWorkflow += "Score du noeud selectionne : " + bestNode.getScore().ToString() + " | Threat = " + bestNode.getChildrenThreat().ToString() + "\n";

			return bestNode;
		}

		public static Node AlphaBeta(Node root, int depth, int alpha, int beta, bool isGote, List<Node> movesPlayed, int index, ref string gameWorkflow)
		{
			List<Node> children;
			int maxScore = -999999999;
			int selectedNode;
			bool foundNode;
			List<int> neverPlayedNode;
			int i;
			int tic;
			int tac;

//			Console.WriteLine ("Entree de l'algo AlphaBeta Gote Turn ? {0}", isGote);
//			gameWorkflow += "Entree de l'algo AlphaBeta Gote Turn ? " + isGote.ToString () + "\n";

			nodeCount = 0;

			root.createChildren (isGote, true /* canDrop */, true /* sort */);
			if (root.getChildren ().Count == 0)
				return null;

			children = new List<Node> (root.getChildren ());
			root.clearChildren ();

//			for (i = 0; i < children.Count; i++)
//			{
//				Console.Write (children[i].getChildrenThreat ());
//				gameWorkflow += children[i].getChildrenThreat().ToString ();
//				if (i < children.Count - 1)
//				{
//					Console.Write (" | ");
//					gameWorkflow += " | ";
//				}
//				else
//				{
//					Console.Write ("\n");
//					gameWorkflow += "\n";
//				}
//			}

			selectedNode = 0;
			foundNode = false;
			neverPlayedNode = new List<int>();
			tic = Environment.TickCount;
			for (i = 0; i < children.Count; i++)
			{
				nodeCount++;

				if (!Node.ListContainsNode(movesPlayed, children[i]) && children[i].getChildrenThreat() != 999999999)
				{
					neverPlayedNode.Add(i);
				}

				if ((!children[i].stillAlive(!isGote) || children[i].getChildrenThreat() == -999999999) && !Node.ListContainsNode(movesPlayed, children[i]))        // terminal node
				{
					selectedNode = i;
					foundNode = true;
					break;
				}

				children[i].setScore(AlphaBetaMin(children[i], depth - 1, alpha, beta, isGote, root, index));

				if (children[i].getScore() > maxScore && children[i].getChildrenThreat() != 999999999 && !Node.ListContainsNode(movesPlayed, children[i]))
				{
					selectedNode = i;
					maxScore = children[selectedNode].getScore();
					foundNode = true;
				}

				if (maxScore >= beta)
				{
					foundNode = true;
					break;
				}

				if (maxScore > alpha)
				{
					alpha = maxScore;
				}

			}
			tac = Environment.TickCount;
			if (!foundNode && neverPlayedNode.Count > 0)
			{
//				Console.WriteLine ("I'm Here i = {0} | children Count = {1}", i, children.Count);
				selectedNode = neverPlayedNode[new Random().Next (neverPlayedNode.Count)];
			}
//			Console.WriteLine ("Sortie de l'algo AlphaBeta. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms");
//			gameWorkflow += "Sortie de l'algo AlphaBeta. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms\n";

//			Console.WriteLine ("Score du noeud selectionne : {0} | Threat = {1}", children[selectedNode].getScore(), children [selectedNode].getChildrenThreat());
//			gameWorkflow += "Score du noeud selectionne : " + children[selectedNode].getScore().ToString() + " | Threat = " + children [selectedNode].getChildrenThreat().ToString() + "\n";

			return children[selectedNode];
		}

		private static int AlphaBetaMin(Node node, int depth, int alpha, int beta, bool isGote, Node root, int index)
		{
			List<Node> children;
			int score = int.MaxValue;
			int minScore = int.MaxValue;

			nodeCount++;

			if (depth <= 0 || !node.stillAlive(isGote) || !node.stillAlive(!isGote))        // terminal node
				return node.evaluate(isGote);

			node.createChildren(!isGote, true /* canDrop */, false /* sort */);
			children = new List<Node>(node.getChildren());
			node.clearChildren();

			if (children.Count == 0)        // terminal node
				return node.evaluate(isGote);

			for (int i = 0; i < children.Count; i++)
			{
				score = AlphaBetaMax(children[i], depth - 1, alpha, beta, !isGote, root, index);

				if (score < minScore)
					minScore = score;

				if (minScore <= alpha)
					return minScore;        // alpha cut off

				if (minScore < beta)
					beta = minScore;
			}
			return minScore;
		}

		private static int AlphaBetaMax(Node node, int depth, int alpha, int beta, bool isGote, Node root, int index)
		{
			List<Node> children;
			int score = int.MinValue;
			int maxScore = int.MinValue;

			node.createChildren (!isGote, true /* canDrop */, false /* sort */);
			if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
				return node.evaluate (isGote);

			children = new List<Node> (node.getChildren());
			node.clearChildren ();

			nodeCount++;

			for (int i = 0; i < children.Count; i++)
			{
				score = AlphaBetaMin(children[i], depth - 1, alpha, beta, isGote, root, index);

				if (score > maxScore)
					maxScore = score;

				if (maxScore >= beta)
					return maxScore;        // beta cut off

				if (maxScore > alpha)
					alpha = maxScore;
			}
			return maxScore;
		}
	}
}



