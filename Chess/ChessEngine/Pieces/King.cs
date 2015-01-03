using System;
using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	class King : Piece
	{
		public King(PieceColor color, Position startPos) : base(color, startPos)
		{
		}

		public override PieceType Type
		{
			get { return PieceType.King; }
		}

		public override List<PieceMove> GetValidMoves()
		{
			throw new NotImplementedException();
		}
	}
}
