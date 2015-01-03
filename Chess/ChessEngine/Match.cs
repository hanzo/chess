using System;
using System.Collections.Generic;
using Chess.ChessEngine.Pieces;

namespace Chess.ChessEngine
{
	public class Match
	{
		public Player WhitePlayer;
		public Player BlackPlayer;
		public PieceColor PlayerOnTop;
		
		public List<Turn> Turns { get; private set; }

		public int CurrentTurn;

		public Match(PieceColor playerOnTop, string whitePlayerName = null, string blackPlayerName = null)
		{
			// If no name was provided, give player the name "Black player" or "White player"
			if (String.IsNullOrWhiteSpace(whitePlayerName))
				whitePlayerName = "White player";

			if (String.IsNullOrWhiteSpace(blackPlayerName))
				blackPlayerName = "Black player";

			WhitePlayer = new Player(PieceColor.White, whitePlayerName);
			BlackPlayer = new Player(PieceColor.Black, blackPlayerName);
			PlayerOnTop = playerOnTop;
			Turns = new List<Turn>();
			CurrentTurn = 0;

			InitializeBoard();
		}

		public void InitializeBoard()
		{
			if (PlayerOnTop == PieceColor.White)
			{
				InitializeTopPlayer(WhitePlayer);
				InitializeBottomPlayer(BlackPlayer);
			}
			else
			{
				InitializeTopPlayer(BlackPlayer);
				InitializeBottomPlayer(WhitePlayer);
			}
		}

		private void InitializeTopPlayer(Player player)
		{
			// Set up pawns
			for (int col = 0; col < 8; col++)
			{
				player.Pieces.Add(new Pawn(player.Color, new Position(col, 6)));
			}			
		}

		private void InitializeBottomPlayer(Player player)
		{
			// Set up pawns
			for (int col = 0; col < 8; col++)
			{
				player.Pieces.Add(new Pawn(player.Color, new Position(col, 1)));
			}
		}

		public void NextTurn()
		{
			
		}

		public void PreviousTurn()
		{
			
		}

		public void GoToTurn(int turnNumber)
		{
			
		}

		public int[,] BoardState(int turn)
		{
			var board = new int[8,8];

			// can we skip initializing the board manually?
			for (int col = 0; col < 8; col++)
			{
				for (int row = 0; row < 8; row++)
				{
					board[row, col] = 0;
				}
			}

			AddPlayersPiecesToBoard(board, WhitePlayer);
			AddPlayersPiecesToBoard(board, BlackPlayer);

			return board;
		}

		private void AddPlayersPiecesToBoard(int[,] board, Player player)
		{
			foreach (Piece piece in player.Pieces)
			{
				board[piece.CurrentPosition.Col, piece.CurrentPosition.Row] = (int)piece.Type;
			}
		}
	}
}
