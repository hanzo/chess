using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	public abstract class Piece
	{
		public PieceColor Color { get; private set; }
		public abstract PieceType Type { get; }

		public Position StartingPosition { get; private set; }
		public Position CurrentPosition { get; set; }
		public bool IsCaptured { get; set; }

		protected Piece(PieceColor color, Position startPos)
		{
			Color = color;
			StartingPosition = startPos;
			CurrentPosition = startPos;
			IsCaptured = false;
		}

		// TODO: Should the Piece class hold a reference to its parent Match object instead of passing it as a param?
		public abstract List<Turn> GetValidTurns(Match match);
	}
}
