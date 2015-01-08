using System;
using System.Collections.Generic;
using Chess.ChessEngine.Pieces;

namespace Chess.ChessEngine
{
	/// <summary>
	/// Represents a chess match between two players.
	/// </summary>
	public class Match
	{
		// TODO: do we need both sets of player members? 
		public Player WhitePlayer;
		public Player BlackPlayer;
		public Player PlayerOnTop;
		public Player PlayerOnBottom;

		/// <summary>
		/// Returns the Player whose turn it is at the current state of the game.
		/// </summary>
		public Player ActivePlayer
		{
			// The white player always plays first on turn 0, so we can find the active player by checking if the turn number is even.
			get { return LastTurn % 2 == 0 ? WhitePlayer : BlackPlayer; }
		}

		/// <summary>
		/// The number of the last turn that has been played in the match.
		/// </summary>
		public int LastTurn;

		/// <summary>
		/// The last turn that was played as of the board state that is currently being viewed.
		/// </summary>
		public int ViewingLastTurn;

		/// <summary>
		/// The list of valid turns available as of the latest state of the game.
		/// </summary>
		public List<Turn> CurrentValidTurns { get; private set; }

		// A board state is an 8x8 integer pair representation of the board after each move. 
		// Board state 0 is the starting state of the board.
		private readonly List<Tuple<int, int>[,]> _boardStates;

		// Each turn represents the change in board state as a result of a player's turn.
		// Turn 0 is the first turn played by the White player.
		private readonly List<Turn> _turns;

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
			LastTurn = 0;
			ViewingLastTurn = 0;

			InitializeBoard();

			_boardStates.Add(ComputeCurrentBoardState()); // boardStates[0] is the starting board
			CurrentValidTurns = ComputeCurrentTurns(); // set CurrentValidTurns within the method?
		}

		#region Board initialization

		/// <summary>
		/// Set up the pieces on the board in their initial locations.
		/// </summary>
		private void InitializeBoard()
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

		// Add to the board the pieces of the player on the top side of the board.
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

		// Add to the board the pieces of the player on the bottom side of the board.
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

		// Return a list of all valid turns available as of the latest state of the game.
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

		/// <summary>
		/// Advance the currently viewed state of the board to the next turn.
		/// </summary>
		/// <returns>True if the board state was advanced, false if the currently viewed turn is the most recent turn.</returns>
		public bool NextTurn()
		{
			if (ViewingLastTurn >= LastTurn)
				return false;

			ApplyTurn(_turns[ViewingLastTurn]);

			ViewingLastTurn++;

			return true;
		}

		/// <summary>
		/// Move back the currently viewed state of the board to the previous turn.
		/// </summary>
		/// <returns>True if the board state was moved back, false if the currently viewed turn is the start of the match.</returns>
		public bool PreviousTurn()
		{
			if (ViewingLastTurn <= 0)
				return false;

			UndoTurn(_turns[ViewingLastTurn-1]);

			ViewingLastTurn--;

			return true;
		}

		/// <summary>
		/// Move the state of the board forward or backward until the given turn number is reached.
		/// </summary>
		/// <param name="newTurnNumber">The desired turn number to view.</param>
		/// <returns>True if the board state was moved forward or back, false otherwise.</returns>
		public bool GoToTurn(int newTurnNumber)
		{
			if (newTurnNumber < 0 || newTurnNumber > LastTurn)
				return false;

			if (newTurnNumber < ViewingLastTurn)
			{
				while (PreviousTurn() && newTurnNumber < ViewingLastTurn)
				{
				}	
			}
			else
			{
				while (NextTurn() && newTurnNumber > ViewingLastTurn)
				{
				}	
			}

			return true;
		}

		/// <summary>
		/// Revert the board state to its starting position. 
		/// </summary>
		/// <returns>True if the board state was moved back, false if the currently viewed turn is the start of the match.</returns>
		public bool GoToStart()
		{
			return GoToTurn(0);
		}

		/// <summary>
		/// Advance the board state to the latest state of the game.
		/// </summary>
		/// <returns>True if the board state was advanced, false if the currently viewed turn is the most recent turn.</returns>
		public bool GoToEnd()
		{
			return GoToTurn(LastTurn);
		}

		/// <summary>
		/// Applies the given turn to the latest state of the board and advances to the next turn.
		/// </summary>
		/// <param name="turn">The turn played by the active player.</param>
		/// <returns>The list of valid turns after the given turn was applied.</returns>
		public List<Turn> ExecuteNewTurn(Turn turn)
		{
			if (!CurrentValidTurns.Contains(turn))
				throw new ArgumentException("An invalid turn was passed to ExecuteTurn.");

			_turns.Add(turn);

			ApplyTurn(turn);

			LastTurn++;
			ViewingLastTurn++;
			_boardStates.Add(ComputeCurrentBoardState());
			CurrentValidTurns = ComputeCurrentTurns();

			return CurrentValidTurns;
		}

		// Update the state of all pieces affected by the given turn.
		private void ApplyTurn(Turn turn)
		{
			foreach (var move in turn.Moves)
			{
				if (move.IsCaptured)
				{
					move.Piece.IsCaptured = true;
				}
				else
				{
					move.Piece.CurrentPosition = move.EndPosition;
				}	
			}
		}

		// Revert the state of all pieces affected by the given turn.
		private void UndoTurn(Turn turn)
		{
			foreach (var move in turn.Moves)
			{
				if (move.IsCaptured)
				{
					move.Piece.IsCaptured = false;
				}
				else
				{
					move.Piece.CurrentPosition = move.StartPosition;			
				}
			}
		}

		/// <summary>
		/// The board state as of the latest turn of the game.
		/// </summary>
		/// <returns>The board state as of the latest turn of the game.</returns>
		public Tuple<int, int>[,] GetBoardState()
		{
			return _boardStates[LastTurn];
		}

		/// <summary>
		/// The board state as of the given turn.
		/// </summary>
		/// <param name="turnNumber">The desired turn number.</param>
		/// <returns>The board state as of the given turn.</returns>
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

		// Add the pieces of the given player to the board.
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

		/// <summary>
		/// Return the piece located at the given location on the board.
		/// </summary>
		/// <returns>The piece at the given location if one exists, else null.</returns>
		public Piece GetPieceAtPosition(int column, int row)
		{
			// TODO: find a more efficient way of doing this
			foreach (Player player in new List<Player>{WhitePlayer, BlackPlayer})
			{
				foreach (Piece piece in player.Pieces)
				{
					if (piece.CurrentPosition.Col == column && piece.CurrentPosition.Row == row)
						return piece;
				}
			}
			return null;
		}
	}
}
