using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Tools
{
	[Serializable]
	public class Node
	{
		public enum PNSValue
		{
			disproven = -1, proven = 1, unknown = 0
		}

		public Token.TokenInfo _tokenInfo = new Token.TokenInfo();
		public List<Token.TokenType> _goteCapturedTokens;
		public List<Token.TokenType> _senteCapturedTokens;
		public List<Node> _children;
		public int _childrenThreat;
		public bool _isGote;
		public Node _parent = null;
		public int _index = 0;
		public int _score = 0;
		public bool  _expanded = false;
		public PNSValue _value = PNSValue.unknown;
		public int _proof = 1;
		public int _disproof = 1;

		public int[][] _state;

		public Node ()
		{
			this._tokenInfo.Clear ();
			this._goteCapturedTokens = new List<Token.TokenType> ();
			this._senteCapturedTokens = new List<Token.TokenType> ();
			this._children = new List<Node> ();

			this._state = new int[9][];
			for (int i = 0; i < 9; i++)
				this._state [i] = new int[9]; 

			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					this._state [i] [j] = 0;
		}

		public Node (bool isGote, Node parent, Token.TokenInfo tokenInfo, List<Token.TokenType> goteCapturedTokens, List<Token.TokenType> senteCapturedTokens, List<Node> children, ref int [][] initState)
		{
			this._tokenInfo = tokenInfo;
			this._goteCapturedTokens = new List<Token.TokenType> (goteCapturedTokens);
			this._senteCapturedTokens = new List<Token.TokenType> (senteCapturedTokens);
			this._children = new List<Node> (children);
			this._parent = parent;
			this._isGote = isGote;

			this._state = new int[9][];
			for (int i = 0; i < 9; i++)
				this._state [i] = new int[9]; 

			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					this._state [i] [j] = initState [i] [j];
		}

		public Node (Node src)
		{
			Token.TokenInfo srcTokenInfo = src.getTokenInfo ();

			this._tokenInfo.tokenType = srcTokenInfo.tokenType;
			this._tokenInfo.droppedPiece = srcTokenInfo.droppedPiece;
			this._tokenInfo.oldX = srcTokenInfo.oldX;
			this._tokenInfo.oldY = srcTokenInfo.oldY;
			this._tokenInfo.oldValue = srcTokenInfo.oldValue;
			this._tokenInfo.newX = srcTokenInfo.newX;
			this._tokenInfo.newY = srcTokenInfo.newY;
			this._tokenInfo.promoted = srcTokenInfo.promoted;
			this._tokenInfo.newValue = srcTokenInfo.newValue;
			this._tokenInfo.capturedSomething = srcTokenInfo.capturedSomething;
			this._tokenInfo.capturedType = srcTokenInfo.capturedType;
			this._tokenInfo.capturedValue = srcTokenInfo.capturedValue;

			this._goteCapturedTokens = new List<Token.TokenType> (src.getGoteCapturedTokens());
			this._senteCapturedTokens = new List<Token.TokenType> (src.getSenteCapturedTokens());
			this._children = new List<Node> (src.getChildren());
			this._childrenThreat = src.getChildrenThreat ();
			this._isGote = src.getIsGote ();
			this._parent = src.getParent ();
			this._index = src.getIndex ();
			this._score = src.getScore ();
			this._expanded = src.getExpanded ();
			this._value = src.getValue ();
			this._proof = src.getProof ();
			this._disproof = src.getDisproof ();

			int [][] srcState = new int[9][];
			for (int i = 0; i < 9; i++)
				srcState [i] = new int[9]; 			
			src.getState (ref srcState);

			this._state = new int[9][];
			for (int i = 0; i < 9; i++)
				this._state [i] = new int[9]; 

			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					this._state [i] [j] = srcState [i] [j];
		}

		public static int compareValue (Node src1, Node src2)
		{
			// this will return: 0 if they have the same value
			//					<0 if src1 value is lower than src2 value
			//					>0 if src1 value is greater than src2 value

			return  src2.getValue () - src1.getValue ();
		}

		public bool isSameNode (Node src)
		{
			List<Token.TokenType> srcGoteCapturedTokens = new List<Token.TokenType> (src.getGoteCapturedTokens());
			List<Token.TokenType> srcSenteCapturedTokens = new List<Token.TokenType> (src.getSenteCapturedTokens());

			if ((srcGoteCapturedTokens.Count != this._goteCapturedTokens.Count) ||
				(srcSenteCapturedTokens.Count != this._senteCapturedTokens.Count))
				return false;

			for (int i = 0; i < srcGoteCapturedTokens.Count; i++)
				if (srcGoteCapturedTokens [i] != this._goteCapturedTokens [i])
					return false;

			for (int i = 0; i < srcSenteCapturedTokens.Count; i++)
				if (srcSenteCapturedTokens [i] != this._senteCapturedTokens [i])
					return false;

			int [][] srcState = new int[9][];
			for (int i = 0; i < 9; i++)
				srcState [i] = new int[9]; 
			src.getState (ref srcState);

			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					if (srcState [i][j] != this._state [i][j])
						return false;

			return true;
		}

		public static bool ListContainsNode (List<Node> srcList, Node targetNode)
		{
			for (int i = 0; i < srcList.Count; i++)
				if (targetNode.isSameNode (srcList [i]))
					return true;

			return false;
		}

		public void clear ()
		{
			this._tokenInfo.Clear();
			this._goteCapturedTokens.Clear();
			this._senteCapturedTokens.Clear();
			this._children.Clear();
			this._children = null;
			this._childrenThreat = 0;
			this._score = 0;
			this._expanded = false;
			this._value = PNSValue.unknown;
			this._proof = 1;
			this._disproof = 1;
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					this._state [i] [j] = 0;
		}

		public Token.TokenInfo getTokenInfo ()
		{
			return this._tokenInfo;
		}

		public void setTokenInfo (Token.TokenInfo srcTokenInfo)
		{
			this._tokenInfo.tokenType = srcTokenInfo.tokenType;
			this._tokenInfo.droppedPiece = srcTokenInfo.droppedPiece;
			this._tokenInfo.oldX = srcTokenInfo.oldX;
			this._tokenInfo.oldY = srcTokenInfo.oldY;
			this._tokenInfo.oldValue = srcTokenInfo.oldValue;
			this._tokenInfo.newX = srcTokenInfo.newX;
			this._tokenInfo.newY = srcTokenInfo.newY;
			this._tokenInfo.promoted = srcTokenInfo.promoted;
			this._tokenInfo.newValue = srcTokenInfo.newValue;
			this._tokenInfo.capturedSomething = srcTokenInfo.capturedSomething;
			this._tokenInfo.capturedType = srcTokenInfo.capturedType;
			this._tokenInfo.capturedValue = srcTokenInfo.capturedValue;
		}

		public List<Token.TokenType> getGoteCapturedTokens ()
		{
			return this._goteCapturedTokens;
		}

		public void setGoteCapturedTokens (List<Token.TokenType> goteCapturedTokens)
		{
			this._goteCapturedTokens = goteCapturedTokens;
		}

		public string getGoteCapturedTokensInString ()
		{
			int i;
			string GCTstring = "";

			for (i = 0; i < this._goteCapturedTokens.Count; i++)
			{
				GCTstring += Token.getTokenTypeString (this._goteCapturedTokens[i]);
				if (i < this._goteCapturedTokens.Count - 1)
					GCTstring += " | ";
			}
			if (i == 0)
				GCTstring += "{ }";

			return GCTstring;
		}

		public List<Token.TokenType> getSenteCapturedTokens ()
		{
			return this._senteCapturedTokens;
		}

		public void setSenteCapturedTokens (List<Token.TokenType> senteCapturedTokens)
		{
			this._senteCapturedTokens = senteCapturedTokens;
		}

		public string getSenteCapturedTokensInString ()
		{
			int i;
			string SCTstring = "";

			for (i = 0; i < this._senteCapturedTokens.Count; i++)
			{
				SCTstring += Token.getTokenTypeString (this._senteCapturedTokens[i]);
				if (i < this._senteCapturedTokens.Count - 1)
					SCTstring += " | ";
			}
			if (i == 0)
				SCTstring += "{ }";

			return SCTstring;
		}

		public List<Node> getChildren ()
		{
			return this._children;
		}

		public void setChildren (List<Node> children)
		{
			this._children = children;
		}

		public void clearChildren ()
		{
			this._children.Clear();
		}

		public int getChildrenThreat ()
		{
			return (this._childrenThreat);
		}

		public void setChildrenThreat (int threat)
		{
			this._childrenThreat = threat;
		}

		public void calculateChildrenThreat (bool isGote)
		{
			this._childrenThreat = 0;

			for (int i = 0; i < this._children.Count; i++)
			{
				if (! this._children [i].stillAlive (isGote))
				{
					this._childrenThreat = -999999999;
					return;
				}
				if (! this._children [i].stillAlive (!isGote))
				{
					this._childrenThreat = 999999999;
					return;
				}
				this._childrenThreat += this._children [i].evaluate (isGote);
			}
		}

		public bool getIsGote()
		{
			return this._isGote;
		}

		public void setIsGote(bool isGote)
		{
			this._isGote = isGote;
		}

		public Node getParent()
		{
			return this._parent;
		}

		public void setParent(Node parent)
		{
			this._parent = parent;
		}

		public int getIndex ()
		{
			return this._index;
		}

		public void setIndex (int index)
		{
			this._index = index;
		}

		public int getScore ()
		{
			return (this._score);
		}

		public void setScore (int score)
		{
			this._score = score;;
		}

		public bool getExpanded()
		{
			return this._expanded;
		}

		public void setExpanded(bool expanded)
		{
			this._expanded = expanded;
		}

		public PNSValue getValue ()
		{
			return this._value;
		}

		public void setValue (PNSValue value)
		{
			this._value = value;
		}

		public int getProof()
		{
			return this._proof;
		}

		public void setProof(int proof)
		{
			this._proof = proof;
		}

		public int getDisproof()
		{
			return this._disproof;
		}

		public void setDisproof(int disproof)
		{
			this._disproof = disproof;
		}

		public void getState (ref int [][] refState)
		{
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					refState [i] [j] = this._state [i] [j];
		}

		public void setState (ref int [][] refState)
		{
			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					this._state [i] [j] = refState [i] [j];
		}

		public void showState ()
		{
			Console.WriteLine (this.getStateInString());
		}

		public string getStateInString ()
		{
			string stateString = "";

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (this._state [i][j] == 0)
						stateString += "     ";
					else if (this._state [i][j] < 0)
					{
						if (this._state [i][j] > -1000)
							stateString += " " + this._state [i][j].ToString ();
						else
							stateString += this._state [i][j].ToString ();
					}
					else
					{
						if (this._state [i][j] < 1000)
							stateString += " " + this._state [i][j].ToString () + " ";
						else
							stateString += " " + this._state [i][j].ToString ();
					}
					if (j < 8)
						stateString += " | ";
					else
						stateString += "\n";
				}
			}

			return stateString;
		}

		public int evaluate (bool isGote)
		{
			int i;
			int j;
			int yKing = -1;
			int xKing = -1;
			int yJewel = -1;
			int xJewel = -1;
			int goteScore = 0;
			int senteScore = 0;

			if (!this.stillAlive (isGote))		// The current player lost
				return -999999999;

			if (!this.stillAlive (!isGote))		// The current player won
				return 999999999;

			if (!this.getKingPositions (ref yKing, ref xKing, true))		// Gote has already lost; Should never occur
				return isGote ? -999999999 : 999999999;

			if (!this.getKingPositions (ref yJewel, ref xJewel, false))		// Sente has already lost; Should never occur
				return isGote ? 999999999 : -999999999;

			for (i = 0; i < 9; i++)
			{
				for (j = 0; j < 9; j++)
				{
					if (this._state [i][j] > 0)
					{
						goteScore += this._state [i][j];
						switch (this._state [i][j])
						{
							case Token.GPAWN:
								goteScore += this.getPawnDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.P_GPAWN:
								goteScore += this.getSilverGoldDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.GLANCE:
								goteScore += this.getLanceDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.P_GLANCE:
								goteScore += this.getSilverGoldDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.GKNIGHT:
								goteScore += this.getKnightDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.P_GKNIGHT:
								goteScore += this.getSilverGoldDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.GSILVER:
								goteScore += this.getSilverGoldDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.P_GSILVER:
								goteScore += this.getSilverGoldDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.GGOLD:
								goteScore += this.getSilverGoldDistance (i, j, yJewel, xJewel, true);
								break;
							case Token.GBISHOP:
							case Token.P_GBISHOP:
								goteScore += this.getBishopDistance (i, j, yJewel, xJewel);
								break;
							case Token.GROOK:
							case Token.P_GROOK:
								goteScore += this.getRookDistance (i, j, yJewel, xJewel);
								break;
							default:
								if (this._state[i][j] != Token.KING)
									Console.WriteLine ("Gote Unknown Token state[{0},{1}] = {2}", i, j, this._state[i][j]);
								break;
						}
					}
					else if (this._state [i][j] < 0)
					{
						senteScore += -this._state [i][j];
						switch (this._state [i][j])
						{
							case Token.SPAWN:
								senteScore += this.getPawnDistance (i, j, yKing, xKing, false);
								break;
							case Token.P_SPAWN:
								senteScore += this.getSilverGoldDistance (i, j, yKing, xKing, false);
								break;
							case Token.SLANCE:
								senteScore += this.getLanceDistance (i, j, yKing, xKing, false);
								break;
							case Token.P_SLANCE:
								senteScore += this.getSilverGoldDistance (i, j, yKing, xKing, false);
								break;
							case Token.SKNIGHT:
								senteScore += this.getKnightDistance (i, j, yKing, xKing, false);
								break;
							case Token.P_SKNIGHT:
								senteScore += this.getSilverGoldDistance (i, j, yKing, xKing, false);
								break;
							case Token.SSILVER:
								senteScore += this.getSilverGoldDistance (i, j, yKing, xKing, false);
								break;
							case Token.P_SSILVER:
								senteScore += this.getSilverGoldDistance (i, j, yKing, xKing, false);
								break;
							case Token.SGOLD:
								senteScore += this.getSilverGoldDistance (i, j, yKing, xKing, false);
								break;
							case Token.SBISHOP:
							case Token.P_SBISHOP:
								senteScore += this.getBishopDistance (i, j, yKing, xKing);
								break;
							case Token.SROOK:
							case Token.P_SROOK:
								senteScore += this.getRookDistance (i, j, yKing, xKing);
								break;
							default:
								if (this._state[i][j] != Token.JEWEL)
									Console.WriteLine ("Sente Unknown Token state[{0},{1}] = {2}", i, j, this._state[i][j]);
								break;
						}
					}
				}
			}

			for (i = 0; i < this._goteCapturedTokens.Count; i++)
				goteScore += Token.getCapturedTokenValue (this._goteCapturedTokens [i]);

			for (i = 0; i < this._senteCapturedTokens.Count; i++)
				senteScore += Token.getCapturedTokenValue (this._senteCapturedTokens [i]);

			return isGote ? goteScore - senteScore : senteScore - goteScore;;
		}

		public int oldEvaluate (bool isGote)
		{
			int i;
			int j;
			int goteScore = 0;
			int senteScore = 0;

			for (i = 0; i < 9; i++)
			{
				for (j = 0; j < 9; j++)
				{
					if (this._state [i][j] > 0)
						goteScore += this._state [i][j];
					else if (this._state [i][j] < 0)
						senteScore += -this._state [i][j];
				}
			}

			for (i = 0; i < this._goteCapturedTokens.Count; i++)
				goteScore += Token.getCapturedTokenValue (this._goteCapturedTokens [i]);

			for (i = 0; i < this._senteCapturedTokens.Count; i++)
				senteScore += Token.getCapturedTokenValue (this._senteCapturedTokens [i]);

			return isGote ? goteScore - senteScore : senteScore - goteScore;;
		}

		public void PNSHeuristic (bool isGote)
		{
			this._score = this.evaluate (isGote);

			if (999999999 <= this._score)
			{
				this._value = PNSValue.proven;
				return;		// proven node (the current player has won)
			}
			if (this._score <= -999999999)
			{
				this._value = PNSValue.disproven;
				return;		// disproven node (the current player has lost)
			}

			int oldScore = (this.getParent() == null) ? 0 : this.getParent().getScore();

			if (this._score == oldScore)		//actual this._score = old this._score -> draw
				this._value = PNSValue.unknown;
			else if (isGote)
				this._value = (this._score > oldScore) ? PNSValue.proven : PNSValue.disproven;	//if actual this._score > ol this._score -> win ;; else loss 
			else
				this._value = (this._score > oldScore) ? PNSValue.disproven : PNSValue.proven;  //if actual this._score < ol this._score -> win ;; else loss
		}

		public bool stillAlive (bool isGote)
		{
			int king = isGote ? Token.KING : Token.JEWEL;

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
					if (this._state [i][j] == king)
						return true;
			}

			return false;
		}

		private bool getKingPositions (ref int yKing, ref int xKing, bool isGote)
		{
			int king = isGote ? Token.KING : Token.JEWEL;

			for (int i = 0; i < 9; i++)
			{
				for (int j = 0; j < 9; j++)
				{
					if (this._state [i][j] == king)
					{
						yKing = i;
						xKing = j;
						return true;
					}
				}
			}
			return false;
		}

		private int getPawnDistance (int yPawn, int xPawn, int yKing, int xKing, bool isGote)
		{
			if ((isGote && yPawn > yKing) || (!isGote && yPawn < yKing) || (xPawn != xKing) || (Math.Abs (yKing - yPawn) == 8))
				return 0;

			if (Math.Abs (yKing - yPawn) == 2)
				return 360;

			return 320 - (40 * Math.Abs (yKing - yPawn));
		}

		private int getLanceDistance (int yLance, int xLance, int yKing, int xKing, bool isGote)
		{
			if ((isGote && yLance >= yKing) || (!isGote && yLance <= yKing) || (xLance != xKing))
				return 0;

			if (Math.Abs (yKing - yLance) > 2 || Math.Abs (yKing - yLance) < 2)
				return 200 - (25 * Math.Abs (yKing - yLance));
			else if ((isGote && this._state[yKing - 1][xKing] == 0) || (!isGote && this._state[yKing + 1][xKing] == 0))		// there are no token between the lance and the king
				return 225;

			return 150;		// 200 - 25 * 2
		}

		private int getKnightDistance (int yKnight, int xKnight, int yKing, int xKing, bool isGote)
		{
			if ((isGote && yKnight >= yKing) || (!isGote && yKnight <= yKing))
				return 0;

			if (Math.Abs (yKing - yKnight) > 2 || Math.Abs (yKing - yKnight) < 2)
				return 200 - (25 * Math.Abs (yKing - yKnight));
			else if (xKing - 1 <= xKnight && xKnight <= xKing + 1)
				return 225;

			return 150;		// 200 - 25 * 2
		}

		private int getSilverGoldDistance (int yToken, int xToken, int yKing, int xKing, bool isGote)
		{
			if (Math.Abs (yKing - yToken) > 2 || Math.Abs (yKing - yToken) < 2 ||
				Math.Abs (xKing - xToken) > 2 || Math.Abs (xKing - xToken) < 2)
				return 200 - (25 * Math.Max (Math.Abs (yKing - yToken), Math.Abs (xKing - xToken)));

			return 225;
		}

		private int getBishopDistance (int yBishop, int xBishop, int yKing, int xKing)
		{
			int deltaY = Math.Abs (yKing - yBishop);
			int deltaX = Math.Abs (xKing - xBishop);
			bool obstacle = false;
			int bonus = 0;
			int xDir;
			int x;
			int y;

			if (yKing == yBishop || xKing == xBishop || deltaY != deltaX)		// this bishop is not in the king's diagonal
				return 0;

			if (yKing > yBishop)
			{
				x = xBishop + 1;
				xDir = xKing > xBishop ? 1 : -1;
				for (y = yBishop + 1; x + xDir < xKing && y + 1 < yKing; x += xDir, y++)
				{
					if (this._state [y][x] != 0)
					{
						obstacle = true;
						break;
					}
				}
				if (deltaX >= 2)
					bonus = 200;
			}
			else
			{
				x = xBishop + 1;
				xDir = xKing > xBishop ? 1 : -1;
				for (y = yBishop + 1; x + xDir < xKing && y + 1 < yKing; x += xDir, y++)
				{
					if (this._state [y][x] != 0)
					{
						obstacle = true;
						break;
					}
				}
				if (deltaX >= 2)
					bonus = 200;
			}

			return obstacle ? 100 + bonus : 400 + bonus;
		}

		private int getRookDistance (int yRook, int xRook, int yKing, int xKing)
		{
			bool obstacle = false;
			int bonus = 0;
			int xDir;
			int yDir;
			int x;
			int y;

			if (yKing != yRook && xKing != xRook)
				return 0;

			if (yKing == yRook)
			{
				xDir = xKing > xRook ? 1 : -1;
				for (x = xRook + 1; x + xDir < xKing; x += xDir)
				{
					if (this._state [yKing][x] != 0)
					{
						obstacle = true;
						break;
					}
				}
				if (Math.Abs (xKing - xRook) == 2)
					bonus = 300;
			}
			else		// if yKing != yRook then xKing == xRook (first check of the function)
			{
				yDir = yKing > yRook ? 1 : -1;
				for (y = yRook + 1; y + yDir < yKing; y += yDir)
				{
					if (this._state [y][xKing] != 0)
					{
						obstacle = true;
						break;
					}
				}
				if (Math.Abs (yKing - yRook) == 2)
					bonus = 300;
			}

			return obstacle ? 100 + bonus : 400 + bonus;
		}

		public void createChildren (bool isGote, bool canDrop, bool sort)
		{
			int i, j;

			this._children.Clear ();
			if (!this.stillAlive (isGote))
				return;

			for (i = 0; i < 9; i++)
			{
				for (j = 0; j < 9; j++)
				{
					if (this._state [i][j] != 0)
					{
						switch (this._state [i][j])
						{
							case Token.GPAWN:
							case Token.SPAWN:
								this.movePawn (i, j, isGote);
								break;
							case Token.P_GPAWN:
							case Token.P_SPAWN:
								this.moveWithGoldPatern (i, j, Token.TokenType.PROMOTED_PAWN, isGote);
								break;
							case  Token.GLANCE:
							case  Token.SLANCE:
								this.moveLance (i, j, isGote);
								break;
							case  Token.P_GLANCE:
							case  Token.P_SLANCE:
								this.moveWithGoldPatern (i, j, Token.TokenType.PROMOTED_LANCE, isGote);
								break;
							case  Token.GKNIGHT:
							case  Token.SKNIGHT:
								this.moveKnight (i, j, isGote);
								break;
							case  Token.P_GKNIGHT:
							case  Token.P_SKNIGHT:
								this.moveWithGoldPatern (i, j, Token.TokenType.PROMOTED_KNIGHT, isGote);
								break;
							case  Token.GSILVER:
							case  Token.SSILVER:
								this.moveSilver (i, j, isGote);
								break;
							case  Token.P_GSILVER:
							case  Token.P_SSILVER:
								this.moveWithGoldPatern (i, j, Token.TokenType.PROMOTED_SILVER, isGote);
								break;
							case  Token.GGOLD:
							case  Token.SGOLD:
								this.moveWithGoldPatern (i, j, Token.TokenType.GOLD, isGote);
								break;
							case  Token.GBISHOP:
							case  Token.SBISHOP:
								this.moveBishop (i, j, isGote);
								break;
							case  Token.P_GBISHOP:
							case  Token.P_SBISHOP:
								this.movePromotedBishop (i, j, isGote);
								break;
							case  Token.GROOK:
							case  Token.SROOK:
								this.moveRook (i, j, isGote);
								break;
							case  Token.P_GROOK:
							case  Token.P_SROOK:
								this.movePromotedRook (i, j, isGote);
								break;
							case  Token.KING:
							case  Token.JEWEL:
								this.moveKing (i, j, isGote);
								break;
							default:
								return;
						}
					}
				}
			}

			if (canDrop)
				this.drop (isGote);

			if (sort)
				this.sortChildren (isGote, canDrop);
		}

		public void createAllChildren (bool canDrop, bool sort)
		{
			List <Node> allChildren = new List<Node>();

			this.createChildren (true /* isGote */, canDrop, sort);
			allChildren.AddRange (this._children);
			this.createChildren (false /* isGote */, canDrop, sort);
			allChildren.AddRange (this._children);
			this._children.Clear ();
			this._children.AddRange (allChildren);
			allChildren.Clear ();
		}

		public void sortChildren (bool isGote, bool canDrop)
		{
			int i, j;
			Node switchNode;

			for (i = 0; i < this._children.Count; i++)
			{
				this._children[i].createChildren (!isGote, canDrop, false /* sort */);
				if (this._children [i].getChildren ().Count == 0)
					this._children [i].setChildrenThreat (this._children[i].stillAlive (isGote) ? -999999999 : 999999999);
				else
				{
					this._children [i].calculateChildrenThreat (!isGote);
					this._children [i].clearChildren ();
				}
			}

			for (i = 0; i < this._children.Count; i++)
			{
				for (j = this._children.Count - 1; j > 0; j--)
				{
					if (this._children[j - 1].getChildrenThreat() > this._children[j].getChildrenThreat())
					{
						switchNode = this._children[j];
						this._children[j] = this._children[j - 1];
						this._children[j - 1] = switchNode;
					}
				}
			}
		}

		private void drop (bool isGote)
		{
			List<Token.TokenType> playerCapturedTokens;
			List<Token.TokenType> playerNewCapturedTokens;
			List<Token.TokenType> enemyCapturedTokens;
			bool alreadyTaken;
			int [][] nextState = new int[9][];
			for (int i = 0; i < 9; i++)
				nextState [i] = new int[9];
			Token.TokenInfo token = new Token.TokenInfo();

			if (isGote)
			{
				playerCapturedTokens = new List<Token.TokenType> (this._goteCapturedTokens);
				enemyCapturedTokens = new List<Token.TokenType> (this._senteCapturedTokens);
			}
			else
			{
				playerCapturedTokens = new List<Token.TokenType> (this._senteCapturedTokens);
				enemyCapturedTokens = new List<Token.TokenType> (this._goteCapturedTokens);
			}

			for (int i = 0; i < playerCapturedTokens.Count; i++)
			{
				for (int y = 0; y < 9; y++)
				{
					if ((playerCapturedTokens [i] == Token.TokenType.PAWN) ||
						(playerCapturedTokens [i] == Token.TokenType.LANCE))
					{
						if ((isGote && y == 8) || (!isGote && y == 0))
							continue;
					}
					else if (playerCapturedTokens [i] == Token.TokenType.KNIGHT)
					{
						if ((isGote && y >= 7) || (!isGote && y <= 1))
							continue;
					}

					for (int x = 0; x < 9; x++)
					{
						if (this._state[y][x] == 0)
						{
							if (playerCapturedTokens [i] == Token.TokenType.PAWN)
							{
								alreadyTaken = false;

								for (int y_prime = 0; y_prime < 9; y_prime++)
								{
									if (this._state [y_prime][x] == (isGote ? Token.GPAWN : Token.SPAWN))
									{
										alreadyTaken = true;
										break;
									}
								}

								if (alreadyTaken)
									continue;
							}

							token.Clear ();

							token.tokenType = playerCapturedTokens[i];
							token.droppedPiece = true;
							token.newX = x;
							token.newY = y;
							token.newValue = Token.getTokenValue (token.tokenType, isGote);

							for (int a = 0; a < 9; a++)
								for (int b = 0; b < 9; b++)
									nextState [a][b] = 0;

							for (int a = 0; a < 9; a++)
								for (int b = 0; b < 9; b++)
									nextState [a] [b] = this._state [a] [b];
							
							playerNewCapturedTokens = new List<Token.TokenType> (playerCapturedTokens);
							playerNewCapturedTokens.RemoveAt (i);

							if (isGote)
								this._children.Add (new Node (!isGote ,this ,token, playerNewCapturedTokens, enemyCapturedTokens, new List<Node> (), ref nextState));
							else
								this._children.Add (new Node (!isGote ,this, token, enemyCapturedTokens, playerNewCapturedTokens, new List<Node> (), ref nextState));
						}
					}
				}
			}

			playerCapturedTokens.Clear ();
			enemyCapturedTokens.Clear ();
		}

		private void moveEmptyCase (int oldX, int oldY, int newX, int newY, Token.TokenType tokenType, bool promoted, int newValue)
		{
			int [][] nextState = new int[9][];
			for (int i = 0; i < 9; i++)
				nextState [i] = new int[9];
			Token.TokenInfo token = new Token.TokenInfo();

			token.Clear ();

			token.tokenType = tokenType;
			token.oldX = oldX;
			token.oldY = oldY;
			token.oldValue = this._state[oldY][oldX];
			token.newX = newX;
			token.newY = newY;
			token.promoted = promoted;
			token.newValue = newValue;
			token.capturedSomething = false;
			token.capturedType = Token.TokenType.UNDEFINED;
			token.capturedValue = 0;

			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					nextState [i] [j] = this._state [i] [j];
			
			nextState [newY][newX] = token.newValue;
			nextState [oldY][oldX] = 0;
			this._children.Add (new Node (!this._isGote ,this, token, this._goteCapturedTokens, this._senteCapturedTokens, new List<Node> (), ref nextState));
		}

		private void moveBusyCase (int oldX, int oldY, int newX, int newY, Token.TokenType tokenType, bool promoted, int newValue, bool isGote)
		{
			List<Token.TokenType> goteCapturedTokens;
			List<Token.TokenType> senteCapturedTokens;
			int [][] nextState = new int[9][];
			for (int i = 0; i < 9; i++)
				nextState [i] = new int[9];

			Token.TokenInfo token = new Token.TokenInfo();

			token.Clear ();

			token.tokenType = tokenType;
			token.oldX = oldX;
			token.oldY = oldY;
			token.oldValue = this._state[oldY][oldX];
			token.newX = newX;
			token.newY = newY;
			token.promoted = promoted;
			token.newValue = newValue;
			token.capturedSomething = true;
			token.capturedType = Token.getUnpromotedTokenType (this._state [newY][newX]);
			token.capturedValue = this._state [newY][newX];

			goteCapturedTokens = new List<Token.TokenType> (this._goteCapturedTokens);
			senteCapturedTokens = new List<Token.TokenType> (this._senteCapturedTokens);

			if (isGote)
				goteCapturedTokens.Add (token.capturedType);
			else
				senteCapturedTokens.Add (token.capturedType);

			for (int i = 0; i < 9; i++)
				for (int j = 0; j < 9; j++)
					nextState [i] [j] = this._state [i] [j];
			nextState [newY][newX] = token.newValue;
			nextState [oldY][oldX] = 0;
			this._children.Add (new Node (!isGote ,this, token, goteCapturedTokens, senteCapturedTokens, new List<Node> (), ref nextState));
		}

		public void movePawn (int y, int x, bool isGote)
		{
			int newY = isGote ? y + 1 : y - 1;

			if ((isGote && (this._state[y][x] <= 0 || y == 8 || this._state [newY][x] > 0)) ||
				(!isGote && (this._state[y][x] >= 0 || y == 0 || this._state [newY][x] < 0)))		// you can't move the enemy tokens + you can't move if you reached the last line (should never occur) + you can't move if the case is occupied by an ally
				return;

			if (this._state [newY][x] == 0)		// move to an empty case
			{
				if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted pawn in the last line
					moveEmptyCase (x, y, x, newY, Token.TokenType.PAWN, false /* promoted */, Token.getTokenValue (Token.TokenType.PAWN, isGote));
				if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
					moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_PAWN, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_PAWN, isGote));
			}

			if ((isGote && this._state [newY][x] < 0) || (!isGote && this._state [newY][x] > 0))		// move to a busy case
			{
				if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted pawn in the last line
					moveBusyCase (x, y, x, newY, Token.TokenType.PAWN, false /* promoted */, Token.getTokenValue (Token.TokenType.PAWN, isGote), isGote);
				if ((isGote && newY >= 6) || (!isGote && newY <= 2))// promotion zone
					moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_PAWN, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_PAWN, isGote), isGote);
			}
		}

		public void moveLance (int y, int x, bool isGote)
		{
			int newY = isGote ? y + 1 : y - 1;

			if ((isGote && (this._state[y][x] <= 0 || y == 8 || this._state [newY][x] > 0)) ||
				(!isGote && (this._state[y][x] >= 0 || y == 0 || this._state [newY][x] < 0)))		// you can't move the enemy tokens + you can't move if you reached the last line (should never occur) + you can't move if the case is occupied by an ally
				return;

			if (this._state [newY][x] == 0)		// move to an empty case
			{
				if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted lance in the last line
					moveEmptyCase (x, y, x, newY, Token.TokenType.LANCE, false /* promoted */, Token.getTokenValue (Token.TokenType.LANCE, isGote));

				if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
					moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_LANCE, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_LANCE, isGote));
			}
			else if ((isGote && this._state [newY][x] < 0) || (!isGote && this._state [newY][x] > 0))		// move to a busy case
			{
				if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted lance in the last line
					moveBusyCase (x, y, x, newY, Token.TokenType.LANCE, false /* promoted */, Token.getTokenValue (Token.TokenType.LANCE, isGote), isGote);

				if ((isGote && newY >= 6) || (!isGote && newY <= 2))// promotion zone
					moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_LANCE, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_LANCE, isGote), isGote);
			}

			if ((isGote && y == 7) || (!isGote && y == 1) || (this._state [newY][x] != 0))		// you can't make a jump of 2 cases if you don't have 2 cases xD + something is in front of you so you can't make a jump of 2 cases 
				return;

			newY = isGote ? newY + 1 : newY - 1;

			if (this._state [newY][x] == 0)		// move to an empty case
			{
				if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted lance in the last line
					moveEmptyCase (x, y, x, newY, Token.TokenType.LANCE, false /* promoted */, Token.getTokenValue (Token.TokenType.LANCE, isGote));

				if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
					moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_LANCE, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_LANCE, isGote));
			}
			else if ((isGote && this._state [newY][x] < 0) || (!isGote && this._state [newY][x] > 0))		// move to a busy case
			{
				if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted lance in the last line
					moveBusyCase (x, y, x, newY, Token.TokenType.LANCE, false /* promoted */, Token.getTokenValue (Token.TokenType.LANCE, isGote), isGote);

				if ((isGote && newY >= 6) || (!isGote && newY <= 2))// promotion zone
					moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_LANCE, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_LANCE, isGote), isGote);
			}
		}

		public void moveKnight (int y, int x, bool isGote)
		{
			int newY = isGote ? y + 2 : y - 2;
			int newX;

			if ((isGote && (this._state [y][x] <= 0 || y >= 7)) ||
				(!isGote && (this._state [y][x] >= 0 || y <= 1)))		// you can't move the enemy tokens + you can't move if you reached the last 2 lines
				return;

			newX = x - 1;
			if (newX >= 0)
			{
				if (this._state [newY][newX] == 0)		// move to an empty case
				{
					if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted knight in the last line
						moveEmptyCase (x, y, newX, newY, Token.TokenType.KNIGHT, false /* promoted */, Token.getTokenValue (Token.TokenType.KNIGHT, isGote));

					if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_KNIGHT, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_KNIGHT, isGote));
				}

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))		// move to a busy case
				{
					if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted knight in the last line
						moveBusyCase (x, y, newX, newY, Token.TokenType.KNIGHT, false /* promoted */, Token.getTokenValue (Token.TokenType.KNIGHT, isGote), isGote);

					if ((isGote && newY >= 6) || (!isGote && newY <= 2))// promotion zone
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_KNIGHT, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_KNIGHT, isGote), isGote);
				}
			}

			newX = x + 1;
			if (newX <= 8)
			{
				if (this._state [newY][newX] == 0)		// move to an empty case
				{
					if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted knight in the last line
						moveEmptyCase (x, y, newX, newY, Token.TokenType.KNIGHT, false /* promoted */, Token.getTokenValue (Token.TokenType.KNIGHT, isGote));

					if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_KNIGHT, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_KNIGHT, isGote));
				}

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))		// move to a busy case
				{
					if ((isGote && newY < 8) || (!isGote && newY > 0))		// we can't let an unpromoted knight in the last line
						moveBusyCase (x, y, newX, newY, Token.TokenType.KNIGHT, false /* promoted */, Token.getTokenValue (Token.TokenType.KNIGHT, isGote), isGote);

					if ((isGote && newY >= 6) || (!isGote && newY <= 2))// promotion zone
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_KNIGHT, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_KNIGHT, isGote), isGote);
				}
			}
		}

		public void moveSilver (int y, int x, bool isGote)
		{
			int newX, newY;

			if ((isGote && this._state[y][x] <= 0) || (!isGote && this._state[y][x] >= 0))		// you can't move enemy tokens
				return;

			newY = y - 1;
			if (y > 0)
			{
				newX = x - 1;
				if ((x > 0) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
					{
						moveEmptyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote));
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote));
					}
					else		// move to an busy case
					{
						moveBusyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote), isGote);
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote), isGote);
					}
				}

				if (!isGote && this._state [newY][x] >= 0)
				{
					if (this._state [newY][x] == 0)		// move to an empty case
					{
						moveEmptyCase (x, y, x, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote));
						if (newY <= 2)		// promotion zone
							moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote));
					}
					else		// move to an busy case
					{
						moveBusyCase (x, y, x, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote), isGote);
						if (newY <= 2)		// promotion zone
							moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote), isGote);
					}
				}

				newX = x + 1;
				if ((x < 8) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
					{
						moveEmptyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote));
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote));
					}
					else		// move to an busy case
					{
						moveBusyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote), isGote);
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote), isGote);
					}
				}
			}

			newY = y + 1;
			if (y < 8)
			{
				newX = x - 1;
				if ((x > 0) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
					{
						moveEmptyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote));
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote));
					}
					else		// move to an busy case
					{
						moveBusyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote), isGote);
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote), isGote);
					}
				}

				if (isGote && this._state [newY][x] <= 0)
				{
					if (this._state [newY][x] == 0)		// move to an empty case
					{
						moveEmptyCase (x, y, x, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote));
						if (newY >= 6)		// promotion zone
							moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote));
					}
					else		// move to an busy case
					{
						moveBusyCase (x, y, x, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote), isGote);
						if (newY >= 6)		// promotion zone
							moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote), isGote);
					}
				}

				newX = x + 1;
				if ((x < 8) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
					{
						moveEmptyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote));
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote));
					}
					else		// move to an busy case
					{
						moveBusyCase (x, y, newX, newY, Token.TokenType.SILVER, false /* promoted */, Token.getTokenValue (Token.TokenType.SILVER, isGote), isGote);
						if ((isGote && newY >= 6) || (!isGote && newY <= 2))		// promotion zone
							moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_SILVER, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_SILVER, isGote), isGote);
					}
				}
			}
		}

		public void moveWithGoldPatern (int y, int x, Token.TokenType tokenType, bool isGote)
		{
			int newX, newY;

			if ((isGote && this._state[y][x] <= 0) || (!isGote && this._state[y][x] >= 0))		// you can't move enemy tokens
				return;

			newY = y - 1;
			if ((y > 0) && ((isGote && this._state [newY][x] <= 0) || (!isGote && this._state [newY][x] >= 0)))
			{
				if (this._state [newY][x] == 0)		// move to an empty case
					moveEmptyCase (x, y, x, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, x, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote), isGote);
			}

			newY = y + 1;
			if ((y < 8) && ((isGote && this._state [newY][x] <= 0) || (!isGote && this._state [newY][x] >= 0)))
			{
				if (this._state [newY][x] == 0)		// move to an empty case
					moveEmptyCase (x, y, x, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, x, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote), isGote);
			}

			newX = x - 1;
			if ((x > 0) && ((isGote && this._state [y][newX] <= 0) || (!isGote && this._state [y][newX] >= 0)))
			{
				if (this._state [y][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, y, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, newX, y, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote), isGote);
			}

			newX = x + 1;
			if ((x < 8) && ((isGote && this._state [y][newX] <= 0) || (!isGote && this._state [y][newX] >= 0)))
			{
				if (this._state [y][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, y, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, newX, y, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote), isGote);
			}

			newY = isGote ? y + 1 : y - 1;

			newX = x - 1;
			if ((newY >= 0) && (newY <= 8) && (x > 0) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
			{
				if (this._state [newY][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, newX, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote), isGote);
			}

			newX = x + 1;
			if ((newY >= 0) && (newY <= 8) && (x < 8) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
			{
				if (this._state [newY][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, newX, newY, tokenType, false /* promoted */, Token.getTokenValue (tokenType, isGote), isGote);
			}
		}

		public void moveBishop (int y, int x, bool isGote)
		{
			int newX, newY;

			if ((isGote && this._state[y][x] <= 0) || (!isGote && this._state[y][x] >= 0))		// you can't move enemy tokens
				return;

			/* Top Right */

			newY = y + 1;
			for (newX = x + 1; newY < 9 && newX < 9; newY++, newX++)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote));
					if ((isGote && newY >= 6) || (!isGote && newY <= 2))	// promotion zone
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
				}

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote), isGote);
					if ((isGote && newY >= 6) || (!isGote && newY <= 2)) // promotion zone
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}

			/* Top Left */

			newY = y + 1;
			for (newX = x - 1; newY < 9 && newX >= 0; newY++, newX--)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote));
					if ((isGote && newY >= 6) || (!isGote && newY <= 2))	// promotion zone
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
				}

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote), isGote);
					if ((isGote && newY >= 6) || (!isGote && newY <= 2)) // promotion zone
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}

			/* Bottom Right */

			newY = y - 1;
			for (newX = x + 1; newY >= 0 && newX < 9; newY--, newX++)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote));
					if ((isGote && newY >= 6) || (!isGote && newY <= 2))	// promotion zone
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
				}

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote), isGote);
					if ((isGote && newY >= 6) || (!isGote && newY <= 2)) // promotion zone
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}

			/* Bottom Left */

			newY = y - 1;
			for (newX = x - 1; newY >= 0 && newX >= 0; newY--, newX--)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote));
					if ((isGote && newY >= 6) || (!isGote && newY <= 2))	// promotion zone
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
				}

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.BISHOP, isGote), isGote);
					if ((isGote && newY >= 6) || (!isGote && newY <= 2)) // promotion zone
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}
		}

		public void movePromotedBishop (int y, int x, bool isGote)
		{
			int newX, newY;

			if ((isGote && this._state[y][x] <= 0) || (!isGote && this._state[y][x] >= 0))		// you can't move enemy tokens
				return;

			if (y > 0)
			{
				newY = y - 1;
				if ((isGote && this._state [newY][x] <= 0) || (!isGote && this._state [newY][x] >= 0))
				{
					if (this._state [newY][x] == 0)		// move to an empty case
						moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
					else
						moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
				}
			}

			if (y < 8)
			{
				newY = y + 1;
				if ((isGote && this._state [newY][x] <= 0) || (!isGote && this._state [newY][x] >= 0))
				{
					if (this._state [newY][x] == 0)		// move to an empty case
						moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
					else
						moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
				}
			}

			if (x > 0)
			{
				newX = x - 1;
				if ((isGote && this._state [y][newX] <= 0) || (!isGote && this._state [y][newX] >= 0))
				{
					if (this._state [y][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, y, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
					else
						moveBusyCase (x, y, newX, y, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
				}
			}

			if (x < 8)
			{
				newX = x + 1;
				if ((isGote && this._state [y][newX] <= 0) || (!isGote && this._state [y][newX] >= 0))
				{
					if (this._state [y][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, y, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));
					else
						moveBusyCase (x, y, newX, y, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
				}
			}

			/* Top Right */

			newY = y + 1;
			for (newX = x + 1; newY < 9 && newX < 9; newY++, newX++)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}

			/* Top Left */

			newY = y + 1;
			for (newX = x - 1; newY < 9 && newX >= 0; newY++, newX--)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}

			/* Bottom Right */

			newY = y - 1;
			for (newX = x + 1; newY >= 0 && newX < 9; newY--, newX++)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}

			/* Bottom Left */

			newY = y - 1;
			for (newX = x - 1; newY >= 0 && newX >= 0; newY--, newX--)
			{
				if ((isGote && this._state [newY][newX] > 0) || (!isGote && this._state [newY][newX] < 0))
					break;		// case busy with ally

				if (this._state [newY][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote));

				if ((isGote && this._state [newY][newX] < 0) || (!isGote && this._state [newY][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_BISHOP, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_BISHOP, isGote), isGote);
					break;
				}
			}
		}

		public void moveRook (int y, int x, bool isGote)
		{
			int newY, newX;

			if ((isGote && this._state[y][x] <= 0) || (!isGote && this._state[y][x] >= 0))		// you can't move enemy tokens
				return;

			/* Top */

			for (newY = y + 1; newY < 9; newY++)
			{
				if ((isGote && this._state [newY][x] > 0) || (!isGote && this._state [newY][x] < 0))
					break;		// case busy with ally

				if (this._state [newY][x] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, x, newY, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote));
					if ((isGote && newY >= 6) || (!isGote && newY <= 2))	// promotion zone
						moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
				}

				if ((isGote && this._state [newY][x] < 0) || (!isGote && this._state [newY][x] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, x, newY, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote), isGote);
					if ((isGote && newY >= 6) || (!isGote && newY <= 2)) // promotion zone
						moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}

			/* Bot */

			for (newY = y - 1; newY >= 0; newY--)
			{
				if ((isGote && this._state [newY][x] > 0) || (!isGote && this._state [newY][x] < 0))
					break;		// case busy with ally

				if (this._state [newY][x] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, x, newY, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote));
					if ((isGote && newY >= 6) || (!isGote && newY <= 2))	// promotion zone
						moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
				}

				if ((isGote && this._state [newY][x] < 0) || (!isGote && this._state [newY][x] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, x, newY, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote), isGote);
					if ((isGote && newY >= 6) || (!isGote && newY <= 2)) // promotion zone
						moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}

			/* Right */

			for (newX = x + 1; newX < 9; newX++)
			{
				if ((isGote && this._state [y][newX] > 0) || (!isGote && this._state [y][newX] < 0))
					break;		// case busy with ally

				if (this._state [y][newX] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, newX, y, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote));
					if ((isGote && y >= 6) || (!isGote && y <= 2))	// promotion zone
						moveEmptyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
				}

				if ((isGote && this._state [y][newX] < 0) || (!isGote && this._state [y][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, y, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote), isGote);
					if ((isGote && y >= 6) || (!isGote && y <= 2)) // promotion zone
						moveBusyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}

			/* Left */

			for (newX = x - 1; newX >= 0; newX--)
			{
				if ((isGote && this._state [y][newX] > 0) || (!isGote && this._state [y][newX] < 0))
					break;		// case busy with ally

				if (this._state [y][newX] == 0)		// move to an empty case
				{
					moveEmptyCase (x, y, newX, y, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote));
					if ((isGote && y >= 6) || (!isGote && y <= 2))	// promotion zone
						moveEmptyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
				}

				if ((isGote && this._state [y][newX] < 0) || (!isGote && this._state [y][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, y, Token.TokenType.ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.ROOK, isGote), isGote);
					if ((isGote && y >= 6) || (!isGote && y <= 2)) // promotion zone
						moveBusyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, true /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}
		}

		public void movePromotedRook (int y, int x, bool isGote)
		{
			int newY, newX;

			if ((isGote && this._state[y][x] <= 0) || (!isGote && this._state[y][x] >= 0))		// you can't move enemy tokens
				return;

			if (y > 0 && x > 0)
			{
				newY = y - 1;
				newX = x - 1;
				if ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
					else
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
				}
			}

			if (y > 0 && x < 8)
			{
				newY = y - 1;
				newX = x + 1;
				if ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
					else
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
				}
			}

			if (y < 8 && x > 0)
			{
				newY = y + 1;
				newX = x - 1;
				if ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
					else
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
				}
			}

			if (y < 8 && x < 8)
			{
				newY = y + 1;
				newX = x + 1;
				if ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));
					else
						moveBusyCase (x, y, newX, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
				}
			}

			/* Top */

			for (newY = y + 1; newY < 9; newY++)
			{
				if ((isGote && this._state [newY][x] > 0) || (!isGote && this._state [newY][x] < 0))
					break;		// case busy with ally

				if (this._state [newY][x] == 0)		// move to an empty case
					moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));

				if ((isGote && this._state [newY][x] < 0) || (!isGote && this._state [newY][x] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}

			/* Bot */

			for (newY = y - 1; newY >= 0; newY--)
			{
				if ((isGote && this._state [newY][x] > 0) || (!isGote && this._state [newY][x] < 0))
					break;		// case busy with ally

				if (this._state [newY][x] == 0)		// move to an empty case
					moveEmptyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));

				if ((isGote && this._state [newY][x] < 0) || (!isGote && this._state [newY][x] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, x, newY, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}

			/* Right */

			for (newX = x + 1; newX < 9; newX++)
			{
				if ((isGote && this._state [y][newX] > 0) || (!isGote && this._state [y][newX] < 0))
					break;		// case busy with ally

				if (this._state [y][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));

				if ((isGote && this._state [y][newX] < 0) || (!isGote && this._state [y][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}

			/* Left */

			for (newX = x - 1; newX >= 0; newX--)
			{
				if ((isGote && this._state [y][newX] > 0) || (!isGote && this._state [y][newX] < 0))
					break;		// case busy with ally

				if (this._state [y][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote));

				if ((isGote && this._state [y][newX] < 0) || (!isGote && this._state [y][newX] > 0))	// move to a busy case
				{
					moveBusyCase (x, y, newX, y, Token.TokenType.PROMOTED_ROOK, false /* promoted */, Token.getTokenValue (Token.TokenType.PROMOTED_ROOK, isGote), isGote);
					break;
				}
			}
		}

		public void moveKing (int y, int x, bool isGote)
		{
			int newY, newX;

			if ((isGote && this._state[y][x] <= 0) || (!isGote && this._state[y][x] >= 0))		// you can't move enemy tokens
				return;

			newY = y - 1;
			if (y > 0)
			{
				newX = x - 1;
				if ((x > 0) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
					else		// move to an busy case
						moveBusyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
				}

				if ((isGote && this._state [newY][x] <= 0) || (!isGote && this._state [newY][x] >= 0))
				{
					if (this._state [newY][x] == 0)		// move to an empty case
						moveEmptyCase (x, y, x, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
					else		// move to an busy case
						moveBusyCase (x, y, x, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
				}

				newX = x + 1;
				if ((x < 8) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
					else		// move to an busy case
						moveBusyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
				}
			}

			newX = x - 1;
			if ((x > 0) && ((isGote && this._state [y][newX] <= 0) || (!isGote && this._state [y][newX] >= 0)))
			{
				if (this._state [y][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, y, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, newX, y, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
			}

			newX = x + 1;
			if ((x < 8) && ((isGote && this._state [y][newX] <= 0) || (!isGote && this._state [y][newX] >= 0)))
			{
				if (this._state [y][newX] == 0)		// move to an empty case
					moveEmptyCase (x, y, newX, y, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
				else		// move to an busy case
					moveBusyCase (x, y, newX, y, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
			}

			newY = y + 1;
			if (y < 8)
			{
				newX = x - 1;
				if ((x > 0) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
					else		// move to an busy case
						moveBusyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
				}

				if ((isGote && this._state [newY][x] <= 0) || (!isGote && this._state [newY][x] >= 0))
				{
					if (this._state [newY][x] == 0)		// move to an empty case
						moveEmptyCase (x, y, x, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
					else		// move to an busy case
						moveBusyCase (x, y, x, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
				}

				newX = x + 1;
				if ((x < 8) && ((isGote && this._state [newY][newX] <= 0) || (!isGote && this._state [newY][newX] >= 0)))
				{
					if (this._state [newY][newX] == 0)		// move to an empty case
						moveEmptyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote));
					else		// move to an busy case
						moveBusyCase (x, y, newX, newY, Token.TokenType.KING, false /* promoted */, Token.getTokenValue (Token.TokenType.KING, isGote), isGote);
				}
			}
		}
	}
}

