using System;
using System.Collections;
using System.Collections.Generic;
using Tools;

public class ProofNumberSearch : IPlayer
{
	public ProofNumberSearch () { }

	protected override void RunAlgorithm (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		this._bestMove = runPns (root, depth, isGote, movesPlayed);
	}

	private Node runPns (Node root, int depth, bool isGote, List<Node> movesPlayed)
	{
		List <Node> children;
		int selectedNode = 0;
		List<Node> neverPlayedNode = new List<Node> ();
		int tic;
		int tac = 0;
		Node bestNode = null;

		selectedNode = 0;
		//foundNode = false;
		neverPlayedNode = new List<Node> ();
		tic = Environment.TickCount;
		// Start Process--------------------------------------------------------------------------------------------

		root.setIsGote (isGote);
		root.PNSHeuristic (isGote);
		root.setValue (Node.PNSValue.unknown);
		setProofAndDisproofNumbers (root, isGote);
		Node current = root;

		while (root.getProof () != 0 && root.getDisproof () != 0 && Environment.TickCount - tic < 5000)
		{
			Node mostProving = selectMostProvingNode (current, depth, isGote);
			current = updateAncestors (mostProving, root, isGote);
		}
		// End Process--------------------------------------------------------------------------------------------

		children = new List<Node> (root.getChildren ());

		if (children.Count == 0)
			return null;

		// Best Node Selection---------------------------------------------------------------

		//Default best node selection (lowest proof number)
		bestNode = defaultSelection (root, isGote, neverPlayedNode);

		//case : best node not found (should never occur)
		if (bestNode == null)
		{
			selectedNode = new Random ().Next (children.Count);
			bestNode = children [selectedNode];
		}
		else if (neverPlayedNode.Count > 1)
		{
			//case where there's multiple nodes with the same best score
			bestNode = selectionByScore (neverPlayedNode, isGote);
			//bestNode = getNodeLowThreat(neverPlayedNode, isGote);
		}
		tac = Environment.TickCount;
		// ------------------------------------------------------------------------------

		this.duration = tac - tic;
		this.nodeScore = bestNode.getScore ();
		this.nodeThreat = bestNode.getChildrenThreat ();

		return bestNode;
	}

	/// <summary>
	/// Initialize Proof and Disproof Numbers of the node from input.  isGote : true if GOTE ; false if SENTE
	/// </summary>
	/// <param name="node">Node.</param>
	/// <param name="isGote">If set to <c>true</c> is gote.</param>
	private static void setProofAndDisproofNumbers(Node node, bool isGote)
	{
		//Console.WriteLine ("****Node proof value : " + node._proof + " disproof value : " + node._disproof+" state "+node._value);
		if (node.getChildren ().Count != 0) /*node expanded*/
		{
			if (node.getIsGote() == !isGote) /*AND node (Opponent)*/
			{
				//Console.WriteLine ("AND node");
				node.setProof(0);
				node.setDisproof(1000000);

				foreach (Node child in node.getChildren())
				{
					node.setProof (node.getProof() + child.getProof());
					node.setDisproof (Math.Min (node.getDisproof(), child.getDisproof()));
				}
			}
			else /*OR node (IA)*/
			{
				//Console.WriteLine ("OR node");
				node.setProof(1000000);
				node.setDisproof(0);

				foreach (Node child in node.getChildren())
				{
					node.setDisproof (node.getDisproof() + child.getDisproof());
					node.setProof (Math.Min (node.getProof(), child.getProof()));
				}
			}
		}
		else /*leaf node*/
		{
			//Console.WriteLine ("Leaf node");
			switch (node.getValue())
			{
				case Node.PNSValue.disproven:			//disproven
					node.setProof(1000000);
					node.setDisproof(0);
					break;
				case Node.PNSValue.proven:			//proven
					node.setProof(0);
					node.setDisproof(1000000);
					break;
				case Node.PNSValue.unknown:			//unknown
					node.setProof(1);
					node.setDisproof(1);
					break;
				default:
					break;
			}
		}
	}

	/// <summary>
	/// Selects the Most Proving Node.
	/// </summary>
	/// <returns>The most proving node.</returns>
	/// <param name="node">Node.</param>
	/// <param name="depth">Depth.</param>
	/// <param name="isGote">If set to <c>true</c> is gote.</param>
	private static Node selectMostProvingNode(Node node, int depth, bool isGote)
	{
		int depthMax = depth;

		while (node.getChildren ().Count != 0 && depth != 0)	//loop until we get a leaf node
		{
			int value = 1000000;
			Node best = null;

			if (node.getIsGote() == !isGote)	/*AND node (Opponent)*/
			{
				foreach (Node child in node.getChildren())
				{

					if (value > child.getDisproof())
					{
						best = child;
						value = child.getDisproof();
					}
				}
			}
			else
			{	/*OR node (IA)*/
				foreach (Node child in node.getChildren())
				{

					if (value > child.getProof())
					{
						best = child;
						value = child.getProof();
					}
				}
			}
			depthMax--;
			node = best;
		}

		//Most Proving Node expansion
		expandNode (node, depthMax, isGote);

		return node;
	}

