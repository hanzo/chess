﻿using System;
using System.Collections.Generic;

namespace Chess.ChessEngine.Pieces
{
	class Rook : Piece
	{
		public Rook(PieceColor color, Position startPos) : base(color, startPos)
		{
		}

		public override PieceType Type
		{
			get { return PieceType.Rook; }
		}

		public override List<Turn> GetValidTurns(Match match)
		{
			return new List<Turn>();
		}
	}
}
