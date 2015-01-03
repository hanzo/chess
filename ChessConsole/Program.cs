using System;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Chess.ChessEngine;

namespace ChessConsole
{
	class Program
	{
		static void Main(string[] args)
		{
			RunSampleGame();
		}

		private static void RunSampleGame()
		{
			var match = new Match(PieceColor.Black, "Gandalf the White", "Blackbeard");

			PrintBoard(match);
		}

		private static void PrintBoard(Match match)
		{
			var board = match.BoardState(0);

			Console.WriteLine("------------------");

			for (int row = 7; row >= 0; row--)
			{
				var printRowStr = "";
				printRowStr += "|";
				for (int col = 0; col < 8; col++)
				{
					printRowStr += GetPieceString((PieceType) board[col, row]);
				}
				printRowStr += "|";
				Console.WriteLine(printRowStr);
			}

			Console.WriteLine("------------------");
		}

		private static string GetPieceString(PieceType type)
		{
			switch (type)
			{
				case PieceType.Pawn:
					return "P ";
				case PieceType.Knight:
					return "Kn";
				case PieceType.Bishop:
					return "B ";
				case PieceType.Rook:
					return "R ";
				case PieceType.Queen:
					return "Q ";
				case PieceType.King:
					return "Ki";
				default:
					return "  ";
			}

		}
	}
}
