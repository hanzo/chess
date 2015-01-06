﻿using System;
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

			var validTurns = match.GetCurrentValidTurns();

			// TODO: update this loop condition to quit once we know when the game is over
			while (true)
			{
				PrintBoard(match);

				PrintValidTurns(match.ActivePlayer, validTurns);

				int selectedTurn;

				while (true)
				{
					string userInput = Console.ReadLine();

					// TODO: find a less hacky way to print a specific turn via the command line
					if (!String.IsNullOrWhiteSpace(userInput) && userInput.Contains("printturn"))
					{
						var tokens = userInput.Split(' ');

						if (tokens.Length == 2)
						{
							int turnToPrint;
							try
							{
								turnToPrint = Convert.ToInt32(tokens[1]);
							}
							catch (Exception)
							{
								Console.WriteLine("Usage: printturn [turn number]");
								continue;
							}
							PrintBoard(match, turnToPrint);
							continue;
						}
						else
						{
							Console.WriteLine("Usage: printturn [turn number]");
							continue;
						}
					}

					try
					{
						selectedTurn = Convert.ToInt32(userInput);
					}
					catch (Exception)
					{
						Console.WriteLine("Choose the number corresponding to the desired move number, e.g. '2'");
						continue;
					}

					// Turns are printed with 1-indexing, so we need to undo that to match the array
					selectedTurn -= 1;

					if (selectedTurn < 0 || selectedTurn >= validTurns.Count)
					{
						Console.WriteLine("Choose the number corresponding to the desired move number, e.g. '2'");
						continue;
					}

					// TODO: improve this flow
					break;
				}

				// Execute the selected turn and update validTurns with the new set of possible turns (for the other player)
				validTurns = match.ExecuteTurn(validTurns[selectedTurn]);
			}
		}

		private static void PrintValidTurns(Player activePlayer, List<Turn> validTurns)
		{
			Console.WriteLine("\nActive player: {0}. Valid plays: ", activePlayer.Color);
			for (int turnNum = 0; turnNum < validTurns.Count; turnNum++)
			{
				// TODO: print moves in proper algebraic notation

				Turn turn = validTurns[turnNum];

				var turnPrintStr = String.Format("{0}.\t", turnNum + 1);
				foreach (var move in turn.Moves)
				{
					turnPrintStr += String.Format("{0} -> {1}, ", move.StartPosition, move.EndPosition);
				}

				turnPrintStr = turnPrintStr.TrimEnd(',', ' '); // remove trailing comma
				Console.WriteLine(turnPrintStr);
			}
			Console.Write("Enter the number corresponding to your desired move and hit enter: ");
		}

		private static void PrintBoard(Match match, int turnNumber = -1)
		{
			if (turnNumber == -1)
				turnNumber = match.CurrentTurn;

			var board = match.GetBoardState(turnNumber);

			Console.WriteLine("{0} player: {1}\n", match.PlayerOnTop.Color, match.PlayerOnTop.Name);
			Console.WriteLine("   ------------------");

			for (int row = 7; row >= 0; row--)
			{
				var printRowStr = " ";
				printRowStr += row + 1;
				printRowStr += " |";
				for (int col = 0; col < 8; col++)
				{
					printRowStr += GetPieceString((PieceType)board[col, row].Item1, (PieceColor)board[col,row].Item2);
				}
				printRowStr += "|";
				Console.WriteLine(printRowStr);
			}

			Console.WriteLine("   ------------------");
			Console.WriteLine("    A B C D E F G H ");

			Console.WriteLine("\n{0} player: {1}", match.PlayerOnBottom.Color, match.PlayerOnBottom.Name);
		}

		private static string GetPieceString(PieceType type, PieceColor color)
		{
			char colorChar = (color == PieceColor.Black) ? '+' : ' ';

			switch (type)
			{
				case PieceType.Pawn:
					return "P" + colorChar;
				case PieceType.Knight:
					return "N" + colorChar;
				case PieceType.Bishop:
					return "B" + colorChar;
				case PieceType.Rook:
					return "R" + colorChar;
				case PieceType.Queen:
					return "Q" + colorChar;
				case PieceType.King:
					return "K" + colorChar;
				default:
					return "  ";
			}
		}

		private static string GetPieceStringWithCaps(PieceType type, PieceColor color)
		{
			switch (type)
			{
				case PieceType.Pawn:
					return (color == PieceColor.White ) ? "P " : "p ";
				case PieceType.Knight:
					return (color == PieceColor.White) ? "N " : "n ";
				case PieceType.Bishop:
					return (color == PieceColor.White) ? "B " : "b ";
				case PieceType.Rook:
					return (color == PieceColor.White) ? "R " : "r ";
				case PieceType.Queen:
					return (color == PieceColor.White) ? "Q " : "q ";
				case PieceType.King:
					return (color == PieceColor.White) ? "K " : "k ";
				default:
					return "  ";
			}
		}

		// Piece type only, not color

		//private static string GetPieceString(PieceType type)
		//{
		//	switch (type)
		//	{
		//		case PieceType.Pawn:
		//			return "P ";
		//		case PieceType.Knight:
		//			return "N ";
		//		case PieceType.Bishop:
		//			return "B ";
		//		case PieceType.Rook:
		//			return "R ";
		//		case PieceType.Queen:
		//			return "Q ";
		//		case PieceType.King:
		//			return "K ";
		//		default:
		//			return "  ";
		//	}
		//}
	}
}
