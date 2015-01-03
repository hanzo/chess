using System;
using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	class Bishop : Piece
	{
		public Bishop(PieceColor color, Position startPos) : base(color, startPos)
		{
		}

		public override PieceType Type
		{
			get { return PieceType.Bishop; }
		}

		public override List<PieceMove> GetValidMoves()
		{
			throw new NotImplementedException();
		}
	}
}
