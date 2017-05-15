using System;
using System.Text;
using System.Collections.Generic;
using Tools;

namespace NegaScout
{
	public class NegaScoutClass
	{
		public static int nodeCount;

		public static Node NegaScout (Node root, int depth, bool isGote, List<Node> movesPlayed, ref string gameWorkflow)
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

			Console.WriteLine ("Entree de l'algo NegaScout Gote Turn ? {0}", isGote);
			gameWorkflow += "Entree de l'algo NegaScout Gote Turn ? " + isGote.ToString () + "\n";

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

				if (children [i].getChildrenThreat () == 999999999)		// Suicide Move
					break;

				if (! Node.ListContainsNode (movesPlayed, children [i]))
				{
					neverPlayedNode.Add (i);
				}

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
			{
				selectedNode = neverPlayedNode[new Random().Next (neverPlayedNode.Count)];
			}
			Console.WriteLine ("Sortie de l'algo NegaScout. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms");
			gameWorkflow += "Sortie de l'algo NegaScout. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms\n";

			Console.WriteLine ("Score du noeud selectionne : {0} | Threat = {1}", children[selectedNode].getScore(), children [selectedNode].getChildrenThreat());
			gameWorkflow += "Score du noeud selectionne : " + children[selectedNode].getScore().ToString() + " | Threat = " + children [selectedNode].getChildrenThreat().ToString() + "\n";

			return children [selectedNode];
		}

		private static int NegaScoutMin (Node node, int alpha, int beta, int depth, bool isGote)
		{
			List <Node> children;
			int minScore = int.MaxValue;
			int score = int.MaxValue;

			node.createChildren (!isGote, true /* canDrop */, false /* sort */);
			if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
				return node.evaluate (isGote);

			children = new List<Node> (node.getChildren());
			node.clearChildren ();

			nodeCount++;
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

		private static int NegaScoutMax (Node node, int alpha, int beta, int depth, bool isGote)
		{
			List <Node> children;
			int maxScore = int.MinValue;
			int score = int.MinValue;

			node.createChildren (isGote, true /* canDrop */, false /* sort */);
			if (depth <= 0 || ! node.stillAlive (isGote) || ! node.stillAlive (!isGote) || node.getChildren().Count == 0)		// terminal node
				return node.evaluate (isGote);

			children = new List<Node> (node.getChildren());
			node.clearChildren ();

			nodeCount++;
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

//////////////////////////////////////////// ALPHA BETA ALGORITHM ////////////////////////////////////////////

		public static Node AlphaBeta (Node root, int depth, bool isGote, List<Node> movesPlayed, ref string gameWorkflow)
		{
			List <Node> children;
			int alpha = int.MinValue;
			int beta = int.MaxValue;
			int maxScore = -999999999;
			int selectedNode;
			bool foundNode;
			List<int> neverPlayedNode;
			int i;
			int tic;
			int tac;

			Console.WriteLine ("Entree de l'algo AlphaBeta Gote Turn ? {0}", isGote);
			gameWorkflow += "Entree de l'algo AlphaBeta Gote Turn ? " + isGote.ToString () + "\n";

			nodeCount = 0;

			root.createChildren (isGote, true /* canDrop */, true /* sort */);
			if (root.getChildren ().Count == 0)
				return null;

			children = new List<Node> (root.getChildren ());
			root.clearChildren ();

			for (i = 0; i < children.Count; i++)
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
			for (i = 0; i < children.Count; i++)
			{
				nodeCount++;

				if (! Node.ListContainsNode (movesPlayed, children [i]) && children [i].getChildrenThreat() != 999999999)
				{
					neverPlayedNode.Add (i);
				}		

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
				{
					alpha = maxScore;
				}
			}
			tac = Environment.TickCount;
			if (!foundNode && neverPlayedNode.Count > 0)
			{
				Console.WriteLine ("I'm Here i = {0} | children Count = {1}", i, children.Count);
				selectedNode = neverPlayedNode[new Random().Next (neverPlayedNode.Count)];
			}
			Console.WriteLine ("Sortie de l'algo AlphaBeta. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms");
			gameWorkflow += "Sortie de l'algo AlphaBeta. Node Index = " + selectedNode + " | Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms\n";

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
				return node.evaluate (isGote);

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
				return node.evaluate (isGote);

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

