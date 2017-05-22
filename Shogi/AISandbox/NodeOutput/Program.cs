using System;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Collections.Generic;
using Tools;

class NodeOutputClass
{
	public static int [,] testState = { {      0       ,       0       ,       0       ,      0      ,      0      , Token.KING  ,  Token.GPAWN  ,        0      , Token.GPAWN  },
										{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       , Token.SBISHOP ,      0       },
										{      0       ,       0       ,       0       ,      0      , Token.GROOK ,      0      ,       0       ,        0      ,      0       },
										{      0       ,       0       ,       0       ,      0      , Token.JEWEL ,      0      ,       0       ,        0      ,      0       },
										{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       },
										{      0       ,       0       ,       0       ,      0      ,      0      ,      0      ,       0       ,        0      ,      0       } };

	public static void Main (string[] args)
	{
		Node initNode = new Node ();

		int [][] jaggedTestState = new int [9][];
		for (int i = 0; i < 9; i++)
			jaggedTestState [i] = new int[9];
		
		for (int i = 0; i < 9; i++)
			for (int j = 0; j < 9; j++)
				jaggedTestState [i][j] = NodeOutputClass.testState[i,j];

		initNode.setState (ref jaggedTestState);
		List<Node> movesPlayed = new List<Node> ();
		movesPlayed.Add (new Node ());

		SerializeNode.Serialize (initNode, @"C:\Cygwin\home\Dylan\Unity\Shogi\AISandbox\NodeOutput\root.xml");
		SerializeNode.SerializeList (movesPlayed, @"C:\Cygwin\home\Dylan\Unity\Shogi\AISandbox\NodeOutput\movesPlayed.xml");

		ProcessStartInfo processInfo = new ProcessStartInfo (@"C:\Cygwin\home\Dylan\Unity\Shogi\AISandbox\NodeOutput\NegaScoutProgram.exe");
		processInfo.Arguments = @"C:\Cygwin\home\Dylan\Unity\Shogi\AISandbox\NodeOutput\root.xml" + " 4 " + true + " " + @"C:\Cygwin\home\Dylan\Unity\Shogi\AISandbox\NodeOutput\movesPlayed.xml" + " lol";
		processInfo.CreateNoWindow = true;
		processInfo.UseShellExecute = false;
		processInfo.RedirectStandardOutput = true;

		Process proc = Process.Start (processInfo);

		proc.WaitForExit ();
		String stringNode = proc.StandardOutput.ReadToEnd ();

		Node returnNode = SerializeNode.Deserialize	(stringNode);
		returnNode.showState ();
	}
}
