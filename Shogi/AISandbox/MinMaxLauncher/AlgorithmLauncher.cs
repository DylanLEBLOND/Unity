using System;
using System.Text;
using System.Collections.Generic;
using NegaScout;
using Tools;

class AlgorithmLauncher
{
	public static void Main (string[] args)
	{
		Node root = SerializeNode.Deserialize (args[0]);
		int depth = int.Parse (args [1]);
		bool isGote = bool.Parse (args [2]);
		List<Node> movesPlayed = SerializeNode.DeserializeList (args [3]);
		string resultPath = args [4];
		string gameWorkflow = "";

		Node result = NegaScoutClass.NegaScout(root, depth, isGote, movesPlayed, ref gameWorkflow);
		SerializeNode.Serialize (result, resultPath);
	}
}
