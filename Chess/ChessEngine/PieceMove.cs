using Chess.ChessEngine.Pieces;

namespace Chess.ChessEngine
{
	public class PieceMove
	{
		public Piece Piece;
		public Position StartPosition;
		public Position EndPosition;
		public bool Captured;

		public PieceMove(Piece piece, Position startPosition, Position endPosition, bool captured = false)
		{
			Piece = piece;
			StartPosition = startPosition;
			EndPosition = endPosition;
			Captured = captured;
		}
	}
}