	/// <summary>
	/// This method creates all node's child and evaluate each of them.
	/// </summary>
	/// <param name="node">Node.</param>
	/// <param name="depth">Depth.</param>
	/// <param name="isGote">If set to <c>true</c> is gote.</param>
	private static void expandNode(Node node, int depth, bool isGote)
	{
		//Console.WriteLine ("isGote ?" + isGote);

		if(depth == 0){		//depth limit
			node.createChildren(node.getIsGote(), true, false);		//compute node threat
			node.calculateChildrenThreat (isGote);					//-------------------
			node.clearChildren ();									//-------------------

			node.PNSHeuristic (isGote);
			setProofAndDisproofNumbers (node, isGote);
			return;
		}

		node.createChildren (isGote, true, false);
		//node.calculateChildrenThreat (isGote);

		foreach(Node child in node.getChildren())
		{
			//Console.WriteLine(child.getIsGote());
			child.createChildren(node.getIsGote(), true, false);	//compute chil threat
			child.calculateChildrenThreat (isGote);					//-------------------
			child.clearChildren ();									//-------------------

			child.PNSHeuristic (isGote);
			//	Console.WriteLine ("isGote ?" + child._isGote);
			setProofAndDisproofNumbers (child, isGote);


			if ((node.getIsGote () == !isGote && child.getDisproof() != 0) || (node.getIsGote () == isGote && child.getProof() != 0)) {
				break;
			}
		}
		node.setExpanded(true);
	}

	/// <summary>
	/// Updates Proof and Disproof Numbers of the node from input and those numbers are backed up to the node's ancestors.
	/// </summary>
	/// <returns>The ancestors.</returns>
	/// <param name="node">Node.</param>
	/// <param name="root">Root.</param>
	/// <param name="isGote">If set to <c>true</c> is gote.</param>
	private static Node updateAncestors(Node node, Node root, bool isGote)
	{

		while(node != root){
			int oldProof = node.getProof();
			int oldDisproof = node.getDisproof();

			setProofAndDisproofNumbers (node, isGote);

			if (node.getProof() == oldProof && node.getDisproof() == oldDisproof)
				return node;

			node = node.getParent ();
		}

		setProofAndDisproofNumbers (root, isGote);
		return root;
	}


	//----------------------------------------------------------------------------------------------------------------------------------------------------
	private static Node getNodeLowThreat(List<Node> listNode, bool isGote){
		int minVal;
		Node nodeFound = null;

		if (listNode.Count == 0)
			return null;

		minVal = 1000000000;
		foreach (Node node in listNode) {
			//Console.WriteLine ("Child threat " + node.getChildrenThreat ());
			node.createChildren (node.getIsGote (), true, false);
			node.calculateChildrenThreat (isGote);
			node.clearChildren ();

			if (node.getChildrenThreat () < minVal) {	//we keep the node with the lowest threat value
				minVal = node.getChildrenThreat ();
				nodeFound = node;
			}
			//node.clearChildren ();
		}
		return nodeFound;
	}

	/// <summary>
	/// Default best node selection. If isGote is true, this method will choose the child with the lowest Proof Number.
	///  If isGote is false, this method will choose the child with the lowest Disproof Number.
	/// </summary>
	/// <returns>The selection.</returns>
	/// <param name="root">Root.</param>
	/// <param name="isGote">If set to <c>true</c> is gote.</param>
	/// <param name="neverPlayedNode">Never played node.</param>
	private static Node defaultSelection(Node root, bool isGote, List<Node> neverPlayedNode){
		int value = 10000000;
		Node bestNode = null;

		neverPlayedNode.Clear ();

		if (isGote) {	//GOTE
			Console.WriteLine ("Default selection GOTE");

			foreach (Node child in root.getChildren()) {
				if (child.getScore () == 999999999) {	//can eat the Jewel {end game}
					Console.WriteLine ("Gote Win");
					return child;
				}

				if (child.getProof () < value) {		//best node is the child with the lowest proof value
					value = child.getProof ();
					bestNode = child;

					neverPlayedNode.Clear ();
					neverPlayedNode.Add (child);

				} else if (child.getProof () == value || child.getScore () == 0) //if node's score == 0, node is not evaluated yet
					neverPlayedNode.Add (child);
			}
		} else {	//SENTE
			Console.WriteLine ("Default selection SENTE");

			foreach (Node child in root.getChildren()) {
				if (child.getScore () == -999999999) {	//can eat the King {end game}
					Console.WriteLine ("Sente Win");
					return child;
				}

				if (child.getDisproof () < value) {		//best node is the child with the lowest disproof value
					value = child.getDisproof ();
					bestNode = child;

					neverPlayedNode.Clear ();
					neverPlayedNode.Add (child);

				} else if (child.getDisproof () == value || child.getScore () == 0)
					neverPlayedNode.Add (child);
			}
		}

		return bestNode;
	}

	/// <summary>
	/// Best node selection by score. If isGote is true, this method will choose the child with the greatest score.
	///  If isGote is false, this method will choose the child with the lowest score.
	/// </summary>
	/// <returns>The by score.</returns>
	/// <param name="nodeList">Node list.</param>
	/// <param name="isGote">If set to <c>true</c> is gote.</param>
	private static Node selectionByScore(List<Node> nodeList, bool isGote){
		Console.WriteLine ("Score selection");
		int value = (isGote) ? -1000000 : 1000000;
		Node bestNode = null;

		foreach (Node child in nodeList) {
			if (child.getScore () == 0) {
				child.PNSHeuristic (isGote);
			}
		}

		if (isGote) {
			foreach (Node child in nodeList) {
				if (child.getScore () == 999999999) {	//can eat the Jewel {end game}

					return child;
				}

				if (child.getScore () > value) {
					bestNode = child;
					value = child.getScore ();
				}
			}
		} else {
			foreach (Node child in nodeList) {
				if (child.getScore () == -999999999) {	//can eat the Jewel {end game}
					Console.WriteLine ("Sente Win");
					return child;
				}

				if (child.getScore () < value) {
					bestNode = child;
					value = child.getScore ();
				}
			}
		}

		return bestNode;
	}
}
