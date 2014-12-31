using System;
using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	class Pawn : Piece
	{
		public Pawn(PieceColor color, Position startPos) : base(color, startPos)
		{
		}

		public override List<PieceMove> GetValidMoves()
		{
			throw new NotImplementedException();
		}
	}
}
