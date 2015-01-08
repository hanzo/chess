using Chess.ChessEngine.Pieces;

namespace Chess.ChessEngine
{
	/// <summary>
	/// The change in state of a single piece as part of a player's turn.
	/// </summary>
	public class PieceMove
	{
		public Piece Piece;

		/// <summary>
		/// The piece's postion before the turn.
		/// </summary>
		public Position StartPosition;

		/// <summary>
		/// The piece's position after the turn.
		/// </summary>
		public Position EndPosition;

		/// <summary>
		/// True if the piece was captured this turn, false otherwise.
		/// </summary>
		public bool IsCaptured;

		public PieceMove(Piece piece, Position startPosition, Position endPosition, bool isCaptured = false)
		{
			Piece = piece;
			StartPosition = startPosition;
			EndPosition = endPosition;
			IsCaptured = isCaptured;
		}
	}
}
