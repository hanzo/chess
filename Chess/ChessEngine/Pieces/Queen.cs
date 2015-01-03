using System;
using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	class Queen : Piece
	{
		public Queen(PieceColor color, Position startPos) : base(color, startPos)
		{
		}

		public override PieceType Type
		{
			get { return PieceType.Queen; }
		}

		public override List<PieceMove> GetValidMoves()
		{
			throw new NotImplementedException();
		}
	}
}
