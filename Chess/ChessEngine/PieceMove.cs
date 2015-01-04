using Chess.ChessEngine.Pieces;

namespace Chess.ChessEngine
{
	public class PieceMove
	{
		public Piece Piece;
		public Position StartPosition;
		public Position EndPosition;

		public PieceMove(Piece piece, Position startPosition, Position endPosition)
		{
			Piece = piece;
			StartPosition = startPosition;
			EndPosition = endPosition;
		}
	}
}
