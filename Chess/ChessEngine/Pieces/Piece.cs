using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	public abstract class Piece
	{
		public PieceColor Color { get; private set; }
		public abstract PieceType Type { get; }

		public Position StartingPosition { get; private set; }
		public Position CurrentPosition { get; private set; }

		protected Piece(PieceColor color, Position startPos)
		{
			Color = color;
			StartingPosition = startPos;
			CurrentPosition = startPos;
		}

		public abstract List<PieceMove> GetValidMoves();
	}
}
