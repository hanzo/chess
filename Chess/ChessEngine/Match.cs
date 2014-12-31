using System.Collections.Generic;

namespace Chess.ChessEngine
{
	class Match
	{
		public Player WhitePlayer;
		public Player BlackPlayer;
		public PieceColor PlayerOnTop;
		
		public List<Turn> Turns { get; private set; }

		public int CurrentTurn;

		public Match(Player whitePlayer, Player blackPlayer, PieceColor playerOnTop)
		{
			WhitePlayer = whitePlayer;
			BlackPlayer = blackPlayer;
			PlayerOnTop = playerOnTop;
			Turns = new List<Turn>();
			CurrentTurn = 0;
		}

		public void InitializeBoard()
		{
			if (PlayerOnTop == PieceColor.White)
			{
				InitializeTopPlayer(PieceColor.Black);
				InitializeBottomPlayer(PieceColor.White);
			}
			else
			{
				InitializeTopPlayer(PieceColor.White);
				InitializeBottomPlayer(PieceColor.Black);
			}
		}

		private void InitializeTopPlayer(PieceColor playerColor)
		{
			
		}

		private void InitializeBottomPlayer(PieceColor playerColor)
		{
			
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
	}
}
