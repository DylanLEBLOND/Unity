#define DLE
//#define BMA
//#define VSE
// Specified the log directory to use


using System;
using System.IO;
using System.Text;
using System.Collections.Generic;
using Tools;
using MinMax;
using OldAlphaBeta;
using AlphaBeta;
using NegaScout;
using MonteCarlo;
using ProofNumberSearch;

namespace SandboxPlayer
{
	class Launcher
	{
		public enum Algorithm
		{
			PROOFNUMBERSEARCH, MONTECARLO, NEGASCOUT, ALPHABETA, OLDALPHABETA, MINMAX
		};

		public static Algorithm goteAlgorithm = Algorithm.NEGASCOUT;
		public static Algorithm senteAlgorithm = Algorithm.OLDALPHABETA;

		public static bool firstPlayerIsGote = true;
		public static int depth = 3;
		public static int nbParties = 1;

		#if DLE
		public static string logPath = @"C:\Cygwin\home\Dylan\Unity\Shogi\AISandbox\SandboxPlayer\PartiesDLE\NegaScoutvsOldAlphaBeta\";
		#elif BMA
		public static string logPath = @"C:\Users\Bryan\Desktop\SHOGUNITY\Dev_IA\SandboxProofNumberSearch\PartiesBMA\";
		#else	// VSE
		public static string logPath = @"/Users/Vasanth/Desktop/Log/PartiesVSE/";
		#endif

// Ce tableau est defini a l'envers parce que l'on va de 0 (en haut) a 9 (en bas)
//		public static int [,] testState = { { Token.GLANCE , Token.GKNIGHT , Token.GSILVER , Token.GGOLD , Token.KING  , Token.GGOLD , Token.GSILVER , Token.GKNIGHT , Token.GLANCE },
//											{      0       , Token.GBISHOP ,       0       ,      0      ,      0      ,      0      ,       0       ,  Token.GROOK  ,      0       },
//											{ Token.GPAWN  ,  Token.GPAWN  ,  Token.GPAWN  , Token.GPAWN , Token.GPAWN , Token.GPAWN ,  Token.GPAWN  ,  Token.GPAWN  , Token.GPAWN  },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
//											{ Token.SPAWN  ,  Token.SPAWN  ,  Token.SPAWN  , Token.SPAWN , Token.SPAWN , Token.SPAWN ,  Token.SPAWN  ,  Token.SPAWN  , Token.SPAWN  },
//											{      0       ,  Token.SROOK  ,       0       ,      0      ,      0      ,      0      ,       0       , Token.SBISHOP ,      0       },
//											{ Token.SLANCE , Token.SKNIGHT , Token.SSILVER , Token.SGOLD , Token.JEWEL , Token.SGOLD , Token.SSILVER , Token.SKNIGHT , Token.SLANCE } };

//		public static int [,] testState = { {      0       ,       0       ,       0       ,      0      ,      0      , Token.KING  ,  Token.GPAWN  ,        0      , Token.GPAWN  },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       , Token.SBISHOP ,      0       },
//											{      0       ,       0       ,       0       ,      0      , Token.GROOK ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      , Token.JEWEL ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
//											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       } };

