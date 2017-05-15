using System;
using System.IO;
using System.Text;
using System.Collections.Generic;

namespace Tools
{
	public class Token
	{
		// GOTE TOKENS
		public const int GPAWN = 100;
		public const int P_GPAWN = 420;
		public const int GLANCE = 430;
		public const int P_GLANCE = 630;
		public const int GKNIGHT = 450;
		public const int P_GKNIGHT = 650;
		public const int GSILVER = 640;
		public const int P_GSILVER = 670;
		public const int GGOLD = 690;
		public const int GBISHOP = 890;
		public const int P_GBISHOP = 1150;
		public const int GROOK = 1040;
		public const int P_GROOK = 1300;
		public const int KING = 4720;

		// SENTE TOKEN
		public const int SPAWN = -100;
		public const int P_SPAWN = -420;
		public const int SLANCE = -430;
		public const int P_SLANCE = -630;
		public const int SKNIGHT = -450;
		public const int P_SKNIGHT = -650;
		public const int SSILVER = -640;
		public const int P_SSILVER = -670;
		public const int SGOLD = -690;
		public const int C_SGOLD = -780;
		public const int SBISHOP = -890;
		public const int P_SBISHOP = -1150;
		public const int SROOK = -1040;
		public const int P_SROOK = -1300;
		public const int JEWEL = -4720;

		// CAPTURED TOKENS
//		public const int C_PAWN = 85;
		public const int C_PAWN = 100;
//		public const int C_PAWN = 115;

//		public const int C_LANCE = 380;
		public const int C_LANCE = 430;
//		public const int C_LANCE = 480;

//		public const int C_KNIGHT = 390;
		public const int C_KNIGHT = 450;
//		public const int C_KNIGHT = 510;

//		public const int C_SILVER = 560;
		public const int C_SILVER = 640;
//		public const int C_SILVER = 720;

//		public const int C_GOLD = 600;
		public const int C_GOLD = 690;
//		public const int C_GOLD = 780;

//		public const int C_BISHOP = 670;
		public const int C_BISHOP = 890;
//		public const int C_BISHOP = 1110;

//		public const int C_ROOK = 810;
		public const int C_ROOK = 1040;
//		public const int C_ROOK = 1270;

		public enum TokenType
		{
			PAWN, PROMOTED_PAWN, LANCE, PROMOTED_LANCE, KNIGHT, PROMOTED_KNIGHT, SILVER, PROMOTED_SILVER, GOLD, BISHOP, PROMOTED_BISHOP, ROOK, PROMOTED_ROOK, KING, UNDEFINED
		};

		public static int getCapturedTokenValue (TokenType token)
		{
			switch (token)
			{
				case TokenType.PAWN:
				case TokenType.PROMOTED_PAWN:
					return C_PAWN;
				case TokenType.LANCE:
				case TokenType.PROMOTED_LANCE:
					return C_LANCE;
				case TokenType.KNIGHT:
				case TokenType.PROMOTED_KNIGHT:
					return C_KNIGHT;
				case TokenType.SILVER:
				case TokenType.PROMOTED_SILVER:
					return C_SILVER;
				case TokenType.GOLD:
					return C_GOLD;
				case TokenType.BISHOP:
				case TokenType.PROMOTED_BISHOP:
					return C_BISHOP;
				case TokenType.ROOK:
				case TokenType.PROMOTED_ROOK:
					return C_ROOK;
				default:
					return 0;
			}
		}

		public static bool isAPromotableToken (TokenType token)
		{
			switch (token)
			{
				case TokenType.PAWN:
				case TokenType.LANCE:
				case TokenType.KNIGHT:
				case TokenType.SILVER:
				case TokenType.BISHOP:
				case TokenType.ROOK:
					return true;
				default:
					return false;
			}
		}

		public static TokenType getPromotedTokenType (TokenType token)
		{
			switch (token)
			{
				case TokenType.PAWN:
					return TokenType.PROMOTED_PAWN;
				case TokenType.LANCE:
					return TokenType.PROMOTED_LANCE;
				case TokenType.KNIGHT:
					return TokenType.PROMOTED_KNIGHT;
				case TokenType.SILVER:
					return TokenType.PROMOTED_SILVER;
				case TokenType.BISHOP:
					return TokenType.PROMOTED_BISHOP;
				case TokenType.ROOK:
					return TokenType.PROMOTED_ROOK;
				default:
					return TokenType.UNDEFINED;
			}
		}

		public static string getTokenTypeString (TokenType token)
		{
			switch (token)
			{
				case TokenType.PAWN:
					return "PAWN";
				case TokenType.PROMOTED_PAWN:
					return "PROMOTED PAWN";
				case TokenType.LANCE:
					return "LANCE";
				case TokenType.PROMOTED_LANCE:
					return "PROMOTED LANCE";
				case TokenType.KNIGHT:
					return "KNIGHT";
				case TokenType.PROMOTED_KNIGHT:
					return "PROMOTED KNIGHT";
				case TokenType.SILVER:
					return "SILVER";
				case TokenType.PROMOTED_SILVER:
					return "PROMOTED SILVER";
				case TokenType.GOLD:
					return "GOLD";
				case TokenType.BISHOP:
					return "BISHOP";
				case TokenType.PROMOTED_BISHOP:
					return "PROMOTED BISHOP";
				case TokenType.ROOK:
					return "ROOK";
				case TokenType.PROMOTED_ROOK:
					return "PROMOTED ROOK";
				case TokenType.KING:
					return "KING";
				default:
					return "";
			}
		}


