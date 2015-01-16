using System;
using System.Collections.Generic;
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

			var validTurns = match.CurrentValidTurns;

			// TODO: update this loop condition to quit once we know when the game is over
			while (true)
			{
				PrintBoard(match);

				PrintValidTurns(match, validTurns);

				int selectedTurn;

				while (true)
				{
					string userInput = Console.ReadLine();

					// TODO: find a less hacky way to input commands
					if (userInput == null || String.IsNullOrWhiteSpace(userInput))
					{
						Console.WriteLine("Enter a command");
					}
					else if (userInput.Contains("next"))
					{
						if (match.NextTurn())
						{
							PrintBoard(match);

							// If we're at the latest state of the board, print the available moves again
							if (match.ViewingLastTurn == match.LastTurn)
								PrintValidTurns(match, validTurns);
						}
						else
						{
							Console.WriteLine("This is the latest turn");
						}
					}
					else if (userInput.Contains("prev"))
					{
						if (match.PreviousTurn())
						{
							PrintBoard(match);
						}
						else
						{
							Console.WriteLine("This is the start of the game");
						}
					}
					else if (userInput.Contains("gototurn"))
					{
						var tokens = userInput.Split(' ');

						if (tokens.Length != 2)
						{
							Console.WriteLine("Usage: gototurn [turn number]");
						}

						try
						{
							int newTurnNum = Convert.ToInt32(tokens[1]);
							if (match.GoToTurn(newTurnNum))
							{
								PrintBoard(match);

								// If we're at the latest state of the board, print the available moves again
								if (match.ViewingLastTurn == match.LastTurn)
									PrintValidTurns(match, validTurns);
							}
							else
							{
								Console.WriteLine("Must choose a turn number between 0 and " + match.LastTurn);
							}
						}
						catch (Exception)
						{
							Console.WriteLine("Usage: gototturn [turn number]");
						}
					}
					else if (userInput.Contains("gotostart"))
					{
						match.GoToStart();
						PrintBoard(match);
					}
					else if (userInput.Contains("gotoend"))
					{
						match.GoToEnd();
						PrintBoard(match);
						PrintValidTurns(match, validTurns);
					}
					else  // assume user input is a number signifying a turn to submit
					{
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

						// we only break from the inner while loop when the user submits a new move to advance the state of the game
						// TODO: improve this flow
						break;
					}
				}

				// Execute the selected turn and update validTurns with the new set of possible turns (for the other player)
				validTurns = match.ExecuteNewTurn(validTurns[selectedTurn]);
			}
		}

		// This should only be called to print valid moves as of the latest state of the board
		private static void PrintValidTurns(Match match, List<Turn> validTurns)
		{
			Console.WriteLine("\nActive player: {0}. Valid plays: ", match.CurrentActivePlayer.Color);
			for (int turnNum = 0; turnNum < validTurns.Count; turnNum++)
			{
				// TODO: print moves in proper algebraic notation

				Turn turn = validTurns[turnNum];

				var turnPrintStr = String.Format("{0}.\t", turnNum + 1);
				foreach (var move in turn.Moves)
				{
					turnPrintStr += (move.IsCaptured) 
						? String.Format("{0} captured, ", move.StartPosition)
						: String.Format("{0} -> {1}, ", move.StartPosition, move.EndPosition);
				}

				turnPrintStr = turnPrintStr.TrimEnd(',', ' '); // remove trailing comma
				Console.WriteLine(turnPrintStr);
			}
			Console.Write("Enter the number corresponding to your desired move and hit enter: ");
		}

		private static void PrintBoard(Match match)
		{
			var board = match.GetBoardState(match.ViewingLastTurn);

			string boardTitleStr = "\n---------- ";
			if (match.ViewingLastTurn == 0)
			{
				boardTitleStr += "Starting board";
			}
			else
			{
				boardTitleStr += (match.ViewingLastTurn == match.LastTurn)
					? String.Format("Board after turn {0}", match.LastTurn)
					: String.Format("Board after turn {0} of {1}", match.ViewingLastTurn, match.LastTurn);	
			}
			boardTitleStr += " ----------\n";

			Console.WriteLine(boardTitleStr);
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
	}
}
