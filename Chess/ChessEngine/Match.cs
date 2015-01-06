using System;
using System.Collections.Generic;
using Chess.ChessEngine.Pieces;

namespace Chess.ChessEngine
{
	public class Match
	{
		// TODO: do we need both sets of player members? 
		public Player WhitePlayer;
		public Player BlackPlayer;
		public Player PlayerOnTop;
		public Player PlayerOnBottom;

		public int CurrentTurn;

		public Player ActivePlayer
		{
			// The white player always plays first on turn 0, so we can find the active player by checking if the turn number is even.
			get { return CurrentTurn % 2 == 0 ? WhitePlayer : BlackPlayer; }
		}

		private readonly List<Tuple<int, int>[,]> _boardStates;
		private readonly List<Turn> _turns;
		private List<Turn> _currentValidTurns;

		public Match(PieceColor playerOnTop, string whitePlayerName = null, string blackPlayerName = null)
		{
			// If no name was provided, give player the name "Black player" or "White player"
			if (String.IsNullOrWhiteSpace(whitePlayerName))
				whitePlayerName = "White player";

			if (String.IsNullOrWhiteSpace(blackPlayerName))
				blackPlayerName = "Black player";

			WhitePlayer = new Player(PieceColor.White, whitePlayerName);
			BlackPlayer = new Player(PieceColor.Black, blackPlayerName);
			PlayerOnTop = (playerOnTop == PieceColor.White) ? WhitePlayer : BlackPlayer;
			PlayerOnBottom = (playerOnTop == PieceColor.White) ? BlackPlayer : WhitePlayer;
			_turns = new List<Turn>();
			_boardStates = new List<Tuple<int, int>[,]>();
			CurrentTurn = 0;

			InitializeBoard();

			_boardStates.Add(ComputeCurrentBoardState()); // boardStates[0] is the starting board
			_currentValidTurns = ComputeCurrentTurns(); // set CurrentValidTurns within the method?
		}

		#region Board initialization

		public void InitializeBoard()
		{
			if (PlayerOnTop.Color == PieceColor.White)
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

			// Set up back row
			player.Pieces.Add(new Rook(player.Color, new Position(0,7)));
			player.Pieces.Add(new Knight(player.Color, new Position(1, 7)));
			player.Pieces.Add(new Bishop(player.Color, new Position(2, 7)));

			if (player.Color == PieceColor.White)
			{
				player.Pieces.Add(new King(player.Color, new Position(3, 7)));
				player.Pieces.Add(new Queen(player.Color, new Position(4, 7)));
			}
			else
			{
				player.Pieces.Add(new Queen(player.Color, new Position(3, 7)));
				player.Pieces.Add(new King(player.Color, new Position(4, 7)));
			}
			
			player.Pieces.Add(new Bishop(player.Color, new Position(5, 7)));
			player.Pieces.Add(new Knight(player.Color, new Position(6, 7)));
			player.Pieces.Add(new Rook(player.Color, new Position(7, 7)));
		}

		private void InitializeBottomPlayer(Player player)
		{
			// Set up pawns
			for (int col = 0; col < 8; col++)
			{
				player.Pieces.Add(new Pawn(player.Color, new Position(col, 1)));
			}

			// Set up back row
			player.Pieces.Add(new Rook(player.Color, new Position(0, 0)));
			player.Pieces.Add(new Knight(player.Color, new Position(1, 0)));
			player.Pieces.Add(new Bishop(player.Color, new Position(2, 0)));

			if (player.Color == PieceColor.White)
			{
				player.Pieces.Add(new Queen(player.Color, new Position(3, 0)));
				player.Pieces.Add(new King(player.Color, new Position(4, 0)));
			}
			else
			{
				player.Pieces.Add(new King(player.Color, new Position(3, 0)));
				player.Pieces.Add(new Queen(player.Color, new Position(4, 0)));
			}

			player.Pieces.Add(new Bishop(player.Color, new Position(5, 0)));
			player.Pieces.Add(new Knight(player.Color, new Position(6, 0)));
			player.Pieces.Add(new Rook(player.Color, new Position(7, 0)));
		}

		#endregion

		public List<Turn> GetCurrentValidTurns()
		{
			return _currentValidTurns;
		}

		private List<Turn> ComputeCurrentTurns()
		{
			var validTurns = new List<Turn>();

			foreach (var piece in ActivePlayer.Pieces)
			{
				// Captured pieces can't do anything
				if (piece.IsCaptured)
					continue;

				validTurns.AddRange(piece.GetValidTurns(this));
			}

			return validTurns;
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

		public List<Turn> ExecuteTurn(Turn turn)
		{
			if (!_currentValidTurns.Contains(turn))
				throw new ArgumentException("An invalid turn was passed to ExecuteTurn.");

			_turns.Add(turn);

			foreach (var move in turn.Moves)
			{
				move.Piece.CurrentPosition = move.EndPosition;
				// TODO: deal with captured pieces
			}

			CurrentTurn++;
			_boardStates.Add(ComputeCurrentBoardState());
			_currentValidTurns = ComputeCurrentTurns();

			return _currentValidTurns;
		}

		public Tuple<int, int>[,] GetBoardState()
		{
			return _boardStates[CurrentTurn];
		}

		public Tuple<int, int>[,] GetBoardState(int turnNumber)
		{
			return _boardStates[turnNumber];
		}

		// Return a representation of the board where each square contains two ints to indicate piece type and color
		private Tuple<int,int>[,] ComputeCurrentBoardState()
		{
			var board = new Tuple<int, int>[8, 8];

			// can we skip initializing the board manually?
			for (int col = 0; col < 8; col++)
			{
				for (int row = 0; row < 8; row++)
				{
					// TODO: figure out how best to represent a square with no piece. I don't like using -1 as the default piece color
					board[col, row] = new Tuple<int, int>(0, -1);
				}
			}

			AddPlayersPiecesToBoard(board, WhitePlayer);
			AddPlayersPiecesToBoard(board, BlackPlayer);

			return board;
		}

		private void AddPlayersPiecesToBoard(Tuple<int, int>[,] board, Player player)
		{
			foreach (Piece piece in player.Pieces)
			{
				// TODO: might consider adding separate lists for captured pieces, that way player.Pieces only contains 'live' pieces
				if (!piece.IsCaptured)
				{
					board[piece.CurrentPosition.Col, piece.CurrentPosition.Row] = new Tuple<int, int>((int)piece.Type, (int)player.Color);
				}	
			}
		}
	}
}