		public static TokenType getTokenType (int tokenValue)
		{
			switch (tokenValue)
			{
				case GPAWN:
				case SPAWN:
					return TokenType.PAWN;
				case P_GPAWN:
				case P_SPAWN:
					return TokenType.PROMOTED_PAWN;
				case GLANCE:
				case SLANCE:
					return TokenType.LANCE;
				case P_GLANCE:
				case P_SLANCE:
					return TokenType.PROMOTED_LANCE;
				case GKNIGHT:
				case SKNIGHT:
					return TokenType.KNIGHT;
				case P_GKNIGHT:
				case P_SKNIGHT:
					return TokenType.PROMOTED_KNIGHT;
				case GSILVER:
				case SSILVER:
					return TokenType.SILVER;
				case P_GSILVER:
				case P_SSILVER:
					return TokenType.PROMOTED_SILVER;
				case GGOLD:
				case SGOLD:
					return TokenType.GOLD;
				case GBISHOP:
				case SBISHOP:
					return TokenType.BISHOP;
				case P_GBISHOP:
				case P_SBISHOP:
					return TokenType.PROMOTED_BISHOP;
				case GROOK:
				case SROOK:
					return TokenType.ROOK;
				case P_GROOK:
				case P_SROOK:
					return TokenType.PROMOTED_ROOK;
				case KING:
				case JEWEL:
					return TokenType.KING;
				default:
					return TokenType.UNDEFINED;
			}
		}

		public static TokenType getUnpromotedTokenType (int tokenValue)
		{
			switch (tokenValue)
			{
				case GPAWN:
				case SPAWN:
				case P_GPAWN:
				case P_SPAWN:
					return TokenType.PAWN;
				case GLANCE:
				case SLANCE:
				case P_GLANCE:
				case P_SLANCE:
					return TokenType.LANCE;
				case GKNIGHT:
				case SKNIGHT:
				case P_SKNIGHT:
				case P_GKNIGHT:
					return TokenType.KNIGHT;
				case GSILVER:
				case SSILVER:
				case P_GSILVER:
				case P_SSILVER:
					return TokenType.SILVER;
				case GGOLD:
				case SGOLD:
					return TokenType.GOLD;
				case GBISHOP:
				case SBISHOP:
				case P_SBISHOP:
				case P_GBISHOP:
					return TokenType.BISHOP;
				case GROOK:
				case SROOK:
				case P_GROOK:
				case P_SROOK:
					return TokenType.ROOK;
				case KING:
				case JEWEL:
					return TokenType.KING;
				default:
					return TokenType.UNDEFINED;
			}
		}

		public static int getTokenValue (TokenType token, bool isGote)
		{
			switch (token)
			{
				case TokenType.PAWN:
					return isGote ? GPAWN : SPAWN;
				case TokenType.PROMOTED_PAWN:
					return isGote ? P_GPAWN : P_SPAWN;
				case TokenType.LANCE:
					return isGote ? GLANCE : SLANCE;
				case TokenType.PROMOTED_LANCE:
					return isGote ? P_GLANCE : P_SLANCE;
				case TokenType.KNIGHT:
					return isGote ? GKNIGHT : SKNIGHT;
				case TokenType.PROMOTED_KNIGHT:
					return isGote ? P_GKNIGHT : P_SKNIGHT;
				case TokenType.SILVER:
					return isGote ? GSILVER : SSILVER;
				case TokenType.PROMOTED_SILVER:
					return isGote ? P_GSILVER : P_SSILVER;
				case TokenType.GOLD:
					return isGote ? GGOLD : SGOLD;
				case TokenType.BISHOP:
					return isGote ? GBISHOP : SBISHOP;
				case TokenType.PROMOTED_BISHOP:
					return isGote ? P_GBISHOP : P_SBISHOP;
				case TokenType.ROOK:
					return isGote ? GROOK : SROOK;
				case TokenType.PROMOTED_ROOK:
					return isGote ? P_GROOK : P_SROOK;
				case TokenType.KING:
					return isGote ? KING : JEWEL;
				default:
					return 0;
			}
		}


		public struct TokenInfo
		{
			public Token.TokenType tokenType;
			public bool droppedPiece;
			public int oldX;
			public int oldY;
			public int oldValue;
			public int newX;
			public int newY;
			public bool promoted;
			public int newValue;
			public bool capturedSomething;
			public Token.TokenType capturedType;
			public int capturedValue;

			public void Clear ()
			{
				this.tokenType = Token.TokenType.UNDEFINED;
				this.droppedPiece = false;
				this.oldX = -1;
				this.oldY = -1;
				this.oldValue = 0;
				this.newX = -1;
				this.newY = -1;
				this.promoted = false;
				this.newValue = 0;
				this.capturedSomething = false;
				this.capturedType = Token.TokenType.UNDEFINED;
				this.capturedValue = 0;
			}
		}
	}
}