		public static int [,] testState = { {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       	0       ,        0      ,      0       },
											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
											{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
											{      0       ,       0       ,       0       ,      0      , Token.KING  ,      0      ,       0       ,        0      ,      0       },
											{      0       ,       0       ,       0       ,      0      , Token.SROOK ,      0      ,       0       ,        0      ,      0       },
											{      0       , Token.GBISHOP ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
											{ Token.SPAWN  ,       0       ,  Token.SPAWN  , Token.JEWEL ,      0      ,      0      ,       0       ,        0      ,      0       } };

		//	public static int [,] testState = { {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
		//								 {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
		//								 {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
		//								 {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
		//								 {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
		//								 {      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
		//								 {      0       ,       0       ,       0       ,      0      , Token.KING  ,      0      ,       0       ,        0      ,      0       },
		//								 {  Token.SPAWN ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
		//								 {      0       ,       0       ,Token.P_GBISHOP, Token.JEWEL ,      0      ,      0      ,       0       ,        0      ,      0       } };


		/*	  init state

		L N S G K G S N L
		. B . . . . . R .
		P P P P P P P P P
		. . . . . . . . .
		. . . . . . . . .
		. . . . . . . . .
		P P P P P P P P P
		. R . . . . . B .
		L N S G J G S N L
	*/

		public static void Main ()
		{
			for (int currentPartie = 0; currentPartie < nbParties; currentPartie++)
			{
				Node root = new Node ();
				Node move;
				List<Node> movesPlayed = new List<Node> ();
				string gameWorkflow = "";
				int i = 0;
				bool stillAlive = true;
				bool isGote = firstPlayerIsGote;
				int index = 0;
				int tic;
				int tac;
				int ticTurn;
				int tacTurn;

				Console.WriteLine ("********** Partie " + currentPartie.ToString () + " **********");
				gameWorkflow += "********** Partie " + currentPartie.ToString () + " **********\n";

				int [][] jaggedTestState = new int [9][];
				for (i = 0; i < 9; i++)
					jaggedTestState [i] = new int[9];

					for (i = 0; i < 9; i++)
						for (int j = 0; j < 9; j++)
							jaggedTestState [i][j] = Launcher.testState[i,j];

				root.setState (ref jaggedTestState);

				switch (goteAlgorithm)
				{
					case Algorithm.PROOFNUMBERSEARCH:
						Console.Write ("********** Gote Algorithm: ProofNumberSearch | ");
						gameWorkflow += "********** Gote Algorithm: ProofNumberSearch | ";
						break;
					case Algorithm.MONTECARLO:
						Console.Write ("********** Gote Algorithm: MonteCarlo | ");
						gameWorkflow += "********** Gote Algorithm: MonteCarlo | ";
						break;
					case Algorithm.NEGASCOUT:
						Console.Write ("********** Gote Algorithm: NegaScout | ");
						gameWorkflow += "********** Gote Algorithm: NegaScout | ";
						break;
					case Algorithm.ALPHABETA:
						Console.Write ("********** Gote Algorithm: AlphaBeta | ");
						gameWorkflow += "********** Gote Algorithm: AlphaBeta | ";
						break;
					case Algorithm.OLDALPHABETA:
						Console.Write ("********** Gote Algorithm: Old AlphaBeta | ");
						gameWorkflow += "********** Gote Algorithm: Old AlphaBeta | ";
						break;
					case Algorithm.MINMAX:
						Console.Write ("********** Gote Algorithm: MinMax | ");
						gameWorkflow += "********** Gote Algorithm: MinMax | ";
						break;
				}

				switch (senteAlgorithm)
				{
					case Algorithm.PROOFNUMBERSEARCH:
						Console.WriteLine ("Sente Algorithm: ProofNumberSearch | Depth: {0} **********", depth);
						gameWorkflow += "Sente Algorithm: ProofNumberSearch | Depth: " + depth.ToString () + " **********\n";
						break;
					case Algorithm.MONTECARLO:
						Console.WriteLine ("Sente Algorithm: MonteCarlo | Depth: {0} **********", depth);
						gameWorkflow += "Sente Algorithm: MonteCarlo | Depth: " + depth.ToString () + " **********\n";
						break;
					case Algorithm.NEGASCOUT:
						Console.WriteLine ("Sente Algorithm: NegaScout | Depth: {0} **********", depth);
						gameWorkflow += "Sente Algorithm: NegaScout | Depth: " + depth.ToString () + " **********\n";
						break;
					case Algorithm.ALPHABETA:
						Console.WriteLine ("Sente Algorithm: AlphaBeta | Depth: {0} **********", depth);
						gameWorkflow += "Sente Algorithm: AlphaBeta | Depth: " + depth.ToString () + " **********\n";
						break;
					case Algorithm.OLDALPHABETA:
						Console.WriteLine ("Sente Algorithm: Old AlphaBeta | Depth: {0} **********", depth);
						gameWorkflow += "Sente Algorithm: Old AlphaBeta | Depth: " + depth.ToString () + " **********\n";
						break;
					case Algorithm.MINMAX:
						Console.WriteLine ("Sente Algorithm: MinMax | Depth: {0} **********", depth);
						gameWorkflow += "Sente Algorithm: MinMax | Depth: " + depth.ToString () + " **********\n";
						break;
				}

				Console.WriteLine ("root Gote score = " + root.evaluate (true).ToString ());
				gameWorkflow += "root Gote score = " + root.evaluate (true).ToString () + "\n";
				Console.WriteLine ("root Sente score = " + root.evaluate (false).ToString ());
				gameWorkflow += "root Sente score = " + root.evaluate (false).ToString () + "\n";
				Console.WriteLine ("root Gote Captured Tokens = " + root.getGoteCapturedTokensInString ());
				gameWorkflow += "root Gote Captured Tokens = " + root.getGoteCapturedTokensInString () + "\n";
				Console.WriteLine ("root Sente Captured Tokens = " + root.getSenteCapturedTokensInString ());
				gameWorkflow += "root Sente Captured Tokens = " + root.getSenteCapturedTokensInString () + "\n";
				Console.WriteLine (root.getStateInString ());
				gameWorkflow += root.getStateInString ();

				i = 0;
				tic = Environment.TickCount;
				while (stillAlive)
				{
					move = null;
					ticTurn = Environment.TickCount;
					if (isGote)
					{
						Console.WriteLine ("********** Turn " + (i + 1).ToString () + " Gote Playing **********");
						gameWorkflow += "********** Turn " + (i + 1).ToString () + " Gote Playing **********\n";
						switch (goteAlgorithm)
						{
							case Algorithm.PROOFNUMBERSEARCH:
								move = ProofNumberSearchClass.Pns (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
							case Algorithm.MONTECARLO:
								move = MonteCarloClass.MonteCarlo (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
							case Algorithm.NEGASCOUT:
								move = NegaScoutClass.NegaScout (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
							case Algorithm.ALPHABETA:
								move = AlphaBetaClass.AspirationSearch (root, depth, isGote, movesPlayed, index, ref gameWorkflow);
								break;
							case Algorithm.OLDALPHABETA:
								move = OldAlphaBetaClass.OldAlphaBeta (root, depth, isGote, ref gameWorkflow);
								break;
							case Algorithm.MINMAX:
								move = MinMaxClass.MinMax (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
						}
					}
					else
					{
						Console.WriteLine ("********** Turn " + (i + 1).ToString () + " Sente Playing **********");
						gameWorkflow += "********** Turn " + (i + 1).ToString () + " Sente Playing **********\n";
						switch (senteAlgorithm)
						{
							case Algorithm.PROOFNUMBERSEARCH:
								move = ProofNumberSearchClass.Pns (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
							case Algorithm.MONTECARLO:
								move = MonteCarloClass.MonteCarlo (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
							case Algorithm.NEGASCOUT:
								move = NegaScoutClass.NegaScout (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
							case Algorithm.ALPHABETA:
								move = AlphaBetaClass.AspirationSearch (root, depth, isGote, movesPlayed, index, ref gameWorkflow);
								break;
							case Algorithm.OLDALPHABETA:
								move = OldAlphaBetaClass.OldAlphaBeta (root, depth, isGote, ref gameWorkflow);
								break;
							case Algorithm.MINMAX:
								move = MinMaxClass.MinMax (root, depth, isGote, movesPlayed, ref gameWorkflow);
								break;
						}
					}
					tacTurn = Environment.TickCount;

					if (move == null)
					{
						Console.WriteLine ("CRITICAL ERROR: Null Move returned");
						gameWorkflow += "CRITICAL ERROR: Null Move returned\n";
						break;
					}

					movesPlayed.Add (new Node (move));
					Console.WriteLine ("movesPlayed = {0}", movesPlayed.Count);

					Console.WriteLine ("Gote score = " + move.evaluate (true));
					gameWorkflow += "Gote score = " + move.evaluate (true) + "\n";
					Console.WriteLine ("Sente score = " + move.evaluate (false));
					gameWorkflow += "Sente score = " + move.evaluate (false) + "\n";

					Console.WriteLine ("Gote Captured Tokens = " + move.getGoteCapturedTokensInString ());
					gameWorkflow += "Gote Captured Tokens = " + move.getGoteCapturedTokensInString () + "\n";

					Console.WriteLine ("Sente Captured Tokens = " + move.getSenteCapturedTokensInString ());
					gameWorkflow += "Sente Captured Tokens = " + move.getSenteCapturedTokensInString () + "\n";

					Console.WriteLine ("Turn Duration: " + (tacTurn - ticTurn).ToString () + " ms");
					gameWorkflow += "Turn Duration: " + (tacTurn - ticTurn).ToString () + " ms\n";
					Console.WriteLine (move.getStateInString ());
					gameWorkflow += move.getStateInString ();
					root.clear ();
					root = move;
					isGote = !isGote;
					i++;

					stillAlive = move.stillAlive (true /* isGote */) && move.stillAlive (false /* isGote */);
					if (!stillAlive)
					{
						if (move.stillAlive (true /* isGote */))
						{
							Console.WriteLine ("Gote won");
							gameWorkflow += "Gote won\n";
						}
						else
						{
							Console.WriteLine ("Sente won");
							gameWorkflow += "Sente won\n";
						}
					}
				}
				tac = Environment.TickCount;
				Console.WriteLine ("Game Duration: " + (tac - tic).ToString () + " ms");
				gameWorkflow += "Game Duration: " + (tac - tic).ToString () + " ms\n";

				string file = Launcher.logPath + "Game_" + DateTime.Now.ToString ("yyyy-MM-dd_HH-mm-ss") + ".txt";

				Console.WriteLine (file);
				System.IO.File.WriteAllText (file, gameWorkflow);
			}
		}
	}
}
