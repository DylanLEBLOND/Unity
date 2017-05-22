using System.Collections;
using System.Collections.Generic;
using ShogiUtils;
using Tools;

public interface IPlayer
{
	void searchMove (Node root, int depth, bool isGote, List<Node> movesPlayed);
	bool moveReady ();
	Node getBestMove ();
}
