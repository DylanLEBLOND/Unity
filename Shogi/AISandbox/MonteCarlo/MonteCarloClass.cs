using System;
using System.Text;
using System.Collections.Generic;
using Tools;

namespace MonteCarlo
{
	public class MonteCarloClass
	{
		public static int nodeCount;

		public static Node MonteCarlo (Node root, int depth, bool isGote, List<Node> movesPlayed, ref string gameWorkflow)
		{
			int tic;
			int tac;

			Console.WriteLine ("Entree de l'algo MonteCarlo Gote Turn ? {0}", isGote);
			gameWorkflow += "Entree de l'algo MonteCarlo Gote Turn ? " + isGote.ToString () + "\n";

			nodeCount = 0;
			tic = Environment.TickCount;

			//TODO BMA

			tac = Environment.TickCount;

			Console.WriteLine ("Sortie de l'algo MonteCarlo. Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms");
			gameWorkflow += "Sortie de l'algo MonteCarlo. Nodes Checked = " + nodeCount.ToString () + " | Duree = " + (tac - tic).ToString() + " ms\n";

			return null;		// TO REMOVE BMA
		}
	}
}

